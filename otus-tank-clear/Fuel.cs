using System;
using System.Numerics;
using System.Collections.Generic;

namespace OtusTankClear
{

	//Проверка бензина
	public interface ICheckableFuel
	{
		float GetCurrentFuelLevel();
		float GetLowFuelLevel();
		float GetFuelRate();
	}


	public class CheckFuelAdapter : ICheckableFuel
	{
		readonly IUObject Obj;
		public CheckFuelAdapter(IUObject Obj)
		{
			this.Obj = Obj;
		}

		public float GetCurrentFuelLevel()
		{
			if (Obj.GetProperty("fuelLevel") == null) throw new Exception("Error: fuelLevel is null");
			return (float)Obj.GetProperty("fuelLevel");
		}
		public float GetLowFuelLevel()
		{
			if (Obj.GetProperty("fuelLowLevel") == null) throw new Exception("Error: fuelLowLevel is null");
			return (float)Obj.GetProperty("fuelLowLevel");
		}
		public float GetFuelRate()
		{
			if (Obj.GetProperty("fuelRate") == null) throw new Exception("Error: fuelRate is null");
			return (float)Obj.GetProperty("fuelRate");
		}

	}

	public class CheckFuel : ICommand
	{
		Random random = new();
		readonly ICheckableFuel check;
		public CheckFuel(ICheckableFuel check)
		{
			this.check = check;
		}

		public void Execute()
		{
			if (check.GetCurrentFuelLevel() - check.GetFuelRate() < check.GetLowFuelLevel())
				throw new Exception("Level of fuel is low");
		}
	}

	//Сжигание бензина

	public interface IBurnableFuel
	{
		float GetCurentFuelLevel();
		void SetCurentFuelLevel(float newValue);
		float GetFuelRate();
		void SetFuelRate(float newValue);
	}


	public class BurnFuelAdapter : IBurnableFuel
	{
		readonly IUObject Obj;
		public BurnFuelAdapter(IUObject Obj)
		{
			this.Obj = Obj;
		}

		public float GetCurentFuelLevel()
		{
			if (Obj.GetProperty("fuelLevel") == null) throw new Exception("Error: fuelLevel is null");
			return (float)Obj.GetProperty("fuelLevel");
		}

		public float GetFuelRate()
		{
			if (Obj.GetProperty("fuelRate") == null) throw new Exception("Error: fuelRate is null");
			return (float)Obj.GetProperty("fuelRate");
		}

		public void SetCurentFuelLevel(float newValue)
        {
			Obj.SetProperty("fuelLevel", newValue);
		}

		public void SetFuelRate(float newValue)
        {
			Obj.SetProperty("fuelRate", newValue);
		}

	}

	public class BurnFuel : ICommand
	{
		readonly IBurnableFuel burn;
		public BurnFuel(IBurnableFuel burn)
		{
			this.burn = burn;
		}

		public void Execute()
		{
			burn.SetCurentFuelLevel(burn.GetCurentFuelLevel() - burn.GetFuelRate());
			burn.SetFuelRate(0f);
		}
	}


}
