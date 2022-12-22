// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.TableControl;
using Microsoft.VisualStudio.Shell.TableManager;
using Moq;
using NuGet.Common;
using Xunit;

namespace NuGet.VisualStudio.Common.Test
{
    public class ErrorListTableDataSourceTests
    {
        [Fact]
        public void ErrorListTableDataSource_AddEntriesVerifyEntryExists()
        {
            // Arrange
            var errorList = Mock.Of<Microsoft.VisualStudio.Shell.IErrorList>();
            var tableControl = Mock.Of<IWpfTableControl>();
            var tableManager = Mock.Of<ITableManager>();

            Mock.Get(errorList)
                .Setup(x => x.TableControl)
                .Returns(tableControl);

            Mock.Get(tableControl)
                .Setup(x => x.SubscribeToDataSource(
                    It.IsAny<ITableDataSource>()));

            Mock.Get(tableManager)
                .Setup(x => x.AddSource(
                    It.IsAny<ITableDataSource>()));

            Mock.Get(tableManager)
                .Setup(x => x.RemoveSource(
                    It.IsAny<ITableDataSource>()));

            var source = new ErrorListTableDataSource(errorList, tableManager);

            // Act
            var entry = new ErrorListTableEntry("test", LogLevel.Minimal);
            source.AddNuGetEntries(entry);

            // Assert
            Assert.Equal(entry, source.GetEntries().Single());
        }

        [Fact]
        public void ErrorListTableDataSource_AddMultipleEntriesVerifyEntriesExists()
        {
            // Arrange
            var errorList = Mock.Of<Microsoft.VisualStudio.Shell.IErrorList>();
            var tableControl = Mock.Of<IWpfTableControl>();
            var tableManager = Mock.Of<ITableManager>();

            Mock.Get(errorList)
                .Setup(x => x.TableControl)
                .Returns(tableControl);

            Mock.Get(tableControl)
                .Setup(x => x.SubscribeToDataSource(
                    It.IsAny<ITableDataSource>()));

            Mock.Get(tableManager)
                .Setup(x => x.AddSource(
                    It.IsAny<ITableDataSource>()));

            Mock.Get(tableManager)
                .Setup(x => x.RemoveSource(
                    It.IsAny<ITableDataSource>()));

            var source = new ErrorListTableDataSource(errorList, tableManager);

            // Act
            var entry = new ErrorListTableEntry("test", LogLevel.Minimal);
            source.AddNuGetEntries(entry);

            var entry2 = new ErrorListTableEntry("test2", LogLevel.Minimal);
            source.AddNuGetEntries(entry2);

            // Assert
            Assert.Equal(2, source.GetEntries().Length);
            Assert.Equal(entry, source.GetEntries().First());
            Assert.Equal(entry2, source.GetEntries().Skip(1).First());
        }

        [Fact]
        public void ErrorListTableDataSource_ClearEntriesVerifyEmpty()
        {
            // Arrange
            var errorList = Mock.Of<Microsoft.VisualStudio.Shell.IErrorList>();
            var tableControl = Mock.Of<IWpfTableControl>();
            var tableManager = Mock.Of<ITableManager>();

            Mock.Get(errorList)
                .Setup(x => x.TableControl)
                .Returns(tableControl);

            Mock.Get(tableControl)
                .Setup(x => x.SubscribeToDataSource(
                    It.IsAny<ITableDataSource>()));

            Mock.Get(tableManager)
                .Setup(x => x.AddSource(
                    It.IsAny<ITableDataSource>()));

            Mock.Get(tableManager)
                .Setup(x => x.RemoveSource(
                    It.IsAny<ITableDataSource>()));

            var source = new ErrorListTableDataSource(errorList, tableManager);

            // Act
            source.ClearNuGetEntries();

            // Assert
            Assert.Equal(0, source.GetEntries().Length);
        }

        [Fact]
        public void ErrorListTableDataSource_SubscribeAndVerifyResult()
        {
            // Arrange
            var errorList = Mock.Of<Microsoft.VisualStudio.Shell.IErrorList>();
            var tableControl = Mock.Of<IWpfTableControl>();
            var tableManager = Mock.Of<ITableManager>();
            var sink = Mock.Of<ITableDataSink>();

            Mock.Get(errorList)
                .Setup(x => x.TableControl)
                .Returns(tableControl);

            Mock.Get(tableControl)
                .Setup(x => x.SubscribeToDataSource(
                    It.IsAny<ITableDataSource>()));

            Mock.Get(tableManager)
                .Setup(x => x.AddSource(
                    It.IsAny<ITableDataSource>()));

            Mock.Get(tableManager)
                .Setup(x => x.RemoveSource(
                    It.IsAny<ITableDataSource>()));

            Mock.Get(sink)
                .Setup(x => x.AddEntries(
                    It.IsAny<IReadOnlyList<ITableEntry>>(),
                    It.IsAny<bool>()));

            Mock.Get(sink)
                .Setup(x => x.RemoveEntries(
                    It.IsAny<IReadOnlyList<ITableEntry>>()));

            var source = new ErrorListTableDataSource(errorList, tableManager);

            // Act
            var subscription = source.Subscribe(sink);

            // Assert
            Assert.NotNull(subscription);
        }

        [Fact]
        public void ErrorListTableDataSource_SubscribeDisposeAndVerifyResult()
        {
            // Arrange
            var errorList = Mock.Of<Microsoft.VisualStudio.Shell.IErrorList>();
            var tableControl = Mock.Of<IWpfTableControl>();
            var tableManager = Mock.Of<ITableManager>();
            var sink = Mock.Of<ITableDataSink>();

            Mock.Get(errorList)
                .Setup(x => x.TableControl)
                .Returns(tableControl);

            Mock.Get(tableControl)
                .Setup(x => x.SubscribeToDataSource(
                    It.IsAny<ITableDataSource>()));

            Mock.Get(tableManager)
                .Setup(x => x.AddSource(
                    It.IsAny<ITableDataSource>()));

            Mock.Get(tableManager)
                .Setup(x => x.RemoveSource(
                    It.IsAny<ITableDataSource>()));


            var removeCalled = new HashSet<ITableDataSink>();
            var addCalled = new HashSet<ITableDataSink>();

            Mock.Get(sink)
                .Setup(x => x.AddEntries(
                    It.IsAny<IReadOnlyList<ITableEntry>>(),
                    It.IsAny<bool>()))
                 .Callback(() => addCalled.Add(sink));

            Mock.Get(sink)
                .Setup(x => x.RemoveEntries(
                    It.IsAny<IReadOnlyList<ITableEntry>>()))
                .Callback(() => removeCalled.Add(sink));

            var source = new ErrorListTableDataSource(errorList, tableManager);

            // Act
            var subscription = source.Subscribe(sink);

            var entry = new ErrorListTableEntry("test", LogLevel.Minimal);
            source.AddNuGetEntries(entry);

            source.ClearNuGetEntries();

            // Assert no errors.
            subscription.Dispose();

            // Assert
            Assert.Contains(sink, addCalled);
            Assert.Contains(sink, removeCalled);
        }

        [Fact]
        public void ErrorListTableDataSource_SubscribeDisposeNullSinkAndVerifyResult()
        {
            // Arrange
            var errorList = Mock.Of<Microsoft.VisualStudio.Shell.IErrorList>();
            var tableControl = Mock.Of<IWpfTableControl>();
            var tableManager = Mock.Of<ITableManager>();

            Mock.Get(errorList)
                .Setup(x => x.TableControl)
                .Returns(tableControl);

            Mock.Get(tableControl)
                .Setup(x => x.SubscribeToDataSource(
                    It.IsAny<ITableDataSource>()));

            Mock.Get(tableManager)
                .Setup(x => x.AddSource(
                    It.IsAny<ITableDataSource>()));

            Mock.Get(tableManager)
                .Setup(x => x.RemoveSource(
                    It.IsAny<ITableDataSource>()));

            var source = new ErrorListTableDataSource(errorList, tableManager);

            // Act
            var subscription = source.Subscribe(null);

            // Assert no errors.
            subscription.Dispose();
        }

        [Fact]
        public void ErrorListTableDataSource_VerifyDisposedSinksAreNotCalled()
        {
            // Arrange
            var errorList = Mock.Of<Microsoft.VisualStudio.Shell.IErrorList>();
            var tableControl = Mock.Of<IWpfTableControl>();
            var tableManager = Mock.Of<ITableManager>();
            var sink = Mock.Of<ITableDataSink>();
            var sink2 = Mock.Of<ITableDataSink>();
            var sink3 = Mock.Of<ITableDataSink>();

            Mock.Get(errorList)
                .Setup(x => x.TableControl)
                .Returns(tableControl);

            Mock.Get(tableControl)
                .Setup(x => x.SubscribeToDataSource(
                    It.IsAny<ITableDataSource>()));

            Mock.Get(tableManager)
                .Setup(x => x.AddSource(
                    It.IsAny<ITableDataSource>()));

            Mock.Get(tableManager)
                .Setup(x => x.RemoveSource(
                    It.IsAny<ITableDataSource>()));

            var removeCalled = new HashSet<ITableDataSink>();
            var addCalled = new HashSet<ITableDataSink>();

            Mock.Get(sink)
                .Setup(x => x.AddEntries(
                    It.IsAny<IReadOnlyList<ITableEntry>>(),
                    It.IsAny<bool>()))
                 .Callback(() => addCalled.Add(sink));

            Mock.Get(sink)
                .Setup(x => x.RemoveEntries(
                    It.IsAny<IReadOnlyList<ITableEntry>>()))
                .Callback(() => removeCalled.Add(sink));

            Mock.Get(sink2)
                .Setup(x => x.AddEntries(
                    It.IsAny<IReadOnlyList<ITableEntry>>(),
                    It.IsAny<bool>()))
                 .Callback(() => addCalled.Add(sink2));

            Mock.Get(sink2)
                .Setup(x => x.RemoveEntries(
                    It.IsAny<IReadOnlyList<ITableEntry>>()))
                .Callback(() => removeCalled.Add(sink2));

            Mock.Get(sink3)
                .Setup(x => x.AddEntries(
                    It.IsAny<IReadOnlyList<ITableEntry>>(),
                    It.IsAny<bool>()))
                 .Callback(() => addCalled.Add(sink3));

            Mock.Get(sink3)
                .Setup(x => x.RemoveEntries(
                    It.IsAny<IReadOnlyList<ITableEntry>>()))
                .Callback(() => removeCalled.Add(sink3));

            var source = new ErrorListTableDataSource(errorList, tableManager);

            // Act
            var s1 = source.Subscribe(sink);
            var s2 = source.Subscribe(sink2);
            var s3 = source.Subscribe(sink3);

            s2.Dispose();
            s3.Dispose();

            var entry = new ErrorListTableEntry("test", LogLevel.Minimal);
            source.AddNuGetEntries(entry);

            source.ClearNuGetEntries();

            // Assert
            Assert.Contains(sink, addCalled);
            Assert.DoesNotContain(sink2, addCalled);
            Assert.DoesNotContain(sink3, addCalled);

            Assert.Contains(sink, removeCalled);
            Assert.DoesNotContain(sink2, removeCalled);
            Assert.DoesNotContain(sink3, removeCalled);
        }

        [Fact]
        public void ErrorListTableDataSource_WithAllDisposedSinksVerifyNoCalls()
        {
            // Arrange
            var errorList = Mock.Of<Microsoft.VisualStudio.Shell.IErrorList>();
            var tableControl = Mock.Of<IWpfTableControl>();
            var tableManager = Mock.Of<ITableManager>();
            var sink = Mock.Of<ITableDataSink>();
            var sink2 = Mock.Of<ITableDataSink>();
            var sink3 = Mock.Of<ITableDataSink>();

            Mock.Get(errorList)
                .Setup(x => x.TableControl)
                .Returns(tableControl);

            Mock.Get(tableControl)
                .Setup(x => x.SubscribeToDataSource(
                    It.IsAny<ITableDataSource>()));

            Mock.Get(tableManager)
                .Setup(x => x.AddSource(
                    It.IsAny<ITableDataSource>()));

            Mock.Get(tableManager)
                .Setup(x => x.RemoveSource(
                    It.IsAny<ITableDataSource>()));

            var removeCalled = new HashSet<ITableDataSink>();
            var addCalled = new HashSet<ITableDataSink>();

            Mock.Get(sink)
                .Setup(x => x.AddEntries(
                    It.IsAny<IReadOnlyList<ITableEntry>>(),
                    It.IsAny<bool>()))
                 .Callback(() => addCalled.Add(sink));

            Mock.Get(sink)
                .Setup(x => x.RemoveEntries(
                    It.IsAny<IReadOnlyList<ITableEntry>>()))
                .Callback(() => removeCalled.Add(sink));

            Mock.Get(sink2)
                .Setup(x => x.AddEntries(
                    It.IsAny<IReadOnlyList<ITableEntry>>(),
                    It.IsAny<bool>()))
                 .Callback(() => addCalled.Add(sink2));

            Mock.Get(sink2)
                .Setup(x => x.RemoveEntries(
                    It.IsAny<IReadOnlyList<ITableEntry>>()))
                .Callback(() => removeCalled.Add(sink2));

            Mock.Get(sink3)
                .Setup(x => x.AddEntries(
                    It.IsAny<IReadOnlyList<ITableEntry>>(),
                    It.IsAny<bool>()))
                 .Callback(() =>
                 {
                     addCalled.Add(sink3);
                 });

            Mock.Get(sink3)
                .Setup(x => x.RemoveEntries(
                    It.IsAny<IReadOnlyList<ITableEntry>>()))
                .Callback(() => removeCalled.Add(sink3));

            var source = new ErrorListTableDataSource(errorList, tableManager);

            // Act
            var s1 = source.Subscribe(sink);
            var s2 = source.Subscribe(sink2);
            var s3 = source.Subscribe(sink3);

            s1.Dispose();
            s2.Dispose();
            s3.Dispose();

            var entry = new ErrorListTableEntry("test", LogLevel.Minimal);
            source.AddNuGetEntries(entry);

            source.ClearNuGetEntries();

            // Assert
            Assert.DoesNotContain(sink, addCalled);
            Assert.DoesNotContain(sink2, addCalled);
            Assert.DoesNotContain(sink3, addCalled);

            Assert.DoesNotContain(sink, removeCalled);
            Assert.DoesNotContain(sink2, removeCalled);
            Assert.DoesNotContain(sink3, removeCalled);
        }

        [Fact]
        public void ErrorListTableDataSource_VerifyExistingEntriesAreAddedToNewSinks()
        {
            // Arrange
            var errorList = Mock.Of<Microsoft.VisualStudio.Shell.IErrorList>();
            var tableControl = Mock.Of<IWpfTableControl>();
            var tableManager = Mock.Of<ITableManager>();
            var sink = Mock.Of<ITableDataSink>();
            var sink2 = Mock.Of<ITableDataSink>();

            Mock.Get(errorList)
                .Setup(x => x.TableControl)
                .Returns(tableControl);

            Mock.Get(tableControl)
                .Setup(x => x.SubscribeToDataSource(
                    It.IsAny<ITableDataSource>()));

            Mock.Get(tableManager)
                .Setup(x => x.AddSource(
                    It.IsAny<ITableDataSource>()));

            Mock.Get(tableManager)
                .Setup(x => x.RemoveSource(
                    It.IsAny<ITableDataSource>()));

            var removeCalled = new HashSet<ITableDataSink>();
            var addCalled = new HashSet<ITableDataSink>();

            Mock.Get(sink)
                .Setup(x => x.AddEntries(
                    It.IsAny<IReadOnlyList<ITableEntry>>(),
                    It.IsAny<bool>()))
                 .Callback(() => addCalled.Add(sink));

            Mock.Get(sink)
                .Setup(x => x.RemoveEntries(
                    It.IsAny<IReadOnlyList<ITableEntry>>()))
                .Callback(() => removeCalled.Add(sink));

            Mock.Get(sink2)
                .Setup(x => x.AddEntries(
                    It.IsAny<IReadOnlyList<ITableEntry>>(),
                    It.IsAny<bool>()))
                 .Callback(() => addCalled.Add(sink2));

            Mock.Get(sink2)
                .Setup(x => x.RemoveEntries(
                    It.IsAny<IReadOnlyList<ITableEntry>>()))
                .Callback(() => removeCalled.Add(sink2));

            var source = new ErrorListTableDataSource(errorList, tableManager);

            // Act
            var s1 = source.Subscribe(sink);

            var entry = new ErrorListTableEntry("test", LogLevel.Minimal);
            source.AddNuGetEntries(entry);

            var s2 = source.Subscribe(sink2);

            // Assert
            Assert.Contains(sink, addCalled);
            Assert.Contains(sink2, addCalled);
        }

        [Fact]
        public void ErrorListTableDataSource_AddWithNoSinksVerifyAllAddedToNextSink()
        {
            // Arrange
            var errorList = Mock.Of<Microsoft.VisualStudio.Shell.IErrorList>();
            var tableControl = Mock.Of<IWpfTableControl>();
            var tableManager = Mock.Of<ITableManager>();
            var sink = Mock.Of<ITableDataSink>();
            var sink2 = Mock.Of<ITableDataSink>();

            Mock.Get(errorList)
                .Setup(x => x.TableControl)
                .Returns(tableControl);

            Mock.Get(tableControl)
                .Setup(x => x.SubscribeToDataSource(
                    It.IsAny<ITableDataSource>()));

            Mock.Get(tableManager)
                .Setup(x => x.AddSource(
                    It.IsAny<ITableDataSource>()));

            Mock.Get(tableManager)
                .Setup(x => x.RemoveSource(
                    It.IsAny<ITableDataSource>()));

            var removeCalled = new HashSet<ITableDataSink>();
            var addCalled = new HashSet<ITableDataSink>();

            Mock.Get(sink)
                .Setup(x => x.AddEntries(
                    It.IsAny<IReadOnlyList<ITableEntry>>(),
                    It.IsAny<bool>()))
                 .Callback(() => addCalled.Add(sink));

            Mock.Get(sink)
                .Setup(x => x.RemoveEntries(
                    It.IsAny<IReadOnlyList<ITableEntry>>()))
                .Callback(() => removeCalled.Add(sink));

            Mock.Get(sink2)
                .Setup(x => x.AddEntries(
                    It.IsAny<IReadOnlyList<ITableEntry>>(),
                    It.IsAny<bool>()))
                 .Callback(() => addCalled.Add(sink2));

            Mock.Get(sink2)
                .Setup(x => x.RemoveEntries(
                    It.IsAny<IReadOnlyList<ITableEntry>>()))
                .Callback(() => removeCalled.Add(sink2));

            var source = new ErrorListTableDataSource(errorList, tableManager);

            // Act
            source.ClearNuGetEntries();

            var entry = new ErrorListTableEntry("test", LogLevel.Minimal);
            source.AddNuGetEntries(entry);

            var s1 = source.Subscribe(sink);
            var s2 = source.Subscribe(sink2);

            // Assert
            Assert.Contains(sink, addCalled);
            Assert.Contains(sink2, addCalled);
        }

        [Fact]
        public void ErrorListTableDataSource_ClearWithNoEntriesVerifyNoErrors()
        {
            // Arrange
            var errorList = Mock.Of<Microsoft.VisualStudio.Shell.IErrorList>();
            var tableControl = Mock.Of<IWpfTableControl>();
            var tableManager = Mock.Of<ITableManager>();
            var sink = Mock.Of<ITableDataSink>();
            var sink2 = Mock.Of<ITableDataSink>();

            Mock.Get(errorList)
                .Setup(x => x.TableControl)
                .Returns(tableControl);

            Mock.Get(tableControl)
                .Setup(x => x.SubscribeToDataSource(
                    It.IsAny<ITableDataSource>()));

            Mock.Get(tableManager)
                .Setup(x => x.AddSource(
                    It.IsAny<ITableDataSource>()));

            Mock.Get(tableManager)
                .Setup(x => x.RemoveSource(
                    It.IsAny<ITableDataSource>()));

            var source = new ErrorListTableDataSource(errorList, tableManager);

            // Act && Assert no errors
            source.ClearNuGetEntries();
        }
    }
}
