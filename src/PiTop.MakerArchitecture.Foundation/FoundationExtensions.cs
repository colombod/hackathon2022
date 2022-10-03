using PiTop.MakerArchitecture.Foundation.Components;
using PiTop.MakerArchitecture.Foundation.Sensors;

using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace PiTop.MakerArchitecture.Foundation;

/// <summary>
/// Extensions for Foundation Plate
/// </summary>
public static class FoundationExtensions
{
    /// <summary>
    /// Gets or Create a <see cref="FoundationPlate"/> instance.
    /// </summary>
    /// <param name="module"></param>
    /// <returns>A <see cref="FoundationPlate"/> instance.</returns>
    public static FoundationPlate GetOrCreateFoundationPlate(this PiTop4Board module)
    {
        return module.GetOrCreatePlate<FoundationPlate>();
    }

    public static SoundSensor GetOrCreateSoundSensor(this FoundationPlate plate, AnaloguePort port, string? deviceName = null)
    {
        return plate.GetOrCreateConnectedDevice(port.ToString(), () => new SoundSensor { Name = deviceName });
    }

    public static LightSensor GetOrCreateLightSensor(this FoundationPlate plate, AnaloguePort port, string? deviceName = null)
    {
        return plate.GetOrCreateConnectedDevice(port.ToString(), () => new LightSensor { Name = deviceName });
    }

    public static Potentiometer GetOrCreatePotentiometer(this FoundationPlate plate, AnaloguePort port, string? deviceName = null)
    {
        return plate.GetOrCreateConnectedDevice(port.ToString(), () => new Potentiometer { Name = deviceName });
    }

    public static Button GetOrCreateButton(this FoundationPlate plate, DigitalPort port, string? deviceName = null)
    {
        return plate.GetOrCreateConnectedDevice(port.ToString(), () => new Button { Name = deviceName });
    }

    public static Buzzer GetOrCreateBuzzer(this FoundationPlate plate, DigitalPort port, string? deviceName = null)
    {
        return plate.GetOrCreateConnectedDevice(port.ToString(), () => new Buzzer { Name = deviceName });
    }

    public static UltrasonicSensor GetOrCreateUltrasonicSensor(this FoundationPlate plate, DigitalPort port, string? deviceName = null)
    {
        return plate.GetOrCreateConnectedDevice(port.ToString(), () => new UltrasonicSensorGpio { Name = deviceName });
    }

    public static Led GetOrCreateLed(this FoundationPlate plate, DigitalPort port, string? deviceName = null)
    {
        return plate.GetOrCreateConnectedDevice(port.ToString(), () => new Led { Name = deviceName });
    }

    public static Led GetOrCreateLed(this FoundationPlate plate, DigitalPort port, Color displayColor, string? deviceName = null)
    {
        var led = plate.GetOrCreateLed(port, deviceName);
        var p = displayColor.ToPixel<Argb32>();
        var alpha = p.A / 255.0;
        led.DisplayProperties.Add(new RgbaCssColor(p.R, p.G, p.B, alpha));
        return led;
    }
}