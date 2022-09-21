using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace Iot.Device.Model.Reflection
{
    public class ReflectionPropertyNode : PropertyNode
    {
        public override NodeType Type { get; }

        public override bool IsReadable => Get != null;

        public override bool IsWritable => Set != null;

        internal string MemberName { get; }
        public Type PropertyType { get; }
        public MemberInfo MemberInfo { get; }
        public Func<object?, object?>? Get { get; }
        public Action<object?, object?>? Set { get; }

        private ReflectionPropertyNode(PropertyAttribute attribute, MemberInfo memberInfo)
        {
            MemberInfo = memberInfo;
            MemberName = attribute.Name!;

            switch (memberInfo)
            {
                case PropertyInfo propertyInfo:
                    {
                        MemberName ??= propertyInfo.Name;
                        PropertyType = propertyInfo.PropertyType;
                        Type = NodeType.FromType(propertyInfo.PropertyType);
                        Get = propertyInfo.GetGetMethod() != null ? propertyInfo.GetValue : null;
                        Set = propertyInfo.GetSetMethod() != null ? propertyInfo.SetValue : null;
                        break;
                    }
                default:
                    throw new NotSupportedException($"{nameof(memberInfo)} type `{memberInfo.GetType().FullName}` not supported for properties.");
            }
        }

        internal static bool TryGetPropertyFromMemberInfo(Type declaringType, MemberInfo memberInfo, [NotNullWhen(true)] out ReflectionPropertyNode? propertyNode)
        {
            // declaringType to match Telemetry signature in case we want to add support for MethodInfo in the future
            PropertyAttribute? propertyAttribute = memberInfo.GetCustomAttribute<PropertyAttribute>();

            if (propertyAttribute != null)
            {
                propertyNode = new ReflectionPropertyNode(propertyAttribute, memberInfo);
                return true;
            }

            propertyNode = null;
            return false;
        }
    }
}
