﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace api
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.Routes.MapHttpRoute(
                name: "api",
                routeTemplate: "{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            config.Routes.MapHttpRoute(
                name: "apiCustom",
                routeTemplate: "{controller}/{action}/{id}",
                defaults: new { action = "GET", id = RouteParameter.Optional }
            );
        }
    }
}
