﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Moq;
using NUnit.Framework;

namespace Bounce.Console.Tests
{
    [TestFixture]
    public class TargetsAssemblyArgumentsParserTest
    {
        [Test]
        public void ShouldParseTargetsFromParameterWithColon() {
            var finder = new Mock<ITargetsAssemblyFinder>();
            var parser = new TargetsAssemblyArgumentsParser(finder.Object);
            var targetsAndArgs = parser.GetTargetsAssembly(new[] {"/targets:one.dll", "other", "args"});

            Assert.That(targetsAndArgs.RemainingArguments, Is.EqualTo(new[] {"other", "args"}));
            Assert.That(targetsAndArgs.TargetsAssembly.Executable, Is.EqualTo("one.dll"));
            Assert.That(targetsAndArgs.TargetsAssembly.ExecutableType, Is.EqualTo(BounceDirectoryExecutableType.Targets));
        }

        [Test]
        public void ShouldParseTargetsFromParameter() {
            var finder = new Mock<ITargetsAssemblyFinder>();
            var parser = new TargetsAssemblyArgumentsParser(finder.Object);
            var targetsAndArgs = parser.GetTargetsAssembly(new[] {"/targets", "one.dll", "other", "args"});

            Assert.That(targetsAndArgs.RemainingArguments, Is.EqualTo(new[] {"other", "args"}));
            Assert.That(targetsAndArgs.TargetsAssembly.Executable, Is.EqualTo("one.dll"));
            Assert.That(targetsAndArgs.TargetsAssembly.ExecutableType, Is.EqualTo(BounceDirectoryExecutableType.Targets));
        }

        [Test]
        public void ShouldParseTargetsAndRecurseFromArguments() {
            var finder = new Mock<ITargetsAssemblyFinder>();
            var parser = new TargetsAssemblyArgumentsParser(finder.Object);
            var targetsAndArgs = parser.GetTargetsAssembly(new[] {"/targets", "one.dll", "/recurse", "other", "args"});

            Assert.That(targetsAndArgs.RemainingArguments, Is.EqualTo(new[] {"other", "args"}));
            Assert.That(targetsAndArgs.TargetsAssembly.Executable, Is.EqualTo("one.dll"));
            Assert.That(targetsAndArgs.TargetsAssembly.ExecutableType, Is.EqualTo(BounceDirectoryExecutableType.Targets));
            Assert.That(targetsAndArgs.Recurse);
        }

        [Test]
        public void ShouldParseRecurseFromArguments() {
            var finder = new Mock<ITargetsAssemblyFinder>();
            var bounceDirectoryExecutable = new BounceDirectoryExecutable();
            finder.Setup(f => f.FindTargetsAssembly()).Returns(bounceDirectoryExecutable);

            var parser = new TargetsAssemblyArgumentsParser(finder.Object);
            var targetsAndArgs = parser.GetTargetsAssembly(new[] {"/recurse", "other", "args"});

            Assert.That(targetsAndArgs.Recurse);
            Assert.That(targetsAndArgs.RemainingArguments, Is.EqualTo(new[] { "other", "args" }));
            Assert.That(targetsAndArgs.TargetsAssembly, Is.SameAs(bounceDirectoryExecutable));
        }

        [Test]
        public void ShouldFindTargetsIfNoTargetsParameterGiven() {
            var finder = new Mock<ITargetsAssemblyFinder>();
            var bounceDirectoryExecutable = new BounceDirectoryExecutable();
            finder.Setup(f => f.FindTargetsAssembly()).Returns(bounceDirectoryExecutable);

            var parser = new TargetsAssemblyArgumentsParser(finder.Object);
            var targetsAndArgs = parser.GetTargetsAssembly(new[] {"build", "SomeTarget", "/other", "args"});

            Assert.That(targetsAndArgs.RemainingArguments, Is.EqualTo(new[] {"build", "SomeTarget", "/other", "args"}));
            Assert.That(targetsAndArgs.TargetsAssembly, Is.SameAs(bounceDirectoryExecutable));
            Assert.That(targetsAndArgs.Recurse, Is.False);
        }

        [Test]
        public void ShouldThrowIfTargetsAssemblyNotFound() {
            var finder = new Mock<ITargetsAssemblyFinder>();
            finder.Setup(f => f.FindTargetsAssembly()).Returns((BounceDirectoryExecutable) null);

            var parser = new TargetsAssemblyArgumentsParser(finder.Object);
            Assert.That(() => parser.GetTargetsAssembly(new[] {"build", "SomeTarget", "/other", "args"}), Throws.InstanceOf(typeof(TargetsAssemblyNotFoundException)));
        }

        [Test]
        public void ShouldAttemptToFindAssemblyIfNoArgsGiven() {
            var finder = new Mock<ITargetsAssemblyFinder>();
            var bounceDirectoryExecutable = new BounceDirectoryExecutable();
            finder.Setup(f => f.FindTargetsAssembly()).Returns(bounceDirectoryExecutable);

            var parser = new TargetsAssemblyArgumentsParser(finder.Object);
            var targetsAndArgs = parser.GetTargetsAssembly(new string[0]);

            Assert.That(targetsAndArgs.RemainingArguments, Is.Empty);
            Assert.That(targetsAndArgs.TargetsAssembly, Is.SameAs(bounceDirectoryExecutable));
        }
    }
}
