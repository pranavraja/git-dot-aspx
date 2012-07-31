
namespace GitAspx.ViewModels {
	using System.Collections.Generic;
	using System.IO;
	using GitAspx.Lib;

	public class TeamListViewModel {
		public string RepositoriesDirectory { get; set; }
		public IEnumerable<DirectoryInfo> Directories { get; set; }
	}
}