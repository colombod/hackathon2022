using System.Collections.Generic;

namespace System.Device.Model
{
    public abstract class CommandNode : ModelNode
    {
        public abstract List<(string, NodeType)> Parameters { get; }
    }
}
