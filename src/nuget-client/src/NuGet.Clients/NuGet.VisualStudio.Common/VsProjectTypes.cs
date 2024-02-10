// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace NuGet.VisualStudio
{
    public static class VsProjectTypes
    {
        // Project type guids
        public const string WebApplicationProjectTypeGuid = "{349C5851-65DF-11DA-9384-00065B846F21}";
        public const string WebSiteProjectTypeGuid = "{E24C65DC-7377-472B-9ABA-BC803B73C61A}";
        public const string CsharpProjectTypeGuid = "{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}";
        public const string VbProjectTypeGuid = "{F184B08F-C81C-45F6-A57F-5ABD9991F28F}";
        public const string CppProjectTypeGuid = "{8BC9CEB8-8B4A-11D0-8D11-00A0C91BC942}";
        public const string FsharpProjectTypeGuid = "{F2A71F9B-5D33-465A-A702-920D77279786}";
        public const string JsProjectTypeGuid = "{262852C6-CD72-467D-83FE-5EEB1973A190}";
        public const string WixProjectTypeGuid = "{930C7802-8A8C-48F9-8165-68863BCCD9DD}";
        public const string LightSwitchProjectTypeGuid = "{ECD6D718-D1CF-4119-97F3-97C25A0DFBF9}";
        public const string NemerleProjectTypeGuid = "{edcc3b85-0bad-11db-bc1a-00112fde8b61}";
        public const string InstallShieldLimitedEditionTypeGuid = "{FBB4BD86-BF63-432a-A6FB-6CF3A1288F83}";
        public const string WindowsStoreProjectTypeGuid = "{BC8A1FFA-BEE3-4634-8014-F334798102B3}";
        public const string SynergexProjectTypeGuid = "{BBD0F5D1-1CC4-42fd-BA4C-A96779C64378}";
        public const string NomadForVisualStudioProjectTypeGuid = "{4B160523-D178-4405-B438-79FB67C8D499}";
        public const string TDSProjectTypeGuid = "{CAA73BB0-EF22-4d79-A57E-DF67B3BA9C80}";
        public const string TDSItemTypeGuid = "{6877B9B0-CDF7-4ff2-BC09-9608387B37F2}";
        public const string DxJsProjectTypeGuid = "{1B19158F-E398-40A6-8E3B-350508E125F1}";
        public const string DeploymentProjectTypeGuid = "{151d2e53-a2c4-4d7d-83fe-d05416ebd58e}";
        public const string CosmosProjectTypeGuid = "{471EC4BB-E47E-4229-A789-D1F5F83B52D4}";
        public const string ManagementPackProjectTypeGuid = "{d4b43eb3-688b-4eee-86bd-088f0b28abb3}";
        public const string WindowsPhoneSilverlightProjectTypeGuid = "{C089C8C0-30E0-4E22-80C0-CE093F111A43}";
        public const string WindowsPhone81ProjectTypeGuid = "{76F1466A-8B6D-4E39-A767-685A06062A39}";
        public const string SilverlightProjectTypeGuid = "{A1591282-1198-4647-A2B1-27E5FF5F6F3B}";
        public const string LightSwitchCsharpProjectTypeGuid = "{8BB0C5E8-0616-4F60-8E55-A43933E57E9C}";
        public const string LightSwitchLsxtProjectTypeGuid = "{581633EB-B896-402F-8E60-36F3DA191C85}";
        public const string ESProjTypeGuid = "{54a90642-561a-4bb1-a94e-469adee60c69}";

        // Copied from EnvDTE.Constants since that type can't be embedded
        public const string VsProjectItemKindPhysicalFile = "{6BB5F8EE-4483-11D3-8BCF-00C04F8EC28C}";
        public const string VsProjectItemKindPhysicalFolder = "{6BB5F8EF-4483-11D3-8BCF-00C04F8EC28C}";
        public const string VsProjectItemKindSolutionFolder = "{66A26720-8FB5-11D2-AA7E-00C04F688DDE}";
        public const string VsProjectItemKindSolutionItem = "{66A26722-8FB5-11D2-AA7E-00C04F688DDE}";
        public const string VsWindowKindSolutionExplorer = "{3AE79031-E1BC-11D0-8F78-00A0C9110057}";
        public const string VsProjectKindMisc = "{66A2671D-8FB5-11D2-AA7E-00C04F688DDE}";

        // All unloaded projects have this Kind value
        public const string UnloadedProjectTypeGuid = "{67294A52-A4F0-11D2-AA88-00C04F688DDE}";
    }
}
