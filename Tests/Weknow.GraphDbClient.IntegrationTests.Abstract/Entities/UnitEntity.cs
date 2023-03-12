// https://neo4j.com/docs/cypher-refcard/current/

using Generator.Equals;

using Riok.Mapperly.Abstractions;

using Weknow.Mapping;

namespace Weknow.GraphDbClient.IntegrationTests.Abstract;

[Dictionaryable]
[Equatable]
internal partial record UnitEntity(int Id, string Title, UnitType UnitType, int ParentId = -1);

//internal partial record UnitEntityWithChildren : UnitEntity
//{
//    public UnitEntityWithChildren(int Id, string Title, UnitType UnitType) : base(Id, Title, UnitType)
//    {
//    }

//    UnitEntityWithChildren[] Childrens { get; init; } = Array.Empty<UnitEntityWithChildren>();
//}

//[Mapper]
//internal partial class UnitEntityMapper
//{
//    public partial UnitEntity Cast(UnitEntityWithChildren unit);
//}
