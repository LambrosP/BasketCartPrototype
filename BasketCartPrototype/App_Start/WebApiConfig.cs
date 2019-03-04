using BasketCartPrototype.Core.Interfaces.Cache;
using BasketCartPrototype.Core.Interfaces.Services;
using BasketCartPrototype.Data.Implementation.Context;
using BasketCartPrototype.Data.Interfaces;
using BasketCartPrototype.Service.Implementation;
using BasketCartPrototype.Service.Implementation.Cache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Unity;
using Unity.Injection;
using Unity.Lifetime;
using WebApiDepInject.Models;

namespace BasketCartPrototype
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            var container = new UnityContainer();
            container.RegisterType<ICacheManager, CacheManager>(new ContainerControlledLifetimeManager());
            container.RegisterType<IBasketContext, BasketContext>(new ContainerControlledLifetimeManager(), 
                new InjectionConstructor(new ResolvedParameter<ICacheManager>()));
            container.RegisterType<IProductContext, ProductContext>(new ContainerControlledLifetimeManager(), 
                new InjectionConstructor(new ResolvedParameter<ICacheManager>()));
            container.RegisterType<IBasketCartService, BasketCartService>(new ContainerControlledLifetimeManager(),
                new InjectionConstructor(new ResolvedParameter<ICacheManager>(), new ResolvedParameter<IProductContext>(),
                new ResolvedParameter<IBasketContext>()));

            config.DependencyResolver = new UnityResolver(container);

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
