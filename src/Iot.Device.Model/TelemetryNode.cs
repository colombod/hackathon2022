namespace Iot.Device.Model;

public abstract class TelemetryNode : ModelNode
{
    public abstract NodeType Type { get; }
}