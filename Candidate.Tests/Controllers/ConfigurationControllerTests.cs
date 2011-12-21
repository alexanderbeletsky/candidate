﻿using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Candidate.Areas.Dashboard.Controllers;
using Candidate.Areas.Dashboard.Models;
using Candidate.Core.Settings;
using Candidate.Core.Settings.Model;
using Moq;
using NUnit.Framework;
using SharpTestsEx;

namespace Candidate.Tests.Controllers
{
    [TestFixture]
    public class ConfigurationControllerTests
    {
        protected ConfigurationController Controller { get; set; }
        protected Mock<ISettingsManager> SettingsManager { get; set; }

        [SetUp]
        public void Setup()
        {
            SettingsManager = new Mock<ISettingsManager>();
            Controller = new ConfigurationController(SettingsManager.Object);
        }

        [Test]
        public void Index_Get_Returns_View()
        {
            // arrange

            // act
            var result = Controller.Index("testJob") as ViewResult;

            // assert
            result.Should().Not.Be.Null();
        }

        [Test]
        public void Github_Get_Returns_Model()
        {
            // arrange
            SettingsManager.Setup(s => s.ReadSettings<SitesConfigurationList>()).Returns(
                new SitesConfigurationList
                {
                    Configurations = new List<SiteConfiguration> { 
                    new SiteConfiguration { JobName = "testJob", Github = new GitHub() } }
                });

            // act
            var result = Controller.Github("testJob") as ViewResult;

            // assert
            Assert.That(result.Model, Is.Not.Null);
        }

        [Test]
        public void Github_Post_Creates_New_Settings_Section()
        {
            // arrange
            object savedObject = null;
            SettingsManager.Setup(s => s.ReadSettings<SitesConfigurationList>()).Returns(
                new SitesConfigurationList
                {
                    Configurations = new List<SiteConfiguration>()
                });
            SettingsManager.Setup(s => s.SaveSettings(It.IsAny<object>())).Callback<object>((o) => savedObject = o);

            // act
            var result = Controller.Github("testJob", new GitHub { Branch = "branch", Url = "url" });

            // assert
            var savedConfiguration = savedObject as SitesConfigurationList;
            var config = savedConfiguration.Configurations.Where(c => c.JobName == "testJob").Single();
            Assert.That(config.JobName, Is.EqualTo("testJob"));
            Assert.That(config.Github.Branch, Is.EqualTo("branch"));
            Assert.That(config.Github.Url, Is.EqualTo("url"));
        }

        [Test]
        public void Github_Post_Updates_Settings_Section()
        {
            // arrange
            object savedObject = null;
            SettingsManager.Setup(s => s.ReadSettings<SitesConfigurationList>()).Returns(
                new SitesConfigurationList
                {
                    Configurations = new List<SiteConfiguration>()
                    {
                        new SiteConfiguration { JobName = "testJob", Github = new GitHub { Branch = "branch", Url = "url" } }
                    }
                });
            SettingsManager.Setup(s => s.SaveSettings(It.IsAny<object>())).Callback<object>((o) => savedObject = o);

            // act
            var result = Controller.Github("testJob", new GitHub { Branch = "branch2", Url = "url2" });

            // assert
            var savedConfiguration = savedObject as SitesConfigurationList;
            var config = savedConfiguration.Configurations.Where(c => c.JobName == "testJob").Single();
            Assert.That(config.JobName, Is.EqualTo("testJob"));
            Assert.That(config.Github.Branch, Is.EqualTo("branch2"));
            Assert.That(config.Github.Url, Is.EqualTo("url2"));
        }

        [Test]
        public void Iis_Get_Returns_Model()
        {
            // arrange
            SettingsManager.Setup(s => s.ReadSettings<SitesConfigurationList>()).Returns(
                new SitesConfigurationList
                {
                    Configurations = new List<SiteConfiguration> { 
                    new SiteConfiguration { JobName = "testJob", Iis = new Iis() } }
                });

            // act
            var result = Controller.Iis("testJob") as ViewResult;

            // assert
            Assert.That(result.Model, Is.Not.Null);
        }

        [Test]
        public void Iis_Post_Creates_New_Settings_Section()
        {
            // arrange
            object savedObject = null;
            SettingsManager.Setup(s => s.ReadSettings<SitesConfigurationList>()).Returns(
                new SitesConfigurationList
                {
                    Configurations = new List<SiteConfiguration>()
                });
            SettingsManager.Setup(s => s.SaveSettings(It.IsAny<object>())).Callback<object>((o) => savedObject = o);

            // act
            var result = Controller.Iis("testJob", new Iis { SiteName = "site" });

            // assert
            var savedConfiguration = savedObject as SitesConfigurationList;
            var config = savedConfiguration.Configurations.Where(c => c.JobName == "testJob").Single();
            Assert.That(config.JobName, Is.EqualTo("testJob"));
            Assert.That(config.Iis.SiteName, Is.EqualTo("site"));
        }

        [Test]
        public void Iis_Post_Updates_Settings_Section()
        {
            // arrange
            object savedObject = null;
            SettingsManager.Setup(s => s.ReadSettings<SitesConfigurationList>()).Returns(
                new SitesConfigurationList
                {
                    Configurations = new List<SiteConfiguration>()
                    {
                        new SiteConfiguration { JobName = "testJob", Iis = new Iis { SiteName = "site" } }
                    }
                });
            SettingsManager.Setup(s => s.SaveSettings(It.IsAny<object>())).Callback<object>((o) => savedObject = o);

            // act
            var result = Controller.Iis("testJob", new Iis { SiteName = "site2" });

            // assert
            var savedConfiguration = savedObject as SitesConfigurationList;
            var config = savedConfiguration.Configurations.Where(c => c.JobName == "testJob").Single();
            Assert.That(config.JobName, Is.EqualTo("testJob"));
            Assert.That(config.Iis.SiteName, Is.EqualTo("site2"));
        }

        [Test]
        public void Delete_Get_Returns_Model()
        {
            // arrange
            // act
            var result = Controller.Delete("testJob") as ViewResult;

            // assert
            var model = result.Model as DeleteJobModel;
            Assert.That(model.JobName, Is.EqualTo("testJob"));
        }

        [Test]
        public void Delete_Post_Removes_Job_From_List()
        {
            // arrange
            object savedObject = null;
            SettingsManager.Setup(s => s.ReadSettings<SitesConfigurationList>()).Returns(
                new SitesConfigurationList { Configurations = new List<SiteConfiguration> { new SiteConfiguration { JobName = "testJob" } } }
                );

            SettingsManager.Setup(s => s.SaveSettings(It.IsAny<object>())).Callback<object>((o) => savedObject = o);

            // act
            var result = Controller.Delete(new DeleteJobModel { JobName = "testJob" }) as ViewResult;

            // assert
            var savedConfiguration = savedObject as SitesConfigurationList;
            var config = savedConfiguration.Configurations.Where(c => c.JobName == "testJob").SingleOrDefault();
            Assert.That(config, Is.Null);
            Assert.That(savedConfiguration.Configurations.Count, Is.EqualTo(0));
        }

        [Test]
        public void Post_Get_Returns_Model()
        {
            // arrange
            SettingsManager.Setup(s => s.ReadSettings<SitesConfigurationList>()).Returns(
                new SitesConfigurationList
                {
                    Configurations = new List<SiteConfiguration> { 
                                new SiteConfiguration { JobName = "testJob", Post = new Post { PostBatch = "deploy.bat" } } }
                });

            // act
            var result = Controller.Post("testJob") as ViewResult;

            // assert
            var model = result.Model as Post;
            Assert.That(model.PostBatch, Is.EqualTo("deploy.bat"));
        }

        [Test]
        public void Post_Post_Updates_Model()
        {
            // arrange
            object savedObject = null;
            SettingsManager.Setup(s => s.ReadSettings<SitesConfigurationList>()).Returns(
                new SitesConfigurationList
                {
                    Configurations = new List<SiteConfiguration> { 
                                new SiteConfiguration { JobName = "testJob", Post = new Post { PostBatch = "deploy.bat" } } }
                });
            SettingsManager.Setup(s => s.SaveSettings(It.IsAny<object>())).Callback<object>((o) => savedObject = o);

            // act
            Controller.Post("testJob", new Post { PostBatch = "newDeploy.bat" });

            // assert
            var savedConfiguration = savedObject as SitesConfigurationList;
            var config = savedConfiguration.Configurations.Where(c => c.JobName == "testJob").SingleOrDefault();
            Assert.That(config.Post.PostBatch, Is.EqualTo("newDeploy.bat"));
        }
    }
}
