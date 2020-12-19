using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Linq;

namespace Ffo.UnitTest
{
    [TestClass]
    public class AbsoluteFfolderTest
    {
        private readonly string _current;

        public AbsoluteFfolderTest()
        {
            _current = Path.Combine(System.Environment.CurrentDirectory, "Base");
        }

        [TestMethod]
        public void Test()
        {
            var absoluteFolder = new AbsoluteFfolder(_current);
            var subFolders = absoluteFolder.GetFolders().ToList();

            Assert.AreEqual(1, subFolders.Count);
            Assert.IsTrue(subFolders.Contains(new RelativeFfolder(absoluteFolder, "Sub1")));
        }

        [TestMethod]
        public void Test1()
        {
            var absoluteFolder = new AbsoluteFfolder(_current);
            var subFolders = absoluteFolder.GetFolders("*.*", SearchOption.AllDirectories).ToList();

            Assert.AreEqual(3, subFolders.Count);
            Assert.IsTrue(subFolders.Contains(new RelativeFfolder(absoluteFolder, "Sub1")));
        }
    }
}
