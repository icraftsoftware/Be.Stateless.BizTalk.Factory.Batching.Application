#region Copyright & License

# Copyright © 2012 - 2022 François Chabot
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

#Requires -Modules @{ ModuleName = 'BizTalk.Deployment' ; ModuleVersion = '2.1.0.0' ; MaximumVersion = '2.2.0.0' ; GUID = '533b5f59-49ce-4f51-a293-cb78f5cf81b5' }

[CmdletBinding()]
[OutputType([HashTable])]
param(
   [Parameter(Mandatory = $false)]
   [ValidateNotNullOrEmpty()]
   [string]
   $EnvironmentSettingOverridesTypeName,

   [Parameter(Mandatory = $false)]
   [ValidateScript( { ($_ | Test-None) -or ($_ | Test-Path -PathType Container) } )]
   [string[]]
   $AssemblyProbingFolderPath = @(),

   [Parameter(Mandatory = $false)]
   [ValidateNotNullOrEmpty()]
   [string]
   $ProcessingServer = (Get-BizTalkGroupSettings).SubscriptionDBServerName
)

Set-StrictMode -Version Latest

ApplicationManifest -Name BizTalk.Factory.Batching -Description 'BizTalk.Factory''s batching application add-on for general purpose BizTalk Server development.' -Reference BizTalk.Factory -Build {
   Assembly -Path (Get-ResourceItem -Name Be.Stateless.BizTalk.Batching)
   Binding -Path (Get-ResourceItem -Name Be.Stateless.BizTalk.Factory.Batching.Binding) `
      -EnvironmentSettingOverridesTypeName $EnvironmentSettingOverridesTypeName `
      -AssemblyProbingFolderPath $AssemblyProbingFolderPath
   Map -Path (Get-ResourceItem -Name Be.Stateless.BizTalk.Batching.Maps)
   ProcessDescriptor -Path (Get-ResourceItem -Name Be.Stateless.BizTalk.Batching)
   Schema -Path (Get-ResourceItem -Name Be.Stateless.BizTalk.Batching.Schemas)
   SqlDeploymentScript -Path (Get-ResourceItem -Extension .sql -Name Create.Batching.Objects) -Server $ProcessingServer
   SqlUndeploymentScript -Path (Get-ResourceItem -Extension .sql -Name Drop.Batching.Objects) -Server $ProcessingServer
}
