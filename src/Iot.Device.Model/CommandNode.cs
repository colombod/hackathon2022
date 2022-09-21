using System.Collections.Generic;

namespace Iot.Device.Model
{
    public abstract class CommandNode : ModelNode
    {
        public abstract List<(string, NodeType)> Parameters { get; }
    }
}
