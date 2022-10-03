using System.Reflection;
using System.Text.Json.Serialization;
using UnitsNet;

namespace Iot.Device.Model;

[JsonConverter(typeof(NodeTypeJsonConverter))]
public abstract class NodeType
{
    private static Dictionary<Type, NodeType> s_simpleTypes = new()
    {
        [typeof(string)] = SimpleNodeType.String,
        [typeof(bool)] = SimpleNodeType.Boolean,
        [typeof(float)] = SimpleNodeType.Number,
        [typeof(double)] = SimpleNodeType.Number,
        [typeof(int)] = SimpleNodeType.Number
    };

    public abstract override string ToString();
    public static NodeType FromString(string typeIdentifier) { throw new NotImplementedException(); }
    public static NodeType FromType(Type type)
    {
        if (s_simpleTypes.TryGetValue(type, out NodeType? nodeType))
            return nodeType;
     

        if (type.IsAssignableTo(typeof(IQuantity)))
        {
            return new QuantityNodeType(type);
        }

#if USE_IOT_DEVICE_BINDINGS
        return null!;
#else
        throw new NotSupportedException($"Mapping from `{type.FullName}` to {nameof(NodeType)} is currently not supported.");
#endif
    }
}