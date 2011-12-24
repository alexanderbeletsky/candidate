﻿using System;
using Candidate.Core.Model;
using Candidate.Core.Model.Configurations;

namespace Candidate.Core.Extensions
{
    public static class VisitorExtensions
    {
        public static void Visit(this VisualStudioConfiguration self, ConfigurationNodeVisitor visitor)
        {
            if (self == null)
            {
                throw new ArgumentNullException("SiteConfiguration");
            }

            self.Github.Visit(visitor);
            self.Solution.Visit(visitor);
            self.Iis.Visit(visitor);
        }

        static void Visit(this Pre self, ConfigurationNodeVisitor visitor)
        {
            if (self != null)
            {
                visitor.Visit(self);
            }
        }

        static void Visit(this Github self, ConfigurationNodeVisitor visitor)
        {
            if (self != null)
            {
                visitor.Visit(self);
            }

        }

        static void Visit(this Solution self, ConfigurationNodeVisitor visitor)
        {
            if (self != null)
            {
                visitor.Visit(self);
            }
        }

        static void Visit(this Iis self, ConfigurationNodeVisitor visitor)
        {
            if (self != null)
            {
                visitor.Visit(self);
            }
        }

        static void Visit(this Post self, ConfigurationNodeVisitor visitor)
        {
            if (self != null)
            {
                visitor.Visit(self);
            }
        }
    }
}