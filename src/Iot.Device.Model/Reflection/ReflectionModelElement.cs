namespace Iot.Device.Model.Reflection
{
    public class ReflectionModelElement : ModelElement
    {
        internal ReflectionModelElement(Type type, InterfaceAttribute? attribute)
        {
            Type = type;

            if (attribute?.DisplayName != null)
            {
                this.SetDisplayName(attribute.DisplayName);
            }
        }

        public override Dictionary<string, ModelElement> Elements { get; } = new();

        public override Dictionary<string, TelemetryNode> Telemetries { get; } = new();

        public override Dictionary<string, PropertyNode> Properties { get; } = new();

        public override Dictionary<string, CommandNode> Commands { get; } = new();

        // TODO: misnamer, in this context `Type` suggests `NodeType`
        public Type Type { get; }
    }
}
