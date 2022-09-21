namespace Iot.Device.Model
{
    public class MapNodeType : NodeType
    {
        public MapNodeType(NodeType keyType, NodeType valueType)
        {
            KeyType = keyType;
            ValueType = valueType;
        }

        public NodeType KeyType { get; }
        public NodeType ValueType { get; }

        public override string ToString() => $"map<{KeyType},{ValueType}>";
    }
}
