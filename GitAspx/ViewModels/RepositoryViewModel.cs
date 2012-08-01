namespace GitAspx.ViewModels {
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using GitAspx.Lib;

	public class RepositoryViewModel {
		private readonly Repository repository;

		public RepositoryViewModel(Repository repository) {
			this.repository = repository;
		}

		public string Name {
			get { return repository.Name; }
		}

	    public CommitInfoViewModel LatestCommit {
            get { return new CommitInfoViewModel(repository.GetLatestCommit()); }
	    }

	    public IEnumerable<CommitInfoViewModel> RecentCommits {
            get { return repository.GetRecentCommits(10).Select(commitInfo => new CommitInfoViewModel(commitInfo));  }
	    } 

	}
}