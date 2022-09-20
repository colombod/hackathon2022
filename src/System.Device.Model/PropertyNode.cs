namespace System.Device.Model
{
    public abstract class PropertyNode : ModelNode
    {
        public abstract NodeType Type { get; }
        public abstract bool IsReadable { get; }
        public abstract bool IsWritable { get; }
    }
}
