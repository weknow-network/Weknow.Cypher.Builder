
namespace Weknow.CypherBuilder
{
    internal static class Schema
    {
        public static ILabel Person => ILabel.Fake;
        public static ILabel Friend => ILabel.Fake;
        public static ILabel Animal => ILabel.Fake;
        public static ILabel Language => ILabel.Fake;
        public static ILabel Locale => ILabel.Fake;
        public static ILabel Manager => ILabel.Fake;
        public static ILabel Maintainer => ILabel.Fake;
        public static IType KNOWS => IType.Fake;
        public static IType LIKE => IType.Fake;
        public static IType By => IType.Fake;
        public static ILabel Prod => ILabel.Fake;
    }
}
