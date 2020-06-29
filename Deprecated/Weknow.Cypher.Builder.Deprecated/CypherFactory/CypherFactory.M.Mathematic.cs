// https://neo4j.com/docs/cypher-refcard/current/
// https://github.com/Readify/Neo4jClient
// https://github.com/Readify/Neo4jClient/wiki/cypher-examples
// https://neo4jmapper.tk/guide.html
// https://github.com/barnardos-au/Neo4jMapper

// TODO: main phrases + prop setup + where


#pragma warning disable RCS1102 // Make class static.

namespace Weknow
{
    partial class CypherFactory
    {
        /// <summary>
        /// Cypher Mathematical Functions 
        /// </summary>
        public class M : Mathematic { }
        /// <summary>
        /// Cypher Mathematical Functions 
        /// </summary>
        public class Mathematic
        {
            #region Abs

            /// <summary>
            /// The absolute value.
            /// </summary>
            /// <param name="expression">The expression.</param>
            /// <returns></returns>
            public static FluentCypher Abs(string expression) => CypherBuilder.Default.Add($"abs({expression})");

            #endregion // Abs

            #region Rand

            /// <summary>
            /// Returns a random number
            /// in the range from 0 (inclusive) to 1 (exclusive), [0,1).
            /// Returns a new value for each call.
            /// Also useful for selecting a subset or random ordering.
            /// </summary>
            /// <returns></returns>
            public static FluentCypher Rand() => CypherBuilder.Default.Add($"rand()");

            #endregion // Rand

            #region Round

            /// <summary>
            /// Round to the nearest integer; 
            /// ceil() and floor() find the next integer up or down.
            /// </summary>
            /// <param name="expression">The expression.</param>
            /// <returns></returns>
            public static FluentCypher Round(string expression) => CypherBuilder.Default.Add($"round({expression})");

            #endregion // Round

            #region Sqrt

            /// <summary>
            /// The square root.
            /// </summary>
            /// <param name="expression">The expression.</param>
            /// <returns></returns>
            public static FluentCypher Sqrt(string expression) => CypherBuilder.Default.Add($"sqrt({expression})");

            #endregion // Sqrt

            #region Sign

            /// <summary>
            /// 0 if zero, -1 if negative, 1 if positive.
            /// </summary>
            /// <param name="expression">The expression.</param>
            /// <returns></returns>
            public static FluentCypher Sign(string expression) => CypherBuilder.Default.Add($"sign({expression})");

            #endregion // Sign

            #region Sin

            /// <summary>
            /// Trigonometric functions also include 
            /// cos(), tan(), cot(), asin(), acos(), atan(), atan2(), 
            /// and haversin(). All arguments for the trigonometric functions 
            /// should be in radians, if not otherwise specified.
            /// </summary>
            /// <param name="expression">The expression.</param>
            /// <returns></returns>
            public static FluentCypher Sin(string expression) => CypherBuilder.Default.Add($"sin({expression})");

            #endregion // Sin

            #region Cos

            /// <summary>
            /// Trigonometric functions also include 
            /// sin(), cos(), tan(), cot(), asin(), acos(), atan(), atan2(), 
            /// and haversin(). All arguments for the trigonometric functions 
            /// should be in radians, if not otherwise specified.
            /// </summary>
            /// <param name="expression">The expression.</param>
            /// <returns></returns>
            public static FluentCypher Cos(string expression) => CypherBuilder.Default.Add($"cos({expression})");

            #endregion // Cos

            #region Tan

            /// <summary>
            /// Trigonometric functions also include 
            /// sin(), cos(), tan(), cot(), asin(), acos(), atan(), atan2(), 
            /// and haversin(). All arguments for the trigonometric functions 
            /// should be in radians, if not otherwise specified.
            /// </summary>
            /// <param name="expression">The expression.</param>
            /// <returns></returns>
            public static FluentCypher Tan(string expression) => CypherBuilder.Default.Add($"tan({expression})");

            #endregion // Tan

            #region Cot

            /// <summary>
            /// Trigonometric functions also include 
            /// sin(), cos(), tan(), cot(), asin(), acos(), atan(), atan2(), 
            /// and haversin(). All arguments for the trigonometric functions 
            /// should be in radians, if not otherwise specified.
            /// </summary>
            /// <param name="expression">The expression.</param>
            /// <returns></returns>
            public static FluentCypher Cot(string expression) => CypherBuilder.Default.Add($"cot({expression})");

            #endregion // Cot

            #region Asin

            /// <summary>
            /// Trigonometric functions also include 
            /// sin(), cos(), tan(), cot(), asin(), acos(), atan(), atan2(), 
            /// and haversin(). All arguments for the trigonometric functions 
            /// should be in radians, if not otherwise specified.
            /// </summary>
            /// <param name="expression">The expression.</param>
            /// <returns></returns>
            public static FluentCypher Asin(string expression) => CypherBuilder.Default.Add($"asin({expression})");

            #endregion // Asin

            #region Acos

            /// <summary>
            /// Trigonometric functions also include 
            /// sin(), cos(), tan(), cot(), asin(), acos(), atan(), atan2(), 
            /// and haversin(). All arguments for the trigonometric functions 
            /// should be in radians, if not otherwise specified.
            /// </summary>
            /// <param name="expression">The expression.</param>
            /// <returns></returns>
            public static FluentCypher Acos(string expression) => CypherBuilder.Default.Add($"acos({expression})");

            #endregion // Acos

            #region Atan

            /// <summary>
            /// Trigonometric functions also include 
            /// sin(), cos(), tan(), cot(), asin(), acos(), atan(), atan2(), 
            /// and haversin(). All arguments for the trigonometric functions 
            /// should be in radians, if not otherwise specified.
            /// </summary>
            /// <param name="expression">The expression.</param>
            /// <returns></returns>
            public static FluentCypher Atan(string expression) => CypherBuilder.Default.Add($"atan({expression})");

            #endregion // Atan

            #region Atan2

            /// <summary>
            /// Trigonometric functions also include 
            /// sin(), cos(), tan(), cot(), asin(), acos(), atan(), atan2(), 
            /// and haversin(). All arguments for the trigonometric functions 
            /// should be in radians, if not otherwise specified.
            /// </summary>
            /// <param name="expression">The expression.</param>
            /// <returns></returns>
            public static FluentCypher Atan2(string expression) => CypherBuilder.Default.Add($"atan2({expression})");

            #endregion // Atan2

            #region Haversin

            /// <summary>
            /// Trigonometric functions also include 
            /// sin(), cos(), tan(), cot(), asin(), acos(), atan(), atan2(), 
            /// and haversin(). All arguments for the trigonometric functions 
            /// should be in radians, if not otherwise specified.
            /// </summary>
            /// <param name="expression">The expression.</param>
            /// <returns></returns>
            public static FluentCypher Haversin(string expression) => CypherBuilder.Default.Add($"haversin({expression})");

            #endregion // Haversin

            #region Radians

            /// <summary>
            /// Converts radians into degrees; use radians() for the reverse, and pi() for π.
            /// </summary>
            /// <param name="expression">The expression.</param>
            /// <returns></returns>
            public static FluentCypher Radians(string expression) => CypherBuilder.Default.Add($"radians({expression})");

            #endregion // Radians

            #region Pi

            /// <summary>
            /// Converts radians into degrees; use radians() for the reverse, and pi() for π.
            /// </summary>
            /// <returns></returns>
            public static FluentCypher Pi() => CypherBuilder.Default.Add($"pi()");

            #endregion // Pi

            #region Log10

            /// <summary>
            /// Logarithm base 10, natural logarithm, e to the power of the parameter, and the value of e.
            /// </summary>
            /// <param name="expression">The expression.</param>
            /// <returns></returns>
            public static FluentCypher Log10(string expression) => CypherBuilder.Default.Add($"log10({expression})");

            #endregion // Log10

            #region Log

            /// <summary>
            /// Logarithm base 10, natural logarithm, e to the power of the parameter, and the value of e.
            /// </summary>
            /// <param name="expression">The expression.</param>
            /// <returns></returns>
            public static FluentCypher Log(string expression) => CypherBuilder.Default.Add($"log({expression})");

            #endregion // Log

            #region Exp

            /// <summary>
            /// Logarithm base 10, natural logarithm, e to the power of the parameter, and the value of e.
            /// </summary>
            /// <param name="expression">The expression.</param>
            /// <returns></returns>
            public static FluentCypher Exp(string expression) => CypherBuilder.Default.Add($"exp({expression})");

            #endregion // Exp

            #region E

            /// <summary>
            /// Converts radians into degrees; use radians() for the reverse, and pi() for π.
            /// </summary>
            /// <returns></returns>
            public static FluentCypher E() => CypherBuilder.Default.Add($"e()");

            #endregion // E
        }
    }
}
