// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Diagnostics;
using System.Threading;
using Microsoft.Css.Parser.Parser;
using Microsoft.Css.Parser.Text;
using Microsoft.Css.Parser.Tokens;
using Microsoft.Css.Parser.TreeItems;

namespace Microsoft.Css.Parser.Document
{
    /// <summary>
    /// The owner of the root CSS StyleSheet object. This also deals with
    /// parsing and updating the StyleSheet as text changes (when OnTextChange is called)
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1001:TypesThatOwnDisposableFieldsShouldBeDisposable")]
    public partial class CssTree : IStyleSheetLockFactory
    {
        public event EventHandler<CssItemsChangedEventArgs> ItemsChanged;
        public event EventHandler<CssTreeUpdateEventArgs> TreeUpdated;

        private readonly ICssParserFactory _parserFactory;
        private readonly ReaderWriterLockSlim _styleSheetLock;
        private readonly int _ownerThreadId;

        public CssTree(ICssParserFactory parserFactory)
        {
            _parserFactory = parserFactory ?? new DefaultParserFactory();
            _styleSheetLock = new ReaderWriterLockSlim(LockRecursionPolicy.SupportsRecursion);
            _ownerThreadId = Thread.CurrentThread.ManagedThreadId;
        }

        public StyleSheet StyleSheet { get; private set; }

        internal TokenList Tokens { get; private set; }

        /// <summary>
        /// Changes after every parse.
        /// </summary>
        public int TreeChangeStamp { get; private set; }

        /// <summary>
        /// Setting this property will parse the text.
        /// </summary>
        public ITextProvider TextProvider
        {
            get { return StyleSheet?.TextProvider; }
            set { ParseNewStyleSheet(value, null); }
        }

        /// <summary>
        /// Internal function to parse a new StyleSheet object
        /// </summary>
        private void ParseNewStyleSheet(ITextProvider textProvider, TokenList tokens)
        {
            ICssParser parser = _parserFactory.CreateParser();

            if (tokens == null)
            {
                ICssTokenizer tokenizer = parser.TokenizerFactory.CreateTokenizer();
                tokens = tokenizer.Tokenize(textProvider, 0, textProvider.Length, keepWhiteSpace: false);
            }

            using (CreateWriteLock())
            {
                Tokens = tokens;
                StyleSheet = parser.Parse(textProvider, tokens, insertComments: true);
            }

            TreeUpdated?.Invoke(this, new CssTreeUpdateEventArgs(this));
        }

        private void FireOnItemsChanged(
            ParseItemList deletedItems,
            ParseItemList insertedItems,
            ParseItemList errorsChangedItems)
        {
#if DEBUG
            Debug.Assert(IsOwnerThread);

            for (int i = 0; i + 1 < insertedItems.Count; i++)
            {
                // Inserted items must not overlap (no need to add children as well as their parent)
                Debug.Assert(insertedItems[i].AfterEnd <= insertedItems[i + 1].Start);
            }
#endif

            if (IsMassiveChange(deletedItems, insertedItems))
            {
                TreeUpdated?.Invoke(this, new CssTreeUpdateEventArgs(this));
            }
            else
            {
                ItemsChanged?.Invoke(this, new CssItemsChangedEventArgs(deletedItems, insertedItems, errorsChangedItems));
            }
        }

        private bool IsMassiveChange(ParseItemList deletedItems, ParseItemList insertedItems)
        {
            // If over half of the root stylesheet changed, then the change is massive.
            // That should catch formatting or deleting the whole document.
            // Massive changes inside of @directives aren't detected.

            if (insertedItems.Count > 0 &&
                insertedItems[0].Parent == StyleSheet &&
                insertedItems.Count > StyleSheet.Children.Count / 2)
            {
                return true;
            }

            if (deletedItems.Count > 0 &&
                deletedItems[0].Parent == StyleSheet &&
                deletedItems.Count > StyleSheet.Children.Count)
            {
                return true;
            }

            return false;
        }

        private class CssTreeLock : IStyleSheetLock
        {
            private CssTree _tree;
            private readonly bool _readOnly;

            public CssTreeLock(CssTree tree, bool readOnly)
            {
                if (tree.AcquireLock(readOnly, waitForReadAccess: false))
                {
                    _tree = tree;
                    _readOnly = readOnly;
                }
            }

            public void Dispose()
            {
                if (_tree != null)
                {
                    _tree.ReleaseLock(_readOnly);
                    _tree = null;
                }
            }

            public StyleSheet StyleSheet
            {
                get { return _tree?.StyleSheet; }
            }

            public bool ShouldCancelReading
            {
                // MSDN says not to use WaitingWriteCount. That's only because it could
                // be dangerous. Since I'm only using it during a read lock, and since
                // false positives are OK, there is no danger.
                get { return _tree == null || _tree._styleSheetLock.WaitingWriteCount != 0; }
            }
        }

        public IStyleSheetLock CreateReadLock()
        {
            return new CssTreeLock(this, readOnly: true);
        }

        /// <summary>
        /// This must be called whenever the main thread is modifying _styleSheet or _tokens.
        /// </summary>
        internal IStyleSheetLock CreateWriteLock()
        {
            return new CssTreeLock(this, readOnly: false);
        }

        private bool AcquireLock(bool readOnly, bool waitForReadAccess)
        {
            if (readOnly)
            {
                Debug.Assert(!IsOwnerThread, "No need to lock CssTree for reading on the main thread.");

                return _styleSheetLock.TryEnterReadLock(waitForReadAccess ? -1 : 0);
            }
            else
            {
                Debug.Assert(IsOwnerThread, "Only the main thread can lock CssTree for writing.");

                if (IsOwnerThread)
                {
                    _styleSheetLock.EnterWriteLock();
                    return true;
                }
            }

            return false;
        }

        private void ReleaseLock(bool readOnly)
        {
            if (readOnly)
            {
                _styleSheetLock.ExitReadLock();
            }
            else
            {
                TreeChangeStamp++;
                _styleSheetLock.ExitWriteLock();
            }
        }

        private bool IsOwnerThread
        {
            get { return _ownerThreadId == Thread.CurrentThread.ManagedThreadId; }
        }
    }
}
