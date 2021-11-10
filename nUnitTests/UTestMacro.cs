using NUnit.Framework;
using OtusTankClear;
using System.Numerics;

namespace nUnitTests
{
    [TestFixture]
    public class UTestMacro
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
                new CheckFuel(new CheckFuelAdapter(tank)),
                new BurnFuel(new BurnFuelAdapter(tank)),
                new Rotate(new RotateableAdapter(tank)),
                new Move(new MovableAdapter(tank))
            };
            splitCommands = new MacroCommand(macroCommands);
                
        }

        [Test]
        public void TestMacro()
        {
            tank.SetProperty("fuelLevel", 5f);
            tank.SetProperty("fuelLowLevel", 2f);
            tank.SetProperty("fuelRate", 4f);

            tank.SetProperty("position", new Vector3(12, 0, 5));
            tank.SetProperty("velocity", new Vector3(-7, 0, 3));

            tank.SetProperty("rotation", new Quaternion(1, 1, 1, 1));
            tank.SetProperty("angle", 30f);
            tank.SetProperty("axis", new Vector3(0, 1, 0));

            Assert.Catch(() => splitCommands.Execute(), "List of macrocommands does not stop");
        }
    }
}
        

