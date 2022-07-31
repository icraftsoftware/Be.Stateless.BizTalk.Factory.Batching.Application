# Be.Stateless.BizTalk.Factory.Batching.Application

##### Build Pipelines

[![][pipeline.mr.badge]][pipeline.mr]

[![][pipeline.ci.badge]][pipeline.ci]

##### Latest Release

[![][package.badge]][package]

[![][nuget.badge]][nuget]

[![][nuget.schemas.badge]][nuget.schemas]

[![][nuget.maps.badge]][nuget.maps]

[![][nuget.unit.badge]][nuget.unit]

[![][release.badge]][release]

##### Release Preview

<!-- TODO preview deployment packages -->

[![][nuget.preview.badge]][nuget.preview]

[![][nuget.schemas.preview.badge]][nuget.schemas.preview]

[![][nuget.maps.preview.badge]][nuget.maps.preview]

[![][nuget.unit.preview.badge]][nuget.unit.preview]

##### Documentation

[![][doc.main.badge]][doc.main]

[![][doc.this.badge]][doc.this]

[![][help.badge]][help]

[![][help.schemas.badge]][help.schemas]

[![][help.maps.badge]][help.maps]

[![][help.unit.badge]][help.unit]

## Overview

`BizTalk.Factory.Batching` is an application add-on for Microsoft BizTalk Server® that provides rich and performant batching capabilities. `BizTalk.Factory.Batching` Application Package is part of the larger [BizTalk.Factory][doc.main] SDK and is made of the following components:

- [Be.Stateless.BizTalk.Factory.Batching.Schemas][nuget.schemas], which is a `NuGet` package providing schemas definitions and who is obviously meant to be referenced at development time;
- [Be.Stateless.BizTalk.Factory.Batching.Maps][nuget.maps], which is a `NuGet` package providing message transformation maps and who is obviously meant to be referenced at development time;
- [Be.Stateless.BizTalk.Factory.Batching][nuget], which is a `NuGet` package providing all other components &mdash;e.g. context builders, micro components, customized activity tracker...&mdash; necessary for this application and who is obviously meant to be referenced at development time;
- [Be.Stateless.BizTalk.Factory.Batching.Application.Deployment.zip][package], which is the whole application add-on itself and is meant to be deployed on Microsoft BizTalk Server®.

<!-- badges -->

[doc.main.badge]: https://img.shields.io/static/v1?label=BizTalk.Factory%20SDK&message=User's%20Guide&color=8CA1AF&logo=readthedocs
[doc.main]: https://www.stateless.be/ "BizTalk.Factory SDK User's Guide"
[doc.this.badge]: https://img.shields.io/static/v1?label=Be.Stateless.BizTalk.Factory.Batching.Application&message=User's%20Guide&color=8CA1AF&logo=readthedocs
[doc.this]: https://www.stateless.be/BizTalk/Factory/Batching/Application "Be.Stateless.BizTalk.Factory.Batching.Application User's Guide"
[github.badge]: https://img.shields.io/static/v1?label=Repository&message=Be.Stateless.BizTalk.Pipelines&logo=github
[github]: https://github.com/icraftsoftware/Be.Stateless.BizTalk.Pipelines "Be.Stateless.BizTalk.Pipelines GitHub Repository"
[help.badge]: https://img.shields.io/static/v1?label=Be.Stateless.BizTalk.Batching&message=Developer%20Help&color=8CA1AF&logo=microsoftacademic
[help]: https://github.com/icraftsoftware/biztalk.factory.github.io/blob/master/Help/BizTalk/Batching/README.md "Be.Stateless.BizTalk.Batching Developer Help"
[help.maps.badge]: https://img.shields.io/static/v1?label=Be.Stateless.BizTalk.Batching.Maps&message=Developer%20Help&color=8CA1AF&logo=microsoftacademic
[help.maps]: https://github.com/icraftsoftware/biztalk.factory.github.io/blob/master/Help/BizTalk/Batching/Maps/README.md "Be.Stateless.BizTalk.Batching.Maps Developer Help"
[help.schemas.badge]: https://img.shields.io/static/v1?label=Be.Stateless.BizTalk.Batching.Schemas&message=Developer%20Help&color=8CA1AF&logo=microsoftacademic
[help.schemas]: https://github.com/icraftsoftware/biztalk.factory.github.io/blob/master/Help/BizTalk/Batching/Schemas/README.md "Be.Stateless.BizTalk.Batching.Schemas Developer Help"
[help.unit.badge]: https://img.shields.io/static/v1?label=Be.Stateless.BizTalk.Batching.Unit&message=Developer%20Help&color=8CA1AF&logo=microsoftacademic
[help.unit]: https://github.com/icraftsoftware/biztalk.factory.github.io/blob/master/Help/BizTalk/Batching/Unit/README.md "Be.Stateless.BizTalk.Batching.Unit Developer Help"
[nuget.badge]: https://img.shields.io/nuget/v/Be.Stateless.BizTalk.Batching.svg?label=Be.Stateless.BizTalk.Batching&style=flat&logo=nuget
[nuget]: https://www.nuget.org/packages/Be.Stateless.BizTalk.Batching "Be.Stateless.BizTalk.Batching NuGet Package"
[nuget.maps.badge]: https://img.shields.io/nuget/v/Be.Stateless.BizTalk.Batching.Maps.svg?label=Be.Stateless.BizTalk.Batching.Maps&style=flat&logo=nuget
[nuget.maps]: https://www.nuget.org/packages/Be.Stateless.BizTalk.Batching.Maps "Be.Stateless.BizTalk.Batching.Maps NuGet Package"
[nuget.schemas.badge]: https://img.shields.io/nuget/v/Be.Stateless.BizTalk.Batching.Schemas.svg?label=Be.Stateless.BizTalk.Batching.Schemas&style=flat&logo=nuget
[nuget.schemas]: https://www.nuget.org/packages/Be.Stateless.BizTalk.Batching.Schemas "Be.Stateless.BizTalk.Batching.Schemas NuGet Package"
[nuget.unit.badge]: https://img.shields.io/nuget/v/Be.Stateless.BizTalk.Batching.Unit.svg?label=Be.Stateless.BizTalk.Batching.Unit&style=flat&logo=nuget
[nuget.unit]: https://www.nuget.org/packages/Be.Stateless.BizTalk.Batching.Unit "Be.Stateless.BizTalk.Batching.Unit NuGet Package"
[nuget.preview.badge]: https://badge-factory.azurewebsites.net/package/icraftsoftware/be.stateless/BizTalk.Factory.Preview/Be.Stateless.BizTalk.Batching?logo=nuget
[nuget.preview]: https://dev.azure.com/icraftsoftware/be.stateless/_packaging?_a=package&feed=BizTalk.Factory.Preview&package=Be.Stateless.BizTalk.Batching&protocolType=NuGet "Be.Stateless.BizTalk.Batching Preview NuGet Package"
[nuget.maps.preview.badge]: https://badge-factory.azurewebsites.net/package/icraftsoftware/be.stateless/BizTalk.Factory.Preview/Be.Stateless.BizTalk.Batching.Maps?logo=nuget
[nuget.maps.preview]: https://dev.azure.com/icraftsoftware/be.stateless/_packaging?_a=package&feed=BizTalk.Factory.Preview&package=Be.Stateless.BizTalk.Batching.Maps&protocolType=NuGet "Be.Stateless.BizTalk.Batching.Maps Preview NuGet Package"
[nuget.schemas.preview.badge]: https://badge-factory.azurewebsites.net/package/icraftsoftware/be.stateless/BizTalk.Factory.Preview/Be.Stateless.BizTalk.Batching.Schemas?logo=nuget
[nuget.schemas.preview]: https://dev.azure.com/icraftsoftware/be.stateless/_packaging?_a=package&feed=BizTalk.Factory.Preview&package=Be.Stateless.BizTalk.Batching.Schemas&protocolType=NuGet "Be.Stateless.BizTalk.Batching.Schemas Preview NuGet Package"
[nuget.unit.preview.badge]: https://badge-factory.azurewebsites.net/package/icraftsoftware/be.stateless/BizTalk.Factory.Preview/Be.Stateless.BizTalk.Batching.Unit?logo=nuget
[nuget.unit.preview]: https://dev.azure.com/icraftsoftware/be.stateless/_packaging?_a=package&feed=BizTalk.Factory.Preview&package=Be.Stateless.BizTalk.Batching.Unit&protocolType=NuGet "Be.Stateless.BizTalk.Batching.Unit Preview NuGet Package"
[package.badge]: https://img.shields.io/github/v/release/icraftsoftware/Be.Stateless.BizTalk.Factory.Batching.Application?label=Be.Stateless.BizTalk.Factory.Batching.Application.Deployment.zip&style=flat&logo=github
[package]: https://github.com/icraftsoftware/Be.Stateless.BizTalk.Factory.Batching.Application/releases/latest/download/Be.Stateless.BizTalk.Factory.Batching.Application.Deployment.zip "Be.Stateless.BizTalk.Factory.Batching.Application Deployment Package"
[pipeline.ci.badge]: https://dev.azure.com/icraftsoftware/be.stateless/_apis/build/status/Be.Stateless.BizTalk.Factory.Batching.Application%20Continuous%20Integration?branchName=master&label=Continuous%20Integration%20Build
[pipeline.ci]: https://dev.azure.com/icraftsoftware/be.stateless/_build/latest?definitionId=94&branchName=master "Be.Stateless.BizTalk.Factory.Batching.Application Continuous Integration Build Pipeline"
[pipeline.mr.badge]: https://dev.azure.com/icraftsoftware/be.stateless/_apis/build/status/Be.Stateless.BizTalk.Factory.Batching.Application%20Manual%20Release?branchName=master&label=Manual%20Release%20Build
[pipeline.mr]: https://dev.azure.com/icraftsoftware/be.stateless/_build/latest?definitionId=95&branchName=master "Be.Stateless.BizTalk.Factory.Batching.Application Manual Release Build Pipeline"
[release.badge]: https://img.shields.io/github/v/release/icraftsoftware/Be.Stateless.BizTalk.Factory.Batching.Application?label=Release&logo=github
[release]: https://github.com/icraftsoftware/Be.Stateless.BizTalk.Factory.Batching.Application/releases/latest "Be.Stateless.BizTalk.Factory.Batching.Application GitHub Release"
