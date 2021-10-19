using NUnit.Framework;
using OtusTankClear;
using System.Numerics;

namespace nUnitTests
{
    [TestFixture]
    public class UTestMoveFuel
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
                new MoveFuel(new MovableFuelAdapter(tank)),
                new BurnFuel(new BurnFuelAdapter(tank))
            };
            splitCommands = new MacroCommand(macroCommands);
                
        }

        [Test]
        public void TestMoveFuleSpent()
        {
            tank.SetProperty("fuelLevel", 10f);
            tank.SetProperty("fuelLowLevel", 0f);
            tank.SetProperty("fuelRate", 0f);
            tank.SetProperty("fuelRateMove", 2f);

            tank.SetProperty("position", new Vector3(12, 0, 5));
            tank.SetProperty("velocity", new Vector3(-7, 0, 3));

            splitCommands.Execute();

            Assert.IsTrue((float)tank.GetProperty("fuelLevel")== 8f, "Fuel does not spent");
            Assert.IsTrue((float)tank.GetProperty("fuelRate") == 0f, "FuelRate does not stop");
        }
    }
}
        

