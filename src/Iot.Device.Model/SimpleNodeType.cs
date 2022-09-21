namespace Iot.Device.Model
{
    public class SimpleNodeType : NodeType
    {
        public static readonly SimpleNodeType String = new SimpleNodeType("string");
        public static readonly SimpleNodeType Boolean = new SimpleNodeType("boolean");
        public static readonly SimpleNodeType Float = new SimpleNodeType("float");
        // ...

        public SimpleNodeType(string name)
        {
            Name = name;
        }

        public string Name { get; } // TODO: validate A-Z a-z 0-9 _; does not use reserved names: 'array', 'map'

        public override string ToString() => Name;
    }
}
