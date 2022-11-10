﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Weknow.GraphDbCommands;

namespace Weknow.GraphDbCommands;

/// <summary>
/// Constraint options
/// </summary>
/// <example>
/// <![CDATA[
/// CREATE FULLTEXT INDEX index_name
/// FOR()-[r: KNOWS]-() ON EACH[r.info, r.note]
/// OPTIONS
/// {
///     indexConfig: {
///         `fulltext.analyzer`: 'english',
///     },
/// }
/// 
/// CREATE FULLTEXT INDEX index_name
/// FOR()-[r: KNOWS]-() ON EACH[r.info, r.note]
/// OPTIONS
/// {
///     indexConfig: {
///         `spatial.wgs-84.min`: [-100.0, -100.0],
///         `spatial.wgs-84.max`: [100.0, 100.0]    /// 
///     },
///     indexProvider: 'native-btree-1.0'
/// }
/// ]]>
/// </example>
public class ConstraintOptions
{
    /// <summary>
    /// Full-text analyzer.
    /// </summary>
    public FullTextLanguage? FulltextAnalyzer { get; init; }
    //public string IndexProvider { get; init; }

    ///// <summary>
    ///// Spatial WGS84.
    ///// </summary>
    //public MinMax SpatialWgs84 { get; init; }

    //public record MinMax
    //{
    //    public (double from, double to) Min{ get; init; }
    //    public (double from, double to) Max{ get; init; }
    //}

}