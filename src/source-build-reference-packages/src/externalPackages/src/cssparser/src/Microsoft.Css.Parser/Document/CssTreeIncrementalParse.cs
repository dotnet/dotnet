// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Css.Parser.Parser;
using Microsoft.Css.Parser.Text;
using Microsoft.Css.Parser.Tokens;
using Microsoft.Css.Parser.TreeItems;
using Microsoft.Css.Parser.TreeItems.Comments;

namespace Microsoft.Css.Parser.Document
{
    /// <summary>
    /// Adds incremental parsing support to the CssTree class.
    /// See also US patent 7596747 (Peter Spada, Microsoft)
    /// http://www.patentstorm.us/patents/7596747/description.html
    /// for prior art. In that document, the CSS data structure is a flat list that is
    /// getting incrementally updated. In this code, there is a flat list of tokens
    /// getting updated, and then a flat list of children of a single tree item
    /// that get updated.
    /// </summary>
    public partial class CssTree
    {
        public void OnTextChange(ITextProvider fullNewText, int changeStart, int deletedLength, int insertedLength)
        {
            Debug.Assert(IsOwnerThread, "CssTree.OnTextChange must be called on the main thread");

            if (StyleSheet == null || !IsOwnerThread)
            {
                return;
            }
#if DEBUG_INCREMENTAL_PARSE
            DateTime startTime = DateTime.UtcNow;
#endif
            // Figure out which tokens changed
            ICssParser parser = _parserFactory.CreateParser();
            IncrementalTokenizer.Result tokenResult = IncrementalTokenizer.TokenizeChange(
                parser.TokenizerFactory, Tokens, TextProvider, fullNewText, changeStart, deletedLength, insertedLength);

            // Adjust the input to match what was actually tokenized
            changeStart = tokenResult.TokenizationStart;
            deletedLength = tokenResult.TextDeletedLength;
            insertedLength = tokenResult.TextInsertedLength;

            // Figure out where to start incrementally parsing
            IIncrementalParseItem parentItem = GetIncrementalParseParent(changeStart, deletedLength);
            ComplexItem parentComplexItem = (ComplexItem)parentItem;

            int firstReparseChild = FindFirstChildToReparse(parentComplexItem, tokenResult.OldTokenStart);
            int firstCleanChild = FindFirstCleanChild(parentComplexItem, tokenResult.OldTokenStart + tokenResult.OldTokenCount);
            int firstCleanTokenAfterParent = FindFirstCleanTokenAfterParent(parentComplexItem);

            using (CreateWriteLock())
            {
                // Update the tokens and text
                IncrementalTokenizer.ApplyResult(tokenResult);
                firstCleanTokenAfterParent += tokenResult.NewTokens.Count - tokenResult.OldTokenCount;

                // Init the token stream for parsing
                TokenStream tokenStream = new TokenStream(Tokens);
                int streamPositionStart = FindTokenToStartParsing(parentComplexItem, firstReparseChild);
                tokenStream.Position = streamPositionStart;
                Debug.Assert(tokenStream.Position <= tokenResult.OldTokenStart);

                // Init parsing
                ItemFactory itemFactory = new ItemFactory(parser.ExternalItemFactory, fullNewText, tokenStream);
                tokenStream.SkipComments = true; // must be set after extracting comments

                // Init the old and new child lists
                ParseItemList oldChildren = parentComplexItem.Children;
                ParseItemList newChildren = new ParseItemList();
                ParseItemList deletedChildren = new ParseItemList();
                ParseItemList errorsChangedItems = new ParseItemList();
                int deleteChildCount = oldChildren.Count - firstReparseChild;

                // CreateNextChild needs to know the previous child for context
                ParseItem prevChild = (firstReparseChild > 0) ? oldChildren[firstReparseChild - 1] : null;

                while (true)
                {
                    ParseItem newChild = parentItem.CreateNextChild(prevChild, itemFactory, fullNewText, tokenStream);

                    if (newChild != null)
                    {
                        // Are we done parsing yet?
                        if (newChild.Start >= changeStart + insertedLength)
                        {
                            // See if this new child exactly matches an old child
                            int oldChildIndex = oldChildren.FindInsertIndex(newChild.Start, beforeExisting: true);
                            ParseItem oldChild = (oldChildIndex < oldChildren.Count) ? oldChildren[oldChildIndex] : null;

                            if (oldChild != null &&
                                oldChildIndex >= firstCleanChild &&
                                oldChild.Start == newChild.Start &&
                                oldChild.Length == newChild.Length &&
                                oldChild.GetType() == newChild.GetType())
                            {
                                // Found a match, stop parsing
                                deleteChildCount = oldChildIndex - firstReparseChild;
                                break;
                            }
                        }

                        newChildren.Add(newChild);
                        prevChild = newChild;
                    }
                    else if (tokenStream.Position != firstCleanTokenAfterParent)
                    {
                        // When the parse doesn't stop exactly on the first clean token after the parent,
                        // then the tree structure changed too much. Just fall back to a full parse:
                        ParseNewStyleSheet(fullNewText, Tokens);
                        //Debug.WriteLine("CSS: Full parse:{0}ms", (DateTime.UtcNow - startTime).TotalMilliseconds);

                        return;
                    }
                    else
                    {
                        break;
                    }
                }

                // Replace items in the parent (saving the deleted items for later)
                oldChildren.RemoveRange(firstReparseChild, deleteChildCount, deletedChildren);
                oldChildren.AddRange(newChildren);

                if (oldChildren.Count == 0)
                {
                    // The parent was deleted, currently can't deal with that as an incremental change
                    ParseNewStyleSheet(fullNewText, Tokens);

                    return;
                }

                // Collect comments over the parsed region
                tokenStream.SkipComments = false;
                int tokenCount = tokenStream.Position - streamPositionStart;
                tokenStream.Position = streamPositionStart;
                IList<Comment> comments = parser.ExtractComments(fullNewText, Tokens, tokenStream.Position, tokenCount);

                // All done parsing and updating the tree, so now update caches and fire the "on changed" event

                StyleSheet.TextProvider = fullNewText;

                parentItem.UpdateCachedChildren();

                if (parentItem.UpdateParseErrors())
                {
                    errorsChangedItems.Add(parentComplexItem);
                }

                InsertComments(comments, newChildren);

#if DEBUG_INCREMENTAL_PARSE
                Debug.WriteLine("CSS: Inc parse:{0}ms. Deleted:{1}, Inserted:{2}", (DateTime.UtcNow - startTime).TotalMilliseconds, deletedChildren.Count, newChildren.Count);
                VerifyTokensAfterIncrementalChange(parser.TokenizerFactory, fullNewText, Tokens);
                VerifyTreeAfterIncrementalParse(fullNewText, Tokens, StyleSheet);
#endif
                FireOnItemsChanged(deletedChildren, newChildren, errorsChangedItems);

                // Clean up the deleted items (must be after the event is fired)
                foreach (ParseItem deletedItem in deletedChildren)
                {
                    deletedItem.Parent = null;
                }
            }
        }

        /// <summary>
        /// Inserts comments into the StyleSheet that were in the range of changed text
        /// </summary>
        private void InsertComments(IEnumerable<Comment> comments, IList<ParseItem> newChildren)
        {
            if (comments != null)
            {
                foreach (ParseItem comment in comments)
                {
                    ComplexItem parent = StyleSheet.ComplexItemFromRange(comment.Start, comment.Length) ?? StyleSheet;

                    if (!(parent is Comment))
                    {
                        parent.InsertChildIntoSubtree(comment);

                        // Don't save the comment if its parent or itself is already in the list
                        bool alreadyAdded = false;

                        for (ParseItem commentParent = comment; commentParent != null; commentParent = commentParent.Parent)
                        {
                            if (newChildren.Contains(commentParent))
                            {
                                alreadyAdded = true;
                                break;
                            }
                        }

                        if (!alreadyAdded)
                        {
                            newChildren.Add(comment);
                        }
                    }
                    else // the comment is already in the tree, so we passed the reparsed text
                    {
                        Debug.Assert(parent.Start == comment.Start && parent.Length == comment.Length);
                        break;
                    }
                }
            }
        }

        private int FindTokenToStartParsing(ComplexItem parent, int child)
        {
            if (parent.Parent == null && child == 0)
            {
                // The user typed in the whitespace before the start of the root StyleSheet node.
                // Always start parsing from the first token.
                return 0;
            }

            if (child >= parent.Children.Count)
            {
                // Only hit when there are no children (like when typing in a blank StyleSheet)
                return Tokens.FindInsertIndex(parent.AfterEnd, beforeExisting: true);
            }

            return Tokens.FindInsertIndex(parent.Children[child].Start, beforeExisting: true);
        }

        /// <summary>
        /// Returns the child index for the first item that will be replaced by incremental parsing.
        /// </summary>
        private int FindFirstChildToReparse(ComplexItem parentItem, int firstDirtyTokenIndex)
        {
            CssToken token = Tokens[firstDirtyTokenIndex];

            int childIndex = parentItem.Children.FindInsertIndex(token.Start, beforeExisting: true);

            if (childIndex > 0)
            {
                // Always go to the previous child because it might want to use the new tokens.
                childIndex--;
            }

            return childIndex;
        }

        /// <summary>
        /// Returns the child index for the first item that was unchanged by the token update.
        /// </summary>
        private int FindFirstCleanChild(ComplexItem parentItem, int firstCleanTokenIndex)
        {
            int childIndex = parentItem.Children.Count;

            if (firstCleanTokenIndex < Tokens.Count)
            {
                CssToken token = Tokens[firstCleanTokenIndex];

                // The parent must contain the token (the root StyleSheet contains everything)
                if (parentItem.Parent == null || parentItem.ContainsRange(token.Start, token.Length))
                {
                    childIndex = parentItem.Children.FindInsertIndex(token.Start, beforeExisting: true);
                }
            }

            return childIndex;
        }

        private int FindFirstCleanTokenAfterParent(ComplexItem parentItem)
        {
            int tokenIndex = Tokens.FindInsertIndex(parentItem.AfterEnd, beforeExisting: true);

            // Have to skip comments because that's what the token stream will do while parsing
            while (tokenIndex < Tokens.Count && Tokens[tokenIndex].IsComment)
            {
                tokenIndex++;
            }

            return tokenIndex;
        }

        /// <summary>
        /// Find the item that will contain all of the incremental parsing changes.
        /// This never returns null.
        /// </summary>
        private IIncrementalParseItem GetIncrementalParseParent(int start, int length)
        {
            for (ComplexItem item = StyleSheet.ComplexItemFromRange(start, length); item != null; item = item.Parent)
            {
                if (item is IIncrementalParseItem)
                {
                    return (IIncrementalParseItem)item;
                }
            }

            // Could be typing in whitespace outside all items, so try using the root StyleSheet
            return StyleSheet;
        }

        [Conditional("DEBUG")]
        private void VerifyTokensAfterIncrementalChange(ICssTokenizerFactory tokenizerFactory, ITextProvider newText, TokenList newTokens)
        {
            ICssTokenizer tokenizer = tokenizerFactory.CreateTokenizer();
            TokenList validTokens = tokenizer.Tokenize(newText, 0, newText.Length, keepWhiteSpace: false);

            if (validTokens.Count == newTokens.Count)
            {
                for (int i = 0; i < validTokens.Count && i < newTokens.Count; i++)
                {
                    if (!CssToken.CompareTokens(validTokens[i], newTokens[i], newText, newText))
                    {
                        Debug.Fail("The CssTree.Tokens list is bad");
                        break;
                    }
                }
            }
            else
            {
                Debug.Fail("The CssTree.Tokens list is bad, wrong number of tokens");
            }
        }

        [Conditional("DEBUG")]
        private void VerifyTreeAfterIncrementalParse(ITextProvider newText, TokenList newTokens, StyleSheet newStyleSheet)
        {
            ICssParser parser = _parserFactory.CreateParser();
            StyleSheet validStyleSheet = parser.Parse(newText, newTokens, insertComments: true);

            // By the way, this is the slowest and most lame way to compare two trees
            DebugWriter writer = new DebugWriter();
            string validTreeText = writer.Serialize(newText, validStyleSheet);
            string newTreeText = writer.Serialize(newText, newStyleSheet);

            Debug.Assert(validTreeText == newTreeText, "Bad tree after incremental parsing");
        }
    }
}
