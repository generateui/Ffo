using System;
using System.Collections.Generic;
using System.IO;

namespace Ffo
{
    public sealed class Ffile
	{
		private string _absolutePath;

		public Ffile(IFfolder folder, string name)
		{
			Folder = folder;
			Name = name;
		}

		/// <summary>
		/// Folder where the file resides
		/// </summary>
		public IFfolder Folder { get; }

		/// <summary>
		/// Filename excluding path or path separator(s)
		/// </summary>
		public string Name { get; }

		public string NameWithoutExtension => Path.GetFileNameWithoutExtension(Name);

		// Extension

		/// <summary>
		/// True if the file exists on storage
		/// </summary>
		/// <returns></returns>
		public bool Exists() => File.Exists(FullPath);

        public static Ffile FromAbsolutePath(string absoluteFilePath)
        {
            var folder = new AbsoluteFfolder(Path.GetDirectoryName(absoluteFilePath));
            string name = Path.GetFileName(absoluteFilePath);
            return new Ffile(folder, name);
        }

		/// <summary>
		/// The full path to the file including the folder path
		/// </summary>
        public string FullPath
		{
			get
			{
				if (_absolutePath == null && Folder.Absolute != null)
				{
					if (Folder.Relative != null)
                    {
						_absolutePath = Path.Combine(Folder.Absolute.FullPath, Folder.Relative.RelativePath, Name);
					}
					else
                    {
						_absolutePath = Path.Combine(Folder.Absolute.FullPath, Name);
                    }
				}
				return _absolutePath;
			}
		}

		/// <summary>
		/// Relative path to the file if the folder has a relative path
		/// </summary>
		public string RelativePath
		{
			get
			{
				if (Folder == null || Folder.Relative == null)
				{
					return Name;
				}
				if (Folder.Relative != null)
				{
					return Path.Combine(Folder.Relative.RelativePath, Name);
				}
				throw new ArgumentException("File has no folder");
			}
		}
		public string RelativePathWithoutExtension
        {
			get
			{
				if (Folder == null || Folder.Relative == null)
				{
					return NameWithoutExtension;
				}
				if (Folder.Relative != null)
				{
					return Path.Combine(Folder.Relative.RelativePath, NameWithoutExtension);
				}
				throw new ArgumentException("File has no folder");
			}
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Ffile))
            {
				return false;
            }
			var other = obj as Ffile;
			if (!Equals(Folder, other.Folder))
            {
				return false;
            }
			if (!Equals(Name, other.Name))
            {
				return false;
            }
			return true;
        }
        public override int GetHashCode()
        {
            int hashCode = -1220008309;
            hashCode = hashCode * -1521134295 + EqualityComparer<IFfolder>.Default.GetHashCode(Folder);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Name);
            return hashCode;
        }
    }
}
