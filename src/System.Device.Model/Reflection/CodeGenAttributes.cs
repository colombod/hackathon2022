using System.Collections.Generic;
using System.Data;

namespace System.Device.Model.Reflection
{
    internal static class CodeGenAttributes
    {
        private const string Prefix = "__C#CodeGen_";

        // placeholders: {memberPath}
        public const string DataCollector = Prefix + "DataCollector";
    }
}
