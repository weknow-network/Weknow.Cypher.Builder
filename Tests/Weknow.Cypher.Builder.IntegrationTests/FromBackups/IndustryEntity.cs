using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using Xunit;
using Xunit.Abstractions;

namespace Weknow.Cypher.Builder.IntegrationTests
{
    internal class IndustryEntity
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        public string Id { get; set; } = string.Empty;
        /// <summary>
        /// Gets or sets the shape.
        /// </summary>
        public string Shape { get; set; } = string.Empty;
        /// <summary>
        /// Gets or sets the keywords.
        /// </summary>
        public string[] Keywords { get; set; } = Array.Empty<string>();
        /// <summary>
        /// Gets or sets the color.
        /// </summary>
        public string Color { get; set; } = string.Empty;
        /// <summary>
        /// Gets or sets the label.
        /// </summary>
        public string Label { get; set; } = string.Empty;
        /// <summary>
        /// Gets or sets the price factor.
        /// </summary>
        public double PriceFactor { get; set; }
        /// <summary>
        /// Gets or sets the size of the layout.
        /// </summary>
        public string LayoutSize { get; set; } = string.Empty;
        /// <summary>
        /// Gets or sets the layout x.
        /// </summary>
        public double LayoutX { get; set; }
        /// <summary>
        /// Gets or sets the layout y.
        /// </summary>
        public double LayoutY { get; set; }
    }
}
