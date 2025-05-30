// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.TrustUI;
using System.Globalization;                 // For localization of string conversion

using System.Security.RightsManagement;

namespace MS.Internal.Documents
{
    internal sealed class RightsManagementResourceHelper
    {
        #region Constructors
        //------------------------------------------------------
        // Constructors
        //------------------------------------------------------

        /// <summary>
        /// The constructor
        /// </summary>
        private RightsManagementResourceHelper()
        {
        }

        #endregion Constructors

        #region Internal Methods
        //------------------------------------------------------
        // Internal Methods
        //------------------------------------------------------

        /// <summary>
        /// GetDocumentLevelResources.
        /// </summary>
        internal static DocumentStatusResources GetDocumentLevelResources(RightsManagementStatus status)
        {
            DocumentStatusResources docStatusResources = new DocumentStatusResources();
            
            // Set appropriate Text and ToolTip values.
            switch (status)
            {
                case (RightsManagementStatus.Protected):
                    docStatusResources.Text = SR.RMProtected;
                    docStatusResources.ToolTip = SR.RMAppliedToolTip;
                    break;
                default: // RightsManagementStatus.Unknown or RightsManagementStatus.Unprotected
                    docStatusResources.Text = String.Empty;
                    docStatusResources.ToolTip = SR.RMDefaultToolTip;
                    break;
            }

            docStatusResources.Image = GetDrawingBrushFromStatus(status);

            return docStatusResources;
        }

        /// <summary>
        /// GetCredentialManagementResources.
        /// </summary>
        /// <param name="user">The user object from which to get resources</param>
        internal static string GetCredentialManagementResources(RightsManagementUser user)
        {
            string accountName = null;

            ArgumentNullException.ThrowIfNull(user);

            switch (user.AuthenticationType)
            {
                case (AuthenticationType.Windows):
                    accountName = String.Format(
                                        CultureInfo.CurrentCulture,
                                        SR.RMCredManagementWindowsAccount, 
                                        user.Name);
                    break;
                case (AuthenticationType.Passport):
                    accountName = String.Format(
                                        CultureInfo.CurrentCulture,
                                        SR.RMCredManagementPassportAccount,
                                        user.Name);
                    break;
                default:
                    accountName = String.Format(
                                        CultureInfo.CurrentCulture,
                                        SR.RMCredManagementUnknownAccount,
                                        user.Name);
                    break;
            }

            return accountName;
        }

        #endregion Internal Methods

        #region Private Methods
        //--------------------------------------------------------------------------
        // Private Methods
        //--------------------------------------------------------------------------

        /// <summary>
        /// Get the DrawingBrush icon for the status.
        /// </summary>
        /// <param name="status">Requested status</param>
        /// <returns>A DrawingBrush on success (valid status, DrawingBrush found), null otherwise.</returns>
        private static DrawingBrush GetDrawingBrushFromStatus(RightsManagementStatus status)
        {
            if (_brushResources == null)
            {
                // Get the entire list of RightsManagementStatus values.
                RightsManagementStatus[] statusList = Enum.GetValues<RightsManagementStatus>();

                // Construct the array to hold brush references.
                _brushResources = new DrawingBrush[statusList.Length];

                // To find the DrawingBrushes in the theme resources we need a FrameworkElement.
                // TextBlock was used as it appears to have a very small footprint, and won't
                // take long to construct.  The actual FrameworkElement doesn't matter as long
                // as we have an instance to one
                _frameworkElement = new TextBlock();
            }

            if ((_brushResources != null) && (_frameworkElement != null))
            {
                int index = (int)status;

                // If there is no cached value of the requested DrawingBrush, then find
                // it in the Resources.
                if (_brushResources[index] == null)
                {
                    // Determine resource name.
                    string resourceName = "PUIRMStatus"
                        + Enum.GetName(status)
                        + "BrushKey";

                    // Acquire reference to the brush.
                    object resource = _frameworkElement.FindResource(
                        new ComponentResourceKey(typeof(PresentationUIStyleResources), resourceName));

                    // Set cache value for the brush.
                    _brushResources[index] = resource as DrawingBrush;
                }
                return _brushResources[index];
            }

            return null;
        }

        #endregion Private Methods

        private static DrawingBrush[]       _brushResources;   // To cache DrawingBrushes.
        private static FrameworkElement     _frameworkElement; // Used to search resources.
    }
}
