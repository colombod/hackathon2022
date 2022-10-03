namespace Iot.Device.Model;

public class RecordNodeType : NodeType
{
    public KeyValuePair<string, NodeType>[] Fields { get; }

    public RecordNodeType(params KeyValuePair<string, NodeType>[] fields)
    {
        Fields = fields;
    }

    public override string ToString() => $"record[{string.Join(",",Fields.Select(f => $"{f.Key}:{f.Value}") )}]";
}