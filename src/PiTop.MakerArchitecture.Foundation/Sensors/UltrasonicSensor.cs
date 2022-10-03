using Iot.Device.Model;
using UnitsNet;
using UnitsNet.Units;

namespace PiTop.MakerArchitecture.Foundation.Sensors;

[Interface("UltrasonicSensor")]
public abstract class UltrasonicSensor : PlateConnectedDevice
{
    [Telemetry(PreferredUnit = nameof(LengthUnit.Centimeter))]
    public Length Distance => GetDistance();

    protected abstract Length GetDistance();
}