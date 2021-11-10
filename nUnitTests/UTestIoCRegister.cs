using NUnit.Framework;
using OtusTankClear;
using System.Numerics;

namespace nUnitTests
{
    [TestFixture]
    public class UTestIoCRegister
    {
        Tank tank;
        ICommand register;
        [SetUp]
        public void Setup()
        {
            tank = new();
            register = new Register(new RegisterAdapter(tank));
            tank.SetProperty("IoC.Register", register);
        }

        [Test]
        public void TestRegisteCommand()
        {
            IoC.resolve<ICommand>("IoC.Register", tank, "IoC.Sort", IoC.DelegateToObject((args) => new QSort(new SortableAdapter((IUObject)args))), tank).Execute();
            Assert.IsNotNull(tank.GetProperty("IoC.Sort"));
        }
    }
}


