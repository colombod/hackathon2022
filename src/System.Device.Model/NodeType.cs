using System.Collections.Generic;

namespace System.Device.Model
{
    public abstract class NodeType
    {
        private static Dictionary<Type, NodeType> s_simpleTypes = new Dictionary<Type, NodeType>()
        {
            [typeof(string)] = SimpleNodeType.String,
            [typeof(bool)] = SimpleNodeType.Boolean,
            [typeof(float)] = SimpleNodeType.Float,
            [typeof(double)] = SimpleNodeType.Float,
        };

        public abstract override string ToString();
        public static NodeType FromString(string typeIdentifier) { throw new NotImplementedException(); }
        public static NodeType FromType(Type type)
        {
            if (s_simpleTypes.TryGetValue(type, out NodeType? nodeType))
                return nodeType;

            // TODO: temporarily disabled throwing to unblock testing
            // throw new NotSupportedException($"Mapping from `{type.FullName}` to {nameof(NodeType)} is currently not supported.");
            return null!;
        }
    }
}
