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
            public static string Count() => $"count(*)";

            /// <summary>
            /// Count The number of non-null values..
            /// </summary>
            /// <param name="variable">The variable.</param>
            /// <returns></returns>
            /// <example>count(variable)</example>
            public static string Count(string variable) => $"count({variable})";


            #endregion // Count

            #region CountDistinct

            /// <summary>
            /// List from the values, ignores null,
            /// also take the DISTINCT operator, which removes duplicates from the values.
            /// </summary>
            /// <returns></returns>
            /// <example>count(*)</example>
            public static string CountDistinct() => $"count(DISTINCT *)";

            /// <summary>
            /// Count The number of non-null values,
            /// also take the DISTINCT operator, which removes duplicates from the values.
            /// </summary>
            /// <param name="variable">The variable.</param>
            /// <returns></returns>
            /// <example>count(variable)</example>
            public static string CountDistinct(string variable) => $"count(DISTINCT {variable})";


            #endregion // CountDistinct

            #region Collect

            /// <summary>
            /// Collect list from the values, ignores null.
            /// </summary>
            /// <param name="expression">The expression.</param>
            /// <returns></returns>
            /// <example>collect(n.property)</example>
            public static string Collect(string expression) => $"collect({expression})";

            /// <summary>
            /// Collect list from the values, ignores null.
            /// </summary>
            /// <param name="expression">The expression.</param>
            /// <returns></returns>
            /// <example>
            /// Collect<Foo>(f => f.Name)
            /// collect(f.Name)
            /// </example>
            public static string Collect<T>(Expression<Func<T, dynamic>> expression)
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
            public static string CollectDistinct(string expression) => $"collect(ISTINCT {expression})";

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
            public static string CollectDistinct<T>(Expression<Func<T, dynamic>> expression)
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
            public static string Sum(string expression) => $"sum({expression})";

            /// <summary>
            /// Sum numerical values. Similar functions are avg(), min(), max().
            /// </summary>
            /// <param name="expression">The expression.</param>
            /// <returns></returns>
            /// <example>
            /// Sum<Foo>(f => f.Name)
            /// sum(f.Name)
            /// </example>
            public static string Sum<T>(Expression<Func<T, dynamic>> expression)
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
            public static string SumDistinct(string expression) => $"sum(DISTINCT {expression})";

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
            public static string SumDistinct<T>(Expression<Func<T, dynamic>> expression)
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
            public static string Avg(string expression) => $"avg({expression})";

            /// <summary>
            /// Average numerical values. Similar functions are sum(), min(), max().
            /// </summary>
            /// <param name="expression">The expression.</param>
            /// <returns></returns>
            /// <example>
            /// Avg<Foo>(f => f.Name)
            /// avg(f.Name)
            /// </example>
            public static string Avg<T>(Expression<Func<T, dynamic>> expression)
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
            public static string AvgDistinct(string expression) => $"avg(DISTINCT {expression})";

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
            public static string AvgDistinct<T>(Expression<Func<T, dynamic>> expression)
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
            public static string Min(string expression) => $"min({expression})";

            /// <summary>
            /// Min numerical values. Similar functions are avg(), min(), max().
            /// </summary>
            /// <param name="expression">The expression.</param>
            /// <returns></returns>
            /// <example>
            /// Min<Foo>(f => f.Name)
            /// min(f.Name)
            /// </example>
            public static string Min<T>(Expression<Func<T, dynamic>> expression)
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
            public static string MinDistinct(string expression) => $"min(DISTINCT {expression})";

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
            public static string MinDistinct<T>(Expression<Func<T, dynamic>> expression)
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
            public static string Max(string expression) => $"max({expression})";

            /// <summary>
            /// Max numerical values. Similar functions are avg(), max(), min().
            /// </summary>
            /// <param name="expression">The expression.</param>
            /// <returns></returns>
            /// <example>
            /// Max<Foo>(f => f.Name)
            /// max(f.Name)
            /// </example>
            public static string Max<T>(Expression<Func<T, dynamic>> expression)
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
            public static string MaxDistinct(string expression) => $"maxDistinct(DISTINCT {expression})";

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
            public static string MaxDistinct<T>(Expression<Func<T, dynamic>> expression)
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
            public static string PercentileDisc(string expression, double percentile) => $"percentileDisc({expression}, percentile)";

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
            public static string PercentileDisc<T>(Expression<Func<T, dynamic>> expression,  double percentile)
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
            public static string PercentileDiscDistinct(string expression, double percentile) => $"percentileDisc(Distinct {expression}, percentile)";

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
            public static string PercentileDiscDistinct<T>(Expression<Func<T, dynamic>> expression,  double percentile)
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
            public static string PercentileCount(string expression, double percentile) => $"percentileCont({expression}, percentile)";

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
            public static string PercentileCount<T>(Expression<Func<T, dynamic>> expression,  double percentile)
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
            public static string PercentileCountDistinct(string expression, double percentile) => $"percentileCont(Distinct {expression}, percentile)";

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
            public static string PercentileCountDistinct<T>(Expression<Func<T, dynamic>> expression,  double percentile)
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
            public static string StDev(string expression) => $"stDev({expression})";

            /// <summary>
            /// Standard deviation for a sample of a population. For an entire population use stDevP().
            /// </summary>
            /// <param name="expression">The expression.</param>
            /// <returns></returns>
            /// <example>
            /// StDev<Foo>(f => f.Name)
            /// stDev(f.Name)
            /// </example>
            public static string StDev<T>(Expression<Func<T, dynamic>> expression)
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
            public static string StDevDistinct(string expression) => $"stDev(DISTINCT {expression})";

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
            public static string StDevDistinct<T>(Expression<Func<T, dynamic>> expression)
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
            public static string StDevP(string expression) => $"stDevP({expression})";

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
            public static string StDevP<T>(Expression<Func<T, dynamic>> expression)
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
            public static string StDevPDistinct(string expression) => $"stDevP(DISTINCT {expression})";

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
            public static string StDevPDistinct<T>(Expression<Func<T, dynamic>> expression)
            {
                (string variable, string name) = ExtractLambdaExpression(expression);
                return StDevPDistinct($"{variable}.{name}");
            }

            #endregion // StDevPDistinct
        }
    }
}