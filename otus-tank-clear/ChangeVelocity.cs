using System;
using System.Numerics;
using System.Collections.Generic;

namespace OtusTankClear
{
	public interface IChangeVelocity
	{
		Vector3 GetVelocity();
		void SetVelocity(Vector3 newValue);
		Quaternion GetRotation();


		float GetAngle();
		Vector3 GetAxis();
	}

	public class ChangeVelocityAdapter : IChangeVelocity
	{
		readonly IUObject Obj;
		public ChangeVelocityAdapter(IUObject Obj)
		{
			this.Obj = Obj;
		}

		public Vector3 GetVelocity()
        {
			if (Obj.GetProperty("velocity") == null) throw new Exception("Error: velocity is null");
			return (Vector3)Obj.GetProperty("velocity");
		}
		public void SetVelocity(Vector3 newValue)
		{
			Obj.SetProperty("velocity", newValue);
		}
		public Quaternion GetRotation()
		{
			if (Obj.GetProperty("rotation") == null) throw new Exception("Error: rotation is null");
			return (Quaternion)Obj.GetProperty("rotation");
		}

		public float GetAngle()
        {
			if (Obj.GetProperty("angle") == null) throw new Exception("Error: angle is null");
			return (float)Obj.GetProperty("angle");
		}

		public Vector3 GetAxis()
		{
			if (Obj.GetProperty("axis") == null) throw new Exception("Error: axis is null");
			return (Vector3)Obj.GetProperty("axis");
		}

	}

	public class ChangeVelocity : ICommand
	{
		readonly IChangeVelocity change;
		public ChangeVelocity(IChangeVelocity change)
		{
			this.change = change;
		}

		public void Execute()
		{
			if (change.GetVelocity() != Vector3.Zero)
				change.SetVelocity(Vector3.Transform(change.GetVelocity(), change.GetRotation()));	
		}
	}
}
