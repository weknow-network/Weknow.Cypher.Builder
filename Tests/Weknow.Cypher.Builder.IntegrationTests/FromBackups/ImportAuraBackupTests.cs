using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using Xunit;
using Xunit.Abstractions;

using static Weknow.Cypher.Builder.IntegrationTests.Schema;
using static Weknow.Cypher.Builder.IntegrationTests.SchemaProperties;

using static Weknow.Cypher.Builder.Cypher;

namespace Weknow.Cypher.Builder.IntegrationTests
{
    public class ImportAuraBackupTests
    {
        private readonly ITestOutputHelper _outputHelper;

        #region Ctor

        public ImportAuraBackupTests(ITestOutputHelper outputHelper)
        {
            _outputHelper = outputHelper;
        }

        #endregion // Ctor

        [Fact]
        public async Task ParseTest()
        {
            string REGEX_EXP = @"(\[.*)(,)(.*\])"; // 3 groups for separate the ',' within array
            string REGEX_REPLACE_EXP = @"$1~$3"; // temporally replace ','  with '~' 
            string[] lines = await File.ReadAllLinesAsync(@"backups\backup.csv");
            foreach (string l in lines)
            {
                var line = Regex.Replace(l, REGEX_EXP, REGEX_REPLACE_EXP);
                var dataline = line[2..^2];
                var query = from x in dataline.Split(",")
                            select x.Split(":");
                var map = query.ToDictionary(m => m[0], m => m[1]);

                CypherCommand cypher =
                            _(n => map =>
                             Create(N(n, Industry, map.AsMap))
                             .Return(n)
                            , cfg =>
                            {
                                cfg.AmbientLabels.Add("`WK`");
                                cfg.Naming.Convention = CypherNamingConvention.SCREAMING_CASE;
                            });


                Assert.NotEmpty(cypher.Parameters);
            }
        }

        [Fact]
        public async Task ParseUnwindTest()
        {
            string REGEX_EXP = @"(\[.*)(,)(.*\])"; // 3 groups for separate the ',' within array
            string REGEX_REPLACE_EXP = @"$1~$3"; // temporally replace ','  with '~' 
            string[] lines = await File.ReadAllLinesAsync(@"backups\backup.csv");
            CypherCommand cypher = _(items => map => n =>
                                    Unwind(items, map,
                                    Create(N(n, Industry, map.AsMap))
                                    .Return(n)));
            var entities = new List<IndustryEntity>();
            foreach (string l in lines)
            {
                var line = Regex.Replace(l, REGEX_EXP, REGEX_REPLACE_EXP);
                var dataline = line[2..^2];
                var query = from x in dataline.Split(",")
                            select x.Split(":");
                var map = query.ToDictionary(m => m[0], m => m[1]);

                var keys = map[nameof(IndustryEntity.Keywords)];
                var keywords = keys[1..^1].Split("~");
                var entity = new IndustryEntity
                {
                    Id = map[nameof(IndustryEntity.Id)],
                    Color = map[nameof(IndustryEntity.Color)],
                    Keywords = keywords,
                    Label = map[nameof(IndustryEntity.Label)],
                    LayoutSize = map["Size"],
                    LayoutX = double.Parse(map["X"]),
                    LayoutY = double.Parse(map["Y"]),
                    PriceFactor = double.Parse(map[nameof(IndustryEntity.PriceFactor)]),
                    Shape = map[nameof(IndustryEntity.Shape)],
                };
                entities.Add(entity);
            }
        }
    }
}
