using System.Text.Json;
using System.Text.Json.Serialization;

namespace Iot.Device.Model;

public class NodeTypeJsonConverter : JsonConverter<NodeType>
{
    public override NodeType? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return NodeType.FromString(reader.GetString()!);
    }

    public override void Write(Utf8JsonWriter writer, NodeType value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString());
    }
}