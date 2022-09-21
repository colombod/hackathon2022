namespace Iot.Device.Model
{
    public class ArrayNodeType : NodeType
    {
        public ArrayNodeType(NodeType elementType)
        {
            ElementType = elementType;
        }

        public NodeType ElementType { get; }
        public override string ToString() => $"array<{ElementType}>";
    }
}
