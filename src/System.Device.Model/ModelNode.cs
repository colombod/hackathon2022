using System.Collections.Generic;

namespace System.Device.Model
{
    // Do we need/want a base class?
    public abstract class ModelNode
    {
        // attributes are node specific therefore setter doesn't seem to make sense
        public virtual Dictionary<string, string> Attributes { get; } = new Dictionary<string, string>();

        public static class CommonAttributes
        {
            public const string DisplayName = "DisplayName";
        }
    }
}
