﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using static Weknow.Cypher.Builder.Pattern;
#pragma warning disable CA1063 // Implement IDisposable Correctly

namespace Weknow.Cypher.Builder
{
    public class CypherVisitor : ExpressionVisitor
    {
        public StringBuilder Query { get; } = new StringBuilder();
        public Dictionary<string, object> Parameters { get; } = new Dictionary<string, object>();
        private ContextValue<bool> isProperties = new ContextValue<bool>(false);
        private Dictionary<int, ContextValue<Expression>> expression = new Dictionary<int, ContextValue<Expression>>()
        {
            [0] = new ContextValue<Expression>(null),
            [1] = new ContextValue<Expression>(null),
            [2] = new ContextValue<Expression>(null)
        };

        protected override Expression VisitLambda<T>(Expression<T> node)
        {
            Visit(node.Body);
            return node;
        }

        protected override Expression VisitBinary(BinaryExpression node)
        {
            Visit(node.Left);
            switch (node.NodeType)
            {
                case ExpressionType.GreaterThan:
                    Query.Append("->");
                    break;
                case ExpressionType.LessThan:
                    Query.Append("<-");
                    break;
                case ExpressionType.Subtract:
                    Query.Append("-");
                    break;
                case ExpressionType.Equal:
                    Query.Append(" = ");
                    break;
                case ExpressionType.Add:
                    Query.Append(" += ");
                    break;
            }
            Visit(node.Right);
            return node;
        }

        protected override Expression VisitMethodCall(MethodCallExpression node)
        {
            using var _ = isProperties.Set(isProperties.Value || node.Method.ReturnType == typeof(IProperties));

            var attributes = node.Method.GetCustomAttributes(typeof(CypherAttribute), false);
            var format = attributes.Length > 0 ? (attributes[0] as CypherAttribute)?.Format : null;
            if (format != null)
            {
                IDisposable disp = null;
                for (int i = 0; i < format.Length; i++)
                {
                    switch (format[i])
                    {
                        case '$':
                            Visit(node.Arguments[int.Parse(format[++i].ToString())]);
                            break;
                        case '!':
                            Query.Append(node.Method.GetGenericArguments()[int.Parse(format[++i].ToString())].Name);
                            break;
                        case '+':
                            disp = expression[int.Parse(format[++i].ToString())].Set(node.Arguments[int.Parse(format[++i].ToString())]);
                            break;
                        case '.':
                            disp = expression[int.Parse(format[++i].ToString())].Set(node);
                            break;
                        case '\\':
                            Query.Append(format[++i]);
                            break;
                        default:
                            Query.Append(format[i]);
                            break;
                    }
                }
                disp?.Dispose();
            }
            else if (node.Method.Name == nameof(Range.EndAt))
            {
                Query.Append("*..");
                var index = node.Arguments[0] as UnaryExpression;
                Query.Append(index.Operand);
            }
            else if (node.Method.Name == "get_" + nameof(Range.All))
            {
                Query.Append("*");
            }
            else if (node.Method.Name == nameof(Pattern.All))
            {
                var properties = node.Method.GetGenericArguments()[0].GetProperties();
                foreach (var item in properties)
                {
                    Visit(node.Arguments[0]);
                    Query.Append(".");
                    Query.Append(item.Name);
                    if (item != properties.Last())
                        Query.Append(", ");
                }
            }
            else if (node.Method.Name == nameof(Pattern.Convention))
            {
                var filter = (node.Arguments[0] as Expression<Func<string, bool>>).Compile();
                var arguments = node.Method.IsGenericMethod
                    ? node.Method.GetGenericArguments()
                    : (expression[1].Value as MethodCallExpression).Method.GetGenericArguments();
                var properties = arguments[0].GetProperties().Where(p => filter(p.Name)).ToArray();
                foreach (var item in properties)
                {
                    Query.Append(item.Name);
                    Query.Append(": ");
                    if (expression[2].Value != null)
                    {
                        Visit(expression[2].Value);
                        Query.Append(".");
                    }
                    else
                        Query.Append("$");
                    Query.Append(item.Name);
                    Parameters[item.Name] = null;
                    if (item != properties.Last())
                        Query.Append(", ");
                }
            }
            return node;
        }

        protected override Expression VisitMember(MemberExpression node)
        {
            if (node.Type == typeof(Expression<Func<IProperties>>))
            {
                var expr = (node.Member as FieldInfo).GetValue((node.Expression as ConstantExpression).Value) as Expression;
                Visit(expr);
                return node;
            }
            if (node.Expression != null && !isProperties.Value)
            {
                Visit(node.Expression);
                Query.Append(".");
            }
            Query.Append(node.Member.Name);
            if (node.Member is PropertyInfo pi && pi.PropertyType == typeof(IProperty) || isProperties.Value)
            {
                Query.Append(": ");
                if (expression[0].Value != null)
                {
                    Query.Append("$");
                    Visit(expression[0].Value);
                }
                else if (expression[2].Value != null)
                {
                    Visit(expression[2].Value);
                    Query.Append(".");
                }
                else
                    Query.Append("$");
                Query.Append(node.Member.Name);
                Parameters[node.Member.Name] = null;
            }
            return node;
        }

        protected override Expression VisitNewArray(NewArrayExpression node)
        {
            foreach (var expr in node.Expressions)
            {
                Visit(expr);
                if (expr != node.Expressions.Last())
                    Query.Append(", ");
            }
            return node;
        }

        protected override Expression VisitNew(NewExpression node)
        {
            if (node.Type == typeof(Range))
            {
                Query.Append("*");
                var index = node.Arguments[0] as UnaryExpression;
                Query.Append(index.Operand);
                Query.Append("..");
                index = node.Arguments[1] as UnaryExpression;
                Query.Append(index.Operand);

                return node;
            }

            using var _ = isProperties.Set(true);
            foreach (var expr in node.Arguments)
            {
                Visit(expr);
                if (expr != node.Arguments.Last())
                    Query.Append(", ");
            }
            return node;
        }

        protected override Expression VisitConstant(ConstantExpression node)
        {
            var parameterName = $"p_{Parameters.Count}";
            Query.Append($"${parameterName}");
            Parameters[parameterName] = node.Value;
            return node;
        }

        protected override Expression VisitParameter(ParameterExpression node)
        {
            Query.Append(node.Name);
            return node;
        }
    }

}
