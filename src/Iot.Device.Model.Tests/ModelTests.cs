using Iot.Device.Model.Reflection;
using Xunit;

namespace Iot.Device.Model.Tests
{
    public class ModelTests
    {
        [Fact]
        public void ReflectionModelCanGetModelFromClassWithAttributes()
        {
            ReflectionModelResolver resolver = new();
            ReflectionModelElement root = resolver.Resolve(typeof(ExampleModel1));

            {
                var telemetry1Kv = Assert.Single(root.Telemetries);
                Assert.Equal(nameof(ExampleModel1.Telemetry1), telemetry1Kv.Key);
                ReflectionTelemetryNode telemetry1 = Assert.IsType<ReflectionTelemetryNode>(telemetry1Kv.Value);
                Assert.Equal(typeof(int), telemetry1.TelemetryType);
            }

            Assert.Equal(2, root.Elements.Count);
            {
                ReflectionModelElement subElement1 = Assert.IsType<ReflectionModelElementReference>(root.Elements[nameof(ExampleModel1.SubElement1)]);
                Assert.Empty(subElement1.Elements);
                var telemetry2Kv = Assert.Single(subElement1.Telemetries);
                Assert.Equal(nameof(ExampleModel2.Telemetry2), telemetry2Kv.Key);
                ReflectionTelemetryNode telemetry2 = Assert.IsType<ReflectionTelemetryNode>(telemetry2Kv.Value);
                Assert.Equal(typeof(double), telemetry2.TelemetryType);
            }

            {
                ReflectionModelElement subElement2 = Assert.IsType<ReflectionModelElementReference>(root.Elements[nameof(ExampleModel1.SubElement2)]);
                Assert.Empty(subElement2.Elements);
                var telemetry3Kv = Assert.Single(subElement2.Telemetries);
                Assert.Equal(nameof(ExampleModel3.Telemetry3), telemetry3Kv.Key);
                ReflectionTelemetryNode telemetry3 = Assert.IsType<ReflectionTelemetryNode>(telemetry3Kv.Value);
                Assert.Equal(typeof(string), telemetry3.TelemetryType);
            }
        }

        [Interface("example model #1")]
        private class ExampleModel1
        {
            [Telemetry]
            public int Telemetry1 { get; set; }

            [Component]
            public ExampleModel2 SubElement1 { get; set; }

            [Component]
            public ExampleModel3 SubElement2 { get; set; }
        }

        [Interface("example model #2")]
        private class ExampleModel2
        {
            [Telemetry]
            public double Telemetry2 { get; set; }
        }

        [Interface("example model #3")]
        private class ExampleModel3
        {
            [Telemetry]
            public string Telemetry3 { get; set; }
        }
    }
}