using FlatFieldCalibration.Helper;

namespace FlatFieldCalibration.Test
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {
            VisionHelper.FFC(@"C:\Personal\Python\Vision\VisualApplets\FFC\dark.tif", @"C:\Personal\Python\Vision\VisualApplets\FFC\bright.tif");
        }
    }
}