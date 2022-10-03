using System;
using System.Diagnostics;

using Assent;

using FluentAssertions;

using PiTop.Tests;

using Xunit;

namespace PiTop.MakerArchitecture.Foundation.Tests;

public class FoundationPlateTests : IDisposable
{
    private readonly PiTop4Board _module;
    private readonly Configuration _configuration;

    public FoundationPlateTests()
    {
        _configuration = new Configuration()
            .SetInteractive(Debugger.IsAttached)
            .UsingExtension("json");


        PiTop4Board.Configure(
                new DummyGpioController(),
                i2cDeviceFactory: settings => new DummyI2CDevice(settings));
        _module = PiTop4Board.Instance;
    }

    [Fact]
    public void can_obtain_plate_from_module()
    {
        using var plate = _module.GetOrCreatePlate<FoundationPlate>();

        plate.Should().NotBeNull();
    }

    [Fact]
    public void plate_can_create_led()
    {
        using var plate = _module.GetOrCreatePlate<FoundationPlate>();

        using var led = plate.GetOrCreateLed(DigitalPort.D0);

        led.Should().NotBeNull();
    }

    [Fact]
    public void plate_returns_previously_created_devices()
    {
        using var plate = _module.GetOrCreatePlate<FoundationPlate>();

        var led1 = plate.GetOrCreateLed(DigitalPort.D0);
        var led2 = plate.GetOrCreateLed(DigitalPort.D0);

        led2.Should().BeSameAs(led1);
    }

    [Fact]
    public void cannot_create_a_different_device_on_allocated_pins()
    {
        using var plate = _module.GetOrCreatePlate<FoundationPlate>();

        plate.GetOrCreateLed(DigitalPort.D0);

        var action = new Action(() =>
        {
            plate.GetOrCreateUltrasonicSensor(DigitalPort.D0);
        });

        action.Should().Throw<PlatePortInUseException>()
            .Which
            .Message
            .Should().Match("Port D0 already in use by (another) PiTop.MakerArchitecture.Foundation.Components.Led");
    }

    [Fact]
    public void can_generate_model_from_device()
    {
        using var plate = _module.GetOrCreatePlate<FoundationPlate>();

        plate.GetOrCreateSoundSensor(AnaloguePort.A0, "Left");
        plate.GetOrCreateSoundSensor(AnaloguePort.A1,"Right");

        var model = _module.GenerateModel();
        var modelDocument = System.Text.Json.JsonSerializer.Serialize(model, new System.Text.Json.JsonSerializerOptions
        {
            WriteIndented = true,
        });
        this.Assent(modelDocument, _configuration);
    }

    public void Dispose()
    {
        _module.Dispose();
    }
}