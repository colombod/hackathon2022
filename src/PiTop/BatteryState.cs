using System;
using System.Collections.Generic;
using System.Linq;

namespace PiTop;

public class BatteryState
{
    private sealed class BatteryStateEqualityComparer : IEqualityComparer<BatteryState>
    {
        public bool Equals(BatteryState? x, BatteryState? y)
        {
            if (x == null) throw new ArgumentNullException(nameof(x));
            if (ReferenceEquals(x, y)) return true;
            if (ReferenceEquals(x, null)) return false;
            if (ReferenceEquals(y, null)) return false;
            if (x.GetType() != y.GetType()) return false;
            return x.ChargingState == y.ChargingState && x.Capacity.Equals(y.Capacity) && x.TimeRemaining.Equals(y.TimeRemaining) && x.Wattage.Equals(y.Wattage);
        }

        public int GetHashCode(BatteryState obj)
        {
            return HashCode.Combine((int) obj.ChargingState, obj.Capacity, obj.TimeRemaining, obj.Wattage);
        }
    }

    public static IEqualityComparer<BatteryState> BatteryStateComparer { get; } = new BatteryStateEqualityComparer();

    public static BatteryState FromMessage(PiTopMessage message)
    {
        var args = message.Parameters.ToArray();

        var chargingState = (BatteryChargingState)int.Parse(args[0]);
        var capacity = double.Parse(args[1]);
        var timeRemaining = TimeSpan.FromMinutes(int.Parse(args[2]));
        var wattage = double.Parse(args[3]);
        var state = new BatteryState(chargingState, capacity, timeRemaining, wattage);
        return state;
    }

    public BatteryState(BatteryChargingState chargingState, double capacity, TimeSpan timeRemaining, double wattage)
    {
        ChargingState = chargingState;
        Capacity = capacity;
        TimeRemaining = timeRemaining;
        Wattage = wattage;
    }

    public static BatteryState Empty => new BatteryState(BatteryChargingState.Undefined, double.NaN, TimeSpan.MinValue, double.NaN);

    public BatteryChargingState ChargingState { get; }
    public double Capacity { get; }
    public TimeSpan TimeRemaining { get; }
    public double Wattage { get; }
}