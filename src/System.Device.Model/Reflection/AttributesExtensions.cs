using System.Diagnostics.CodeAnalysis;

namespace System.Device.Model.Reflection
{
    public static class AttributesExtensions
    {
        [return: NotNullIfNotNull(nameof(defaultValue))]
        public static string? GetDisplayName(this ModelNode node, string? defaultValue = null)
            => node.Attributes!.GetValueOrDefault(ModelNode.CommonAttributes.DisplayName, defaultValue);
    }
}
