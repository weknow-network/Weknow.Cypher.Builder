
namespace Weknow.CypherBuilder;

/// <summary>
/// Use to avoid empty params array
/// </summary>
public class ParamsFirst<T>
{
    public static implicit operator T(ParamsFirst<T> item) => throw new NotImplementedException();

    public static implicit operator ParamsFirst<T>(T value) => throw new NotImplementedException();

}
