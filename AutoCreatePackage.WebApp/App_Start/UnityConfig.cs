using System.Web.Mvc;
using Microsoft.Practices.Unity;
using Unity.Mvc5;
using AutoCreatePackage.IBLL;
using AutoCreatePackage.BLL;

namespace AutoCreatePackage.WebApp
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();
            
            // register all your components with the container here
            // it is NOT necessary to register your controllers
            
            // e.g. container.RegisterType<ITestService, TestService>();
            container.RegisterType<IPackageInfoService, PackageInfoService>();
            
            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}