using NUnit.Framework;
using OtusTankClear;
using System.Numerics;

namespace nUnitTests
{
    [TestFixture]
    public class UTestFuel
    {
        Tank tank;
        ICommand command;

        [SetUp]
        public void Setup()
        {
            tank = new();              
        }

        [Test]
        public void TestFuelCheck()
        {

            command = new CheckFuel(new CheckFuelAdapter(tank));
            tank.SetProperty("fuelLevel", 5f);
            tank.SetProperty("fuelLowLevel", 2f);           
            tank.SetProperty("fuelRate", 1f);

            Assert.DoesNotThrow(()=>command.Execute(), "Error of calc normal");

            tank.SetProperty("fuelRate", 4f);

            Assert.Catch(() => command.Execute(), "Error of calc empty");
        }
        [Test]
        public void TestFuelBurn()
        {

            command = new BurnFuel(new BurnFuelAdapter(tank));
            tank.SetProperty("fuelLevel", 5f);
            tank.SetProperty("fuelLowLevel", 2f);
            tank.SetProperty("fuelRate", 3f);
            command.Execute();

            Assert.IsTrue((float)tank.GetProperty("fuelLevel")==2f);
            Assert.IsTrue((float)tank.GetProperty("fuelRate") == 0f);
        }
    }
}
        

