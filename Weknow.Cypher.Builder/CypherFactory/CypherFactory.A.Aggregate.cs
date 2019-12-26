// https://neo4j.com/docs/cypher-refcard/current/
// https://github.com/Readify/Neo4jClient
// https://github.com/Readify/Neo4jClient/wiki/cypher-examples
// https://neo4jmapper.tk/guide.html
// https://github.com/barnardos-au/Neo4jMapper

// TODO: main phrases + prop setup + where

#pragma warning disable RCS1102 // Make class static.

using System;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Reflection;
using System.Linq;
using System.Collections.Generic;
using static Weknow.Helpers.Helper;
using System.Collections;

namespace Weknow
{
    partial class CypherFactory
    {
        /// <summary>
        /// Cypher Aggregate Function
        /// </summary>
        public class A : Aggregate { }
        /// <summary>
        /// Cypher Aggregate Function
        /// </summary>
        public class Aggregate
        {
            #region Count

            /// <summary>
            /// List from the values, ignores null.
            /// </summary>
            /// <returns></returns>
            /// <example>count(*)</example>
            public static FluentCypher Count() => CypherBuilder.Default.Add(CypherBuilder.Default.Add($"count(*)")); 

            /// <summary>
            /// Count The number of non-null values..
            /// </summary>
            /// <param name="variable">The variable.</param>
            /// <returns></returns>
            /// <example>count(variable)</example>
            public static FluentCypher Count(string variable) => CypherBuilder.Default.Add(CypherBuilder.Default.Add($"count({variable})")); 


            #endregion // Count

            #region CountDistinct

            /// <summary>
            /// List from the values, ignores null,
            /// also take the DISTINCT operator, which removes duplicates from the values.
            /// </summary>
            /// <returns></returns>
            /// <example>count(*)</example>
            public static FluentCypher CountDistinct() => CypherBuilder.Default.Add(CypherBuilder.Default.Add($"count(DISTINCT *)")); 

            /// <summary>
            /// Count The number of non-null values,
            /// also take the DISTINCT operator, which removes duplicates from the values.
            /// </summary>
            /// <param name="variable">The variable.</param>
            /// <returns></returns>
            /// <example>count(variable)</example>
            public static FluentCypher CountDistinct(string variable) => CypherBuilder.Default.Add(CypherBuilder.Default.Add($"count(DISTINCT {variable})")); 


            #endregion // CountDistinct

            #region Collect

            /// <summary>
            /// Collect list from the values, ignores null.
            /// </summary>
            /// <param name="expression">The expression.</param>
            /// <returns></returns>
            /// <example>collect(n.property)</example>
            public static FluentCypher Collect(string expression) => CypherBuilder.Default.Add(CypherBuilder.Default.Add($"collect({expression})"));

            /// <summary>
            /// Collect list from the values, ignores null.
            /// </summary>
            /// <param name="expression">The expression.</param>
            /// <returns></returns>
            /// <example>collect(n.property)</example>
            public static FluentCypher Collect(string variable, string asName) => new CypherBuilder($"collect({variable}", CypherPhrase.None, $") AS {asName}");

            /// <summary>
            /// Collect list from the values, ignores null.
            /// </summary>
            /// <param name="expression">The expression.</param>
            /// <returns></returns>
            /// <example>
            /// Collect<Foo>(f => f.Name)
            /// collect(f.Name)
            /// </example>
            public static FluentCypher Collect<T>(Expression<Func<T, dynamic>> expression)
            {
                (string variable, string name) = ExtractLambdaExpression(expression);
                return Collect($"{variable}.{name}");
            }

            #endregion // Collect

            #region CollectDistinct

            /// <summary>
            /// CollectDistinct list from the values, ignores null,
            /// also take the DISTINCT operator, which removes duplicates from the values.
            /// </summary>
            /// <param name="expression">The expression.</param>
            /// <returns></returns>
            /// <example>collect(n.property)</example>
            public static FluentCypher CollectDistinct(string expression) => CypherBuilder.Default.Add(CypherBuilder.Default.Add($"collect(DISTINCT {expression})")); 

            /// <summary>
            /// CollectDistinct list from the values, ignores null,
            /// also take the DISTINCT operator, which removes duplicates from the values.
            /// </summary>
            /// <param name="expression">The expression.</param>
            /// <returns></returns>
            /// <example>
            /// CollectDistinct<Foo>(f => f.Name)
            /// collect(f.Name)
            /// </example>
            public static FluentCypher CollectDistinct<T>(Expression<Func<T, dynamic>> expression)
            {
                (string variable, string name) = ExtractLambdaExpression(expression);
                return CollectDistinct($"{variable}.{name}");
            }

            #endregion // CollectDistinct

            #region Sum

            /// <summary>
            /// Sum numerical values. Similar functions are avg(), min(), max().
            /// </summary>
            /// <param name="expression">The expression.</param>
            /// <returns></returns>
            /// <example>sum(n.property)</example>
            public static FluentCypher Sum(string expression) => CypherBuilder.Default.Add(CypherBuilder.Default.Add($"sum({expression})")); 

            /// <summary>
            /// Sum numerical values. Similar functions are avg(), min(), max().
            /// </summary>
            /// <param name="expression">The expression.</param>
            /// <returns></returns>
            /// <example>
            /// Sum<Foo>(f => f.Name)
            /// sum(f.Name)
            /// </example>
            public static FluentCypher Sum<T>(Expression<Func<T, dynamic>> expression)
            {
                (string variable, string name) = ExtractLambdaExpression(expression);
                return Sum($"{variable}.{name}");
            }

            #endregion // Sum

            #region SumDistinct

            /// <summary>
            /// SumDistinct numerical values. Similar functions are avg(), min(), max(),
            /// also take the DISTINCT operator, which removes duplicates from the values.
            /// </summary>
            /// <param name="expression">The expression.</param>
            /// <returns></returns>
            /// <example>sum(n.property)</example>
            public static FluentCypher SumDistinct(string expression) => CypherBuilder.Default.Add(CypherBuilder.Default.Add($"sum(DISTINCT {expression})")); 

            /// <summary>
            /// SumDistinct numerical values. Similar functions are avg(), min(), max(),
            /// also take the DISTINCT operator, which removes duplicates from the values.
            /// </summary>
            /// <param name="expression">The expression.</param>
            /// <returns></returns>
            /// <example>
            /// SumDistinct<Foo>(f => f.Name)
            /// sum(f.Name)
            /// </example>
            public static FluentCypher SumDistinct<T>(Expression<Func<T, dynamic>> expression)
            {
                (string variable, string name) = ExtractLambdaExpression(expression);
                return SumDistinct($"{variable}.{name}");
            }

            #endregion // SumDistinct

            #region Avg

            /// <summary>
            /// Average numerical values. Similar functions are sum(), min(), max().
            /// </summary>
            /// <param name="expression">The expression.</param>
            /// <returns></returns>
            /// <example>avg(n.property)</example>
            public static FluentCypher Avg(string expression) => CypherBuilder.Default.Add(CypherBuilder.Default.Add($"avg({expression})")); 

            /// <summary>
            /// Average numerical values. Similar functions are sum(), min(), max().
            /// </summary>
            /// <param name="expression">The expression.</param>
            /// <returns></returns>
            /// <example>
            /// Avg<Foo>(f => f.Name)
            /// avg(f.Name)
            /// </example>
            public static FluentCypher Avg<T>(Expression<Func<T, dynamic>> expression)
            {
                (string variable, string name) = ExtractLambdaExpression(expression);
                return Avg($"{variable}.{name}");
            }

            #endregion // Avg

            #region AvgDistinct

            /// <summary>
            /// Average numerical values. Similar functions are sum(), min(), max(),
            /// also take the DISTINCT operator, which removes duplicates from the values.
            /// </summary>
            /// <param name="expression">The expression.</param>
            /// <returns></returns>
            /// <example>avg(n.property)</example>
            public static FluentCypher AvgDistinct(string expression) => CypherBuilder.Default.Add(CypherBuilder.Default.Add($"avg(DISTINCT {expression})")); 

            /// <summary>
            /// Average numerical values. Similar functions are sum(), min(), max(),
            /// also take the DISTINCT operator, which removes duplicates from the values.
            /// </summary>
            /// <param name="expression">The expression.</param>
            /// <returns></returns>
            /// <example>
            /// AvgDistinct<Foo>(f => f.Name)
            /// avg(f.Name)
            /// </example>
            public static FluentCypher AvgDistinct<T>(Expression<Func<T, dynamic>> expression)
            {
                (string variable, string name) = ExtractLambdaExpression(expression);
                return AvgDistinct($"{variable}.{name}");
            }

            #endregion // AvgDistinct

            #region Min

            /// <summary>
            /// Min numerical values. Similar functions are avg(), sum(), max().
            /// </summary>
            /// <param name="expression">The expression.</param>
            /// <returns></returns>
            /// <example>min(n.property)</example>
            public static FluentCypher Min(string expression) => CypherBuilder.Default.Add(CypherBuilder.Default.Add($"min({expression})")); 

            /// <summary>
            /// Min numerical values. Similar functions are avg(), min(), max().
            /// </summary>
            /// <param name="expression">The expression.</param>
            /// <returns></returns>
            /// <example>
            /// Min<Foo>(f => f.Name)
            /// min(f.Name)
            /// </example>
            public static FluentCypher Min<T>(Expression<Func<T, dynamic>> expression)
            {
                (string variable, string name) = ExtractLambdaExpression(expression);
                return Min($"{variable}.{name}");
            }

            #endregion // Min

            #region MinDistinct

            /// <summary>
            /// MinDistinct numerical values. Similar functions are avg(), sum(), max(),
            /// also take the DISTINCT operator, which removes duplicates from the values.
            /// </summary>
            /// <param name="expression">The expression.</param>
            /// <returns></returns>
            /// <example>min(n.property)</example>
            public static FluentCypher MinDistinct(string expression) => CypherBuilder.Default.Add(CypherBuilder.Default.Add($"min(DISTINCT {expression})"));   

            /// <summary>
            /// MinDistinct numerical values. Similar functions are avg(), min(), max(),
            /// also take the DISTINCT operator, which removes duplicates from the values.
            /// </summary>
            /// <param name="expression">The expression.</param>
            /// <returns></returns>
            /// <example>
            /// MinDistinct<Foo>(f => f.Name)
            /// min(f.Name)
            /// </example>
            public static FluentCypher MinDistinct<T>(Expression<Func<T, dynamic>> expression)
            {
                (string variable, string name) = ExtractLambdaExpression(expression);
                return MinDistinct($"{variable}.{name}");
            }

            #endregion // MinDistinct

            #region Max

            /// <summary>
            /// Max numerical values. Similar functions are avg(), sum(), min().
            /// </summary>
            /// <param name="expression">The expression.</param>
            /// <returns></returns>
            /// <example>max(n.property)</example>
            public static FluentCypher Max(string expression) => CypherBuilder.Default.Add($"max({expression})"); 

            /// <summary>
            /// Max numerical values. Similar functions are avg(), max(), min().
            /// </summary>
            /// <param name="expression">The expression.</param>
            /// <returns></returns>
            /// <example>
            /// Max<Foo>(f => f.Name)
            /// max(f.Name)
            /// </example>
            public static FluentCypher Max<T>(Expression<Func<T, dynamic>> expression)
            {
                (string variable, string name) = ExtractLambdaExpression(expression);
                return Max($"{variable}.{name}");
            }

            #endregion // Max

            #region MaxDistinct

            /// <summary>
            /// MaxDistinct numerical values. Similar functions are avg(), sum(), min(),
            /// also take the DISTINCT operator, which removes duplicates from the values.
            /// </summary
            /// <param name="expression">The expression.</param>
            /// <returns></returns>
            /// <example>maxDistinct(n.property)</example>
            public static FluentCypher MaxDistinct(string expression) => CypherBuilder.Default.Add($"max(DISTINCT {expression})"); 

            /// <summary>
            /// MaxDistinct numerical values. Similar functions are avg(), maxDistinct(), min(),
            /// also take the DISTINCT operator, which removes duplicates from the values.
            /// </summary>
            /// <param name="expression">The expression.</param>
            /// <returns></returns>
            /// <example>
            /// MaxDistinct<Foo>(f => f.Name)
            /// maxDistinct(f.Name)
            /// </example>
            public static FluentCypher MaxDistinct<T>(Expression<Func<T, dynamic>> expression)
            {
                (string variable, string name) = ExtractLambdaExpression(expression);
                return MaxDistinct($"{variable}.{name}");
            }

            #endregion // MaxDistinct

            #region PercentileDisc

            /// <summary>
            /// Discrete percentile. Continuous percentile is percentileCont().
            /// The percentile argument is from 0.0 to 1.0.
            /// </summary>
            /// <param name="expression">The expression.</param>
            /// <param name="percentile">he percentile argument is from 0.0 to 1.0.</param>
            /// <returns></returns>
            /// <example>percentileDisc(n.property, $percentile)</example>
            public static FluentCypher PercentileDisc(string expression, double percentile) => CypherBuilder.Default.Add($"percentileDisc({expression}, {percentile})"); 

            /// <summary>
            /// Discrete percentile. Continuous percentile is percentileCont().
            /// The percentile argument is from 0.0 to 1.0.
            /// </summary>
            /// <param name="expression">The expression.</param>
            /// <param name="percentile">he percentile argument is from 0.0 to 1.0.</param>
            /// <returns></returns>
            /// <example>
            /// PercentileDisc<Foo>(f => f.Name)
            /// percentileDisc(f.Name, 0.8)
            /// </example>
            public static FluentCypher PercentileDisc<T>(Expression<Func<T, dynamic>> expression,  double percentile)
            {
                (string variable, string name) = ExtractLambdaExpression(expression);
                return PercentileDisc($"{variable}.{name}", percentile);
            }

            #endregion // PercentileDisc

            #region PercentileDiscDistinct

            /// <summary>
            /// Discrete percentile. Continuous percentile is percentileCont().
            /// The percentile argument is from 0.0 to 1.0.
            /// </summary>
            /// <param name="expression">The expression.</param>
            /// <param name="percentile">he percentile argument is from 0.0 to 1.0.</param>
            /// <returns></returns>
            /// <example>percentileDisc(n.property, $percentile)</example>
            public static FluentCypher PercentileDiscDistinct(string expression, double percentile) => CypherBuilder.Default.Add($"percentileDisc(DISTINCT {expression}, {percentile})"); 

            /// <summary>
            /// Discrete percentile. Continuous percentile is percentileCont().
            /// The percentile argument is from 0.0 to 1.0.
            /// </summary>
            /// <param name="expression">The expression.</param>
            /// <param name="percentile">he percentile argument is from 0.0 to 1.0.</param>
            /// <returns></returns>
            /// <example>
            /// PercentileDiscDistinct<Foo>(f => f.Name)
            /// percentileDisc(f.Name, 0.8)
            /// </example>
            public static FluentCypher PercentileDiscDistinct<T>(Expression<Func<T, dynamic>> expression,  double percentile)
            {
                (string variable, string name) = ExtractLambdaExpression(expression);
                return PercentileDiscDistinct($"{variable}.{name}", percentile);
            }

            #endregion // PercentileDiscDistinct

            #region PercentileCount

            /// <summary>
            /// percentileCont() returns the percentile 
            /// of the given value over a group, with a percentile from 0.0 to 1.0. 
            /// It uses a linear interpolation method, 
            /// calculating a weighted average between two values 
            /// if the desired percentile lies between them. 
            /// For nearest values using a rounding method, see percentileDisc.
            /// </summary>
            /// <param name="expression">The expression.</param>
            /// <param name="percentile">he percentile argument is from 0.0 to 1.0.</param>
            /// <returns></returns>
            /// <example>percentileCont(expression, percentile)</example>
            public static FluentCypher PercentileCount(string expression, double percentile) => CypherBuilder.Default.Add($"percentileCount({expression}, {percentile})"); 

            /// <summary>
            /// percentileCont() returns the percentile 
            /// of the given value over a group, with a percentile from 0.0 to 1.0. 
            /// It uses a linear interpolation method, 
            /// calculating a weighted average between two values 
            /// if the desired percentile lies between them. 
            /// For nearest values using a rounding method, see percentileDisc.
            /// The percentile argument is from 0.0 to 1.0.
            /// </summary>
            /// <param name="expression">The expression.</param>
            /// <param name="percentile">he percentile argument is from 0.0 to 1.0.</param>
            /// <returns></returns>
            /// <example>
            /// PercentileCont<Foo>(f => f.Name, 0.8)
            /// percentileCont(f.Name, 0.8)
            /// </example>
            public static FluentCypher PercentileCount<T>(Expression<Func<T, dynamic>> expression,  double percentile)
            {
                (string variable, string name) = ExtractLambdaExpression(expression);
                return PercentileCount($"{variable}.{name}", percentile);
            }

            #endregion // PercentileCount

            #region PercentileCountDistinct

            /// <summary>
            /// percentileCont() returns the percentile 
            /// of the given value over a group, with a percentile from 0.0 to 1.0. 
            /// It uses a linear interpolation method, 
            /// calculating a weighted average between two values 
            /// if the desired percentile lies between them. 
            /// For nearest values using a rounding method, see percentileDisc.
            /// also take the DISTINCT operator, which removes duplicates from the values.
            /// </summary>
            /// <param name="expression">The expression.</param>
            /// <param name="percentile">he percentile argument is from 0.0 to 1.0.</param>
            /// <returns></returns>
            /// <example>percentileCont(expression, percentile)</example>
            public static FluentCypher PercentileCountDistinct(string expression, double percentile) => CypherBuilder.Default.Add($"percentileCount(DISTINCT {expression}, {percentile})"); 

            /// <summary>
            /// percentileCont() returns the percentile 
            /// of the given value over a group, with a percentile from 0.0 to 1.0. 
            /// It uses a linear interpolation method, 
            /// calculating a weighted average between two values 
            /// if the desired percentile lies between them. 
            /// For nearest values using a rounding method, see percentileDisc.
            /// The percentile argument is from 0.0 to 1.0.
            /// also take the DISTINCT operator, which removes duplicates from the values.
            /// </summary>
            /// <param name="expression">The expression.</param>
            /// <param name="percentile">he percentile argument is from 0.0 to 1.0.</param>
            /// <returns></returns>
            /// <example>
            /// PercentileCont<Foo>(f => f.Name, 0.8)
            /// percentileCont(f.Name, 0.8)
            /// </example>
            public static FluentCypher PercentileCountDistinct<T>(Expression<Func<T, dynamic>> expression,  double percentile)
            {
                (string variable, string name) = ExtractLambdaExpression(expression);
                return PercentileCountDistinct($"{variable}.{name}", percentile);
            }

            #endregion // PercentileCountDistinct

            #region StDev

            /// <summary>
            /// Standard deviation for a sample of a population. For an entire population use stDevP().
            /// </summary>
            /// <param name="expression">The expression.</param>
            /// <returns></returns>
            /// <example>stDev(n.property)</example>
            public static FluentCypher StDev(string expression) => CypherBuilder.Default.Add($"stDev({expression})"); 

            /// <summary>
            /// Standard deviation for a sample of a population. For an entire population use stDevP().
            /// </summary>
            /// <param name="expression">The expression.</param>
            /// <returns></returns>
            /// <example>
            /// StDev<Foo>(f => f.Name)
            /// stDev(f.Name)
            /// </example>
            public static FluentCypher StDev<T>(Expression<Func<T, dynamic>> expression)
            {
                (string variable, string name) = ExtractLambdaExpression(expression);
                return StDev($"{variable}.{name}");
            }

            #endregion // StDev

            #region StDevDistinct

            /// <summary>
            /// Standard deviation for a sample of a population. For an entire population use stDevP().
            /// also take the DISTINCT operator, which removes duplicates from the values.
            /// </summary>
            /// <param name="expression">The expression.</param>
            /// <returns></returns>
            /// <example>stDev(n.property)</example>
            public static FluentCypher StDevDistinct(string expression) => CypherBuilder.Default.Add($"stDev(DISTINCT {expression})"); 

            /// <summary>
            /// Standard deviation for a sample of a population. For an entire population use stDevP().
            /// also take the DISTINCT operator, which removes duplicates from the values.
            /// </summary>
            /// <param name="expression">The expression.</param>
            /// <returns></returns>
            /// <example>
            /// StDevDistinct<Foo>(f => f.Name)
            /// stDev(f.Name)
            /// </example>
            public static FluentCypher StDevDistinct<T>(Expression<Func<T, dynamic>> expression)
            {
                (string variable, string name) = ExtractLambdaExpression(expression);
                return StDevDistinct($"{variable}.{name}");
            }

            #endregion // StDevDistinct

            #region StDevP

            /// <summary>
            /// stDevP() returns the standard deviation for the given value over a group. 
            /// It uses a standard two-pass method, with N as the denominator, 
            /// and should be used when calculating the standard deviation for an entire population. 
            /// When the standard variation of only a sample of the population is being calculated, 
            /// stDev should be used.
            /// </summary>
            /// <param name="expression">The expression.</param>
            /// <returns></returns>
            /// <example>stDevP(n.property)</example>
            public static FluentCypher StDevP(string expression) => CypherBuilder.Default.Add($"stDevP({expression})"); 

            /// <summary>
            /// stDevP() returns the standard deviation for the given value over a group. 
            /// It uses a standard two-pass method, with N as the denominator, 
            /// and should be used when calculating the standard deviation for an entire population. 
            /// When the standard variation of only a sample of the population is being calculated, 
            /// stDev should be used.
            /// </summary>
            /// <param name="expression">The expression.</param>
            /// <returns></returns>
            /// <example>
            /// StDevP<Foo>(f => f.Name)
            /// stDevP(f.Name)
            /// </example>
            public static FluentCypher StDevP<T>(Expression<Func<T, dynamic>> expression)
            {
                (string variable, string name) = ExtractLambdaExpression(expression);
                return StDevP($"{variable}.{name}");
            }

            #endregion // StDevP

            #region StDevPDistinct

            /// <summary>
            /// stDevP() returns the standard deviation for the given value over a group. 
            /// It uses a standard two-pass method, with N as the denominator, 
            /// and should be used when calculating the standard deviation for an entire population. 
            /// When the standard variation of only a sample of the population is being calculated, 
            /// stDev should be used.
            /// also take the DISTINCT operator, which removes duplicates from the values.
            /// </summary>
            /// <param name="expression">The expression.</param>
            /// <returns></returns>
            /// <example>stDevP(n.property)</example>
            public static FluentCypher StDevPDistinct(string expression) => CypherBuilder.Default.Add($"stDevP(DISTINCT {expression})"); 

            /// <summary>
            /// stDevP() returns the standard deviation for the given value over a group. 
            /// It uses a standard two-pass method, with N as the denominator, 
            /// and should be used when calculating the standard deviation for an entire population. 
            /// When the standard variation of only a sample of the population is being calculated, 
            /// stDev should be used.
            /// also take the DISTINCT operator, which removes duplicates from the values.
            /// </summary>
            /// <param name="expression">The expression.</param>
            /// <returns></returns>
            /// <example>
            /// StDevPDistinct<Foo>(f => f.Name)
            /// stDevP(f.Name)
            /// </example>
            public static FluentCypher StDevPDistinct<T>(Expression<Func<T, dynamic>> expression)
            {
                (string variable, string name) = ExtractLambdaExpression(expression);
                return StDevPDistinct($"{variable}.{name}");
            }

            #endregion // StDevPDistinct
        }
    }
}