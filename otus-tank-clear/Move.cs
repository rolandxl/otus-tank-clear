using System;
using System.Numerics;
using System.Collections.Generic;
using System.Threading;

namespace OtusTankClear
{
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

}
