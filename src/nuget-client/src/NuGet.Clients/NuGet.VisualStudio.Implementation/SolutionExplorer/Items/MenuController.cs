// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable enable

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Microsoft.Internal.VisualStudio.PlatformUI;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.ProjectSystem.VS.Tree.Dependencies.AttachedCollections;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;

namespace NuGet.VisualStudio.SolutionExplorer
{
    internal sealed class MenuController : IContextMenuController
    {
        private const int IDM_VS_CTXT_TRANSITIVE_ASSEMBLY_REFERENCE = 0x04B1;
        private const int IDM_VS_CTXT_TRANSITIVE_PACKAGE_REFERENCE = 0x04B2;
        private const int IDM_VS_CTXT_TRANSITIVE_PROJECT_REFERENCE = 0x04B3;

        public static MenuController TransitiveAssembly { get; } = new MenuController(VsMenus.guidSHLMainMenu, IDM_VS_CTXT_TRANSITIVE_ASSEMBLY_REFERENCE);
        public static MenuController TransitivePackage { get; } = new MenuController(VsMenus.guidSHLMainMenu, IDM_VS_CTXT_TRANSITIVE_PACKAGE_REFERENCE);
        public static MenuController TransitiveProject { get; } = new MenuController(VsMenus.guidSHLMainMenu, IDM_VS_CTXT_TRANSITIVE_PROJECT_REFERENCE);

        private readonly Guid _menuGuid;
        private readonly int _menuId;

        private MenuController(Guid menuGuid, int menuId)
        {
            _menuGuid = menuGuid;
            _menuId = menuId;
        }

        bool IContextMenuController.ShowContextMenu(IEnumerable<object> items, Point location)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            bool shouldShowMenu = items.All(item => item is IRelatableItem);

            if (shouldShowMenu)
            {
                if (Package.GetGlobalService(typeof(SVsUIShell)) is IVsUIShell shell)
                {
                    Guid guidContextMenu = _menuGuid;

                    int result = shell.ShowContextMenu(
                        dwCompRole: 0,
                        rclsidActive: ref guidContextMenu,
                        nMenuId: _menuId,
                        pos: new[] { new POINTS { x = (short)location.X, y = (short)location.Y } },
                        pCmdTrgtActive: null);

                    return ErrorHandler.Succeeded(result);
                }
            }

            return false;
        }
    }
}
