using System.Diagnostics.CodeAnalysis;

namespace System.Device.Model
{
    public static class AttributesExtensions
    {
        private const string DisplayName = "DisplayName";

        public static void SetDisplayName(this ModelNode node, string displayName)
            => node.Attributes[DisplayName] = displayName;

        public static string? GetDisplayName(this ModelNode node)
            => node.Attributes!.GetValueOrDefault(DisplayName);
    }
}
