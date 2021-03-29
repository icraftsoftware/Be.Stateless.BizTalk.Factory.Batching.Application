/*
 Copyright © 2012 - 2021 François Chabot

 Licensed under the Apache License, Version 2.0 (the "License");
 you may not use this file except in compliance with the License.
 You may obtain a copy of the License at

 http://www.apache.org/licenses/LICENSE-2.0

 Unless required by applicable law or agreed to in writing, software
 distributed under the License is distributed on an "AS IS" BASIS,
 WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 See the License for the specific language governing permissions and
 limitations under the License.
 */

USE [BizTalkFactoryTransientStateDb]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_batch_Register]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[usp_batch_Register]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_batch_Unregister]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[usp_batch_Unregister]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_batch_AddPart]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[usp_batch_AddPart]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_batch_QueueControlledRelease]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[usp_batch_QueueControlledRelease]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_batch_ReleaseNextBatch]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[usp_batch_ReleaseNextBatch]
GO

IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[vw_batch_AvailableBatchMonitor]'))
DROP VIEW [dbo].[vw_batch_AvailableBatchMonitor]
GO

IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[vw_batch_NextAvailableBatch]'))
DROP VIEW [dbo].[vw_batch_NextAvailableBatch]
GO

IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[vw_batch_ReleasePolicies]'))
DROP VIEW [dbo].[vw_batch_ReleasePolicies]
GO

IF  EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_batch_Parts_Partition]') AND parent_object_id = OBJECT_ID(N'[dbo].[batch_Parts]'))
Begin
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_batch_Parts_Partition]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[batch_Parts] DROP CONSTRAINT [DF_batch_Parts_Partition]
END
End
GO
IF  EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_batch_Parts_Timestamp]') AND parent_object_id = OBJECT_ID(N'[dbo].[batch_Parts]'))
Begin
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_batch_Parts_Timestamp]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[batch_Parts] DROP CONSTRAINT [DF_batch_Parts_Timestamp]
END
End
GO
IF  EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_batch_QueuedControlledReleases_Partition]') AND parent_object_id = OBJECT_ID(N'[dbo].[batch_QueuedControlledReleases]'))
Begin
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_batch_QueuedControlledReleases_Partition]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[batch_QueuedControlledReleases] DROP CONSTRAINT [DF_batch_QueuedControlledReleases_Partition]
END
End
GO
IF  EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_batch_QueuedControlledReleases_Timestamp]') AND parent_object_id = OBJECT_ID(N'[dbo].[batch_QueuedControlledReleases]'))
Begin
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_batch_QueuedControlledReleases_Timestamp]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[batch_QueuedControlledReleases] DROP CONSTRAINT [DF_batch_QueuedControlledReleases_Timestamp]
END
End
GO
IF  EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_batch_ReleasePolicyDefinitions_Partition]') AND parent_object_id = OBJECT_ID(N'[dbo].[batch_ReleasePolicyDefinitions]'))
Begin
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_batch_ReleasePolicyDefinitions_Partition]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[batch_ReleasePolicyDefinitions] DROP CONSTRAINT [DF_batch_ReleasePolicyDefinitions_Partition]
END
End
GO
IF  EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_batch_ReleasePolicyDefinitions_EnforceItemCountLimitOnRelease]') AND parent_object_id = OBJECT_ID(N'[dbo].[batch_ReleasePolicyDefinitions]'))
Begin
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_batch_ReleasePolicyDefinitions_EnforceItemCountLimitOnRelease]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[batch_ReleasePolicyDefinitions] DROP CONSTRAINT [DF_batch_ReleasePolicyDefinitions_EnforceItemCountLimitOnRelease]
END
End
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_batch_Parts_batch_Envelopes]') AND parent_object_id = OBJECT_ID(N'[dbo].[batch_Parts]'))
ALTER TABLE [dbo].[batch_Parts] DROP CONSTRAINT [FK_batch_Parts_batch_Envelopes]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_batch_QueuedControlledReleases_batch_Envelopes]') AND parent_object_id = OBJECT_ID(N'[dbo].[batch_QueuedControlledReleases]'))
ALTER TABLE [dbo].[batch_QueuedControlledReleases] DROP CONSTRAINT [FK_batch_QueuedControlledReleases_batch_Envelopes]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_batch_ReleasePolicyDefinitions_batch_Envelopes]') AND parent_object_id = OBJECT_ID(N'[dbo].[batch_ReleasePolicyDefinitions]'))
ALTER TABLE [dbo].[batch_ReleasePolicyDefinitions] DROP CONSTRAINT [FK_batch_ReleasePolicyDefinitions_batch_Envelopes]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[batch_Parts]') AND type in (N'U'))
DROP TABLE [dbo].[batch_Parts]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[batch_QueuedControlledReleases]') AND type in (N'U'))
DROP TABLE [dbo].[batch_QueuedControlledReleases]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[batch_ReleasePolicyDefinitions]') AND type in (N'U'))
DROP TABLE [dbo].[batch_ReleasePolicyDefinitions]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[batch_Envelopes]') AND type in (N'U'))
DROP TABLE [dbo].[batch_Envelopes]
GO
