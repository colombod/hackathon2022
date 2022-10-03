namespace Iot.Device.Model;

public static class AttributesExtensions
{
    private const string DisplayName = "DisplayName";

    public static void SetDisplayName(this ModelNode node, string displayName)
        => node.Attributes[DisplayName] = displayName;

    public static string? GetDisplayName(this ModelNode node)
        => node.Attributes!.GetValueOrDefault(DisplayName);

    private const string PreferredUnit = "PreferredUnit";

    public static void SetPreferredUnit(this ModelNode node, string preferredUnit)
        => node.Attributes[PreferredUnit] = preferredUnit;

    public static string? GetPreferredUnit(this ModelNode node)
        => node.Attributes!.GetValueOrDefault(PreferredUnit);
}