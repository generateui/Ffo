using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;

namespace Ffo.UnitTest
{
    [TestClass]
    public class ExampleTest
    {
        private static readonly Action<string> WriteLine = text => System.Diagnostics.Debug.WriteLine(text);

        [TestMethod]
        public void Test()
        {
            var root = new AbsoluteFfolder(Directory.GetCurrentDirectory());
            var folder = new RelativeFfolder(root, "folder");
            foreach (var subFolder in folder.GetFolders("*.*", SearchOption.AllDirectories))
            {
                WriteLine(subFolder.Name);
                WriteLine("\t" + subFolder.RelativePath);
                WriteLine("\t" + subFolder.FullPath);
                WriteLine("\t" + subFolder.Absolute.FullPath);
            }

            foreach (var file in folder.GetFiles("*.*", SearchOption.AllDirectories))
            {
                WriteLine(file.Name);
                WriteLine("\t" + file.NameWithoutExtension);
                WriteLine("\t" + file.RelativePath);
                WriteLine("\t" + file.RelativePathWithoutExtension);
                WriteLine("\t" + file.FullPath);
                WriteLine("\t" + file.Folder.FullPath);
                WriteLine("\t" + file.Folder.Absolute.FullPath);
                WriteLine("\t" + file.Folder.Relative.FullPath);
            }
			/* output:
			subFolder1
				folder\subFolder1
				C:\Users\rpoutsma\Desktop\Markdown\Ffo\Ffo.UnitTest\bin\Debug\net48\folder\subFolder1
				C:\Users\rpoutsma\Desktop\Markdown\Ffo\Ffo.UnitTest\bin\Debug\net48
			subFolder2
				folder\subFolder2
				C:\Users\rpoutsma\Desktop\Markdown\Ffo\Ffo.UnitTest\bin\Debug\net48\folder\subFolder2
				C:\Users\rpoutsma\Desktop\Markdown\Ffo\Ffo.UnitTest\bin\Debug\net48
			subFolder1\subSubFolder
				folder\subFolder1\subSubFolder
				C:\Users\rpoutsma\Desktop\Markdown\Ffo\Ffo.UnitTest\bin\Debug\net48\folder\subFolder1\subSubFolder
				C:\Users\rpoutsma\Desktop\Markdown\Ffo\Ffo.UnitTest\bin\Debug\net48
			file1.txt
				file1
				folder\file1.txt
				folder\file1
				C:\Users\rpoutsma\Desktop\Markdown\Ffo\Ffo.UnitTest\bin\Debug\net48\folder\file1.txt
				C:\Users\rpoutsma\Desktop\Markdown\Ffo\Ffo.UnitTest\bin\Debug\net48\folder
				C:\Users\rpoutsma\Desktop\Markdown\Ffo\Ffo.UnitTest\bin\Debug\net48
				C:\Users\rpoutsma\Desktop\Markdown\Ffo\Ffo.UnitTest\bin\Debug\net48\folder
			file1-1.txt
				file1-1
				folder\subFolder1\file1-1.txt
				folder\subFolder1\file1-1
				C:\Users\rpoutsma\Desktop\Markdown\Ffo\Ffo.UnitTest\bin\Debug\net48\folder\subFolder1\file1-1.txt
				C:\Users\rpoutsma\Desktop\Markdown\Ffo\Ffo.UnitTest\bin\Debug\net48\folder\subFolder1
				C:\Users\rpoutsma\Desktop\Markdown\Ffo\Ffo.UnitTest\bin\Debug\net48
				C:\Users\rpoutsma\Desktop\Markdown\Ffo\Ffo.UnitTest\bin\Debug\net48\folder\subFolder1
			file1-1-1.txt
				file1-1-1
				folder\subFolder1\subSubFolder\file1-1-1.txt
				folder\subFolder1\subSubFolder\file1-1-1
				C:\Users\rpoutsma\Desktop\Markdown\Ffo\Ffo.UnitTest\bin\Debug\net48\folder\subFolder1\subSubFolder\file1-1-1.txt
				C:\Users\rpoutsma\Desktop\Markdown\Ffo\Ffo.UnitTest\bin\Debug\net48\folder\subFolder1\subSubFolder
				C:\Users\rpoutsma\Desktop\Markdown\Ffo\Ffo.UnitTest\bin\Debug\net48
				C:\Users\rpoutsma\Desktop\Markdown\Ffo\Ffo.UnitTest\bin\Debug\net48\folder\subFolder1\subSubFolder
			file2-1.txt
				file2-1
				folder\subFolder2\file2-1.txt
				folder\subFolder2\file2-1
				C:\Users\rpoutsma\Desktop\Markdown\Ffo\Ffo.UnitTest\bin\Debug\net48\folder\subFolder2\file2-1.txt
				C:\Users\rpoutsma\Desktop\Markdown\Ffo\Ffo.UnitTest\bin\Debug\net48\folder\subFolder2
				C:\Users\rpoutsma\Desktop\Markdown\Ffo\Ffo.UnitTest\bin\Debug\net48
				C:\Users\rpoutsma\Desktop\Markdown\Ffo\Ffo.UnitTest\bin\Debug\net48\folder\subFolder2
             */
		}
	}
}
