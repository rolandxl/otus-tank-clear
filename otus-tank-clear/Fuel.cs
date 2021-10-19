using System;
using System.Numerics;
using System.Collections.Generic;

namespace OtusTankClear
{

	//Проверка бензина
	public interface ICheckFuelComamnd
	{
		float GetCurrentFuelLevel();
		float GetLowFuelLevel();
		float GetFuelRate();
	}


	public class CheckFuelAdapter : ICheckFuelComamnd
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
		readonly ICheckFuelComamnd check;
		public CheckFuel(ICheckFuelComamnd check)
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

	public interface IBurnFuelComamnd
	{
		float GetCurentFuelLevel();
		void SetCurentFuelLevel(float newValue);
		float GetFuelRate();
		void SetFuelRate(float newValue);
	}


	public class BurnFuelAdapter : IBurnFuelComamnd
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
		readonly IBurnFuelComamnd burn;
		public BurnFuel(IBurnFuelComamnd burn)
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
