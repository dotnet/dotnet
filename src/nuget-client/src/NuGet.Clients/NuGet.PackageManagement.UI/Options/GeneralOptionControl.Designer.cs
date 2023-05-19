namespace NuGet.PackageManagement.UI.Options
{
    partial class GeneralOptionControl
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GeneralOptionControl));
            this.skipBindingRedirects = new System.Windows.Forms.CheckBox();
            this.PackageRestoreHeader = new System.Windows.Forms.Label();
            this.packageRestoreConsentCheckBox = new System.Windows.Forms.CheckBox();
            this.packageRestoreAutomaticCheckBox = new System.Windows.Forms.CheckBox();
            this.BindingRedirectsHeader = new System.Windows.Forms.Label();
            this.localsCommandButton = new System.Windows.Forms.Button();
            this.localsCommandStatusText = new System.Windows.Forms.RichTextBox();
            this.PackageManagementHeader = new System.Windows.Forms.Label();
            this.defaultPackageManagementFormatLabel = new System.Windows.Forms.Label();
            this.showPackageManagementChooser = new System.Windows.Forms.CheckBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.defaultPackageManagementFormatItems = new System.Windows.Forms.ComboBox();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // skipBindingRedirects
            // 
            resources.ApplyResources(this.skipBindingRedirects, "skipBindingRedirects");
            this.skipBindingRedirects.Name = "skipBindingRedirects";
            this.skipBindingRedirects.UseVisualStyleBackColor = true;
            // 
            // PackageRestoreHeader
            // 
            resources.ApplyResources(this.PackageRestoreHeader, "PackageRestoreHeader");
            this.PackageRestoreHeader.Name = "PackageRestoreHeader";
            // 
            // packageRestoreConsentCheckBox
            // 
            resources.ApplyResources(this.packageRestoreConsentCheckBox, "packageRestoreConsentCheckBox");
            this.packageRestoreConsentCheckBox.Name = "packageRestoreConsentCheckBox";
            this.packageRestoreConsentCheckBox.UseVisualStyleBackColor = true;
            this.packageRestoreConsentCheckBox.CheckedChanged += new System.EventHandler(this.OnPackageRestoreConsentCheckBoxCheckedChanged);
            // 
            // packageRestoreAutomaticCheckBox
            // 
            resources.ApplyResources(this.packageRestoreAutomaticCheckBox, "packageRestoreAutomaticCheckBox");
            this.packageRestoreAutomaticCheckBox.Name = "packageRestoreAutomaticCheckBox";
            this.packageRestoreAutomaticCheckBox.UseVisualStyleBackColor = true;
            // 
            // BindingRedirectsHeader
            // 
            resources.ApplyResources(this.BindingRedirectsHeader, "BindingRedirectsHeader");
            this.BindingRedirectsHeader.Name = "BindingRedirectsHeader";
            // 
            // localsCommandButton
            // 
            resources.ApplyResources(this.localsCommandButton, "localsCommandButton");
            this.localsCommandButton.Name = "localsCommandButton";
            this.localsCommandButton.Click += new System.EventHandler(this.OnLocalsCommandButtonOnClick);
            // 
            // localsCommandStatusText
            // 
            this.localsCommandStatusText.AccessibleRole = System.Windows.Forms.AccessibleRole.Text;
            resources.ApplyResources(this.localsCommandStatusText, "localsCommandStatusText");
            this.localsCommandStatusText.BackColor = System.Drawing.SystemColors.Control;
            this.localsCommandStatusText.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.localsCommandStatusText.Name = "localsCommandStatusText";
            this.localsCommandStatusText.ReadOnly = true;
            this.localsCommandStatusText.ContentsResized += new System.Windows.Forms.ContentsResizedEventHandler(this.OnLocalsCommandStatusTextContentChanged);
            this.localsCommandStatusText.LinkClicked += new System.Windows.Forms.LinkClickedEventHandler(this.OnLocalsCommandStatusTextLinkClicked);
            // 
            // PackageManagementHeader
            // 
            resources.ApplyResources(this.PackageManagementHeader, "PackageManagementHeader");
            this.PackageManagementHeader.Name = "PackageManagementHeader";
            // 
            // defaultPackageManagementFormatLabel
            // 
            resources.ApplyResources(this.defaultPackageManagementFormatLabel, "defaultPackageManagementFormatLabel");
            this.defaultPackageManagementFormatLabel.Name = "defaultPackageManagementFormatLabel";
            // 
            // showPackageManagementChooser
            // 
            resources.ApplyResources(this.showPackageManagementChooser, "showPackageManagementChooser");
            this.showPackageManagementChooser.Name = "showPackageManagementChooser";
            this.showPackageManagementChooser.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel1
            // 
            resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
            this.tableLayoutPanel1.Controls.Add(this.defaultPackageManagementFormatItems, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.defaultPackageManagementFormatLabel, 0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            // 
            // defaultPackageManagementFormatItems
            // 
            resources.ApplyResources(this.defaultPackageManagementFormatItems, "defaultPackageManagementFormatItems");
            this.defaultPackageManagementFormatItems.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.defaultPackageManagementFormatItems.FormattingEnabled = true;
            this.defaultPackageManagementFormatItems.Items.AddRange(new object[] {
            resources.GetString("defaultPackageManagementFormatItems.Items"),
            resources.GetString("defaultPackageManagementFormatItems.Items1")});
            this.defaultPackageManagementFormatItems.Name = "defaultPackageManagementFormatItems";
            // 
            // GeneralOptionControl
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.BindingRedirectsHeader);
            this.Controls.Add(this.PackageRestoreHeader);
            this.Controls.Add(this.packageRestoreConsentCheckBox);
            this.Controls.Add(this.packageRestoreAutomaticCheckBox);
            this.Controls.Add(this.skipBindingRedirects);
            this.Controls.Add(this.PackageManagementHeader);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.showPackageManagementChooser);
            this.Controls.Add(this.localsCommandButton);
            this.Controls.Add(this.localsCommandStatusText);
            this.Name = "GeneralOptionControl";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox skipBindingRedirects;
        private System.Windows.Forms.Label PackageRestoreHeader;
        private System.Windows.Forms.CheckBox packageRestoreConsentCheckBox;
        private System.Windows.Forms.CheckBox packageRestoreAutomaticCheckBox;
        private System.Windows.Forms.Label BindingRedirectsHeader;
        private System.Windows.Forms.Button localsCommandButton;
        private System.Windows.Forms.RichTextBox localsCommandStatusText;
        private System.Windows.Forms.Label PackageManagementHeader;
        private System.Windows.Forms.Label defaultPackageManagementFormatLabel;
        private System.Windows.Forms.CheckBox showPackageManagementChooser;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.ComboBox defaultPackageManagementFormatItems;
    }
}
