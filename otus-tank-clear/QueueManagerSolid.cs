using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OtusTankClear
{
	public class QueueSolid: IUObject
	{
		public QueueSolid()
		{
			Propertys = new();
			actionList = new List<Action>();
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

		List<Action> actionList;
		Task Dispatcher;

		public List<Action> GetActionList
		{
			get { return actionList; }
		}

		public Task GetDispatcher
        {
			set { Dispatcher = value; }
			get { return Dispatcher; }
        }


		public void InputAction(Action action)
		{
			actionList.Add(action);
		}

		
		public void Actor()
		{
			Task currentTask = Task.Run(() => ActorStartVoid());
			while ((string)GetProperty("state") == "active")
			{
				if (currentTask.IsCompleted && actionList.Count > 0)
				{
					currentTask = Task.Run(actionList[0]);
					actionList.RemoveAt(0);
				}
			}
		}
		
		public void ActorStartVoid() //процедура попадающая в таск актора при его старте
		{
			//какой-нибудь код логирования
		}

	}

	public interface ISwichable
	{
		string GetState();
		void SetState(string newValue);
	}

	public class SwichableAdapter : ISwichable
	{
		readonly IUObject Obj;
		public SwichableAdapter(IUObject Obj)
		{
			this.Obj = Obj;
		}

		public string GetState()
		{
			if (Obj.GetProperty("state") == null) throw new Exception("Error: state is null");
			return (string)Obj.GetProperty("state");
		}
		public void SetState(string newValue)
		{
			Obj.SetProperty("state", newValue);
		}

		public QueueSolid GetObj
		{
			get { return (QueueSolid)Obj; }
		}
	}


	public class QueueStart : ICommand
	{
		readonly SwichableAdapter swichable;
		public QueueStart(SwichableAdapter swichable)
		{
			this.swichable = swichable;
		}

		public void Execute()
		{
			if (swichable.GetState() == "active") throw new Exception("Queue already start");
			else
			{
				swichable.GetObj.GetDispatcher = Task.Run(() => swichable.GetObj.Actor());
				swichable.SetState("active");
			}
		}
	}
	public class QueueStopHard : ICommand
	{
		readonly SwichableAdapter swichable;
		public QueueStopHard(SwichableAdapter swichable)
		{
			this.swichable = swichable;
		}

		public void Execute()
		{
			if (swichable.GetState() == "inactive") throw new Exception("Queue already stoped");
			else
			{
				swichable.SetState("inactive");
			}
		}
	}

	public class QueueStopSoft : ICommand
	{
		readonly SwichableAdapter swichable;
		public QueueStopSoft(SwichableAdapter swichable)
		{
			this.swichable = swichable;
		}

		public void Execute()
		{
			if (swichable.GetState() == "inactive") throw new Exception("Queue already stoped");
			else
			{
				Task.Run(() =>
				{
					while (this.swichable.GetObj.GetActionList.Count > 0)
					{

					}
					swichable.SetState("inactive");
				});
			}
		}
	}
}
