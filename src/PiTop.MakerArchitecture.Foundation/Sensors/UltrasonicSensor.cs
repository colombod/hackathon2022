using Iot.Device.Model;

using UnitsNet;

namespace PiTop.MakerArchitecture.Foundation.Sensors;

[Interface("UltrasonicSensor")]
public abstract class UltrasonicSensor : PlateConnectedDevice
{
    [Iot.Device.Model.Telemetry]
    public Length Distance => GetDistance();
    protected abstract Length GetDistance();
}