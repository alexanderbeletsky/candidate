﻿using System.Web.Mvc;

namespace Candidate.Areas.Configuration
{
    public class ConfigurationAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Configuration";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Configuration_Simple",
                "configuration/{action}/{id}",
                new { controller = "Configuration", action = "Configure", id = UrlParameter.Optional }
            );

            context.MapRoute(
                "Configuration_Configure",
                "configuration/configure/{id}",
                new { controller = "Configuration", action = "Configure" }
            );

            context.MapRoute(
                "Configuration_Default",
                "configuration/{controller}/{action}/{id}",
                new { controller = "Configuration", action = "Configure", id = UrlParameter.Optional }
            );
        }
    }
}