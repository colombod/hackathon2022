// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Iot.Device.Model;

/// <summary>
/// Telemetry of the interface
/// </summary>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
public class TelemetryAttribute : Attribute
{
    /// <summary>
    /// Name of the telemetry in the interface
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// Display name of the telemetry
    /// </summary>
    public string? DisplayName { get; set; }

    public string? PreferredUnit { get; set; }

}
