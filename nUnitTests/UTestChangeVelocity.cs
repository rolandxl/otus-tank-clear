using NUnit.Framework;
using OtusTankClear;
using System.Numerics;

namespace nUnitTests
{
    [TestFixture]
    public class UTestChangeVelocity
    {
        Tank tank;
        MacroCommand splitCommands;
        ICommand[] macroCommands;

        [SetUp]
        public void Setup()
        {
            tank = new();
            macroCommands = new ICommand[]
            {
                new Rotate(new RotateableAdapter(tank)),
                new ChangeVelocity(new ChangeVelocityAdapter(tank)),
                new Move(new MovableAdapter(tank))
            };
            splitCommands = new MacroCommand(macroCommands);
                
        }

        [Test]
        public void TestChangeVelocity()
        {
            tank.SetProperty("position", new Vector3(1, 1, 1));
            tank.SetProperty("velocity", new Vector3(-7, 0, 3));
            tank.SetProperty("rotation", new Quaternion(1, 1, 1, 1));
            tank.SetProperty("angle", 30f);
            tank.SetProperty("axis", new Vector3(0, 1, 0));

            splitCommands.Execute();

            Assert.IsTrue((Vector3)tank.GetProperty("velocity")== new Vector3(-4.8138685f, - 16.175419f, - 9f), $"Velocity does not change: {((Vector3)tank.GetProperty("velocity"))}");
        }
    }
}
        

