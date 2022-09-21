using System.Collections.Generic;

namespace Iot.Device.Model
{
    public abstract class ModelElement : ModelNode
    {
        // note: can have cycles
        public abstract Dictionary<string, ModelElement> Elements { get; }
        public abstract Dictionary<string, TelemetryNode> Telemetries { get; }
        public abstract Dictionary<string, PropertyNode> Properties { get; }
        public abstract Dictionary<string, CommandNode> Commands { get; }

        // Possibly ShallowClone, DeepClone (because we might have cycles and we might want to customize)
    }
}
