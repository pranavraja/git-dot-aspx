namespace GitAspx.Lib {
	using System;
	using System.Configuration;
	using System.IO;

	public class AppSettings {
		public DirectoryInfo RepositoriesDirectory {
			get {
				var path = ConfigurationManager.AppSettings["RepositoriesDirectory"];
				if(string.IsNullOrEmpty(path)) {
					throw new InvalidOperationException("The 'Repositories' AppSetting has not been initialised.");
				}

				if(! Directory.Exists(path)) {
					throw new DirectoryNotFoundException(string.Format("Could not find the directory '{0}' which is configured as the directory of repositories.", path));
				}

				return new DirectoryInfo(path);
			}
		}

		public bool UploadPack {
			get {
				var raw = ConfigurationManager.AppSettings["UploadPack"];

				if(string.IsNullOrEmpty(raw)) {
					return false;
				}

				bool uploadPack;
				bool.TryParse(raw, out uploadPack);
				return uploadPack;
			}
		}

		public bool ReceivePack {
			get {
				var raw = ConfigurationManager.AppSettings["ReceivePack"];

				if (string.IsNullOrEmpty(raw)) {
					return false;
				}

				bool receivePack;
				bool.TryParse(raw, out receivePack);
				return receivePack;
			}
		}

	}

}