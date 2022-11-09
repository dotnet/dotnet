// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Threading.Tasks;
using Microsoft;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.PlatformUI;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;

namespace NuGet.VisualStudio
{
    /// <summary>
    /// Represent a particular tree node in the SolutionExplorer window.
    /// </summary>
    public sealed class VsHierarchyItem : IEquatable<VsHierarchyItem>
    {
        private readonly uint _vsitemid;
        private readonly IVsHierarchy _hierarchy;

        internal delegate Task<(int processReturn, object newCallerObject)> ProcessItemDelegateAsync(VsHierarchyItem item, object callerObject);

        public IVsHierarchy VsHierarchy => _hierarchy;

        private VsHierarchyItem(IVsHierarchy hierarchy, uint id)
        {
            _vsitemid = id;
            _hierarchy = hierarchy;
        }

        private VsHierarchyItem(IVsHierarchy hierarchy)
            : this(hierarchy, VSConstants.VSITEMID_ROOT)
        {
        }

        public static async Task<VsHierarchyItem> FromDteProjectAsync(EnvDTE.Project project)
        {
            Assumes.Present(project);
            return new VsHierarchyItem(await project.ToVsHierarchyAsync());
        }

        public static VsHierarchyItem FromVsHierarchy(IVsHierarchy project)
        {
            Assumes.Present(project);
            return new VsHierarchyItem(project);
        }

        public bool TryGetProjectId(out Guid projectId)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            var result = _hierarchy.GetGuidProperty(
                VSConstants.VSITEMID_ROOT,
                (int)__VSHPROPID.VSHPROPID_ProjectIDGuid,
                out projectId);

            return result == VSConstants.S_OK;
        }

        internal uint VsItemID => _vsitemid;

        internal async Task<bool> IsExpandableAsync()
        {
            var o = await GetPropertyAsync(__VSHPROPID.VSHPROPID_Expandable);
            if (o is bool)
            {
                return (bool)o;
            }

            return (o is int) && (int)o != 0;
        }

        private async Task<object> GetPropertyAsync(__VSHPROPID propid)
        {
            (bool _, object value) = await TryGetPropertyAsync((int)propid);
            return value;
        }

        private async Task<(bool found, object value)> TryGetPropertyAsync(int propid)
        {
            try
            {
                if (_hierarchy != null)
                {
                    await NuGetUIThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();
                    _hierarchy.GetProperty(_vsitemid, propid, out object value);
                    return (true, value);
                }
            }
            catch (Exception e)
            {
                if (e.IsCritical())
                {
                    throw;
                }

                // Ignore.
            }

            return (false, null);
        }

        internal async Task<int> WalkDepthFirstAsync(bool fVisible, ProcessItemDelegateAsync processCallbackAsync, object callerObject)
        {
            //
            // TODO Need to see what to do if this is a sub project
            //
            // Get the node type
            // Guid nodeGuid;
            // if (hier.GetGuidProperty(_vsitemid, (int)__VSHPROPID.VSHPROPID_TypeGuid, out nodeGuid) != 0)

            if (processCallbackAsync == null)
            {
                return 0;
            }

            (int processReturn, object newCallerObject) = await processCallbackAsync(this, callerObject);
            if (processReturn != 0)
            {
                // Callback says to skip (1) or stop (-1)
                return processReturn;
            }

            // The process callback can change the caller object. If not we just use the one originally
            // passed in.
            if (newCallerObject == null)
            {
                newCallerObject = callerObject;
            }

            // Walk children if there are any
            if (await IsExpandableAsync())
            {
                VsHierarchyItem child = await GetFirstChildAsync(fVisible);
                while (child != null)
                {
                    object isNonMemberItemValue = await child.GetPropertyAsync(__VSHPROPID.VSHPROPID_IsNonMemberItem);
                    // Some project systems (e.g. F#) don't support querying for the VSHPROPID_IsNonMemberItem property.
                    // In that case, we treat this child as belonging to the project
                    bool isMemberOfProject = isNonMemberItemValue == null || (bool)isNonMemberItemValue == false;
                    if (isMemberOfProject)
                    {
                        int returnVal = await child.WalkDepthFirstAsync(fVisible, processCallbackAsync, newCallerObject);
                        if (returnVal == -1)
                        {
                            return returnVal;
                        }
                    }
                    child = await child.GetNextSiblingAsync(fVisible);
                }
            }
            return 0;
        }

        internal async Task<VsHierarchyItem> GetNextSiblingAsync(bool fVisible)
        {
            uint childId = await GetNextSiblingIdAsync(fVisible);
            if (childId != VSConstants.VSITEMID_NIL)
            {
                return new VsHierarchyItem(_hierarchy, childId);
            }

            return null;
        }

        internal async Task<uint> GetNextSiblingIdAsync(bool fVisible)
        {
            object o = await GetPropertyAsync(fVisible ? __VSHPROPID.VSHPROPID_NextVisibleSibling : __VSHPROPID.VSHPROPID_NextSibling);

            if (o is int)
            {
                return unchecked((uint)((int)o));
            }
            if (o is uint)
            {
                return (uint)o;
            }

            return VSConstants.VSITEMID_NIL;
        }

        internal async Task<VsHierarchyItem> GetFirstChildAsync(bool fVisible)
        {
            uint childId = await GetFirstChildIdAsync(fVisible);
            if (childId != VSConstants.VSITEMID_NIL)
            {
                return new VsHierarchyItem(_hierarchy, childId);
            }

            return null;
        }

        internal async Task<uint> GetFirstChildIdAsync(bool fVisible)
        {
            object o = await GetPropertyAsync(fVisible ? __VSHPROPID.VSHPROPID_FirstVisibleChild : __VSHPROPID.VSHPROPID_FirstChild);

            if (o is int)
            {
                return unchecked((uint)((int)o));
            }

            if (o is uint)
            {
                return (uint)o;
            }

            return VSConstants.VSITEMID_NIL;
        }

        public bool Equals(VsHierarchyItem other)
        {
            return VsItemID == other.VsItemID;
        }

        public override bool Equals(object obj)
        {
            var other = obj as VsHierarchyItem;
            return other != null && Equals(other);
        }

        public override int GetHashCode()
        {
            return _vsitemid.GetHashCode();
        }
    }
}
