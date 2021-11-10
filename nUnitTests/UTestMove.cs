using NUnit.Framework;
using OtusTankClear;
using System.Numerics;

namespace nUnitTests
{
    [TestFixture]
    public class UTestMove
    {
        Tank tank;
        ICommand move;
        [SetUp]
        public void Setup()
        {
            tank = new();
            move = new Move(new MovableAdapter(tank));
        }

        [Test]
        public void TestNullPosition()
        {
            tank.SetProperty("velocity", new Vector3(1, 0, 1));
            Assert.Catch(() => move.Execute(), "Exeption not recived");
        }
        [Test]
        public void TestNullVelocity()
        {
            tank.SetProperty("position", new Vector3(1, 0, 1));
            Assert.Catch(() => move.Execute(), "Exeption not recived");
        }
        [Test]
        public void TestSimpleMove()
        {
            tank.SetProperty("position", new Vector3(12, 0, 5));
            tank.SetProperty("velocity", new Vector3(-7, 0, 3));
            move.Execute();
            Assert.IsTrue((Vector3)tank.GetProperty("position") == new Vector3(5, 0, 8));
        }
    }
}