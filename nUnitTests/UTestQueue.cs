using NUnit.Framework;
using OtusTankClear;
using System.Numerics;
using System.Threading.Tasks;
using System.Threading;

namespace nUnitTests
{
	[TestFixture]
	public class UTestQueue
	{
		Tank tank;
		ICommand action;
		Queue queue;
		[SetUp]
		public void Setup()
		{
			tank = new();
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

			queue.Start();

			Thread.Sleep(3000);

			queue.HardStop();

			var countActions = queue.GetActionList.Count;

			Thread.Sleep(3000);

			Assert.IsTrue(queue.GetActionList.Count == countActions && queue.GetQueueState == Queue.State.inactive, "HardStop not work");
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

			}

			Assert.IsTrue(queue.GetActionList.Count == 0 && queue.GetQueueState == Queue.State.inactive, "SoftStop not work");
		}
	}
}
	