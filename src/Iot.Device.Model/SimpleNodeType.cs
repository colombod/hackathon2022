namespace Iot.Device.Model;

public class SimpleNodeType : NodeType
{
    public static readonly SimpleNodeType String = new("string");
    public static readonly SimpleNodeType Boolean = new("boolean");
    public static readonly SimpleNodeType Number = new("number");
    // ...

    public SimpleNodeType(string name)
    {
        Name = name;
    }

    public string Name { get; } // TODO: validate A-Z a-z 0-9 _; does not use reserved names: 'array', 'map'

    public override string ToString() => Name;
}