using System.Reflection;

namespace Iot.Device.Model.Reflection
{
    public class ReflectionModelResolver
    {
        private Dictionary<Type, ReflectionModelElement> _references = new();

        public bool SkipDuplicateNames { get; set; }

        public ReflectionModelResolver() { }

        public bool TryResolve(Type type, out ReflectionModelElement? model)
        {
            var attr = type.GetCustomAttribute<InterfaceAttribute>();
            if (attr is {})
            {
                model = Resolve(type);
                return true;
            }

            model = null;
            return false;
        }

        public ReflectionModelElement Resolve(Type type)
        {
            if (_references.TryGetValue(type, out ReflectionModelElement? model))
                return model;

            var attr = type.GetCustomAttribute<InterfaceAttribute>();

            if (attr == null)
                throw new ArgumentException($"Type `{type.FullName}` must have {nameof(InterfaceAttribute)}");

            // TODO: duplicates between properties/telemetries/elements
            ReflectionModelElement reflectionElement = new(type, attr);

            // assign early to avoid recursion
            _references[type] = reflectionElement;

            foreach (var property in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                if (ReflectionTelemetryNode.TryGetTelemetryFromMemberInfo(type, property, out ReflectionTelemetryNode? telemetryNode))
                {
                    if (!reflectionElement.Telemetries.TryAdd(telemetryNode.Name, telemetryNode))
                    {
                        if (!SkipDuplicateNames)
                        {
                            throw new ArgumentException($"Telemetry '{telemetryNode.Name}' already exists.");
                        }
                    }

                    continue;
                }

                if (ReflectionPropertyNode.TryGetPropertyFromMemberInfo(type, property, out ReflectionPropertyNode? propertyNode))
                {
                    if (!reflectionElement.Properties.TryAdd(propertyNode.MemberName, propertyNode))
                    {
                        if (!SkipDuplicateNames)
                        {
                            throw new ArgumentException($"Property '{propertyNode.MemberName}' already exists.");
                        }
                    }

                    continue;
                }

                ComponentAttribute? component = property.GetCustomAttribute<ComponentAttribute>();

                if (component != null)
                {
                    // TODO: DisplayName broken - interface has display name but it should be on the component
                    //       also Component and interface could be unified into single attribute [Element]/[Model] or something along the lines

                    string name = component.Name ?? property.Name;
                    ReflectionModelElement? child;
                    if (!_references.TryGetValue(property.PropertyType, out child))
                    {
                        child = Resolve(property.PropertyType);
                    }

                    if (!reflectionElement.Elements.TryAdd(name, new ReflectionModelElementReference(child, property)))
                    {
                        if (!SkipDuplicateNames)
                        {
                            throw new ArgumentException($"Element '{name}' already exists.");
                        }
                    }

                    continue;
                }
            }

            foreach (var method in type.GetMethods(BindingFlags.Public | BindingFlags.Instance))
            {
                if (ReflectionTelemetryNode.TryGetTelemetryFromMemberInfo(type, method, out ReflectionTelemetryNode? telemetryNode))
                {
                    if (!reflectionElement.Telemetries.TryAdd(telemetryNode.Name, telemetryNode))
                    {
                        if (!SkipDuplicateNames)
                        {
                            TelemetryNode previous = reflectionElement.Telemetries[telemetryNode.Name];
                            ReflectionTelemetryNode? previousAsReflectionNode = previous as ReflectionTelemetryNode;
                            if (previousAsReflectionNode != null)
                            {
                                throw new ArgumentException($"Telemetry '{telemetryNode.Name}' already exists on member '{previousAsReflectionNode.MemberInfo.DeclaringType!.FullName}.{previousAsReflectionNode.MemberInfo.Name}' but it occured again on member '{telemetryNode.MemberInfo.DeclaringType!.FullName}.{telemetryNode.MemberInfo.Name}'");
                            }
                            else
                            {
                                throw new ArgumentException($"Telemetry '{telemetryNode.Name}' already exists but it occured again on member '{telemetryNode.MemberInfo.DeclaringType!.FullName}.{telemetryNode.MemberInfo.Name}'");
                            }
                        }
                    }
                    continue;
                }

                // TODO: commands
            }

            return reflectionElement;
        }
    }
}
