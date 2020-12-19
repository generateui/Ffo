using System.Collections.Generic;
using System.IO;

namespace Ffo
{
    public sealed class AbsoluteFfolder : IFfolder
    {
		public AbsoluteFfolder(string fullPath)
		{
			FullPath = fullPath;
		}

		public string FullPath { get; }
		public AbsoluteFfolder Absolute => this;
		public RelativeFfolder Relative => null;

		public bool Exists() => Directory.Exists(FullPath);

		public IEnumerable<RelativeFfolder> GetFolders(string searchPattern, SearchOption searchOption)
		{
            return GetFolders(Directory.EnumerateDirectories(FullPath, searchPattern, searchOption));
		}
		public IEnumerable<RelativeFfolder> GetFolders(string searchPattern)
		{
            return GetFolders(Directory.EnumerateDirectories(FullPath, searchPattern));
		}
		public IEnumerable<RelativeFfolder> GetFolders()
		{
            return GetFolders(Directory.EnumerateDirectories(FullPath));
		}
		private IEnumerable<RelativeFfolder> GetFolders(IEnumerable<string> fullFolderPaths)
		{
			foreach (var directory in fullFolderPaths)
			{
				var relativeName = directory.Substring(FullPath.Length + 1); // / or \
				yield return new RelativeFfolder(this, relativeName);
			}
		}

		public IEnumerable<Ffile> GetFiles(string searchPattern, SearchOption searchOption)
		{
			return GetFiles(Directory.EnumerateFiles(FullPath, searchPattern, searchOption));
		}
		public IEnumerable<Ffile> GetFiles(string searchPattern)
		{
			return GetFiles(Directory.EnumerateFiles(FullPath, searchPattern));
		}
		public IEnumerable<Ffile> GetFiles()
		{
			return GetFiles(Directory.EnumerateFiles(FullPath));
		}
		private IEnumerable<Ffile> GetFiles(IEnumerable<string> fullFilePaths)
		{
			foreach (var fullFilePath in fullFilePaths)
			{
                string filename = Path.GetFileName(fullFilePath);
                string folderPath = Path.GetDirectoryName(fullFilePath);
				if (folderPath.Length == FullPath.Length) // we're in the root
                {
					yield return new Ffile(this, filename);
				}
				else
                {
                    string relativeFolderPath = folderPath.Substring(FullPath.Length + 1);
					var relativeFolder = new RelativeFfolder(this, relativeFolderPath);
					yield return new Ffile(relativeFolder, filename);
                }
			}
		}

        public override bool Equals(object obj) => 
			obj is AbsoluteFfolder absolute && FullPath == absolute.FullPath;

        public override int GetHashCode() => FullPath.GetHashCode();
    }
}
