using System;
using System.Collections.Generic;
using System.IO;

namespace Ffo
{
    public sealed class RelativeFfolder : IFfolder
    {
        private string _fullPath;
        private string _relativePath;

        public RelativeFfolder(string name)
		{
			Name = name;
		}
		public RelativeFfolder(RelativeFfolder baseFolder, string name)
        {
			Parent = baseFolder;
            RelativeFfolder parent = Parent;
			while (parent != null)
            {
				if (parent.Absolute != null)
                {
					Absolute = parent.Absolute;
					break;
                }
				parent = parent.Parent;
            }
			Name = name;
        }
		public RelativeFfolder(AbsoluteFfolder baseFolder, string name)
		{
			Absolute = baseFolder;
			Name = name;
		}
		public RelativeFfolder(IFfolder folder, string name)
        {
			if (folder.Relative == null && folder.Absolute == null)
            {
				throw new ArgumentException("absolute and relative are null");
            }
			Name = name;
			if (folder.Relative == null && folder.Absolute != null)
            {
				Absolute = folder.Absolute;
				return;
            }
			if (folder.Relative != null && folder.Absolute == null)
			{
				Parent = folder.Relative;
				Absolute = GetAbsolute(Parent);
				return;
			}
			if (folder.Relative != null && folder.Absolute != null)
			{
				Parent = folder.Relative;
				Absolute = folder.Absolute;
				return;
			}
		}

		private static AbsoluteFfolder GetAbsolute(RelativeFfolder relativeFfolder)
        {
			var relative = relativeFfolder;
			AbsoluteFfolder absolute = null;
			while (relative != null)
            {
				absolute = relative.Absolute;
				relative = relative.Parent;
            }
			return absolute;
        }

		public string Name { get; }

		// includes path of parent RelativeFfolder
		public string RelativePath
        {
			get
            {
				if (_relativePath == null)
                {
					_relativePath = Name;
					RelativeFfolder parent = Parent;
					while (parent != null)
					{
						_relativePath = parent.Name + @"\" + _relativePath;
						parent = parent.Parent;
					}
				}
				return _relativePath;
            }
        }

		public string FullPath
		{
			get
			{
				if (_fullPath == null && Absolute != null)
				{
					_fullPath = Absolute.FullPath + @"\" + RelativePath;
				}
				return _fullPath;
				// TODO: throw if no absolute?
			}
		}

		// can be null
        public RelativeFfolder Parent { get; }

        /// <summary>
        /// Optional absolute base folder
        /// </summary>
        public AbsoluteFfolder Absolute { get; }

        public AbsoluteFfolder ToAbsolute() => new AbsoluteFfolder(FullPath);

        public RelativeFfolder Relative => this;

		public RelativeFfolder RelativePathWithoutRelativeRoot
        {
            get
			{
				RelativeFfolder result = null;
				RelativeFfolder current = this;
				while (current.Parent != null)
				{
					result = result == null ? new RelativeFfolder(current.Name) : new RelativeFfolder(result, current.Name);
					current = current.Parent;
				}
				return result;
            }
        }

		public bool Exists()
        {
			if (Absolute == null)
            {
				throw new Exception("No Absolute folder");
            }
			return Directory.Exists(FullPath);
        }

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
		private IEnumerable<RelativeFfolder> GetFolders(IEnumerable<string> folders)
		{
			foreach (var directory in folders)
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
				if (Absolute.FullPath.Length + 1 + RelativePath.Length == folderPath.Length)
                {
					yield return new Ffile(this, filename); // we're in the root
                }
				else
                {
					var subFolderName = folderPath.Substring(Absolute.FullPath.Length + 1 + RelativePath.Length + 1);
					var relativeFolder = new RelativeFfolder(this, subFolderName);
					yield return new Ffile(relativeFolder, filename);
				}
			}
        }
		public override bool Equals(object obj)
        {
            if (!(obj is RelativeFfolder))
            {
				return false;
            }
			var other = obj as RelativeFfolder;
			if (other.Name != Name)
            {
				return false;
            }
			if (other.Absolute != Absolute)
            {
				return false;
            }
			if (other.FullPath != FullPath)
            {
				return false;
            }
			if (other.Parent != Parent)
            {
				return false;
            }
			return true;
        }

        public override int GetHashCode()
        {
            int hashCode = 432676202;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(RelativePath);
            hashCode = hashCode * -1521134295 + EqualityComparer<AbsoluteFfolder>.Default.GetHashCode(Absolute);
            return hashCode;
        }
    }
}
