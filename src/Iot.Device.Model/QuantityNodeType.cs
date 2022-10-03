using UnitsNet;

namespace Iot.Device.Model;

public class QuantityNodeType : NodeType
{
    public Type QuantityType { get; }

    public QuantityNodeType(Type quantityType)
    {
        if (!quantityType.IsAssignableTo(typeof(IQuantity)))
        {
            throw new ArgumentException("Must be a valid UnitsNet type.",nameof(quantityType));
        }

        QuantityType = quantityType;
    }
    public override string ToString()
    {
        return $"quantity[{QuantityType.Name.ToLowerInvariant()}]";
    }
}