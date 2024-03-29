# DynamicConfiguration

Manage your .NET application configuration dynamically.

[![Quality gate](https://sonarcloud.io/api/project_badges/quality_gate?project=Aguafrommars_DynamicConfiguration)](https://sonarcloud.io/dashboard?id=Aguafrommars_DynamicConfiguration)  

[![Build status](https://ci.appveyor.com/api/projects/status/ufot0jcsr2bw6dg4/branch/main?svg=true)](https://ci.appveyor.com/project/aguacongas/dynamicconfiguration/branch/main)

Nuget packages
--------------
|Services|Redis Configuration Provider|Web API|Razor components|
|:------:|:------:|:------:|:------:|
|[![][Services-badge]][Services-nuget]|[![][Redis-badge]][Redis-nuget]|[![][WepAPI-badge]][WepAPI-nuget]|[![][Razor-badge]][Razor-nuget]|
|[![][Services-downloadbadge]][Services-nuget]|[![][Redis-downloadbadge]][Redis-nuget]|[![][WepAPI-downloadbadge]][WepAPI-nuget]|[![][Razor-downloadbadge]][Razor-nuget]|

[Services-badge]: https://img.shields.io/nuget/v/Aguacongas.DynamicConfiguration.svg
[Services-downloadbadge]: https://img.shields.io/nuget/dt/Aguacongas.DynamicConfiguration.svg
[Services-nuget]: https://www.nuget.org/packages/Aguacongas.DynamicConfiguration/

[Redis-badge]: https://img.shields.io/nuget/v/Aguacongas.DynamicConfiguration.Redis.svg
[Redis-downloadbadge]: https://img.shields.io/nuget/dt/Aguacongas.DynamicConfiguration.Redis.svg
[Redis-nuget]: https://www.nuget.org/packages/Aguacongas.DynamicConfiguration.Redis/

[WepAPI-badge]: https://img.shields.io/nuget/v/Aguacongas.DynamicConfiguration.WebApi.svg
[WepAPI-downloadbadge]: https://img.shields.io/nuget/dt/Aguacongas.DynamicConfiguration.WebApi.svg
[WepAPI-nuget]: https://www.nuget.org/packages/Aguacongas.DynamicConfiguration.WebApi/

[Razor-badge]: https://img.shields.io/nuget/v/Aguacongas.DynamicConfiguration.Razor.svg
[Razor-downloadbadge]: https://img.shields.io/nuget/dt/Aguacongas.DynamicConfiguration.Razor.svg
[Razor-nuget]: https://www.nuget.org/packages/Aguacongas.DynamicConfiguration.Razor/


## Description

This repository contains the source code of .NET library to dynamicaly configure your .NET applications.

``` bash
├─ sample  
|  ├─ RedisConfigurationSample  
|  ├─ YarpSample
├─ src
|  ├─ Aguacongas.DynamicConfiguration
|  ├─ Aguacongas.DynamicConfiguration.Redis
|  ├─ Aguacongas.DynamicConfiguration.WebApi
|  └─ Blazor
|     └── Aguacongas.DynamicConfiguration.Razor
└─ test
```

### Services

[Aguacongas.DynamicConfiguration](src/Aguacongas.DynamicConfiguration/README.md) contains interfaces and services to dynamically configure .NET programs. 

### Providers

[Aguacongas.DynamicConfiguration.Redis](src/Aguacongas.DynamicConfiguration.Redis/README.md) contains a [Configuration provider](https://docs.microsoft.com/en-us/dotnet/core/extensions/configuration-providers) implementation for [Redis](https://redis.io/). 

### Web API
[Aguacongas.DynamicConfiguration.WebApi](src/Aguacongas.DynamicConfiguration.WebApi/README.md) contains a Web API to read and store the configuration. 

![configuration-API-get.jpeg](doc/assets/configuration-API.jpeg)

### Components

[Aguacongas.DynamicConfiguration.Razor](src/Blazor/Aguacongas.DynamicConfiguration.Razor/Readme.md) contains Razor components to manage the configuration.

![configuration-API-get.jpeg](doc/assets/settings-component.jpeg)

## Setup

Each project contains a *Readme.md* containing information to use the library.

## Build from source

You can build the solution with Visual Studio or use the `dotnet build` command.  

## Contribute

We warmly welcome contributions. You can contribute by opening an issue, suggest new a feature, or submit a pull request.

Read [How to contribute](CONTRIBUTING.md) and [Contributor Covenant Code of Conduct](CODE_OF_CONDUCT.md) for more information.
