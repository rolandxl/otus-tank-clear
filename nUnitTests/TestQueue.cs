using NUnit.Framework;
using otus_tank_clear;
using System.Numerics;
using System.Threading.Tasks;
using System.Threading;

namespace nUnitTests
{
	[TestFixture]
	public class TestQueue
	{
		Tank tank;
		ICommand action;
		Queue queue;
		[SetUp]
		public void Setup()
		{
			Tank tank = new();
			action = new Move(new MovableAdapter(tank));
			queue = new Queue();
			tank.SetProperty("position", new Vector3(12, 0, 5));
			tank.SetProperty("velocity", new Vector3(1, 0, 1));
		}

		[Test]
		public void TestStartQueue()
        {
			Assert.IsTrue(queue.Start(), "Queue start fail");

			queue.InputAction(()=> action.Execute());
			queue.InputAction(() => action.Execute());
			queue.InputAction(() => action.Execute());
			Assert.AreEqual(queue.GetActionList().Count, 3, "Queue input correct");

			while(queue.GetActionList().Count>0)
            {
				Thread.Sleep(100);
			}
			Assert.AreEqual(queue.GetActionList().Count, 0, "Queue work correct");
			
		}
		[Test]
		public void TestHardStopQueue()
        {
			queue.InputAction(() => action.Execute());
			queue.InputAction(() => action.Execute());
			queue.InputAction(() => action.Execute());
			queue.InputAction(() => action.Execute());
			queue.InputAction(() => action.Execute());

			queue.Start();

			Thread.Sleep(3000);

			queue.HardStop();

			var countActions = queue.GetActionList().Count;

			Thread.Sleep(3000);

			Assert.IsTrue(queue.GetActionList().Count == countActions && queue.QueueState == Queue.State.inactive, "HardStop not work");
		}
		[Test]
		public void TestSoftStopQueue()
		{
			queue.InputAction(() => action.Execute());
			queue.InputAction(() => action.Execute());
			queue.InputAction(() => action.Execute());
			queue.InputAction(() => action.Execute());
			queue.InputAction(() => action.Execute());

			queue.Start();
			Task currentTask = queue.SoftStop();

			while (!currentTask.IsCompleted)
			{
				Thread.Sleep(100);
			}

			Assert.IsTrue(queue.GetActionList().Count == 0 && queue.QueueState == Queue.State.inactive, "SoftStop not work");
		}


	}
}
