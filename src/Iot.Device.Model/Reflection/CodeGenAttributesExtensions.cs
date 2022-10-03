namespace Iot.Device.Model.Reflection;

public static class CodeGenAttributesExtensions
{
    private const string Prefix = "CSharpCodeGen.";

    // placeholders: {memberPath}
    private const string ValueGetter = Prefix + nameof(ValueGetter);

    private const string MemberName = Prefix + nameof(MemberName);

    /// <summary>
    /// Sets value getter formatter.
    /// Formatter should contain a substring {memberPath} which will be replaced with memberPath when GetValueGetter is called.
    /// Value getter is used by the C# code gen to access corresponding member in the generated code. I.e.:
    /// For properties: {memberPath}
    /// For methods: {memberPath}()
    /// </summary>
    /// <param name="node">Node on which formatter should be set.</param>
    /// <param name="valueGetterFormatter">Formatter used for value getter.</param>
    public static void SetValueGetterFormatter(this ModelNode node, string valueGetterFormatter)
    {
        node.Attributes[ValueGetter] = valueGetterFormatter;
    }

    public static string GetValueGetter(this ModelNode node, string memberPath)
    {
        string formatter = node.GetValueGetterFormatter();
        return formatter.Replace("{memberPath}", memberPath);
    }

    private static string GetValueGetterFormatter(this ModelNode node)
        => node.Attributes.GetValueOrDefault(ValueGetter) ?? throw new ArgumentException($"Formatter missing for {nameof(node)}");

    public static void SetMemberName(this ModelNode node, string memberName)
    {
        node.Attributes[MemberName] = memberName;
    }

    public static string GetMemberName(this ModelNode node)
        => node.Attributes.GetValueOrDefault(MemberName) ?? throw new ArgumentException($"MemberName missing for {nameof(node)}");
}