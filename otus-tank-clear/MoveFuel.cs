using System;
using System.Numerics;
using System.Collections.Generic;

namespace OtusTankClear
{
	public interface IMovableFuel
	{
		float GetCurentFuelRate();
		void SetFuelRate(float newValue);
		float GetMoveFuelRate();
	}

	public class MovableFuelAdapter : IMovableFuel
	{
		readonly IUObject Obj;
		public MovableFuelAdapter(IUObject Obj)
		{
			this.Obj = Obj;
		}

		public float GetCurentFuelRate()
        {
			if (Obj.GetProperty("fuelRate") == null) throw new Exception("Error: fuelRate is null");
			return (float)Obj.GetProperty("fuelRate");
		}
		public void SetFuelRate(float newValue)
		{
			Obj.SetProperty("fuelRate", newValue);
		}
		public float GetMoveFuelRate()
        {
			if (Obj.GetProperty("fuelRateMove") == null) throw new Exception("Error: fuelRateMove is null");
			return (float)Obj.GetProperty("fuelRateMove");
		}

	}

	public class MoveFuel : ICommand
	{
		readonly IMovableFuel change;
		public MoveFuel(IMovableFuel change)
		{
			this.change = change;
		}

		public void Execute()
		{
			change.SetFuelRate(change.GetMoveFuelRate() + change.GetCurentFuelRate());
		}
	}
}
