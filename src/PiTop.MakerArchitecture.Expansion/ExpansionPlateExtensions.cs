using PiTop.MakerArchitecture.Expansion.Sensors;
using PiTop.MakerArchitecture.Foundation;
using PiTop.MakerArchitecture.Foundation.Sensors;

namespace PiTop.MakerArchitecture.Expansion;

/// <summary>
/// Extensions for <see cref="ExpansionPlate"/>.
/// </summary>
public static class ExpansionPlateExtensions
{
    public static ExpansionPlate GetOrCreateExpansionPlate(this PiTop4Board module)
    {
        return module.GetOrCreatePlate<ExpansionPlate>();
    }

    public static ServoMotor GetOrCreateServoMotor(this ExpansionPlate plate, ServoMotorPort port, string? deviceName = null)
    {
        return plate.GetOrCreateConnectedDevice(port.ToString(), () => new ServoMotor(port, plate.GetOrCreateMcu()) { Name = deviceName });
    }

    public static EncoderMotor GetOrCreateEncoderMotor(this ExpansionPlate plate, EncoderMotorPort port, string? deviceName = null)
    {
        return plate.GetOrCreateConnectedDevice(port.ToString(), ()=> new EncoderMotor(port, plate.GetOrCreateMcu()) { Name = deviceName });
    }

    public static UltrasonicSensor GetOrCreateUltrasonicSensor(this ExpansionPlate plate, AnaloguePort port, string? deviceName = null)
    {
        return plate.GetOrCreateConnectedDevice(port.ToString(), () => new UltrasonicSensorSMBus{Name = deviceName});
    }

    public static UltrasonicSensor GetOrCreateUltrasonicSensor(this ExpansionPlate plate, DigitalPort port, string? deviceName = null)
    {
        return plate.GetOrCreateConnectedDevice(port.ToString(), () => new UltrasonicSensorGpio() { Name = deviceName });
    }
}