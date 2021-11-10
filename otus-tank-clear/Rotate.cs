using System;
using System.Numerics;
using System.Collections.Generic;
using System.Threading;

namespace OtusTankClear
{
	public interface IRotateable
	{
		Quaternion GetRotation();
		void SetRotation(Quaternion newValue);
		void GetRotate(ref Vector3 axis, ref float angle);
	}

	public class RotateableAdapter : IRotateable
	{
		readonly IUObject Obj;
		public RotateableAdapter(IUObject Obj)
		{
			this.Obj = Obj;
		}

		public Quaternion GetRotation()
		{
			if (Obj.GetProperty("rotation") == null) throw new Exception("Error: rotation is null");
			return (Quaternion)Obj.GetProperty("rotation");
		}
		public void SetRotation(Quaternion newValue)
		{
			Obj.SetProperty("rotation", newValue);
		}
		public void GetRotate(ref Vector3 axis, ref float angle)
		{
			if (Obj.GetProperty("axis") == null) throw new Exception("Error: axis is null");
			else if (Obj.GetProperty("angle") == null) throw new Exception("Error: angle is null");

			axis = (Vector3)Obj.GetProperty("axis");
			angle = (float)Obj.GetProperty("angle");
		}
	}

	public class Rotate : ICommand
	{
		Random random = new();
		readonly IRotateable rotateable;
		public Rotate(IRotateable rotateable)
		{
			this.rotateable = rotateable;
		}

		public void Execute()
		{
			Thread.Sleep(random.Next(1000));
			Vector3 axis = new();
			float angle = 0;
			rotateable.GetRotate(ref axis, ref angle);
			rotateable.SetRotation(rotateable.GetRotation() * Quaternion.CreateFromAxisAngle(axis, angle));
		}
	}
}
