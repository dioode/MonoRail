// Copyright 2004-2011 Castle Project - http://www.castleproject.org/
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

namespace Castle.MonoRail.WindsorExtension
{
	using System;
	using Core;
	using Framework;
	using Framework.Services;
	using MicroKernel;

	public class WindsorViewComponentFactory : AbstractViewComponentFactory, IInitializable
	{
		private readonly IViewComponentRegistry viewCompRegistry;
		private readonly IKernel kernel;

		public WindsorViewComponentFactory(IViewComponentRegistry viewCompRegistry, IKernel kernel)
		{
			this.viewCompRegistry = viewCompRegistry;
			this.kernel = kernel;
		}

		public override IViewEngine ViewEngine { get; set; }

		public override ViewComponent Create(String name)
		{
			var type = ResolveType(name);
			
			if (kernel.HasComponent(type))
			{
				return (ViewComponent) kernel.Resolve(type);
			}

			return (ViewComponent) Activator.CreateInstance(type);
		}

		protected override IViewComponentRegistry GetViewComponentRegistry()
		{
			return viewCompRegistry;
		}

		public override void Release(ViewComponent instance)
		{
			if (kernel.HasComponent(instance.GetType()))
			{
				kernel.ReleaseComponent(instance);
			}
			else
			{
				base.Release(instance);
			}
		}
	}
}
