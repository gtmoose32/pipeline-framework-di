# Pipeline Framework Dependency Injection
[![Build status](https://dev.azure.com/gtmoose/Mathis%20Home/_apis/build/status/Pipeline%20Framework/Pipeline%20Framework%20DI%20-%20CICD)](https://dev.azure.com/gtmoose/Mathis%20Home/_build/latest?definitionId=7)

## What is it?

Pipeline Framework Dependency Injection is a set of libraries which extends the [Pipeline Framework](https://github.com/gtmoose32/pipeline-framework) to utilize some popular dependency injection (IOC) containers. 

Examples in the [wiki](https://github.com/gtmoose32/pipeline-framwork-di/wiki).

## Installing Pipeline Framework Dependency Injection packages
Install any of the libraries using nuget.

[![nuget](https://img.shields.io/nuget/v/PipelineFramework.Autofac.svg)](https://www.nuget.org/packages/PipelineFramework.Autofac/)

```
Install-Package PipelineFramework.Autofac
```

[![nuget](https://img.shields.io/nuget/v/PipelineFramework.LightInject.svg)](https://www.nuget.org/packages/PipelineFramework.LightInject/)

```
Install-Package PipelineFramework.LightInject
```

or via the .NET Core command line interface:

```
dotnet add package PipelineFramework.Autofact

dotnet add package PipelineFramework.LightInject
```

Either commands, from Package Manager Console or .NET Core CLI, will download and install PipelineFramework.Core and all required dependencies.
