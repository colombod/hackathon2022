using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;

#if USE_IOT_DEVICE_BINDINGS
using System.Device.Model;
#endif

namespace Iot.Device.Model.Reflection
{
    public sealed class ReflectionTelemetryNode : TelemetryNode
    {
        public override NodeType Type { get; }
        internal string Name { get; }
        public Type TelemetryType { get; }
        public MemberInfo MemberInfo { get; }
        public Func<object?, object?> Get { get; }

        private ReflectionTelemetryNode(Type declaringType, TelemetryAttribute attribute, MemberInfo memberInfo, ParameterInfo? parameterInfo, int numberOfNonOptionalParameters = -1)
        {
            // declaring type might not be same as memberInfo.DeclaringType
            MemberInfo = memberInfo;
            Name = attribute.Name!;

            switch (memberInfo)
            {
                case PropertyInfo propertyInfo:
                    {
                        if (propertyInfo.GetGetMethod() == null)
                            throw new ArgumentException($"Type `{declaringType.FullName}` has {nameof(TelemetryAttribute)} on the property `{Name}` which doesn't have a getter.");

                        Name ??= propertyInfo.Name;
                        TelemetryType = propertyInfo.PropertyType;
                        Get = propertyInfo.GetValue;
                        this.SetValueGetterFormatter("{memberPath}");
                        break;
                    }
                case MethodInfo methodInfo:
                    {
                        if (methodInfo.IsGenericMethod)
                            throw new ArgumentException($"Type `{declaringType.FullName}` has {nameof(TelemetryAttribute)} on generic method `{methodInfo.Name}`");

                        if (methodInfo.ReturnType == typeof(void))
                            throw new ArgumentException($"Type `{declaringType.FullName}` has {nameof(TelemetryAttribute)} on method `{methodInfo.Name}` without return value.");

                        ParameterInfo[] parameters = methodInfo.GetParameters();

                        if (numberOfNonOptionalParameters == -1)
                        {
                            numberOfNonOptionalParameters = CountNonOptionalParameters(parameters);
                        }

                        // TODO: does this make sense?
                        // It certainly makes sense here: https://github.com/dotnet/iot/blob/main/src/devices/LiquidLevel/LiquidLevelSwitch.cs#L44
                        // but ReadTemeperature, TryGetTemperature is an obviously not what people will want... perhaps just make it easier for them and remove some prefixes?
                        Name ??= methodInfo.Name;

                        switch (numberOfNonOptionalParameters)
                        {
                            case 0:
                                {
                                    TelemetryType = methodInfo.ReturnType;
                                    object?[] args = parameters.Length == 0 ? Array.Empty<object>() : Enumerable.Repeat(System.Type.Missing, parameters.Length).ToArray();
                                    Get = (obj) => methodInfo.Invoke(obj, args);
                                    this.SetValueGetterFormatter("{memberPath}()");
                                    break;
                                }
                            case 1:
                                {
                                    parameterInfo ??= parameters[0];

                                    if (!parameterInfo.IsOut)
                                        throw new ArgumentException($"Type `{declaringType.FullName}` has {nameof(TelemetryAttribute)} on method `{methodInfo.Name}` which has 1 argument but argument is not `out`.");

                                    if (methodInfo.ReturnType != typeof(bool))
                                        throw new ArgumentException($"Type `{declaringType.FullName}` has {nameof(TelemetryAttribute)} on method `{methodInfo.Name}` which has 1 argument but does not return `bool`.");

                                    if (!methodInfo.Name.StartsWith("Try"))
                                        throw new ArgumentException($"Type `{declaringType.FullName}` has {nameof(TelemetryAttribute)} on method `{methodInfo.Name}` which name does not start with `Try`");

                                    // we need to unwrap from `out`, otherwise we would end up with reference
                                    TelemetryType = parameterInfo.ParameterType.GetElementType()!;
                                    Get = (obj) =>
                                    {
                                        // TODO: Invoke with out and optional args
                                        throw new NotImplementedException();
                                    };

                                    this.SetValueGetterFormatter("{memberPath}(out var measurement) switch { true => measurement, false => throw new System.InvalidOperationException(\"Reading failed.\") }");
                                    break;
                                }
                            default:
                                throw new ArgumentException($"Type `{declaringType.FullName}` has {nameof(TelemetryAttribute)} on parameter of method `{methodInfo.Name}` but method has {numberOfNonOptionalParameters} non-optional parameters. Method should return bool and have one `out` parameter.");
                        }

                        break;
                    }
                default:
                    throw new NotSupportedException($"{nameof(memberInfo)} type `{memberInfo.GetType().FullName}` not supported for telemetries.");
            }

            Type = NodeType.FromType(TelemetryType);
            if (attribute?.DisplayName != null)
            {
                this.SetDisplayName(attribute.DisplayName);
            }

#if !USE_IOT_DEVICE_BINDINGS
            if (attribute?.PreferredUnit != null)
            {
                this.SetPreferredUnit(attribute.PreferredUnit);
            }
#endif

            this.SetMemberName(memberInfo.Name);
        }

        /// <summary>
        /// Returns false if TelemetryAttribute is not on a given member of one of its parameters and ReflectionTelemetryNode otherwise.
        /// </summary>
        internal static bool TryGetTelemetryFromMemberInfo(Type declaringType, MemberInfo memberInfo, [NotNullWhen(true)] out ReflectionTelemetryNode? telemetryNode)
        {
            TelemetryAttribute? telemetryAttribute = memberInfo.GetCustomAttribute<TelemetryAttribute>();

            telemetryNode = telemetryAttribute == null ? null : new ReflectionTelemetryNode(declaringType, telemetryAttribute, memberInfo, parameterInfo: null);

            // If telemetryNode is assigned we still want to validate if none of the args doesn't incorrectly also have TelemetryAttribute
            if (memberInfo is MethodInfo methodInfo)
            {
                ParameterInfo[] parameters = methodInfo.GetParameters();

                int numberOfNonOptionalParameters = CountNonOptionalParameters(parameters);

                if (telemetryAttribute != null && numberOfNonOptionalParameters > 1)
                    throw new ArgumentException($"Type `{declaringType.FullName}` has {nameof(TelemetryAttribute)} on method `{methodInfo.Name}` with {numberOfNonOptionalParameters} non-optional parameters.");

                ParameterInfo? parameterInfo = null;

                foreach (ParameterInfo paramInfo in parameters)
                {
                    telemetryAttribute = paramInfo.GetCustomAttribute<TelemetryAttribute>();

                    if (telemetryAttribute == null)
                        continue;

                    if (telemetryNode != null)
                    {
                        if (parameterInfo == null)
                        {
                            throw new ArgumentException($"Type `{declaringType.FullName}` has {nameof(TelemetryAttribute)} on both method `{methodInfo.Name}` and parameter.");
                        }
                        else
                        {
                            throw new ArgumentException($"Type `{declaringType.FullName}` has {nameof(TelemetryAttribute)} on more than one parameter of method `{methodInfo.Name}`.");

                        }
                    }

                    parameterInfo = paramInfo;
                    telemetryNode = new ReflectionTelemetryNode(declaringType, telemetryAttribute, memberInfo, parameterInfo, numberOfNonOptionalParameters);
                }
            }

            return telemetryNode != null;
        }

        private static int CountNonOptionalParameters(ParameterInfo[] parameters)
        {
            return parameters.Count((p) => !p.IsOptional);
        }
    }
}
