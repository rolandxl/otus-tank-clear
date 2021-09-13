using NUnit.Framework;
using otus_tank_clear;
using System.Numerics;

namespace nUnitTests
{
    [TestFixture]
    public class TestMove
    {
        Tank tank;
        ICommand move;
        [SetUp]
        public void Setup()
        {
            tank = new();
            move = new Move(new MovableAdapter(tank));
            tank.ClearPropertys();
        }

        [Test]
        public void TestNullPosition()
        {
            tank.SetProperty("velocity", new Vector3(1, 0, 1));
            move.Execute();
            Assert.IsNull(move.Execute());
         //   Assert.Pass();
        }
        [Test]
        public void TestNullVelocity()
        {
            tank.SetProperty("position", new Vector3(1, 0, 1));
            move.Execute();
            Assert.IsNull(move.Execute());
          //  Assert.Pass();
        }
        [Test]
        public void TestSimpleMove()
        {
            tank.SetProperty("position", new Vector3(12, 0, 5));
            tank.SetProperty("velocity", new Vector3(-7, 0, 3));
            move.Execute();
            Assert.IsTrue((Vector3)tank.GetProperty("position") == new Vector3(5, 0, 8));
           // Assert.Pass();
        }
    }
}