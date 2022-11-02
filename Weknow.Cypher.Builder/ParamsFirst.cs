﻿#pragma warning disable CA1063 // Implement IDisposable Correctly

namespace Weknow.GraphDbCommands;

/// <summary>
/// Use to avoid empty params array
/// </summary>
public class ParamsFirst<T>
{
    private readonly T _value;

    private ParamsFirst(T value)
    {
        _value = value;
    }

    public static implicit operator T(ParamsFirst<T> item)
    {
        return item._value;
    }

    public static implicit operator ParamsFirst<T>(T value)
    {
        return new ParamsFirst<T>( value );
    }

}