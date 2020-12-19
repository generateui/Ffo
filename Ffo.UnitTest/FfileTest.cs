using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace Ffo.UnitTest
{
    [TestClass]
    public class FfileTest
    {
        private readonly string _current;

        public FfileTest()
        {
            _current = System.Environment.CurrentDirectory;
        }

        [TestMethod]
        public void Test()
        {
            var directory = Path.Combine(_current, "Base");
            var absoluteFolder = new AbsoluteFfolder(directory);
            var file = new Ffile(absoluteFolder, "file1.txt");
        }
    }
}
