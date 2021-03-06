﻿#region License

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

namespace GitAspx {
	using System.Web;
	using System.Web.Mvc;
	using System.Web.Routing;
	using GitAspx.Lib;
	using StructureMap;
	using StructureMap.Configuration.DSL;

	public class MvcApplication : HttpApplication {
		public static void RegisterRoutes(RouteCollection routes) {
			routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
			routes.IgnoreRoute("favicon.ico");

            routes.MapRoute("TeamList", "", new { controller = "TeamList", action = "Index" });
            routes.MapRoute("DirectoryListCreate", "Create", new { controller = "DirectoryList", action = "Create" });
            routes.MapRoute("DirectoryList", "{team}", new { controller = "DirectoryList", action = "Index" });

            routes.MapRoute("info-refs", "{team}/{project}/info/refs",
			                new {controller = "InfoRefs", action = "Execute"},
			                new {method = new HttpMethodConstraint("GET")});

            routes.MapRoute("upload-pack", "{team}/{project}/git-upload-pack",
			                new {controller = "Rpc", action = "UploadPack"},
			                new {method = new HttpMethodConstraint("POST")});


            routes.MapRoute("receive-pack", "{team}/{project}/git-receive-pack",
			                new {controller = "Rpc", action = "ReceivePack"},
			                new {method = new HttpMethodConstraint("POST")});

			// Dumb protocol
            routes.MapRoute("get-text-file", "{team}/{project}/HEAD", new { controller = "Dumb", action = "GetTextFile" });
            routes.MapRoute("get-text-file2", "{team}/{project}/objects/info/alternates", new { controller = "Dumb", action = "GetTextFile" });
            routes.MapRoute("get-text-file3", "{team}/{project}/objects/info/http-alternates", new { controller = "Dumb", action = "GetTextFile" });

            routes.MapRoute("get-info-packs", "{team}/{project}/info/packs", new { controller = "Dumb", action = "GetInfoPacks" });

            routes.MapRoute("get-text-file4", "{team}/{project}/objects/info/{something}", new { controller = "Dumb", action = "GetTextFile" });

            routes.MapRoute("get-loose-object", "{team}/{project}/objects/{segment1}/{segment2}", 
				new {controller = "Dumb", action = "GetLooseObject"});

            routes.MapRoute("get-pack-file", "{team}/{project}/objects/pack/pack-{filename}.pack", 
				new { controller = "Dumb", action = "GetPackFile" });

            routes.MapRoute("get-idx-file", "{team}/{project}/objects/pack/pack-{filename}.idx", 
				new {controller = "Dumb", action = "GetIdxFile"});

            routes.MapRoute("project", "{team}/{project}", new { controller = "Repository", action="Index"});
		}

		protected void Application_Start() {
			AreaRegistration.RegisterAllAreas();

			RegisterRoutes(RouteTable.Routes);

			ObjectFactory.Initialize(cfg => cfg.AddRegistry(new AppRegistry()));
			ControllerBuilder.Current.SetControllerFactory(new StructureMapControllerFactory());
		}

		class AppRegistry : Registry {
			public AppRegistry() {
				For<AppSettings>()
					.Singleton()
					.Use(AppSettings.FromAppConfig);
			}
		}
	}
}