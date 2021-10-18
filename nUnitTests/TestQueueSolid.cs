using NUnit.Framework;
using OtusTankClear;
using System.Numerics;
using System.Threading.Tasks;
using System.Threading;

namespace nUnitTests
{
	[TestFixture]
	public class TestQueueSolid
	{
		Tank tank;
		ICommand action;
		QueueSolid queue;
		ICommand queueSwich;

		[SetUp]
		public void Setup()
		{
			tank = new();
			action = new Move(new MovableAdapter(tank));
			queue = new QueueSolid();			
			tank.SetProperty("position", new Vector3(12, 0, 5));
			tank.SetProperty("velocity", new Vector3(1, 0, 1));
			queue.SetProperty("state", "inactive");
		}

		[Test]
		public void TestStartQueue()
        {
			queueSwich = new QueueStart(new SwichableAdapter(queue));
			queueSwich.Execute();

			queue.InputAction(()=> action.Execute());
			queue.InputAction(() => action.Execute());
			queue.InputAction(() => action.Execute());
			Assert.AreEqual(queue.GetActionList.Count, 3, "Queue input correct");

			while(queue.GetActionList.Count>0)
            {

			}
			Assert.AreEqual(queue.GetActionList.Count, 0, "Queue work correct");
			
		}
		[Test]
		public void TestHardStopQueue()
        {
			queue.InputAction(() => action.Execute());
			queue.InputAction(() => action.Execute());
			queue.InputAction(() => action.Execute());
			queue.InputAction(() => action.Execute());
			queue.InputAction(() => action.Execute());

			queueSwich = new QueueStart(new SwichableAdapter(queue));
			queueSwich.Execute();
			Thread.Sleep(1000);

			queueSwich = new QueueStopHard(new SwichableAdapter(queue));
			queueSwich.Execute();

			var countActions = queue.GetActionList.Count;
			Thread.Sleep(1000);
			Assert.IsTrue(queue.GetActionList.Count == countActions && (string)queue.GetProperty("state") == "inactive", "HardStop not work");
		}
		[Test]
		public void TestSoftStopQueue()
		{
			queue.InputAction(() => action.Execute());
			queue.InputAction(() => action.Execute());
			queue.InputAction(() => action.Execute());
			queue.InputAction(() => action.Execute());
			queue.InputAction(() => action.Execute());

			queueSwich = new QueueStart(new SwichableAdapter(queue));
			queueSwich.Execute();

			queueSwich = new QueueStopSoft(new SwichableAdapter(queue));
			queueSwich.Execute();


			while ((string)queue.GetProperty("state") == "active")
			{

			}

			Assert.IsTrue(queue.GetActionList.Count == 0, $"SoftStop not work {queue.GetActionList.Count}");
		}
	}
}
	