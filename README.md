# Pipeline Framework Dependency Injection

## What is it?

Pipeline Framework Dependency Injection is a set of libraries which extends the [Pipeline Framework](https://github.com/gtmoose32/pipeline-framework) in order to utilize some popular dependency injection (IOC) containers. 

Examples in the [wiki](https://github.com/gtmoose32/pipeline-framwork-di/wiki).

## Installing Pipeline Framework Dependency Injection packages
You should install the any of the libraries using nuget.

[Pipeline Framework with Autofac](https://www.nuget.org/packages/PipelineFramework.Autofac/):

```
Install-Package PipelineFramework.Autofac
```

[Pipeline Framework with LighInject](https://www.nuget.org/packages/PipelineFramework.LightInject/):

```
Install-Package PipelineFramework.LightInject
```

or via the .NET Core command line interface:

```
dotnet add package PipelineFramework.Autofact

dotnet add package PipelineFramework.LightInject
```

Either commands, from Package Manager Console or .NET Core CLI, will download and install PipelineFramework.Core and all required dependencies.
