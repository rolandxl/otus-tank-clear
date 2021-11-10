using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OtusTankClear
{
	public class Queue
    {
		List<Action> actionList;
		Task Dispatcher;
		public enum State {active, inactive}

		volatile State QueueState;

		public State GetQueueState
		{
			get { return QueueState; }
		}
		public Queue()
        {
			QueueState = State.inactive;
			actionList = new List<Action>();
        }	

		public List<Action> GetActionList
        {
			get { return actionList; }
		}


		public void InputAction(Action action)
        {
			actionList.Add(action);
		}


		public void Actor()
        {
			Task currentTask = Task.Run(()=> ActorStartVoid());
			while(QueueState == State.active)
            {			
				if (currentTask.IsCompleted && actionList.Count>0)
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

		public bool Start()
        {
			if (QueueState == State.inactive)
			{
				QueueState = State.active;
				Dispatcher = Task.Run(() => Actor());
				return true;
			}
			else throw new Exception("Queue already active");
		}

		public void HardStop()
        {
			if (QueueState == State.active)
			{
				QueueState = State.inactive;	
			}
			else throw new Exception("Queue already stoped");
		}
		public Task SoftStop()
        {
			if (QueueState == State.active)
			{
				return Task.Run(() =>
				{
					while (actionList.Count>0)
					{

					}
					QueueState = State.inactive;
				});
			}
			else throw new Exception("Queue already stoped");
		}
    }

	public interface IQueueStop
	{
		Queue.State GetState();
		void SetState(Queue.State state);
	}
}
