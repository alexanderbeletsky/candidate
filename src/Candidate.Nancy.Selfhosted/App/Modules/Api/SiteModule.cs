﻿using System;
using Candidate.Core.Model;
using Nancy;
using Nancy.ModelBinding;
using Raven.Client;

namespace Candidate.Nancy.Selfhosted.App.Modules.Api
{
    public class SiteModule : NancyModule
    {
        private readonly IDocumentStore _documentStore;

        public SiteModule(IDocumentStore documentStore)
            : base("/api/site")
        {
            _documentStore = documentStore;

            Post["/"] = parameters =>
                            {
                                var site = this.Bind<Site>("name", "description");

                                site.Created = DateTime.UtcNow;
                                site.Status = "Created";

                                using (var session = _documentStore.OpenSession())
                                {
                                    session.Store(site);
                                    session.SaveChanges();
                                }

                                return Response.AsJson(site);
                            };
        }
    }
}