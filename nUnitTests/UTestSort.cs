using NUnit.Framework;
using OtusTankClear;
using System.Numerics;

namespace nUnitTests
{
    [TestFixture]
    public class UTestSort
    {
        Tank tank;
        ICommand command;

        [SetUp]
        public void Setup()
        {
            tank = new();              
        }

        [Test]
        public void TestQSort()
        {

            command = new QSort(new SortableAdapter(tank));
            tank.SetProperty("sourcePath", 5f);
            tank.SetProperty("resultPath", 2f);           

            Assert.DoesNotThrow(()=>command.Execute(), "Error of calc normal");

            tank.SetProperty("fuelRate", 4f);

            Assert.Catch(() => command.Execute(), "Error of calc empty");
        }
    }
}
        

