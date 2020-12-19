using JetBrains.Annotations;
using System.Collections.Generic;
using System.IO;

namespace Ffo
{
    // TODO: what about enumerating all files in a folder: each (sub) folder is also relativeFolder reference
    // i.e. foreach (var file in folder)
    //           file.folder. <-- relative

    /// <summary>
    /// Represent a folder on disk with relative & absolute path support
    /// </summary>
    /// This enables passing a reference to a folder once and have the caller 
    /// decide to use relative or absolute paths
    public interface IFfolder
	{
		/// <summary>
		/// Full path to folder on disk
		/// </summary>
		[CanBeNull] AbsoluteFfolder Absolute { get; }

        [CanBeNull] RelativeFfolder Relative { get; }

		IEnumerable<RelativeFfolder> GetFolders();
		IEnumerable<RelativeFfolder> GetFolders(string searchPattern);
		IEnumerable<RelativeFfolder> GetFolders(string searchPattern, SearchOption searchOption);
		IEnumerable<Ffile> GetFiles();
		IEnumerable<Ffile> GetFiles(string searchPattern);
		IEnumerable<Ffile> GetFiles(string searchPattern, SearchOption searchOption);

		string FullPath { get; }
	}
}
