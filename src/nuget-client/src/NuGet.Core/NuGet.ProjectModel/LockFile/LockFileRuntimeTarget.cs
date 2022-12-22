// Copyright (c) .NET Foundation and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace NuGet.ProjectModel
{
    public class LockFileRuntimeTarget : LockFileItem
    {
        public static readonly string RidProperty = "rid";
        public static readonly string AssetTypeProperty = "assetType";

        public LockFileRuntimeTarget(string path) : base(path)
        {
        }

        public LockFileRuntimeTarget(string path, string runtime, string assetType) : this(path)
        {
            Runtime = runtime;
            AssetType = assetType;
        }

        public string Runtime
        {
            get
            {
                return GetProperty(RidProperty);
            }
            set
            {
                SetProperty(RidProperty, value);
            }
        }

        public string AssetType
        {
            get
            {
                return GetProperty(AssetTypeProperty);
            }
            set
            {
                SetProperty(AssetTypeProperty, value);
            }
        }
    }
}
