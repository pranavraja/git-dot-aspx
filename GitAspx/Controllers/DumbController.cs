namespace GitAspx.Controllers {
	using System;
	using System.IO;
	using System.Web.Mvc;
	using System.Web.SessionState;
	using GitAspx.Lib;

	[SessionState(SessionStateBehavior.Disabled)]
	public class DumbController : BaseController {
		readonly RepositoryService repositories;

		public DumbController(RepositoryService repositories) {
			this.repositories = repositories;
		}

        public ActionResult GetTextFile(string team, string project)
        {
			return WriteFile(team, project, "text/plain");
		}

        public ActionResult GetInfoPacks(string team, string project)
        {
            return WriteFile(team, project, "text/plain; charset=utf-8");
		}

        public ActionResult GetLooseObject(string team, string project)
        {
            return WriteFile(team, project, "application/x-git-loose-object");
		}

        public ActionResult GetPackFile(string team, string project)
        {
            return WriteFile(team, project, "application/x-git-packed-objects");
		}

        public ActionResult GetIdxFile(string team, string project)
        {
            return WriteFile(team, project, "application/x-git-packed-objects-toc");
		}

		private ActionResult WriteFile(string team, string project, string contentType) {
			Response.WriteNoCache();
			Response.ContentType = contentType;
			var repo = repositories.GetRepository(team, project);

			string path = Path.Combine(repo.GitDirectory(), GetPathToRead(project));

			if(! System.IO.File.Exists(path)) {
				return new NotFoundResult(string.Format("{0}/{1}", team, project));
			}

			Response.WriteFile(path);

			return new EmptyResult();
		}

		private string GetPathToRead(string project) {
			int index = Request.Url.PathAndQuery.IndexOf(project) + project.Length + 1;
			return Request.Url.PathAndQuery.Substring(index);
		}
	}
}