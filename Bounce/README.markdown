# Bounce
A new build framework for C# projects.

(theme track: [Bounce, Rock, Skate, Roll - Vaughan Mason & Crew](http://www.youtube.com/watch?v=dGMD0O7GGP8&feature=related))

## Install
Get the latest release from the [downloads](http://github.com/refractalize/bounce/downloads) page, extract, place `bounce.exe` in your `%PATH%`, and place the DLLs in your project references. Too easy?

You also install the latest build  using [NuGet](http://nuget.org/List/Packages/Bounce-CI):

    PM> Install-Package Bounce-CI

Though, please note that this is still a work in progress.

## Why use Bounce?

For clean, beautiful build scripts! Bounce is a build framework based on functional
programming semantics: In Bounce, each build task is seen as a function that accepts arguments
(in the form of other tasks) and returns a built artefact that can be passed to yet other tasks. For example,
from git checkout to IIS deploy:

    var checkout = new GitCheckout {
        Repository = "git@github.com:refractalize/website.git"
    };
    
    var solution = new VisualStudioSolution {
        SolutionPath = checkout.Files["MySolution.sln"]
    };
    
    var website = new Iis7WebSite {
        Directory = solution.Projects["WebSite"].ProjectDirectory,
        Name = "Some Website",
        Port = 5001,
    }

Naturally, downstream tasks can use properties of built upstream tasks to perform their own builds, affording a refreshingly declarative style.

## Why C#?

Because we hack our production code in C#, it makes a whole lot of sense to hack our build in the same language and development environment.
That way we can reuse code, configuration and know-how between production and build - no language barriers!

## Getting Started

Lets say we've got a VisualStudio solution containing a website and you want it installed on IIS 7.0.
We'd write a C# file containing our targets like this:

	public class BuildTargets {
		[Targets]
		public static object Targets (IParameters parameters) {
			var solution = new VisualStudioSolution {
				SolutionPath = "WebSolution.sln",
			};
			var webProject = solution.Projects["WebSite"];

			return new {
				WebSite = new Iis7WebSite {
					Directory = webProject.ProjectDirectory,
					Name = "My Website",
					Port = 5001,
				},
				Tests = new NUnitTests {
					DllPaths = solution.Projects.Select(p => p.OutputFile),
				},
			};
		}
	}

The above code should be compiled into an assembly called `Targets.dll`, and into an output directory called `Bounce`.
This is how the `bounce` command will find our build configuration - it looks for `Bounce\Targets.dll` in the current
and all parent directories.

Then you can build your website:

    > bounce build WebSite

Or, just:

    > bounce WebSite

`build` is the default.

This code has a `Tests` target too, returned in the anonymous object returned from the `Targets` method. We can watch our tests pass (or not) with this command:

    > bounce Tests

If we're not sure what our build allows us, just run `bounce` alone and it will print our available targets:

    > bounce
	usage: bounce build|clean target-name

	targets:
	  WebSite
	  Tests

### Command-line Arguments

We can also change our build configuration from the command line, by passing in named arguments. Lets say we
want to specify the website port from the command line, but default it to 5001. We'll use the `IParameters` object
passed in to our `Targets` method:

    public static object Targets(IParameters parameters) {
    ...
        return new {
            WebSite = new Iis7WebSite {
                Directory = webProject.Directory,
                Name = "My Website",
                Port = parameters.Default("port", 5001),
            },
			...
		}
	}

Now we can build the website and override the port it will be deployed on:

	> bounce WebSite /port 80

And, `bounce` will tell you what arguments you have available too:

	> bounce
	usage: bounce build|clean target-name

	targets:
	  WebSite
	    /port default: 5001
	  Tests

## Housekeeping

Bounce is built using [CodeBetter's TeamCity Server](http://teamcity.codebetter.com/project.html?projectId=project132).

