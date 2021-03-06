#region License

// Copyright 2010 Jeremy Skinner (http://www.jeremyskinner.co.uk)
//  
// Licensed under the Apache License, Version 2.0 (the "License"); 
// you may not use this file except in compliance with the License. 
// You may obtain a copy of the License at 
// 
// http://www.apache.org/licenses/LICENSE-2.0 
// 
// Unless required by applicable law or agreed to in writing, software 
// distributed under the License is distributed on an "AS IS" BASIS, 
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. 
// See the License for the specific language governing permissions and 
// limitations under the License.
// 
// The latest version of this file can be found at http://github.com/JeremySkinner/git-dot-aspx

#endregion

namespace GitAspx.Lib {
	using System.Collections.Generic;
	using System.IO;
	using System.Linq;

	public class RepositoryService {
		readonly AppSettings appSettings;

		public RepositoryService(AppSettings appSettings) {
			this.appSettings = appSettings;
		}

        public DirectoryInfo GetRootDirectory() {
            return appSettings.RepositoriesDirectory;
        }

		public IEnumerable<Repository> GetAllRepositories(string team) {
			return GetRepositoriesDirectory(team)
				.GetDirectories()
				.Select(Repository.Open)
				.Where(x => x!=null)
				.ToList();
		}

		public Repository GetRepository(string team, string project) {
			var directory = Path.Combine(appSettings.RepositoriesDirectory.FullName, team, project);

			if (!Directory.Exists(directory)) {
				return null;
			}

			//return Repository.Open(directory);
			return new Repository(new DirectoryInfo(directory));
		}

        public DirectoryInfo GetRepositoriesDirectory(string team)
        {
			return appSettings.RepositoriesDirectory.GetDirectories(team).FirstOrDefault();
		}

        public void CreateRepository(string directory, string project)
        {
			var repositoryDirectory = Path.Combine(directory, project + ".git");

            if (!Directory.Exists(repositoryDirectory))
            {
                Directory.CreateDirectory(repositoryDirectory);

                using (var repository = new GitSharp.Core.Repository(new DirectoryInfo(repositoryDirectory)))
                {
					repository.Create(true);
				}
			}
		}
	}
}