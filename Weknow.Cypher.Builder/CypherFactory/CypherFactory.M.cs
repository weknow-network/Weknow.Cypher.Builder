// https://neo4j.com/docs/cypher-refcard/current/
// https://github.com/Readify/Neo4jClient
// https://github.com/Readify/Neo4jClient/wiki/cypher-examples
// https://neo4jmapper.tk/guide.html
// https://github.com/barnardos-au/Neo4jMapper

// TODO: main phrases + prop setup + where

using System;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Reflection;
using System.Linq;
using System.Collections.Generic;
using static Weknow.Helpers.Helper;
using System.Collections;

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
            public static string Abs(string expression) => $"abs({expression})";

            #endregion // Abs

            #region Rand

            /// <summary>
            /// Returns a random number
            /// in the range from 0 (inclusive) to 1 (exclusive), [0,1).
            /// Returns a new value for each call.
            /// Also useful for selecting a subset or random ordering.
            /// </summary>
            /// <returns></returns>
            public static string Rand() => $"rand()";

            #endregion // Rand

            #region Round

            /// <summary>
            /// Round to the nearest integer; 
            /// ceil() and floor() find the next integer up or down.
            /// </summary>
            /// <param name="expression">The expression.</param>
            /// <returns></returns>
            public static string Round(string expression) => $"round({expression})";

            #endregion // Round

            #region Sqrt

            /// <summary>
            /// The square root.
            /// </summary>
            /// <param name="expression">The expression.</param>
            /// <returns></returns>
            public static string Sqrt(string expression) => $"sqrt({expression})";

            #endregion // Sqrt

            #region Sign

            /// <summary>
            /// 0 if zero, -1 if negative, 1 if positive.
            /// </summary>
            /// <param name="expression">The expression.</param>
            /// <returns></returns>
            public static string Sign(string expression) => $"sign({expression})";

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
            public static string Sin(string expression) => $"sin({expression})";

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
            public static string Cos(string expression) => $"cos({expression})";

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
            public static string Tan(string expression) => $"tan({expression})";

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
            public static string Cot(string expression) => $"cot({expression})";

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
            public static string Asin(string expression) => $"asin({expression})";

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
            public static string Acos(string expression) => $"acos({expression})";

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
            public static string Atan(string expression) => $"atan({expression})";

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
            public static string Atan2(string expression) => $"atan2({expression})";

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
            public static string Haversin(string expression) => $"haversin({expression})";

            #endregion // Haversin

            #region Radians

            /// <summary>
            /// Converts radians into degrees; use radians() for the reverse, and pi() for π.
            /// </summary>
            /// <param name="expression">The expression.</param>
            /// <returns></returns>
            public static string Radians(string expression) => $"radians({expression})";

            #endregion // Radians

            #region Pi

            /// <summary>
            /// Converts radians into degrees; use radians() for the reverse, and pi() for π.
            /// </summary>
            /// <param name="expression">The expression.</param>
            /// <returns></returns>
            public static string Pi(string expression) => $"pi()";

            #endregion // Pi

            #region Log10

            /// <summary>
            /// Logarithm base 10, natural logarithm, e to the power of the parameter, and the value of e.
            /// </summary>
            /// <param name="expression">The expression.</param>
            /// <returns></returns>
            public static string Log10(string expression) => $"log10({expression})";

            #endregion // Log10

            #region Log

            /// <summary>
            /// Logarithm base 10, natural logarithm, e to the power of the parameter, and the value of e.
            /// </summary>
            /// <param name="expression">The expression.</param>
            /// <returns></returns>
            public static string Log(string expression) => $"log({expression})";

            #endregion // Log

            #region Exp

            /// <summary>
            /// Logarithm base 10, natural logarithm, e to the power of the parameter, and the value of e.
            /// </summary>
            /// <param name="expression">The expression.</param>
            /// <returns></returns>
            public static string Exp(string expression) => $"exp({expression})";

            #endregion // Exp

            #region E

            /// <summary>
            /// Converts radians into degrees; use radians() for the reverse, and pi() for π.
            /// </summary>
            /// <returns></returns>
            public static string E() => $"e()";

            #endregion // E
        }
    }
}
