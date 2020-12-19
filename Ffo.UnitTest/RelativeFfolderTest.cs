using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Linq;

namespace Ffo.UnitTest
{
    [TestClass]
    public class RelativeFfolderTest
    {
        private readonly string _current;

        public RelativeFfolderTest()
        {
            _current = System.Environment.CurrentDirectory;
        }

        [TestMethod]
        public void AbsoluteFolder_HasCorrectFullPath()
        {
            var directory = Path.Combine(_current, "Base");
            var absoluteFolder = new AbsoluteFfolder(directory);

            Assert.AreEqual(directory, absoluteFolder.FullPath);
            //Assert.AreEqual(directory, absoluteFolder.ToString());
        }

        [TestMethod]
        public void OneLevelRelativeFolder_HasCorrectFullPath()
        {
            var baseFolder = Path.Combine(_current, "Base");
            var expected = Path.Combine(_current, "Base", "Sub1");
            var absoluteFolder = new AbsoluteFfolder(baseFolder);
            var subFolder = new RelativeFfolder(absoluteFolder, "Sub1");

            Assert.AreEqual(expected, subFolder.FullPath);
        }

        [TestMethod]
        public void OneLevelMultiRelativeFolder_HasCorrectFullPath()
        {
            var baseFolder = Path.Combine(_current, "Base");
            var expected = Path.Combine(_current, "Base", "Sub1", "SubSub1");
            var absoluteFolder = new AbsoluteFfolder(baseFolder);
            var subFolder = new RelativeFfolder(absoluteFolder, @"Sub1\SubSub1");

            Assert.AreEqual(expected, subFolder.FullPath);
        }

        [TestMethod]
        public void TwoLevelRelativeFolder_HasCorrectFullPath()
        {
            var baseFolder = Path.Combine(_current, "Base");
            var expected = Path.Combine(_current, "Base", "Sub1", "SubSub1");
            var absoluteFolder = new AbsoluteFfolder(baseFolder);
            var subFolder = new RelativeFfolder(absoluteFolder, "Sub1");
            var subSubFolder = new RelativeFfolder(subFolder, "SubSub1");

            Assert.AreEqual(expected, subSubFolder.FullPath);
        }

        [TestMethod]
        public void ThreeLevelRelativeFolder_HasCorrectFullPath()
        {
            var baseFolder = Path.Combine(_current, "Base");
            var expected = Path.Combine(_current, "Base", "Sub1", "SubSub1", "SubSubSub1");
            var absoluteFolder = new AbsoluteFfolder(baseFolder);
            var subFolder = new RelativeFfolder(absoluteFolder, "Sub1");
            var subSubFolder = new RelativeFfolder(subFolder, "SubSub1");
            var subSubSubFolder = new RelativeFfolder(subSubFolder, "SubSubSub1");

            Assert.AreEqual(expected, subSubSubFolder.FullPath);
            Assert.AreEqual("SubSubSub1", subSubSubFolder.Name);
            Assert.AreEqual(subSubFolder, subSubSubFolder.Parent);
            Assert.AreEqual(baseFolder, subSubSubFolder.Absolute.FullPath);
        }


        [TestMethod]
        public void ThreeLevelAbsoluteFolder_HasCorrectFiles()
        {
            var absoluteFolder = new AbsoluteFfolder(_current);
            var relativeFolder = new RelativeFfolder(absoluteFolder, "Base");

            var files = relativeFolder.GetFiles("*.*", SearchOption.AllDirectories).ToList();

            Assert.AreEqual(4, files.Count);
            Assert.IsTrue(files.Contains(new Ffile(relativeFolder, "file1.txt")));
            Assert.IsTrue(files.Contains(new Ffile(new RelativeFfolder(relativeFolder, "Sub1"), "sfile1.txt")));
            Assert.IsTrue(files.Contains(new Ffile(new RelativeFfolder(relativeFolder, @"Sub1\SubSub1"), "ssfile1.txt")));
            Assert.IsTrue(files.Contains(new Ffile(new RelativeFfolder(relativeFolder, @"Sub1\SubSub1\SubSubSub1"), "sssfile1.txt")));
        }

        [TestMethod]
        public void ThreeLevelRelativeFolder_HasCorrectFiles()
        {
            var baseFolder = Path.Combine(_current, "Base");
            var absoluteFolder = new AbsoluteFfolder(baseFolder);

            var files = absoluteFolder.GetFiles("*.*", SearchOption.AllDirectories).ToList();

            Assert.AreEqual(4, files.Count);
            Assert.IsTrue(files.Contains(new Ffile(absoluteFolder, "file1.txt")));
            Assert.IsTrue(files.Contains(new Ffile(new RelativeFfolder(absoluteFolder, "Sub1"), "sfile1.txt")));
            Assert.IsTrue(files.Contains(new Ffile(new RelativeFfolder(absoluteFolder, @"Sub1\SubSub1"), "ssfile1.txt")));
            Assert.IsTrue(files.Contains(new Ffile(new RelativeFfolder(absoluteFolder, @"Sub1\SubSub1\SubSubSub1"), "sssfile1.txt")));
        }

        [TestMethod]
        public void IFfolderConstructor_ConstructsFromAbsolute()
        {
            var baseFolder = Path.Combine(_current, "Base");
            IFfolder absoluteFolder = new AbsoluteFfolder(baseFolder);
            var sub1 = new RelativeFfolder(absoluteFolder, "Sub1");

            Assert.AreEqual("Sub1", sub1.Relative.Name);
            Assert.AreEqual(baseFolder, sub1.Absolute.FullPath);
        }

        [TestMethod]
        public void IFfolderConstructor_ConstructsFromRelativeLevel1()
        {
            var baseFolder = Path.Combine(_current, "Base");
            IFfolder absoluteFolder = new AbsoluteFfolder(baseFolder);
            var sub1 = new RelativeFfolder(absoluteFolder, "Sub1");

            Assert.IsNull(sub1.Parent);
            Assert.AreEqual("Sub1", sub1.Name);
            Assert.AreEqual(baseFolder, sub1.Absolute.FullPath);
        }

        [TestMethod]
        public void IFfolderConstructor_ConstructsFromRelativeLevel2()
        {
            var baseFolder = Path.Combine(_current, "Base");
            IFfolder absoluteFolder = new AbsoluteFfolder(baseFolder);
            IFfolder sub1 = new RelativeFfolder(absoluteFolder, "Sub1");
            var subSub1 = new RelativeFfolder(sub1, "SubSub1");

            Assert.AreEqual(sub1, subSub1.Parent);

            Assert.AreEqual("SubSub1", subSub1.Name);
            Assert.AreEqual(baseFolder, sub1.Absolute.FullPath);
            Assert.AreEqual(baseFolder, subSub1.Absolute.FullPath);
        }

        [TestMethod]
        public void IFfolderConstructor_ConstructsFromRelativeLevel3()
        {
            var baseFolder = Path.Combine(_current, "Base");
            IFfolder absoluteFolder = new AbsoluteFfolder(baseFolder);
            RelativeFfolder sub1 = new RelativeFfolder(absoluteFolder, "Sub1");
            RelativeFfolder subSub1 = new RelativeFfolder(sub1, "SubSub1");
            var subSubSub1 = new RelativeFfolder(subSub1 as IFfolder, "SubSubSub1");

            Assert.IsNotNull(sub1.Relative);
            Assert.IsNotNull(subSubSub1.Relative);
            Assert.IsNotNull(subSub1.Relative);

            Assert.AreEqual(sub1, subSub1.Parent);
            Assert.AreEqual(subSub1, subSubSub1.Parent);

            Assert.AreEqual("SubSubSub1", subSubSub1.Name);
            Assert.AreEqual(baseFolder, sub1.Absolute.FullPath);
            Assert.AreEqual(baseFolder, subSub1.Absolute.FullPath);
            Assert.AreEqual(baseFolder, subSubSub1.Absolute.FullPath);
        }
    }
}
