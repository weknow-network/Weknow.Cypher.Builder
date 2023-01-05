namespace Weknow.CypherBuilder;

/// <summary>
/// Date/Time convention
/// </summary>
public record ConfigTimeConvention
{
    /// <summary>
    /// Gets or sets the time convention.
    /// </summary>
    public TimeConvention TimeConvention { get; set; } = TimeConvention.AsFunction;
    /// <summary>
    /// Gets or sets the clock convention.
    /// </summary>
    public TimeClockConvention ClockConvention { get; set; } = TimeClockConvention.Default;
}
