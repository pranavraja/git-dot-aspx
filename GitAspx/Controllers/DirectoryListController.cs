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

namespace GitAspx.Controllers {
    using System.IO;
    using System.Web.Mvc;
	using GitAspx.Lib;
	using GitAspx.ViewModels;
	using System.Linq;
    using GitSharp.Core.Exceptions;

    public class DirectoryListController : Controller {
		readonly RepositoryService repositories;

		public DirectoryListController(RepositoryService repositories) {
			this.repositories = repositories;
		}

		public ActionResult Index(string team) {
		    var teamDirectory = repositories.GetRepositoriesDirectory(team);
            if (teamDirectory == null) return new NotFoundResult("Team");
            return View(new DirectoryListViewModel {
				RepositoriesDirectory = teamDirectory.Name,
				Repositories = repositories.GetAllRepositories(team).Select(x => new RepositoryViewModel(x))
			});
		}

		[HttpPost]
		public ActionResult Create(string directory, string project) {
            if (!string.IsNullOrEmpty(project) && !string.IsNullOrEmpty(directory))
            {
                repositories.CreateRepository(directory, project);
			}
			return Redirect("/" + Path.GetFileName(directory));
		}
	}
}