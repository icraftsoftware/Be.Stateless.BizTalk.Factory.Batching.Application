#region Copyright & License

# Copyright © 2012 - 2021 François Chabot
#
# Licensed under the Apache License, Version 2.0 (the "License");
# you may not use this file except in compliance with the License.
# You may obtain a copy of the License at
#
# http://www.apache.org/licenses/LICENSE-2.0
#
# Unless required by applicable law or agreed to in writing, software
# distributed under the License is distributed on an "AS IS" BASIS,
# WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.u
# See the License for the specific language governing permissions and
# limitations under the License.

#endregion

[CmdletBinding()]
[OutputType([hashtable])]
param(
   [Parameter(Mandatory = $false)]
   [ValidateNotNullOrEmpty()]
   [string]
   $EnvironmentSettingOverridesTypeName,

   [Parameter(Mandatory = $false)]
   [ValidateScript( { ($_ | Test-None) -or ($_ | Test-Path -PathType Container) } )]
   [string[]]
   $AssemblyProbingFolderPaths,

   [Parameter(Mandatory = $false)]
   [ValidateNotNullOrEmpty()]
   [string]
   $ManagementServer = $env:COMPUTERNAME,

   [Parameter(Mandatory = $false)]
   [ValidateNotNullOrEmpty()]
   [string]
   $ProcessingServer = $env:COMPUTERNAME
)

Set-StrictMode -Version Latest

ApplicationManifest -Name BizTalk.Factory.Batching -Description 'BizTalk.Factory''s batching application add-on for general purpose BizTalk Server development.' -Reference BizTalk.Factory -Build {
   Assembly -Path (Get-ResourceItem -Name Be.Stateless.BizTalk.Batching)
   Binding -Path (Get-ResourceItem -Name Be.Stateless.BizTalk.Factory.Batching.Binding) `
      -EnvironmentSettingOverridesTypeName $EnvironmentSettingOverridesTypeName `
      -AssemblyProbingFolderPaths $AssemblyProbingFolderPaths
   Map -Path (Get-ResourceItem -Name Be.Stateless.BizTalk.Batching.Maps)
   ProcessDescriptor -Path (Get-ResourceItem -Name Be.Stateless.BizTalk.Batching) -DatabaseServer $ManagementServer
   Schema -Path (Get-ResourceItem -Name Be.Stateless.BizTalk.Batching.Schemas)
   SqlDeploymentScript -Path (Get-ResourceItem -Extension .sql -Name Create.BatchingObjects) -Server $ProcessingServer
   SqlUndeploymentScript -Path (Get-ResourceItem -Extension .sql -Name Drop.BatchingObjects) -Server $ProcessingServer
}
