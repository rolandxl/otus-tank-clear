using NUnit.Framework;
using OtusTankClear;
using System.Numerics;

namespace nUnitTests
{
    [TestFixture]
    public class TestRotate
    {
        Tank tank;
        ICommand rotate;
        [SetUp]
        public void Setup()
        {
            tank = new();
            rotate = new Rotate(new RotateableAdapter(tank));
           // tank.ClearPropertys();
        }

        [Test]
        public void TestNullRotation()
        {
            tank.SetProperty("angle", 30f);
            tank.SetProperty("axis", new Vector3(0, 1, 0));
            Assert.Catch(()=>rotate.Execute(), "Exeption not recived");
        }
        [Test]
        public void TestNullAngle()
        {
            tank.SetProperty("rotation", new Quaternion());
            tank.SetProperty("axis", new Vector3(0, 1, 0));
            Assert.Catch(() => rotate.Execute(), "Exeption not recived");
        }
        [Test]
        public void TestNullAxis()
        {
            tank.SetProperty("rotation", new Quaternion());
            tank.SetProperty("angle", 30f);
            Assert.Catch(() => rotate.Execute(), "Exeption not recived");
        }
        [Test]
        public void TestSimpleRotate()
        {
            tank.SetProperty("rotation", new Quaternion(1, 1, 1, 1));
            tank.SetProperty("angle", 30f);
            tank.SetProperty("axis", new Vector3(0, 1, 0));
            rotate.Execute();
            Assert.IsTrue((Quaternion)tank.GetProperty("rotation") == new Quaternion(-1.4099758f, -0.109400034f, -0.109400034f, -1.4099758f));
        }
    }
}
        

