// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.VisualStudio.SolutionPersistence.Utilities;

namespace Microsoft.VisualStudio.SolutionPersistence.Model;

/// <summary>
/// Represents a solution folder in the solution model.
/// </summary>
public sealed class SolutionFolderModel : SolutionItemModel
{
    private const string CycleBreaker = "***"; // to ensure no cycles
    private string? itemRef; // folder fullPath
    private List<string>? files;
    private string name;

    internal SolutionFolderModel(SolutionModel solutionModel, string name, SolutionFolderModel? parent)
        : base(solutionModel, parent)
    {
        Argument.ThrowIfNullOrEmpty(name, nameof(name));
        this.name = name;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="SolutionFolderModel"/> class.
    /// Copy constructor.
    /// </summary>
    /// <param name="solutionModel">The new solution model parent.</param>
    /// <param name="folderModel">The folder model to copy.</param>
    internal SolutionFolderModel(SolutionModel solutionModel, SolutionFolderModel folderModel)
        : base(solutionModel, folderModel.BeSolutionItemModel)
    {
        this.name = folderModel.name;
        if (folderModel.Files is not null)
        {
            this.files = [.. folderModel.Files];
        }
    }

    /// <summary>
    /// Gets the files in this solution folder.
    /// </summary>
    public IReadOnlyList<string>? Files => this.files;

    /// <summary>
    /// Gets or sets the name of the solution folder.
    /// </summary>
    public string Name
    {
        get => this.name;
        set
        {
            Argument.ThrowIfNullOrEmpty(value, nameof(value));
            SolutionModel.ValidateName(value.AsSpan());

            if (this.name == value)
            {
                return;
            }

            string testName = $"{this.Parent?.ItemRef ?? "/"}{value}/";
            if (this.Solution.FindFolder(testName) is not null)
            {
                throw new SolutionArgumentException(string.Format(Errors.DuplicateItemRef_Args2, testName, "Folder"), nameof(value), SolutionErrorType.DuplicateItemRef);
            }

            string oldName = this.name;
            try
            {
                this.name = value;
                this.OnItemRefChanged();
            }
            catch (Exception)
            {
                // On error revert the name.
                this.name = oldName;
                throw;
            }
        }
    }

    /// <summary>
    /// Gets a unique reference to this folder in the solution.
    /// </summary>
    public string Path => this.ItemRef;

    /// <inheritdoc/>
    public override string ActualDisplayName => this.Name;

    /// <inheritdoc/>
    public override Guid TypeId => ProjectTypeTable.SolutionFolder;

    /// <inheritdoc/>
    internal override string ItemRef
    {
        get
        {
            if (this.itemRef is not null)
            {
                return this.itemRef;
            }

            if (this.Parent is not null)
            {
                this.itemRef = CycleBreaker;
                string parentRef = this.Parent.ItemRef;
                if (!object.ReferenceEquals(parentRef, CycleBreaker))
                {
                    this.itemRef = $"{parentRef}{this.Name}/";
                    return this.itemRef;
                }
            }

            // no parent, or part of cycle move it on top.
            // potential duplicates in this case will be ignored/merged on save.
            this.itemRef = $"/{this.Name}/";
            return this.itemRef;
        }
    }

    /// <summary>
    /// Adds a file to this solution folder.
    /// </summary>
    /// <param name="file">The file to add.</param>
    public void AddFile(string file)
    {
        this.files ??= [];

        if (!this.files.Contains(file))
        {
            this.files.Add(file);
        }
    }

    /// <summary>
    /// Removes a file from this solution folder.
    /// </summary>
    /// <param name="file">The file to remove.</param>
    /// <returns><see langword="true"/> if the item was found and removed.</returns>
    public bool RemoveFile(string file)
    {
        return this.files is not null && this.files.Remove(file);
    }

    internal override void OnItemRefChanged()
    {
        base.OnItemRefChanged();
        this.itemRef = null;

        // Recursively update all children.
        foreach (SolutionItemModel item in this.Solution.SolutionItems)
        {
            if (ReferenceEquals(item.Parent, this))
            {
                item.OnItemRefChanged();
            }
        }
    }

    private protected override Guid GetDefaultId()
    {
        Guid parentId = this.Parent is null ? Guid.Empty : this.Parent.Id;
        return DefaultIdGenerator.CreateIdFrom(parentId, this.Name);
    }

    private protected override void OnParentChanged()
    {
        base.OnParentChanged();
        this.OnItemRefChanged();
    }
}
