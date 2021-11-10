using System;
using System.Numerics;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;

namespace OtusTankClear
{
	class Program
	{
		static void Main(string[] args)
		{
			Console.WriteLine("Старт программы!");
		}
	}


	public interface IUObject
	{
		object GetProperty(string key);
		void SetProperty(string key, object value);
	}


	//реализуем интерфейс в класс 
	public class Tank : IUObject
	{
		public Tank()
		{
			Propertys = new();
		}

		public object GetProperty(string key)
		{
			Propertys.TryGetValue(key, out object value);
			return value;
		}
		public void SetProperty(string key, object value)
		{
			if (Propertys.ContainsKey(key))
				Propertys[key] = value;
			else Propertys.TryAdd(key, value);
		}
		Dictionary<string, object> Propertys;
	}


	//создаем интерфейс команды для последующей реалиции его через конкретные механики 
	public interface ICommand
	{
		void Execute();
	}

	public class MacroCommand : ICommand
	{
		readonly ICommand[] commands;
		public MacroCommand(ICommand[] commands)
		{
			this.commands = commands;
		}

		List<string> errors = new List<string>();

		public void Execute()
		{
			foreach (var i in commands)
				try
				{
					i.Execute();
				}
				catch (Exception e)
				{
					errors.Add(e.Message);
				}
		}
	}

	//создаем интерфейс объекта который движется
	public interface IMovable
	{
		Vector3 GetPosition();
		void SetPosition(Vector3 newValue);
		Vector3 GetVelocity();
	}


	//создаем класс-адаптер реализующий интерфейс движение 
	public class MovableAdapter : IMovable
	{
		readonly IUObject Obj;
		public MovableAdapter(IUObject Obj)
		{
			this.Obj = Obj;
		}

		public Vector3 GetPosition()
		{
			if (Obj.GetProperty("position") == null) throw new Exception("Error: position is null");
			return (Vector3)Obj.GetProperty("position");
		}
		public void SetPosition(Vector3 newValue)
		{
			Obj.SetProperty("position", newValue);
		}
		public Vector3 GetVelocity()
		{
			if (Obj.GetProperty("velocity") == null) throw new Exception("Error: velocity is null");
			return (Vector3)Obj.GetProperty("velocity");
		}
	}

	//создаем реализацию команды перемещения в виде класса
	public class Move : ICommand
	{
		Random random = new();
		readonly IMovable movable;
		public Move(IMovable movable)
		{
			this.movable = movable;
		}

		public void Execute()
		{
			Thread.Sleep(random.Next(1000));
			movable.SetPosition(movable.GetPosition() + movable.GetVelocity());
		}
	}

	//всё тоже самое с вращением

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

