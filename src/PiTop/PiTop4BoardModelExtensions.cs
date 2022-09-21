using System;
using System.Collections.Generic;

using Iot.Device.Model;
using Iot.Device.Model.Reflection;

namespace PiTop;

public static class PiTop4BoardModelExtensions
{
    public static ModelElement GenerateModel(this PiTop4Board board)
    {
        var model = new PiTop4Model();
        var resolver = new ReflectionModelResolver();
        var nameCounter = new Dictionary<string, int>();

        foreach (var connectedPlate in board.ConnectedPlates)
        {
            var plateModel = connectedPlate.GenerateModel();
            if (plateModel is { })
            {
                model.Elements.Add(GetSafeUniqueName(connectedPlate.GetType().Name), plateModel);
            }
            // todo: get the board parts here
        }

        string GetSafeUniqueName(string name)
        {
            var counter = nameCounter.GetValueOrDefault(name, 0);
            nameCounter[name] = counter + 1;
            return $"{name}{counter}";
        }

        return model;
    }

    public static ModelElement? GenerateModel(this PiTopPlate plate)
    {
        var model = new PiTopPlateModel();
        var resolver = new ReflectionModelResolver();
        var nameCounter = new Dictionary<string, int>();


        if (resolver.TryResolve(plate.GetType(), out var plateModel))
        {
            foreach (var element in plateModel.Elements)
            {
                model.Elements.Add(element.Key, element.Value);
            }

            foreach (var command in plateModel.Commands)
            {
                model.Commands.Add(command.Key, command.Value);
            }

            foreach (var telemetry in plateModel.Telemetries)
            {
                model.Telemetries.Add(telemetry.Key, telemetry.Value);
            }

            foreach (var property in plateModel.Properties)
            {
                model.Properties.Add(property.Key, property.Value);
            }
        }
       

        foreach (var connectedDevice in plate.ConnectedDevices)
        {
            if (resolver.TryResolve(connectedDevice.GetType(), out var deviceModel))
            {
                model.Elements.Add(connectedDevice.Name ?? GetSafeUniqueName(deviceModel.Type.Name), deviceModel);
            }
        }

        string GetSafeUniqueName(string name)
        {
            var counter = nameCounter.GetValueOrDefault(name, 0);
            nameCounter[name] = counter + 1;
            return $"{name}{counter}";
        }

        return model;
    }
}

public class PiTop4Model : ModelElement
{
    public override Dictionary<string, ModelElement> Elements { get; } = new();
    public override Dictionary<string, TelemetryNode> Telemetries { get; } = new();
    public override Dictionary<string, PropertyNode> Properties { get; } = new();
    public override Dictionary<string, CommandNode> Commands { get; } = new();
}

public class PiTopPlateModel : ModelElement
{
    public override Dictionary<string, ModelElement> Elements { get; } = new();
    public override Dictionary<string, TelemetryNode> Telemetries { get; } = new();
    public override Dictionary<string, PropertyNode> Properties { get; } = new();
    public override Dictionary<string, CommandNode> Commands { get; } = new();
}