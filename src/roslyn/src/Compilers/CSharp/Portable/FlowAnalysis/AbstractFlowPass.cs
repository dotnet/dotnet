﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

#nullable disable

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using Microsoft.CodeAnalysis.CSharp.Symbols;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.PooledObjects;
using Microsoft.CodeAnalysis.Text;
using Roslyn.Utilities;

namespace Microsoft.CodeAnalysis.CSharp
{
    /// <summary>
    /// An abstract flow pass that takes some shortcuts in analyzing finally blocks, in order to enable
    /// the analysis to take place without tracking exceptions or repeating the analysis of a finally block
    /// for each exit from a try statement.  The shortcut results in a slightly less precise
    /// (but still conservative) analysis, but that less precise analysis is all that is required for
    /// the language specification.  The most significant shortcut is that we do not track the state
    /// where exceptions can arise.  That does not affect the soundness for most analyses, but for those
    /// analyses whose soundness would be affected (e.g. "data flows out"), we track "unassignments" to keep
    /// the analysis sound.
    /// </summary>
    /// <remarks>
    /// Formally, this is a fairly conventional lattice flow analysis (<see
    /// href="https://en.wikipedia.org/wiki/Data-flow_analysis"/>) that moves upward through the <see cref="Join(ref
    /// TLocalState, ref TLocalState)"/> operation.
    /// </remarks>
    internal abstract partial class AbstractFlowPass<TLocalState, TLocalFunctionState> : BoundTreeVisitor
        where TLocalState : AbstractFlowPass<TLocalState, TLocalFunctionState>.ILocalState
        where TLocalFunctionState : AbstractFlowPass<TLocalState, TLocalFunctionState>.AbstractLocalFunctionState
    {
        protected int _recursionDepth;

        /// <summary>
        /// The compilation in which the analysis is taking place.  This is needed to determine which
        /// conditional methods will be compiled and which will be omitted.
        /// </summary>
        protected readonly CSharpCompilation compilation;

        /// <summary>
        /// The method whose body is being analyzed, or the field whose initializer is being analyzed.
        /// May be a top-level member or a lambda or local function. It is used for
        /// references to method parameters. Thus, '_symbol' should not be used directly, but
        /// 'MethodParameters', 'MethodThisParameter' and 'AnalyzeOutParameters(...)' should be used
        /// instead. _symbol is null during speculative binding.
        /// </summary>
        protected Symbol _symbol;

        /// <summary>
        /// Reflects the enclosing member, lambda or local function at the current location (in the bound tree).
        /// </summary>
        protected Symbol CurrentSymbol;

        /// <summary>
        /// The bound node of the method or initializer being analyzed.
        /// </summary>
        protected readonly BoundNode methodMainNode;

        /// <summary>
        /// The flow analysis state at each label, computed by calling <see cref="Join(ref
        /// TLocalState, ref TLocalState)"/> on the state from branches to that label with the state
        /// when we fall into the label.  Entries are created when the label is encountered. One
        /// case deserves special attention: when the destination of the branch is a label earlier
        /// in the code, it is possible (though rarely occurs in practice) that we are changing the
        /// state at a label that we've already analyzed. In that case we run another pass of the
        /// analysis to allow those changes to propagate. This repeats until no further changes to
        /// the state of these labels occurs.  This can result in quadratic performance in unlikely
        /// but possible code such as this: "int x; if (cond) goto l1; x = 3; l5: print x; l4: goto
        /// l5; l3: goto l4; l2: goto l3; l1: goto l2;"
        /// </summary>
        private readonly PooledDictionary<LabelSymbol, TLocalState> _labels;

        /// <summary>
        /// Set to true after an analysis scan if the analysis was incomplete due to state changing
        /// after it was used by another analysis component.  In this case the caller scans again (until
        /// this is false). Since the analysis proceeds by monotonically changing the state computed
        /// at each label, this must terminate.
        /// </summary>
        protected bool stateChangedAfterUse;

        /// <summary>
        /// All of the labels seen so far in this forward scan of the body
        /// </summary>
        private PooledHashSet<BoundStatement> _labelsSeen;

        /// <summary>
        /// Pending escapes generated in the current scope (or more deeply nested scopes). When jump
        /// statements (goto, break, continue, return) are processed, they are placed in the
        /// pendingBranches buffer to be processed later by the code handling the destination
        /// statement. As a special case, the processing of try-finally statements might modify the
        /// contents of the pendingBranches buffer to take into account the behavior of
        /// "intervening" finally clauses.
        /// </summary>
        protected PendingBranchesCollection PendingBranches { get; private set; }

        /// <summary>
        /// The definite assignment and/or reachability state at the point currently being analyzed.
        /// </summary>
        protected TLocalState State;
        protected TLocalState StateWhenTrue;
        protected TLocalState StateWhenFalse;
        protected bool IsConditionalState;

        /// <summary>
        /// Indicates that the transfer function for a particular node (the function mapping the
        /// state before the node to the state after the node) is not monotonic, in the sense that
        /// it can change the state in either direction in the lattice. If the transfer function is
        /// monotonic, the transfer function can only change the state toward the <see
        /// cref="UnreachableState"/>. Reachability and definite assignment are monotonic, and
        /// permit a more efficient analysis. Region analysis and nullable analysis are not
        /// monotonic. This is just an optimization; we could treat all of them as nonmonotonic
        /// without much loss of performance. In fact, this only affects the analysis of (relatively
        /// rare) try statements, and is only a slight optimization.
        /// </summary>
        private readonly bool _nonMonotonicTransfer;

        protected void SetConditionalState((TLocalState whenTrue, TLocalState whenFalse) state)
        {
            SetConditionalState(state.whenTrue, state.whenFalse);
        }

        protected void SetConditionalState(TLocalState whenTrue, TLocalState whenFalse)
        {
            IsConditionalState = true;
            State = default(TLocalState);
            StateWhenTrue = whenTrue;
            StateWhenFalse = whenFalse;
        }

        protected void SetState(TLocalState newState)
        {
            Debug.Assert(newState != null);
            StateWhenTrue = StateWhenFalse = default(TLocalState);
            IsConditionalState = false;
            State = newState;
        }

        protected void Split()
        {
            if (!IsConditionalState)
            {
                SetConditionalState(State, State.Clone());
            }
        }

        protected void Unsplit()
        {
            if (IsConditionalState)
            {
                Join(ref StateWhenTrue, ref StateWhenFalse);
                SetState(StateWhenTrue);
            }
        }

        /// <summary>
        /// Where all diagnostics are deposited.
        /// </summary>
        protected DiagnosticBag Diagnostics { get; }

        #region Region
        // For region analysis, we maintain some extra data.
        protected RegionPlace regionPlace; // tells whether we are currently analyzing code before, during, or after the region
        protected readonly BoundNode firstInRegion, lastInRegion;
        protected readonly bool TrackingRegions;

        /// <summary>
        /// A cache of the state at the backward branch point of each loop.  This is not needed
        /// during normal flow analysis, but is needed for DataFlowsOut region analysis.
        /// </summary>
        private readonly Dictionary<BoundLoopStatement, TLocalState> _loopHeadState;
        #endregion Region

        protected AbstractFlowPass(
            CSharpCompilation compilation,
            Symbol symbol,
            BoundNode node,
            BoundNode firstInRegion = null,
            BoundNode lastInRegion = null,
            bool trackRegions = false,
            bool nonMonotonicTransferFunction = false)
        {
            Debug.Assert(node != null);

            if (firstInRegion != null && lastInRegion != null)
            {
                trackRegions = true;
            }

            if (trackRegions)
            {
                Debug.Assert(firstInRegion != null);
                Debug.Assert(lastInRegion != null);
                int startLocation = firstInRegion.Syntax.SpanStart;
                int endLocation = lastInRegion.Syntax.Span.End;
                int length = endLocation - startLocation;
                Debug.Assert(length >= 0, "last comes before first");
                this.RegionSpan = new TextSpan(startLocation, length);
            }

            PendingBranches = new PendingBranchesCollection();
            _labelsSeen = PooledHashSet<BoundStatement>.GetInstance();
            _labels = PooledDictionary<LabelSymbol, TLocalState>.GetInstance();
            this.Diagnostics = DiagnosticBag.GetInstance();
            this.compilation = compilation;
            _symbol = symbol;
            CurrentSymbol = symbol;
            this.methodMainNode = node;
            this.firstInRegion = firstInRegion;
            this.lastInRegion = lastInRegion;
            _loopHeadState = new Dictionary<BoundLoopStatement, TLocalState>(ReferenceEqualityComparer.Instance);
            TrackingRegions = trackRegions;
            _nonMonotonicTransfer = nonMonotonicTransferFunction;
        }

        protected abstract string Dump(TLocalState state);

        protected string Dump()
        {
            return IsConditionalState
                ? $"true: {Dump(this.StateWhenTrue)} false: {Dump(this.StateWhenFalse)}"
                : Dump(this.State);
        }

#if DEBUG
        protected string DumpLabels()
        {
            StringBuilder result = new StringBuilder();
            result.Append("Labels{");
            bool first = true;
            foreach (var key in _labels.Keys)
            {
                if (!first)
                {
                    result.Append(", ");
                }

                string name = key.Name;
                if (string.IsNullOrEmpty(name))
                {
                    name = "<Label>" + key.GetHashCode();
                }

                result.Append(name).Append(": ").Append(this.Dump(_labels[key]));
                first = false;
            }
            result.Append('}');
            return result.ToString();
        }
#endif

        private void EnterRegionIfNeeded(BoundNode node)
        {
            if (TrackingRegions && node == this.firstInRegion && this.regionPlace == RegionPlace.Before)
            {
                EnterRegion();
            }
        }

        /// <summary>
        /// Subclasses may override EnterRegion to perform any actions at the entry to the region.
        /// </summary>
        protected virtual void EnterRegion()
        {
            Debug.Assert(this.regionPlace == RegionPlace.Before);
            this.regionPlace = RegionPlace.Inside;
        }

        private void LeaveRegionIfNeeded(BoundNode node)
        {
            if (TrackingRegions && node == this.lastInRegion && this.regionPlace == RegionPlace.Inside)
            {
                LeaveRegion();
            }
        }

        /// <summary>
        /// Subclasses may override LeaveRegion to perform any action at the end of the region.
        /// </summary>
        protected virtual void LeaveRegion()
        {
            Debug.Assert(IsInside);
            this.regionPlace = RegionPlace.After;
        }

        protected readonly TextSpan RegionSpan;

        protected bool RegionContains(TextSpan span)
        {
            // TODO: There are no scenarios involving a zero-length span
            // currently. If the assert fails, add a corresponding test.
            Debug.Assert(span.Length > 0);
            if (span.Length == 0)
            {
                return RegionSpan.Contains(span.Start);
            }
            return RegionSpan.Contains(span);
        }

        protected bool IsInside
        {
            get
            {
                return regionPlace == RegionPlace.Inside;
            }
        }

        protected virtual void EnterParameters(ImmutableArray<ParameterSymbol> parameters)
        {
            foreach (var parameter in parameters)
            {
                EnterParameter(parameter);
            }
        }

        protected virtual void EnterParameter(ParameterSymbol parameter)
        { }

        protected virtual void LeaveParameters(
            ImmutableArray<ParameterSymbol> parameters,
            SyntaxNode syntax,
            Location location)
        {
            foreach (ParameterSymbol parameter in parameters)
            {
                LeaveParameter(parameter, syntax, location);
            }
        }

        protected virtual void LeaveParameter(ParameterSymbol parameter, SyntaxNode syntax, Location location)
        { }

        public override BoundNode Visit(BoundNode node)
        {
            return VisitAlways(node);
        }

        protected BoundNode VisitAlways(BoundNode node)
        {
            BoundNode result = null;

            // We scan even expressions, because we must process lambdas contained within them.
            if (node != null)
            {
                EnterRegionIfNeeded(node);
                VisitWithStackGuard(node);
                LeaveRegionIfNeeded(node);
            }

            return result;
        }

        [DebuggerStepThrough]
        private BoundNode VisitWithStackGuard(BoundNode node)
        {
            if (node is BoundExpression or BoundPattern)
            {
                return VisitExpressionOrPatternWithStackGuard(ref _recursionDepth, node);
            }

            return base.Visit(node);
        }

        [DebuggerStepThrough]
        protected override BoundNode VisitExpressionOrPatternWithoutStackGuard(BoundNode node)
        {
            return base.Visit(node);
        }

        protected override bool ConvertInsufficientExecutionStackExceptionToCancelledByStackGuardException()
        {
            return false; // just let the original exception bubble up.
        }

        /// <summary>
        /// A pending branch.  These are created for a return, break, continue, goto statement,
        /// yield return, yield break, await expression, and await foreach/using. The idea is that
        /// we don't know if the branch will eventually reach its destination because of an
        /// intervening finally block that cannot complete normally.  So we store them up and handle
        /// them as we complete processing each construct.  At the end of a block, if there are any
        /// pending branches to a label in that block we process the branch.  Otherwise we relay it
        /// up to the enclosing construct as a pending branch of the enclosing construct.
        /// </summary>
        internal sealed class PendingBranch
        {
            public readonly BoundNode Branch;
            public bool IsConditionalState;
            public TLocalState State;
            public TLocalState StateWhenTrue;
            public TLocalState StateWhenFalse;
#nullable enable
            public readonly LabelSymbol? Label;
#nullable disable

            public PendingBranch(BoundNode branch, TLocalState state, LabelSymbol label, bool isConditionalState = false, TLocalState stateWhenTrue = default, TLocalState stateWhenFalse = default)
            {
                this.Branch = branch;
                this.State = state.Clone();
                this.IsConditionalState = isConditionalState;
                if (isConditionalState)
                {
                    this.StateWhenTrue = stateWhenTrue.Clone();
                    this.StateWhenFalse = stateWhenFalse.Clone();
                }
                this.Label = label;
            }
        }

        /// <summary>
        /// Perform a single pass of flow analysis.  Note that after this pass,
        /// this.backwardBranchChanged indicates if a further pass is required.
        /// </summary>
        protected virtual ImmutableArray<PendingBranch> Scan(ref bool badRegion)
        {
            var oldPending = SavePending();
            Visit(methodMainNode);
            this.Unsplit();
            RestorePending(oldPending);
            if (TrackingRegions && regionPlace != RegionPlace.After)
            {
                badRegion = true;
            }

            ImmutableArray<PendingBranch> result = RemoveReturns();
            return result;
        }

        protected ImmutableArray<PendingBranch> Analyze(ref bool badRegion, Optional<TLocalState> initialState = default)
        {
            ImmutableArray<PendingBranch> returns;
            do
            {
                // the entry point of a method is assumed reachable
                regionPlace = RegionPlace.Before;
                this.State = initialState.HasValue ? initialState.Value : TopState();
                PendingBranches.Clear();
                this.stateChangedAfterUse = false;
                this.Diagnostics.Clear();
                returns = this.Scan(ref badRegion);
            }
            while (this.stateChangedAfterUse);

            return returns;
        }

        protected virtual void Free()
        {
            this.Diagnostics.Free();
            PendingBranches.Free();
            _labelsSeen.Free();
            _labels.Free();
        }

        /// <summary>
        /// If a method is currently being analyzed returns its parameters, returns an empty array
        /// otherwise.
        /// </summary>
        protected ImmutableArray<ParameterSymbol> MethodParameters
        {
            get
            {
                var method = _symbol as MethodSymbol;
                return (object)method == null ? ImmutableArray<ParameterSymbol>.Empty : method.Parameters;
            }
        }

        /// <summary>
        /// If a method is currently being analyzed returns its 'this' parameter, returns null
        /// otherwise.
        /// </summary>
        protected ParameterSymbol MethodThisParameter
        {
            get
            {
                ParameterSymbol thisParameter = null;
                (_symbol as MethodSymbol)?.TryGetThisParameter(out thisParameter);
                return thisParameter;
            }
        }

        /// <summary>
        /// Specifies whether or not method's out parameters should be analyzed.
        /// </summary>
        /// <param name="location">location to be used</param>
        /// <returns>true if the out parameters of the method should be analyzed</returns>
        protected bool ShouldAnalyzeOutParameters(out Location location)
        {
            var method = _symbol as MethodSymbol;
            if ((object)method == null || method.Locations.Length != 1)
            {
                location = null;
                return false;
            }
            else
            {
                location = method.GetFirstLocation();
                return true;
            }
        }

        /// <summary>
        /// Return the flow analysis state associated with a label.
        /// </summary>
        /// <param name="label"></param>
        /// <returns></returns>
        protected virtual TLocalState LabelState(LabelSymbol label)
        {
            TLocalState result;
            if (_labels.TryGetValue(label, out result))
            {
                return result;
            }

            result = UnreachableState();
            _labels.Add(label, result);
            return result;
        }

        /// <summary>
        /// Return to the caller the set of pending return statements.
        /// </summary>
        /// <returns></returns>
        protected virtual ImmutableArray<PendingBranch> RemoveReturns()
        {
            ImmutableArray<PendingBranch> result;
            result = PendingBranches.ToImmutable();
            PendingBranches.Clear();

            // The caller should have handled and cleared labelsSeen.
            Debug.Assert(_labelsSeen.Count == 0);
            return result;
        }

        /// <summary>
        /// Set the current state to one that indicates that it is unreachable.
        /// </summary>
        protected void SetUnreachable()
        {
            this.State = UnreachableState();
        }

        protected void VisitLvalue(BoundExpression node)
        {
            EnterRegionIfNeeded(node);

            switch (node?.Kind)
            {
                case BoundKind.Parameter:
                    VisitLvalueParameter((BoundParameter)node);
                    break;

                case BoundKind.Local:
                    VisitLvalue((BoundLocal)node);
                    break;

                case BoundKind.ThisReference:
                case BoundKind.BaseReference:
                    break;

                case BoundKind.PropertyAccess:
                    {
                        var access = (BoundPropertyAccess)node;

                        if (Binder.AccessingAutoPropertyFromConstructor(access, _symbol))
                        {
                            var backingField = (access.PropertySymbol as SourcePropertySymbolBase)?.BackingField;
                            if (backingField != null)
                            {
                                VisitFieldAccessInternal(access.ReceiverOpt, backingField);
                                break;
                            }
                        }
                    }

                    goto default;

                case BoundKind.FieldAccess:
                    {
                        BoundFieldAccess node1 = (BoundFieldAccess)node;
                        VisitFieldAccessInternal(node1.ReceiverOpt, node1.FieldSymbol);
                        break;
                    }

                case BoundKind.EventAccess:
                    {
                        BoundEventAccess node1 = (BoundEventAccess)node;
                        VisitFieldAccessInternal(node1.ReceiverOpt, node1.EventSymbol.AssociatedField);
                        break;
                    }

                case BoundKind.TupleLiteral:
                case BoundKind.ConvertedTupleLiteral:
                    ((BoundTupleExpression)node).VisitAllElements((x, self) => self.VisitLvalue(x), this);
                    break;

                case BoundKind.InlineArrayAccess:
                    {
                        var access = (BoundInlineArrayAccess)node;
                        VisitLvalue(access);
                        break;
                    }
                default:
                    VisitRvalue(node);
                    break;
            }

            LeaveRegionIfNeeded(node);
        }

        protected virtual void VisitLvalue(BoundLocal node)
        {
        }

        /// <summary>
        /// Visit a boolean condition expression.
        /// </summary>
        /// <param name="node"></param>
        protected void VisitCondition(BoundExpression node)
        {
            Visit(node);
            AdjustConditionalState(node);
        }

        protected void AdjustConditionalState(BoundExpression node)
        {
            if (IsConstantTrue(node))
            {
                Unsplit();
                SetConditionalState(this.State, UnreachableState());
            }
            else if (IsConstantFalse(node))
            {
                Unsplit();
                SetConditionalState(UnreachableState(), this.State);
            }
            else if ((object)node.Type == null || node.Type.SpecialType != SpecialType.System_Boolean)
            {
                // a dynamic type or a type with operator true/false
                Unsplit();
            }

            Split();
        }

        /// <summary>
        /// Visit a general expression, where we will only need to determine if variables are
        /// assigned (or not). That is, we will not be needing AssignedWhenTrue and
        /// AssignedWhenFalse.
        /// </summary>
        /// <param name="isKnownToBeAnLvalue">True when visiting an rvalue that will actually be used as an lvalue,
        /// for example a ref parameter when simulating a read of it, or an argument corresponding to an in parameter</param>
        protected virtual void VisitRvalue(BoundExpression node, bool isKnownToBeAnLvalue = false)
        {
            Visit(node);
            Unsplit();
        }

        /// <summary>
        /// Visit a statement.
        /// </summary>
        [DebuggerHidden]
        protected virtual void VisitStatement(BoundStatement statement)
        {
            Visit(statement);
            Debug.Assert(!this.IsConditionalState);
        }

        protected static bool IsConstantTrue(BoundExpression node)
        {
            return node.ConstantValueOpt == ConstantValue.True;
        }

        protected static bool IsConstantFalse(BoundExpression node)
        {
            return node.ConstantValueOpt == ConstantValue.False;
        }

        protected static bool IsConstantNull(BoundExpression node)
        {
            return node.ConstantValueOpt == ConstantValue.Null;
        }

        /// <summary>
        /// Called at the point in a loop where the backwards branch would go to.
        /// </summary>
        private void LoopHead(BoundLoopStatement node)
        {
            TLocalState previousState;
            if (_loopHeadState.TryGetValue(node, out previousState))
            {
                Join(ref this.State, ref previousState);
            }

            _loopHeadState[node] = this.State.Clone();
        }

        /// <summary>
        /// Called at the point in a loop where the backward branch is placed.
        /// </summary>
        private void LoopTail(BoundLoopStatement node)
        {
            var oldState = _loopHeadState[node];
            if (Join(ref oldState, ref this.State))
            {
                _loopHeadState[node] = oldState;
                this.stateChangedAfterUse = true;
            }
        }

#nullable enable
        /// <summary>
        /// Used to resolve break statements in each statement form that has a break statement
        /// (loops, switch).
        /// </summary>
        private void ResolveBreaks(TLocalState breakState, LabelSymbol label)
        {
            JoinPendingBranches(ref breakState, label);
            SetState(breakState);
        }

        /// <summary>
        /// Used to resolve continue statements in each statement form that supports it.
        /// </summary>
        private void ResolveContinues(LabelSymbol continueLabel)
        {
            // Technically, nothing in the language specification depends on the state
            // at the continue label, so we could just discard them instead of merging
            // the states. In fact, we need not have added continue statements to the
            // pending jump queue in the first place if we were interested solely in the
            // flow analysis.  However, region analysis (in support of extract method)
            // and other forms of more precise analysis
            // depend on continue statements appearing in the pending branch queue, so
            // we process them from the queue here.
            JoinPendingBranches(ref this.State, continueLabel);
        }

        private void JoinPendingBranches(ref TLocalState state, LabelSymbol label)
        {
            var pendingBranches = PendingBranches.GetAndRemoveBranches(label);
            if (pendingBranches is { })
            {
                foreach (var pending in pendingBranches)
                {
                    Join(ref state, ref pending.State);
                }
                pendingBranches.Free();
            }
        }

        /// <summary>
        /// Subclasses override this if they want to take special actions on processing a goto
        /// statement, when both the jump and the label have been located.
        /// </summary>
        protected virtual void NoteBranch(PendingBranch pending, BoundNode gotoStmt, BoundStatement target)
        {
            target.AssertIsLabeledStatement();
        }

        /// <summary>
        /// To handle a label, we resolve all branches to that label.  Returns true if the state of
        /// the label changes as a result.
        /// </summary>
        /// <param name="label">Target label</param>
        /// <param name="target">Statement containing the target label</param>
        private bool ResolveBranches(LabelSymbol label, BoundStatement? target)
        {
            target?.AssertIsLabeledStatementWithLabel(label);

            bool labelStateChanged = false;

            var pendingBranches = PendingBranches.GetAndRemoveBranches(label);
            if (pendingBranches is { })
            {
                foreach (var pending in pendingBranches)
                {
                    ResolveBranch(pending, label, target, ref labelStateChanged);
                }
                pendingBranches.Free();
            }

            return labelStateChanged;
        }

        protected virtual void ResolveBranch(PendingBranch pending, LabelSymbol label, BoundStatement? target, ref bool labelStateChanged)
        {
            var state = LabelState(label);
            if (target != null)
            {
                NoteBranch(pending, pending.Branch, target);
            }

            var changed = Join(ref state, ref pending.State);
            if (changed)
            {
                labelStateChanged = true;
                _labels[label] = state;
            }
        }

        protected readonly struct SavedPending
        {
            public readonly PendingBranchesCollection PendingBranches;
            public readonly PooledHashSet<BoundStatement> LabelsSeen;

            public SavedPending(PendingBranchesCollection pendingBranches, PooledHashSet<BoundStatement> labelsSeen)
            {
                this.PendingBranches = pendingBranches;
                this.LabelsSeen = labelsSeen;
            }
        }

        /// <summary>
        /// Since branches cannot branch into constructs, only out, we save the pending branches
        /// when visiting more nested constructs.  When tracking exceptions, we store the current
        /// state as the exception state for the following code.
        /// </summary>
        protected SavedPending SavePending()
        {
            Debug.Assert(!this.IsConditionalState);
            var result = new SavedPending(PendingBranches, _labelsSeen);

            PendingBranches = new PendingBranchesCollection();
            _labelsSeen = PooledHashSet<BoundStatement>.GetInstance();

            return result;
        }

        /// <summary>
        /// We use this when closing a block that may contain labels or branches
        /// - branches to new labels are resolved
        /// - new labels are removed (no longer can be reached)
        /// - unresolved pending branches are carried forward
        /// </summary>
        /// <param name="oldPending">The old pending branches, which are to be merged with the current ones</param>
        protected void RestorePending(SavedPending oldPending)
        {
            foreach (var node in _labelsSeen)
            {
                switch (node.Kind)
                {
                    case BoundKind.LabeledStatement:
                        {
                            var label = (BoundLabeledStatement)node;
                            stateChangedAfterUse |= ResolveBranches(label.Label, label);
                        }
                        break;
                    case BoundKind.LabelStatement:
                        {
                            var label = (BoundLabelStatement)node;
                            stateChangedAfterUse |= ResolveBranches(label.Label, label);
                        }
                        break;
                    case BoundKind.SwitchSection:
                        {
                            var sec = (BoundSwitchSection)node;
                            foreach (var label in sec.SwitchLabels)
                            {
                                stateChangedAfterUse |= ResolveBranches(label.Label, sec);
                            }
                        }
                        break;
                    default:
                        // there are no other kinds of labels
                        throw ExceptionUtilities.UnexpectedValue(node.Kind);
                }
            }

            oldPending.PendingBranches.AddRange(this.PendingBranches);

            PendingBranches.Free();
            PendingBranches = oldPending.PendingBranches;

            // We only use SavePending/RestorePending when there could be no branch into the region between them.
            // So there is no need to save the labels seen between the calls.  If there were such a need, we would
            // do "this.labelsSeen.UnionWith(oldPending.LabelsSeen);" instead of the following assignment
            _labelsSeen.Free();
            _labelsSeen = oldPending.LabelsSeen;
        }
#nullable disable

        #region visitors

        /// <summary>
        /// Since each language construct must be handled according to the rules of the language specification,
        /// the default visitor reports that the construct for the node is not implemented in the compiler.
        /// </summary>
        public override BoundNode DefaultVisit(BoundNode node)
        {
            RoslynDebug.Assert(false, $"Should Visit{node.Kind} be overridden in {this.GetType().Name}?");
            Diagnostics.Add(ErrorCode.ERR_InternalError, node.Syntax.Location);
            return null;
        }

        public override BoundNode VisitAttribute(BoundAttribute node)
        {
            // No flow analysis is ever done in attributes (or their arguments).
            return null;
        }

        public override BoundNode VisitThrowExpression(BoundThrowExpression node)
        {
            VisitRvalue(node.Expression);
            SetUnreachable();
            return node;
        }

        public override BoundNode VisitPassByCopy(BoundPassByCopy node)
        {
            VisitRvalue(node.Expression);
            return node;
        }

        public override BoundNode VisitIsPatternExpression(BoundIsPatternExpression node)
        {
            Debug.Assert(!IsConditionalState);

            // Local functions below need to handle all patterns that come through here
            Debug.Assert(node.Pattern is
                BoundTypePattern or BoundRecursivePattern or BoundITuplePattern or BoundRelationalPattern or
                BoundDeclarationPattern or BoundConstantPattern or BoundNegatedPattern or BoundBinaryPattern or
                BoundDeclarationPattern or BoundDiscardPattern or BoundListPattern or BoundSlicePattern);

            bool negated = node.Pattern.IsNegated(out var pattern);
            Debug.Assert(negated == node.IsNegated);

            if (VisitPossibleConditionalAccess(node.Expression, out var stateWhenNotNull))
            {
                Debug.Assert(!IsConditionalState);
                SetConditionalState(patternMatchesNull(pattern)
                    ? (State, stateWhenNotNull)
                    : (stateWhenNotNull, State));
            }
            else if (IsConditionalState)
            {
                // Patterns which only match a single boolean value should propagate conditional state
                // for example, `(a != null && a.M(out x)) is true` should have the same conditional state as `(a != null && a.M(out x))`.
                if (isBoolTest(pattern) is bool value)
                {
                    if (!value)
                    {
                        SetConditionalState(StateWhenFalse, StateWhenTrue);
                    }
                }
                else
                {
                    // Patterns which match more than a single boolean value cannot propagate conditional state
                    // for example, `(a != null && a.M(out x)) is bool b` should not have conditional state
                    Unsplit();
                }
            }

            VisitPattern(pattern);
            var reachableLabels = node.ReachabilityDecisionDag.ReachableLabels;
            if (!reachableLabels.Contains(node.WhenTrueLabel))
            {
                SetState(this.StateWhenFalse);
                SetConditionalState(UnreachableState(), this.State);
            }
            else if (!reachableLabels.Contains(node.WhenFalseLabel))
            {
                SetState(this.StateWhenTrue);
                SetConditionalState(this.State, UnreachableState());
            }

            if (negated)
            {
                SetConditionalState(this.StateWhenFalse, this.StateWhenTrue);
            }

            return node;

            static bool patternMatchesNull(BoundPattern pattern)
            {
                switch (pattern)
                {
                    case BoundTypePattern:
                    case BoundRecursivePattern:
                    case BoundITuplePattern:
                    case BoundRelationalPattern:
                    case BoundDeclarationPattern { IsVar: false }:
                    case BoundConstantPattern { ConstantValue: { IsNull: false } }:
                    case BoundListPattern:
                    case BoundSlicePattern: // Only occurs in error cases
                        return false;
                    case BoundConstantPattern { ConstantValue: { IsNull: true } }:
                        return true;
                    case BoundNegatedPattern negated:
                        return !patternMatchesNull(negated.Negated);
                    case BoundBinaryPattern binary:
                        var binaryPatterns = getBinaryPatterns(binary);
                        Debug.Assert(binaryPatterns.Peek().Left is not BoundBinaryPattern);
                        bool currentPatternMatchesNull = patternMatchesNull(binaryPatterns.Peek().Left);

                        while (binaryPatterns.TryPop(out var currentBinary))
                        {
                            if (currentBinary.Disjunction)
                            {
                                // `a?.b(out x) is null or C`
                                // pattern matches null if either subpattern matches null
                                currentPatternMatchesNull = currentPatternMatchesNull || patternMatchesNull(currentBinary.Right);
                                continue;
                            }

                            // `a?.b out x is not null and var c`
                            // pattern matches null only if both subpatterns match null
                            currentPatternMatchesNull = currentPatternMatchesNull && patternMatchesNull(currentBinary.Right);
                        }

                        binaryPatterns.Free();
                        return currentPatternMatchesNull;
                    case BoundDeclarationPattern { IsVar: true }:
                    case BoundDiscardPattern:
                        return true;
                    default:
                        throw ExceptionUtilities.UnexpectedValue(pattern.Kind);
                }
            }

            // Returns `true` if the pattern only matches a `true` input.
            // Returns `false` if the pattern only matches a `false` input.
            // Otherwise, returns `null`.
            static bool? isBoolTest(BoundPattern pattern)
            {
                switch (pattern)
                {
                    case BoundConstantPattern { ConstantValue: { IsBoolean: true, BooleanValue: var boolValue } }:
                        return boolValue;
                    case BoundNegatedPattern negated:
                        return !isBoolTest(negated.Negated);
                    case BoundBinaryPattern binary:
                        var binaryPatterns = getBinaryPatterns(binary);
                        Debug.Assert(binaryPatterns.Peek().Left is not BoundBinaryPattern);
                        bool? currentBoolTest = isBoolTest(binaryPatterns.Peek().Left);

                        while (binaryPatterns.TryPop(out var currentBinary))
                        {
                            if (currentBinary.Disjunction)
                            {
                                // `(a != null && a.b(out x)) is true or true` matches `true`
                                // `(a != null && a.b(out x)) is true or false` matches any boolean
                                // both subpatterns must have the same bool test for the test to propagate out
                                var leftNullTest = currentBoolTest;
                                currentBoolTest = leftNullTest is null ? null :
                                    leftNullTest != isBoolTest(currentBinary.Right) ? null :
                                    leftNullTest;
                                continue;
                            }

                            // `(a != null && a.b(out x)) is true and true` matches `true`
                            // `(a != null && a.b(out x)) is true and var x` matches `true`
                            // `(a != null && a.b(out x)) is true and false` never matches and is a compile error
                            currentBoolTest ??= isBoolTest(currentBinary.Right);
                        }

                        binaryPatterns.Free();
                        return currentBoolTest;
                    case BoundConstantPattern { ConstantValue: { IsBoolean: false } }:
                    case BoundDiscardPattern:
                    case BoundTypePattern:
                    case BoundRecursivePattern:
                    case BoundITuplePattern:
                    case BoundRelationalPattern:
                    case BoundDeclarationPattern:
                    case BoundListPattern:
                    case BoundSlicePattern: // Only occurs in error cases
                        return null;
                    default:
                        throw ExceptionUtilities.UnexpectedValue(pattern.Kind);
                }
            }

            static ArrayBuilder<BoundBinaryPattern> getBinaryPatterns(BoundBinaryPattern binaryPattern)
            {
                // Users (such as ourselves) can have many, many nested binary patterns. To avoid crashing, do left recursion manually.

                var stack = ArrayBuilder<BoundBinaryPattern>.GetInstance();

                while (true)
                {
                    stack.Push(binaryPattern);
                    if (binaryPattern.Left is BoundBinaryPattern leftBinaryPattern)
                    {
                        binaryPattern = leftBinaryPattern;
                    }
                    else
                    {
                        break;
                    }
                }

                return stack;
            }
        }

        public virtual void VisitPattern(BoundPattern pattern)
        {
            Split();
        }

        public override BoundNode VisitConstantPattern(BoundConstantPattern node)
        {
            // All patterns are handled by VisitPattern
            throw ExceptionUtilities.Unreachable();
        }

        public override BoundNode VisitBinaryPattern(BoundBinaryPattern node)
        {
            // All patterns are handled by VisitPattern
            throw ExceptionUtilities.Unreachable();
        }

        public override BoundNode VisitTupleLiteral(BoundTupleLiteral node)
        {
            return VisitTupleExpression(node);
        }

        public override BoundNode VisitConvertedTupleLiteral(BoundConvertedTupleLiteral node)
        {
            return VisitTupleExpression(node);
        }

        private BoundNode VisitTupleExpression(BoundTupleExpression node)
        {
            VisitArguments(node.Arguments, default(ImmutableArray<RefKind>), null, default, false);
            return null;
        }

        public override BoundNode VisitTupleBinaryOperator(BoundTupleBinaryOperator node)
        {
            VisitRvalue(node.Left);
            VisitRvalue(node.Right);
            return null;
        }

        public override BoundNode VisitDynamicObjectCreationExpression(BoundDynamicObjectCreationExpression node)
        {
            VisitArguments(node.Arguments, node.ArgumentRefKindsOpt, null, node.ArgsToParamsOpt, node.Expanded);
            VisitRvalue(node.InitializerExpressionOpt);
            return null;
        }

        public override BoundNode VisitDynamicIndexerAccess(BoundDynamicIndexerAccess node)
        {
            VisitRvalue(node.Receiver);
            VisitArguments(node.Arguments, node.ArgumentRefKindsOpt, null, default, false);
            return null;
        }

        public override BoundNode VisitDynamicMemberAccess(BoundDynamicMemberAccess node)
        {
            VisitRvalue(node.Receiver);
            return null;
        }

        public override BoundNode VisitDynamicInvocation(BoundDynamicInvocation node)
        {
            VisitRvalue(node.Expression);
            VisitArguments(node.Arguments, node.ArgumentRefKindsOpt, null, default, false);
            return null;
        }

#nullable enable
        protected BoundNode? VisitInterpolatedStringBase(BoundInterpolatedStringBase node, InterpolatedStringHandlerData? data)
        {
            // If there can be any branching, then we need to treat the expressions
            // as optionally evaluated. Otherwise, we treat them as always evaluated
            (BoundExpression? construction, bool useBoolReturns, bool firstPartIsConditional) = data switch
            {
                null or { BuilderType: null } => (null, false, false),
                { } d => (d.Construction, d.UsesBoolReturns, d.HasTrailingHandlerValidityParameter)
            };

            VisitInterpolatedStringHandlerConstructor(construction);
            bool hasConditionalEvaluation = useBoolReturns || firstPartIsConditional;
            TLocalState? shortCircuitState = hasConditionalEvaluation ? State.Clone() : default;

            _ = VisitInterpolatedStringHandlerParts(node, useBoolReturns, firstPartIsConditional, ref shortCircuitState);

            if (hasConditionalEvaluation)
            {
                Debug.Assert(shortCircuitState != null);
                Join(ref this.State, ref shortCircuitState);
            }

            return null;
        }

        protected virtual void VisitInterpolatedStringHandlerConstructor(BoundExpression? constructor)
        {
            VisitRvalue(constructor);
        }
#nullable disable

        public override BoundNode VisitInterpolatedString(BoundInterpolatedString node)
        {
            return VisitInterpolatedStringBase(node, node.InterpolationData);
        }

        public override BoundNode VisitUnconvertedInterpolatedString(BoundUnconvertedInterpolatedString node)
        {
            // If the node is unconverted, we'll just treat it as if the contents are always evaluated
            return VisitInterpolatedStringBase(node, data: null);
        }

        public override BoundNode VisitStringInsert(BoundStringInsert node)
        {
            VisitRvalue(node.Value);
            if (node.Alignment != null)
            {
                VisitRvalue(node.Alignment);
            }

            if (node.Format != null)
            {
                VisitRvalue(node.Format);
            }

            return null;
        }

        public override BoundNode VisitInterpolatedStringHandlerPlaceholder(BoundInterpolatedStringHandlerPlaceholder node)
        {
            return null;
        }

        public override BoundNode VisitInterpolatedStringArgumentPlaceholder(BoundInterpolatedStringArgumentPlaceholder node)
        {
            return null;
        }

        public override BoundNode VisitArgList(BoundArgList node)
        {
            // The "__arglist" expression that is legal inside a varargs method has no
            // effect on flow analysis and it has no children.
            return null;
        }

        public override BoundNode VisitArgListOperator(BoundArgListOperator node)
        {
            // When we have M(__arglist(x, y, z)) we must visit x, y and z.
            VisitArguments(node.Arguments, node.ArgumentRefKindsOpt, null, default, false);
            return null;
        }

        public override BoundNode VisitRefTypeOperator(BoundRefTypeOperator node)
        {
            VisitRvalue(node.Operand);
            return null;
        }

        public override BoundNode VisitMakeRefOperator(BoundMakeRefOperator node)
        {
            // Note that we require that the variable whose reference we are taking
            // has been initialized; it is similar to passing the variable as a ref parameter.

            VisitRvalue(node.Operand, isKnownToBeAnLvalue: true);
            return null;
        }

        public override BoundNode VisitRefValueOperator(BoundRefValueOperator node)
        {
            VisitRvalue(node.Operand);
            return null;
        }

        public override BoundNode VisitGlobalStatementInitializer(BoundGlobalStatementInitializer node)
        {
            VisitStatement(node.Statement);
            return null;
        }

        public override BoundNode VisitLambda(BoundLambda node) => null;

        public override BoundNode VisitLocal(BoundLocal node)
        {
            SplitIfBooleanConstant(node);
            return null;
        }

        public override BoundNode VisitLocalDeclaration(BoundLocalDeclaration node)
        {
            if (node.InitializerOpt != null)
            {
                // analyze the expression
                VisitRvalue(node.InitializerOpt, isKnownToBeAnLvalue: node.LocalSymbol.RefKind != RefKind.None);

                // byref assignment is also a potential write
                if (node.LocalSymbol.RefKind != RefKind.None)
                {
                    WriteArgument(node.InitializerOpt, node.LocalSymbol.RefKind, method: null);
                }
            }

            return null;
        }

        public override BoundNode VisitBlock(BoundBlock node)
        {
            VisitStatements(node.Statements);
            return null;
        }

        private void VisitStatements(ImmutableArray<BoundStatement> statements)
        {
            foreach (var statement in statements)
            {
                VisitStatement(statement);
            }
        }

        public override BoundNode VisitScope(BoundScope node)
        {
            VisitStatements(node.Statements);
            return null;
        }

        public override BoundNode VisitExpressionStatement(BoundExpressionStatement node)
        {
            VisitRvalue(node.Expression);
            return null;
        }

        public override BoundNode VisitCall(BoundCall node)
        {
            // If the method being called is a partial method without a definition, or is a conditional method
            // whose condition is not true, then the call has no effect and it is ignored for the purposes of
            // definite assignment analysis.
            bool callsAreOmitted = node.Method.CallsAreOmitted(node.SyntaxTree);
            TLocalState savedState = default(TLocalState);

            if (callsAreOmitted)
            {
                savedState = this.State.Clone();
                SetUnreachable();
            }

            if (node.ReceiverOpt is BoundCall receiver1)
            {
                var calls = ArrayBuilder<BoundCall>.GetInstance();

                calls.Push(node);

                node = receiver1;
                while (node.ReceiverOpt is BoundCall receiver2)
                {
                    calls.Push(node);
                    node = receiver2;
                }

                VisitReceiverBeforeCall(node.ReceiverOpt, node.Method);

                do
                {
                    visitArgumentsAndCompleteAnalysis(node);
                }
                while (calls.TryPop(out node));

                calls.Free();
            }
            else
            {
                VisitReceiverBeforeCall(node.ReceiverOpt, node.Method);
                visitArgumentsAndCompleteAnalysis(node);
            }

            if (callsAreOmitted)
            {
                this.State = savedState;
            }

            return null;

            void visitArgumentsAndCompleteAnalysis(BoundCall node)
            {
                VisitArgumentsBeforeCall(node.Arguments, node.ArgumentRefKindsOpt);

                if (node.Method?.OriginalDefinition is LocalFunctionSymbol localFunc)
                {
                    VisitLocalFunctionUse(localFunc, node.Syntax, isCall: true);
                }

                VisitArgumentsAfterCall(node.Arguments, node.ArgumentRefKindsOpt, node.Method, node.ArgsToParamsOpt, node.Expanded);
                VisitReceiverAfterCall(node.ReceiverOpt, node.Method);
            }
        }

        protected void VisitLocalFunctionUse(LocalFunctionSymbol symbol, SyntaxNode syntax, bool isCall)
        {
            var localFuncState = GetOrCreateLocalFuncUsages(symbol);
            VisitLocalFunctionUse(symbol, localFuncState, syntax, isCall);
        }

        protected virtual void VisitLocalFunctionUse(
            LocalFunctionSymbol symbol,
            TLocalFunctionState localFunctionState,
            SyntaxNode syntax,
            bool isCall)
        {
            if (isCall)
            {
                Join(ref State, ref localFunctionState.StateFromBottom);

                if (!symbol.IsAsync)
                {
                    Meet(ref State, ref localFunctionState.StateFromTop);
                }
            }
            localFunctionState.Visited = true;
        }

        private void VisitReceiverBeforeCall(BoundExpression receiverOpt, MethodSymbol method)
        {
            if (method is null || method.MethodKind != MethodKind.Constructor)
            {
                VisitRvalue(receiverOpt);
            }
        }

        private void VisitReceiverAfterCall(BoundExpression receiverOpt, MethodSymbol method)
        {
            if (receiverOpt is null)
            {
                return;
            }

            if (method is null)
            {
                WriteArgument(receiverOpt, RefKind.Ref, method: null);
            }
            else if (method.TryGetThisParameter(out var thisParameter)
                && thisParameter is object
                && !TypeIsImmutable(thisParameter.Type))
            {
                var thisRefKind = thisParameter.RefKind;
                if (thisRefKind.IsWritableReference())
                {
                    WriteArgument(receiverOpt, thisRefKind, method);
                }
            }
        }

        /// <summary>
        /// Certain (struct) types are known by the compiler to be immutable.  In these cases calling a method on
        /// the type is known (by flow analysis) not to write the receiver.
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        private static bool TypeIsImmutable(TypeSymbol t)
        {
            switch (t.SpecialType)
            {
                case SpecialType.System_Boolean:
                case SpecialType.System_Char:
                case SpecialType.System_SByte:
                case SpecialType.System_Byte:
                case SpecialType.System_Int16:
                case SpecialType.System_UInt16:
                case SpecialType.System_Int32:
                case SpecialType.System_UInt32:
                case SpecialType.System_Int64:
                case SpecialType.System_UInt64:
                case SpecialType.System_Decimal:
                case SpecialType.System_Single:
                case SpecialType.System_Double:
                case SpecialType.System_DateTime:
                    return true;
                default:
                    return t.IsNullableType();
            }
        }

        public override BoundNode VisitIndexerAccess(BoundIndexerAccess node)
        {
            var method = GetReadMethod(node.Indexer);
            VisitReceiverBeforeCall(node.ReceiverOpt, method);
            VisitArguments(node.Arguments, node.ArgumentRefKindsOpt, method, node.ArgsToParamsOpt, node.Expanded);
            if ((object)method != null)
            {
                VisitReceiverAfterCall(node.ReceiverOpt, method);
            }

            return null;
        }

        public override BoundNode VisitImplicitIndexerAccess(BoundImplicitIndexerAccess node)
        {
            // Index or Range implicit indexers evaluate the following in order:
            // 1. The receiver
            // 2. The argument to the access
            // 3. The Count or Length method off the receiver
            // 4. The underlying indexer access or method call
            VisitRvalue(node.Receiver);
            VisitRvalue(node.Argument);

            return null;
        }

        public override BoundNode VisitEventAssignmentOperator(BoundEventAssignmentOperator node)
        {
            VisitRvalue(node.ReceiverOpt);
            VisitRvalue(node.Argument);
            return null;
        }

        /// <summary>
        /// Do not call for a local function.
        /// </summary>
        protected virtual void VisitArguments(ImmutableArray<BoundExpression> arguments, ImmutableArray<RefKind> refKindsOpt, MethodSymbol method, ImmutableArray<int> argsToParamsOpt, bool expanded)
        {
            Debug.Assert(method?.OriginalDefinition.MethodKind != MethodKind.LocalFunction);
            VisitArgumentsBeforeCall(arguments, refKindsOpt);
            VisitArgumentsAfterCall(arguments, refKindsOpt, method, argsToParamsOpt, expanded);
        }

        private void VisitArgumentsBeforeCall(ImmutableArray<BoundExpression> arguments, ImmutableArray<RefKind> refKindsOpt)
        {
            // first value and ref parameters are read...
            for (int i = 0; i < arguments.Length; i++)
            {
                RefKind refKind = GetRefKind(refKindsOpt, i);
                if (refKind != RefKind.Out)
                {
                    VisitRvalue(arguments[i], isKnownToBeAnLvalue: refKind != RefKind.None);
                }
                else
                {
                    VisitLvalue(arguments[i]);
                }
            }
        }

#nullable enable
        /// <summary>
        /// Writes ref and out parameters
        /// </summary>
        private void VisitArgumentsAfterCall(ImmutableArray<BoundExpression> arguments, ImmutableArray<RefKind> refKindsOpt, MethodSymbol? method, ImmutableArray<int> argsToParamsOpt, bool expanded)
        {
            for (int i = 0; i < arguments.Length; i++)
            {
                RefKind refKind = GetRefKind(refKindsOpt, i);
                switch (refKind)
                {
                    case RefKind.None:
                    case RefKind.In:
                    case RefKind.RefReadOnlyParameter:
                    case RefKindExtensions.StrictIn:
                        break;
                    case RefKind.Ref:
                        if (method is null || Binder.GetCorrespondingParameter(i, method.Parameters, argsToParamsOpt, expanded)?.RefKind.IsWritableReference() != false)
                        {
                            goto case RefKind.Out;
                        }
                        break;
                    case RefKind.Out:
                        // passing as a byref argument is also a potential write
                        WriteArgument(arguments[i], refKind, method);
                        break;
                    default:
                        throw ExceptionUtilities.UnexpectedValue(refKind);
                }
            }
        }
#nullable disable

        protected static RefKind GetRefKind(ImmutableArray<RefKind> refKindsOpt, int index)
        {
            return refKindsOpt.IsDefault || refKindsOpt.Length <= index ? RefKind.None : refKindsOpt[index];
        }

        protected virtual void WriteArgument(BoundExpression arg, RefKind refKind, MethodSymbol method)
        {
        }

        public override BoundNode VisitBadExpression(BoundBadExpression node)
        {
            foreach (var child in node.ChildBoundNodes)
            {
                VisitRvalue(child as BoundExpression);
            }

            return null;
        }

        public override BoundNode VisitBadStatement(BoundBadStatement node)
        {
            foreach (var child in node.ChildBoundNodes)
            {
                if (child is BoundStatement)
                {
                    VisitStatement(child as BoundStatement);
                }
                else
                {
                    VisitRvalue(child as BoundExpression);
                }
            }

            return null;
        }

        public override BoundNode VisitArrayInitialization(BoundArrayInitialization node)
        {
            foreach (var child in node.Initializers)
            {
                VisitRvalue(child);
            }

            return null;
        }

        public override BoundNode VisitDelegateCreationExpression(BoundDelegateCreationExpression node)
        {
            var methodGroup = node.Argument as BoundMethodGroup;
            if (methodGroup != null)
            {
                if (node.MethodOpt?.OriginalDefinition is LocalFunctionSymbol localFunc)
                {
                    VisitLocalFunctionUse(localFunc, node.Syntax, isCall: false);
                }
                else if (node.MethodOpt is { } method && methodGroup.ReceiverOpt is { } receiver && !ignoreReceiver(method))
                {
                    EnterRegionIfNeeded(methodGroup);
                    VisitRvalue(receiver);
                    LeaveRegionIfNeeded(methodGroup);
                }
            }
            else
            {
                VisitRvalue(node.Argument);
            }

            return null;

            static bool ignoreReceiver(MethodSymbol method)
            {
                // static methods that aren't extensions get an implicit `this` receiver that should be ignored
                return method.IsStatic && !method.IsExtensionMethod;
            }
        }

        public override BoundNode VisitTypeExpression(BoundTypeExpression node)
        {
            return null;
        }

        public override BoundNode VisitTypeOrValueExpression(BoundTypeOrValueExpression node)
        {
            // If we're seeing a node of this kind, then we failed to resolve the member access
            // as either a type or a property/field/event/local/parameter.  In such cases,
            // the second interpretation applies so just visit the node for that.
            return this.Visit(node.Data.ValueExpression);
        }

        public override BoundNode VisitLiteral(BoundLiteral node)
        {
            SplitIfBooleanConstant(node);
            return null;
        }

        public override BoundNode VisitUtf8String(BoundUtf8String node)
        {
            return null;
        }

        protected void SplitIfBooleanConstant(BoundExpression node)
        {
            if (node.ConstantValueOpt is { IsBoolean: true, BooleanValue: bool booleanValue }
                && node.Type.SpecialType == SpecialType.System_Boolean)
            {
                var unreachable = UnreachableState();
                Split();
                if (booleanValue)
                {
                    StateWhenFalse = unreachable;
                }
                else
                {
                    StateWhenTrue = unreachable;
                }
            }
        }

        public override BoundNode VisitLocalId(BoundLocalId node)
        {
            return null;
        }

        public override BoundNode VisitParameterId(BoundParameterId node)
        {
            return null;
        }

        public override BoundNode VisitMethodDefIndex(BoundMethodDefIndex node)
        {
            return null;
        }

        public override BoundNode VisitStateMachineInstanceId(BoundStateMachineInstanceId node)
        {
            return null;
        }

        public override BoundNode VisitMaximumMethodDefIndex(BoundMaximumMethodDefIndex node)
        {
            return null;
        }

        public override BoundNode VisitModuleVersionId(BoundModuleVersionId node)
        {
            return null;
        }

        public override BoundNode VisitModuleVersionIdString(BoundModuleVersionIdString node)
        {
            return null;
        }

        public override BoundNode VisitInstrumentationPayloadRoot(BoundInstrumentationPayloadRoot node)
        {
            return null;
        }

        public override BoundNode VisitThrowIfModuleCancellationRequested(BoundThrowIfModuleCancellationRequested node)
        {
            return null;
        }

        public override BoundNode VisitSourceDocumentIndex(BoundSourceDocumentIndex node)
        {
            return null;
        }

        public override BoundNode VisitConversion(BoundConversion node)
        {
            if (node.ConversionKind == ConversionKind.MethodGroup)
            {
                if (node.IsExtensionMethod || ((object)node.SymbolOpt != null && node.SymbolOpt.RequiresInstanceReceiver))
                {
                    BoundExpression receiver = ((BoundMethodGroup)node.Operand).ReceiverOpt;
                    // A method group's "implicit this" is only used for instance methods.
                    EnterRegionIfNeeded(node.Operand);
                    VisitRvalue(receiver);
                    LeaveRegionIfNeeded(node.Operand);
                }
                else if (node.SymbolOpt?.OriginalDefinition is LocalFunctionSymbol localFunc)
                {
                    VisitLocalFunctionUse(localFunc, node.Syntax, isCall: false);
                }
            }
            else
            {
                Visit(node.Operand);
            }

            AfterVisitConversion(node);

            return null;
        }

        protected virtual void AfterVisitConversion(BoundConversion node)
        {
        }

        public sealed override BoundNode VisitIfStatement(BoundIfStatement node)
        {
            // 5.3.3.5 If statements

            var stack = ArrayBuilder<(TLocalState, BoundIfStatement)>.GetInstance();

            TLocalState trueState;
            while (true)
            {
                VisitCondition(node.Condition);
                trueState = StateWhenTrue;
                TLocalState falseState = StateWhenFalse;
                SetState(trueState);
                VisitStatement(node.Consequence);
                trueState = this.State;
                SetState(falseState);

                var alternative = node.AlternativeOpt;
                if (alternative is null)
                {
                    break;
                }

                if (alternative is BoundIfStatement elseIfStatement)
                {
                    node = elseIfStatement;
                    stack.Push((trueState, node));
                    EnterRegionIfNeeded(node);
                }
                else
                {
                    VisitStatement(alternative);
                    break;
                }
            }

            while (true)
            {
                Join(ref this.State, ref trueState);
                if (!stack.Any())
                {
                    break;
                }
                (trueState, node) = stack.Pop();
                LeaveRegionIfNeeded(node);
            }

            stack.Free();
            return null;
        }

        public override BoundNode VisitTryStatement(BoundTryStatement node)
        {
            var oldPending = SavePending(); // we do not allow branches into a try statement
            var initialState = this.State.Clone();

            // use this state to resolve all the branches introduced and internal to try/catch
            var pendingBeforeTry = SavePending();

            VisitTryBlockWithAnyTransferFunction(node.TryBlock, node, ref initialState);
            var finallyState = initialState.Clone();
            var endState = this.State;
            foreach (var catchBlock in node.CatchBlocks)
            {
                SetState(initialState.Clone());
                VisitCatchBlockWithAnyTransferFunction(catchBlock, ref finallyState);
                Join(ref endState, ref this.State);
            }

            // Give a chance to branches internal to try/catch to resolve.
            // Carry forward unresolved branches.
            RestorePending(pendingBeforeTry);

            // NOTE: At this point all branches that are internal to try or catch blocks have been resolved.
            //       However we have not yet restored the oldPending branches. Therefore all the branches
            //       that are currently pending must have been introduced in try/catch and do not terminate inside those blocks.
            //
            //       With exception of YieldReturn, these branches logically go through finally, if such present,
            //       so we must Union/Intersect finally state as appropriate

            if (node.FinallyBlockOpt != null)
            {
                // branches from the finally block, while illegal, should still not be considered
                // to execute the finally block before occurring.  Also, we do not handle branches
                // *into* the finally block.
                SetState(finallyState);

                // capture tryAndCatchPending before going into finally
                // we will need pending branches as they were before finally later
                var tryAndCatchPending = SavePending();
                var stateMovedUpInFinally = ReachableBottomState();
                VisitFinallyBlockWithAnyTransferFunction(node.FinallyBlockOpt, ref stateMovedUpInFinally);
                foreach (var pend in tryAndCatchPending.PendingBranches.AsEnumerable())
                {
                    if (pend.Branch == null)
                    {
                        continue; // a tracked exception
                    }

                    if (pend.Branch.Kind != BoundKind.YieldReturnStatement)
                    {
                        updatePendingBranchState(ref pend.State, ref stateMovedUpInFinally);

                        if (pend.IsConditionalState)
                        {
                            updatePendingBranchState(ref pend.StateWhenTrue, ref stateMovedUpInFinally);
                            updatePendingBranchState(ref pend.StateWhenFalse, ref stateMovedUpInFinally);
                        }
                    }
                }

                RestorePending(tryAndCatchPending);
                Meet(ref endState, ref this.State);
                if (_nonMonotonicTransfer)
                {
                    Join(ref endState, ref stateMovedUpInFinally);
                }
            }

            SetState(endState);
            RestorePending(oldPending);
            return null;

            void updatePendingBranchState(ref TLocalState stateToUpdate, ref TLocalState stateMovedUpInFinally)
            {
                Meet(ref stateToUpdate, ref this.State);
                if (_nonMonotonicTransfer)
                {
                    Join(ref stateToUpdate, ref stateMovedUpInFinally);
                }
            }
        }

        protected Optional<TLocalState> NonMonotonicState;

        /// <summary>
        /// Join state from other try block, potentially in a nested method.
        /// </summary>
        protected virtual void JoinTryBlockState(ref TLocalState self, ref TLocalState other)
        {
            Join(ref self, ref other);
        }

        private void VisitTryBlockWithAnyTransferFunction(BoundStatement tryBlock, BoundTryStatement node, ref TLocalState tryState)
        {
            if (_nonMonotonicTransfer)
            {
                Optional<TLocalState> oldTryState = NonMonotonicState;
                NonMonotonicState = ReachableBottomState();
                VisitTryBlock(tryBlock, node, ref tryState);
                var tempTryStateValue = NonMonotonicState.Value;
                Join(ref tryState, ref tempTryStateValue);
                if (oldTryState.HasValue)
                {
                    var oldTryStateValue = oldTryState.Value;
                    JoinTryBlockState(ref oldTryStateValue, ref tempTryStateValue);
                    oldTryState = oldTryStateValue;
                }

                NonMonotonicState = oldTryState;
            }
            else
            {
                VisitTryBlock(tryBlock, node, ref tryState);
            }
        }

        protected virtual void VisitTryBlock(BoundStatement tryBlock, BoundTryStatement node, ref TLocalState tryState)
        {
            VisitStatement(tryBlock);
        }

        private void VisitCatchBlockWithAnyTransferFunction(BoundCatchBlock catchBlock, ref TLocalState finallyState)
        {
            if (_nonMonotonicTransfer)
            {
                Optional<TLocalState> oldTryState = NonMonotonicState;
                NonMonotonicState = ReachableBottomState();
                VisitCatchBlock(catchBlock, ref finallyState);
                var tempTryStateValue = NonMonotonicState.Value;
                Join(ref finallyState, ref tempTryStateValue);
                if (oldTryState.HasValue)
                {
                    var oldTryStateValue = oldTryState.Value;
                    JoinTryBlockState(ref oldTryStateValue, ref tempTryStateValue);
                    oldTryState = oldTryStateValue;
                }

                NonMonotonicState = oldTryState;
            }
            else
            {
                VisitCatchBlock(catchBlock, ref finallyState);
            }
        }

        protected virtual void VisitCatchBlock(BoundCatchBlock catchBlock, ref TLocalState finallyState)
        {
            if (catchBlock.ExceptionSourceOpt != null)
            {
                VisitLvalue(catchBlock.ExceptionSourceOpt);
            }

            if (catchBlock.ExceptionFilterPrologueOpt is { })
            {
                VisitStatementList(catchBlock.ExceptionFilterPrologueOpt);
            }

            if (catchBlock.ExceptionFilterOpt != null)
            {
                VisitCondition(catchBlock.ExceptionFilterOpt);
                SetState(StateWhenTrue);
            }

            VisitStatement(catchBlock.Body);
        }

        private void VisitFinallyBlockWithAnyTransferFunction(BoundStatement finallyBlock, ref TLocalState stateMovedUp)
        {
            if (_nonMonotonicTransfer)
            {
                Optional<TLocalState> oldTryState = NonMonotonicState;
                NonMonotonicState = ReachableBottomState();
                VisitFinallyBlock(finallyBlock, ref stateMovedUp);
                var tempTryStateValue = NonMonotonicState.Value;
                Join(ref stateMovedUp, ref tempTryStateValue);
                if (oldTryState.HasValue)
                {
                    var oldTryStateValue = oldTryState.Value;
                    JoinTryBlockState(ref oldTryStateValue, ref tempTryStateValue);
                    oldTryState = oldTryStateValue;
                }

                NonMonotonicState = oldTryState;
            }
            else
            {
                VisitFinallyBlock(finallyBlock, ref stateMovedUp);
            }
        }

        protected virtual void VisitFinallyBlock(BoundStatement finallyBlock, ref TLocalState stateMovedUp)
        {
            VisitStatement(finallyBlock); // this should generate no pending branches
        }

        public override BoundNode VisitExtractedFinallyBlock(BoundExtractedFinallyBlock node)
        {
            return VisitBlock(node.FinallyBlock);
        }

        public override BoundNode VisitReturnStatement(BoundReturnStatement node)
        {
            var result = VisitReturnStatementNoAdjust(node);
            PendingBranches.Add(new PendingBranch(node, this.State, label: null));
            SetUnreachable();
            return result;
        }

        protected virtual BoundNode VisitReturnStatementNoAdjust(BoundReturnStatement node)
        {
            VisitRvalue(node.ExpressionOpt, isKnownToBeAnLvalue: node.RefKind != RefKind.None);

            // byref return is also a potential write
            if (node.RefKind != RefKind.None)
            {
                WriteArgument(node.ExpressionOpt, node.RefKind, method: null);
            }

            return null;
        }

        public override BoundNode VisitThisReference(BoundThisReference node)
        {
            return null;
        }

        public override BoundNode VisitPreviousSubmissionReference(BoundPreviousSubmissionReference node)
        {
            return null;
        }

        public override BoundNode VisitHostObjectMemberReference(BoundHostObjectMemberReference node)
        {
            return null;
        }

        public override BoundNode VisitParameter(BoundParameter node)
        {
            return null;
        }

        protected virtual void VisitLvalueParameter(BoundParameter node)
        {
        }

        public override BoundNode VisitObjectCreationExpression(BoundObjectCreationExpression node)
        {
            VisitArguments(node.Arguments, node.ArgumentRefKindsOpt, node.Constructor, node.ArgsToParamsOpt, node.Expanded);
            VisitRvalue(node.InitializerExpressionOpt);
            return null;
        }

        public override BoundNode VisitCollectionExpression(BoundCollectionExpression node)
        {
            VisitCollectionExpression(node.Elements);
            return null;
        }

        public override BoundNode VisitUnconvertedCollectionExpression(BoundUnconvertedCollectionExpression node)
        {
            VisitCollectionExpression(node.Elements);
            return null;
        }

        private void VisitCollectionExpression(ImmutableArray<BoundNode> elements)
        {
            foreach (var element in elements)
            {
                if (element is BoundExpression expression)
                {
                    VisitRvalue(expression);
                }
                else
                {
                    Visit(element);
                }
            }
        }

        public override BoundNode VisitCollectionExpressionSpreadElement(BoundCollectionExpressionSpreadElement node)
        {
            VisitRvalue(node.Expression);
            return null;
        }

        public override BoundNode VisitNewT(BoundNewT node)
        {
            VisitRvalue(node.InitializerExpressionOpt);
            return null;
        }

        public override BoundNode VisitNoPiaObjectCreationExpression(BoundNoPiaObjectCreationExpression node)
        {
            VisitRvalue(node.InitializerExpressionOpt);
            return null;
        }

        // represents anything that occurs at the invocation of the property setter
        protected virtual void PropertySetter(BoundExpression node, BoundExpression receiver, MethodSymbol setter, BoundExpression value = null)
        {
            VisitReceiverAfterCall(receiver, setter);
        }

        // returns false if expression is not a property access
        // or if the property has a backing field
        // and accessed in a corresponding constructor
        private bool RegularPropertyAccess(BoundExpression expr)
        {
            if (expr.Kind != BoundKind.PropertyAccess)
            {
                return false;
            }

            return !Binder.AccessingAutoPropertyFromConstructor((BoundPropertyAccess)expr, _symbol);
        }

        public override BoundNode VisitAssignmentOperator(BoundAssignmentOperator node)
        {
            // TODO: should events be handled specially too?
            if (RegularPropertyAccess(node.Left))
            {
                var left = (BoundPropertyAccess)node.Left;
                var property = left.PropertySymbol;
                if (property.RefKind == RefKind.None)
                {
                    var method = GetWriteMethod(property);
                    VisitReceiverBeforeCall(left.ReceiverOpt, method);
                    VisitRvalue(node.Right);
                    PropertySetter(node, left.ReceiverOpt, method, node.Right);
                    return null;
                }
            }

            VisitLvalue(node.Left);
            VisitRvalue(node.Right, isKnownToBeAnLvalue: node.IsRef);

            // byref assignment is also a potential write
            if (node.IsRef)
            {
                // Assume that BadExpression is a ref location to avoid
                // cascading diagnostics
                var refKind = node.Left.Kind == BoundKind.BadExpression
                    ? RefKind.Ref
                    : node.Left.GetRefKind();
                WriteArgument(node.Right, refKind, method: null);
            }

            return null;
        }

        public override BoundNode VisitDeconstructionAssignmentOperator(BoundDeconstructionAssignmentOperator node)
        {
            VisitLvalue(node.Left);
            VisitRvalue(node.Right);
            return null;
        }

        public sealed override BoundNode VisitOutDeconstructVarPendingInference(OutDeconstructVarPendingInference node)
        {
            // OutDeconstructVarPendingInference nodes are only used within initial binding, but don't survive past that stage
            throw ExceptionUtilities.Unreachable();
        }

        public override BoundNode VisitCompoundAssignmentOperator(BoundCompoundAssignmentOperator node)
        {
            VisitCompoundAssignmentTarget(node);
            VisitRvalue(node.Right);
            AfterRightHasBeenVisited(node);
            return null;
        }

        protected void VisitCompoundAssignmentTarget(BoundCompoundAssignmentOperator node)
        {
            // TODO: should events be handled specially too?
            if (RegularPropertyAccess(node.Left))
            {
                var left = (BoundPropertyAccess)node.Left;
                var property = left.PropertySymbol;
                if (property.RefKind == RefKind.None)
                {
                    var readMethod = GetReadMethod(property);
                    Debug.Assert(node.HasAnyErrors || (object)readMethod != (object)GetWriteMethod(property));
                    VisitReceiverBeforeCall(left.ReceiverOpt, readMethod);
                    VisitReceiverAfterCall(left.ReceiverOpt, readMethod);
                    return;
                }
            }

            VisitRvalue(node.Left, isKnownToBeAnLvalue: true);
        }

        protected void AfterRightHasBeenVisited(BoundCompoundAssignmentOperator node)
        {
            if (RegularPropertyAccess(node.Left))
            {
                var left = (BoundPropertyAccess)node.Left;
                var property = left.PropertySymbol;
                if (property.RefKind == RefKind.None)
                {
                    var writeMethod = GetWriteMethod(property);
                    PropertySetter(node, left.ReceiverOpt, writeMethod);
                    VisitReceiverAfterCall(left.ReceiverOpt, writeMethod);
                    return;
                }
            }

        }

        public override BoundNode VisitFieldAccess(BoundFieldAccess node)
        {
            VisitFieldAccessInternal(node.ReceiverOpt, node.FieldSymbol);
            SplitIfBooleanConstant(node);
            return null;
        }

        private void VisitFieldAccessInternal(BoundExpression receiverOpt, FieldSymbol fieldSymbol)
        {
            bool asLvalue = (object)fieldSymbol != null &&
                (fieldSymbol.IsFixedSizeBuffer ||
                !fieldSymbol.IsStatic &&
                fieldSymbol.ContainingType.TypeKind == TypeKind.Struct &&
                receiverOpt != null &&
                receiverOpt.Kind != BoundKind.TypeExpression &&
                (object)receiverOpt.Type != null &&
                !receiverOpt.Type.IsPrimitiveRecursiveStruct());
            if (asLvalue)
            {
                VisitLvalue(receiverOpt);
            }
            else
            {
                VisitRvalue(receiverOpt);
            }
        }

        public override BoundNode VisitFieldInfo(BoundFieldInfo node)
        {
            return null;
        }

        public override BoundNode VisitMethodInfo(BoundMethodInfo node)
        {
            return null;
        }

        public override BoundNode VisitPropertyAccess(BoundPropertyAccess node)
        {
            var property = node.PropertySymbol;

            if (Binder.AccessingAutoPropertyFromConstructor(node, _symbol))
            {
                var backingField = (property as SourcePropertySymbolBase)?.BackingField;
                if (backingField != null)
                {
                    VisitFieldAccessInternal(node.ReceiverOpt, backingField);
                    return null;
                }
            }

            var method = GetReadMethod(property);
            VisitReceiverBeforeCall(node.ReceiverOpt, method);
            VisitReceiverAfterCall(node.ReceiverOpt, method);
            return null;
            // TODO: In an expression such as
            //    M().Prop = G();
            // Exceptions thrown from M() occur before those from G(), but exceptions from the property accessor
            // occur after both.  The precise abstract flow pass does not yet currently have this quite right.
            // Probably what is needed is a VisitPropertyAccessInternal(BoundPropertyAccess node, bool read)
            // which should assume that the receiver will have been handled by the caller.  This can be invoked
            // twice for read/write operations such as
            //    M().Prop += 1
            // or at the appropriate place in the sequence for read or write operations.
            // Do events require any special handling too?
        }

        public override BoundNode VisitEventAccess(BoundEventAccess node)
        {
            VisitFieldAccessInternal(node.ReceiverOpt, node.EventSymbol.AssociatedField);
            return null;
        }

        public override BoundNode VisitRangeVariable(BoundRangeVariable node)
        {
            return null;
        }

        public override BoundNode VisitQueryClause(BoundQueryClause node)
        {
            VisitRvalue(node.UnoptimizedForm ?? node.Value);
            return null;
        }

        private BoundNode VisitMultipleLocalDeclarationsBase(BoundMultipleLocalDeclarationsBase node)
        {
            foreach (var v in node.LocalDeclarations)
            {
                Visit(v);
            }

            return null;
        }

        public override BoundNode VisitMultipleLocalDeclarations(BoundMultipleLocalDeclarations node)
        {
            return VisitMultipleLocalDeclarationsBase(node);
        }

        public override BoundNode VisitUsingLocalDeclarations(BoundUsingLocalDeclarations node)
        {
            if (AwaitUsingAndForeachAddsPendingBranch && node.AwaitOpt != null)
            {
                PendingBranches.Add(new PendingBranch(node, this.State, null));
            }
            return VisitMultipleLocalDeclarationsBase(node);
        }

        public override BoundNode VisitWhileStatement(BoundWhileStatement node)
        {
            // while (node.Condition) { node.Body; node.ContinueLabel: } node.BreakLabel:
            LoopHead(node);
            VisitCondition(node.Condition);
            TLocalState bodyState = StateWhenTrue;
            TLocalState breakState = StateWhenFalse;
            SetState(bodyState);
            VisitStatement(node.Body);
            ResolveContinues(node.ContinueLabel);
            LoopTail(node);
            ResolveBreaks(breakState, node.BreakLabel);
            return null;
        }

        public override BoundNode VisitWithExpression(BoundWithExpression node)
        {
            VisitRvalue(node.Receiver);
            VisitObjectOrCollectionInitializerExpression(node.InitializerExpression.Initializers);
            return null;
        }

        public override BoundNode VisitArrayAccess(BoundArrayAccess node)
        {
            VisitRvalue(node.Expression);
            foreach (var i in node.Indices)
            {
                VisitRvalue(i);
            }

            return null;
        }

        public override BoundNode VisitInlineArrayAccess(BoundInlineArrayAccess node)
        {
            VisitRvalue(node.Expression);
            VisitRvalue(node.Argument);

            AfterVisitInlineArrayAccess(node);
            return null;
        }

        protected virtual void AfterVisitInlineArrayAccess(BoundInlineArrayAccess node)
        {
        }

        protected virtual void VisitLvalue(BoundInlineArrayAccess access)
        {
            VisitLvalue(access.Expression);
            VisitRvalue(access.Argument);
        }

        public override BoundNode VisitBinaryOperator(BoundBinaryOperator node)
        {
            if (node.OperatorKind.IsLogical())
            {
                Debug.Assert(!node.OperatorKind.IsUserDefined());
                VisitBinaryLogicalOperatorChildren(node);
            }
            else if (node.InterpolatedStringHandlerData is { } data)
            {
                VisitBinaryInterpolatedStringAddition(node);
            }
            else
            {
                VisitBinaryOperatorChildren(node);
            }

            return null;
        }

        public override BoundNode VisitUserDefinedConditionalLogicalOperator(BoundUserDefinedConditionalLogicalOperator node)
        {
            VisitBinaryLogicalOperatorChildren(node);
            return null;
        }

        private void VisitBinaryLogicalOperatorChildren(BoundExpression node)
        {
            // Do not blow the stack due to a deep recursion on the left.
            var stack = ArrayBuilder<BoundExpression>.GetInstance();

            BoundExpression binary;
            BoundExpression child = node;

            while (true)
            {
                var childKind = child.Kind;

                if (childKind == BoundKind.BinaryOperator)
                {
                    var binOp = (BoundBinaryOperator)child;

                    if (!binOp.OperatorKind.IsLogical())
                    {
                        break;
                    }

                    Debug.Assert(!binOp.OperatorKind.IsUserDefined());
                    binary = child;
                    child = binOp.Left;
                }
                else if (childKind == BoundKind.UserDefinedConditionalLogicalOperator)
                {
                    binary = child;
                    child = ((BoundUserDefinedConditionalLogicalOperator)binary).Left;
                }
                else
                {
                    break;
                }

                stack.Push(binary);
            }

            VisitBinaryLogicalOperatorChildren(stack);
            stack.Free();
        }

        protected virtual void VisitBinaryLogicalOperatorChildren(ArrayBuilder<BoundExpression> stack)
        {
            BoundExpression binary;
            Debug.Assert(stack.Count > 0);

            binary = stack.Pop();

            BoundExpression child;
            switch (binary.Kind)
            {
                case BoundKind.BinaryOperator:
                    var binOp = (BoundBinaryOperator)binary;
                    child = binOp.Left;
                    break;
                case BoundKind.UserDefinedConditionalLogicalOperator:
                    var udBinOp = (BoundUserDefinedConditionalLogicalOperator)binary;
                    child = udBinOp.Left;
                    break;
                default:
                    throw ExceptionUtilities.UnexpectedValue(binary.Kind);
            }

            VisitCondition(child);

            while (true)
            {
                BinaryOperatorKind kind;
                BoundExpression right;
                switch (binary.Kind)
                {
                    case BoundKind.BinaryOperator:
                        var binOp = (BoundBinaryOperator)binary;
                        kind = binOp.OperatorKind;
                        right = binOp.Right;
                        break;
                    case BoundKind.UserDefinedConditionalLogicalOperator:
                        var udBinOp = (BoundUserDefinedConditionalLogicalOperator)binary;
                        kind = udBinOp.OperatorKind;
                        right = udBinOp.Right;
                        break;
                    default:
                        throw ExceptionUtilities.UnexpectedValue(binary.Kind);
                }

                var op = kind.Operator();
                var isAnd = op == BinaryOperatorKind.And;
                var isBool = kind.OperandTypes() == BinaryOperatorKind.Bool;
                Debug.Assert(!isBool || binary.Kind != BoundKind.UserDefinedConditionalLogicalOperator);

                Debug.Assert(isAnd || op == BinaryOperatorKind.Or);

                var leftTrue = this.StateWhenTrue;
                var leftFalse = this.StateWhenFalse;
                SetState(isAnd ? leftTrue : leftFalse);

                AfterLeftChildOfBinaryLogicalOperatorHasBeenVisited(binary, right, isAnd, isBool, ref leftTrue, ref leftFalse);

                if (stack.Count == 0)
                {
                    break;
                }

                AdjustConditionalState(binary);
                binary = stack.Pop();
            }
        }

        protected virtual void AfterLeftChildOfBinaryLogicalOperatorHasBeenVisited(BoundExpression binary, BoundExpression right, bool isAnd, bool isBool, ref TLocalState leftTrue, ref TLocalState leftFalse)
        {
            Visit(right); // First part of VisitCondition
            AfterRightChildOfBinaryLogicalOperatorHasBeenVisited(right, isAnd, isBool, ref leftTrue, ref leftFalse);
        }

        protected void AfterRightChildOfBinaryLogicalOperatorHasBeenVisited(BoundExpression right, bool isAnd, bool isBool, ref TLocalState leftTrue, ref TLocalState leftFalse)
        {
            AdjustConditionalState(right); // Second part of VisitCondition

            if (!isBool)
            {
                this.Unsplit();
                this.Split();
            }

            var resultTrue = this.StateWhenTrue;
            var resultFalse = this.StateWhenFalse;
            if (isAnd)
            {
                Join(ref resultFalse, ref leftFalse);
            }
            else
            {
                Join(ref resultTrue, ref leftTrue);
            }
            SetConditionalState(resultTrue, resultFalse);

            if (!isBool)
            {
                this.Unsplit();
            }
        }

        private void VisitBinaryOperatorChildren(BoundBinaryOperator node)
        {
            // It is common in machine-generated code for there to be deep recursion on the left side of a binary
            // operator, for example, if you have "a + b + c + ... " then the bound tree will be deep on the left
            // hand side. To mitigate the risk of stack overflow we use an explicit stack.
            //
            // Of course we must ensure that we visit the left hand side before the right hand side.
            var stack = ArrayBuilder<BoundBinaryOperator>.GetInstance();

            BoundBinaryOperator binary = node;
            do
            {
                stack.Push(binary);
                EnterRegionIfNeeded(binary);
                binary = binary.Left as BoundBinaryOperator;
            }
            while (binary != null && !binary.OperatorKind.IsLogical() && binary.InterpolatedStringHandlerData is null);

            VisitBinaryOperatorChildren(stack);
            stack.Free();
        }

#nullable enable
        /// <param name="stack">Nested left-associative binary operators, pushed on from outermost to innermost.</param>
        protected virtual void VisitBinaryOperatorChildren(ArrayBuilder<BoundBinaryOperator> stack)
        {
            var binary = stack.Pop();

            // Only the leftmost operator of a left-associative binary operator chain can learn from a conditional access on the left
            // For simplicity, we just special case it here.
            // For example, `a?.b(out x) == true` has a conditional access on the left of the operator,
            // but `expr == a?.b(out x) == true` has a conditional access on the right of the operator
            if (VisitPossibleConditionalAccess(binary.Left, out var stateWhenNotNull)
                && canLearnFromOperator(binary)
                && isKnownNullOrNotNull(binary.Right))
            {
                if (_nonMonotonicTransfer)
                {
                    // In this very specific scenario, we need to do extra work to track unassignments for region analysis.
                    // See `AbstractFlowPass.VisitCatchBlockWithAnyTransferFunction` for a similar scenario in catch blocks.
                    Optional<TLocalState> oldState = NonMonotonicState;
                    NonMonotonicState = ReachableBottomState();

                    VisitRvalue(binary.Right);

                    var tempStateValue = NonMonotonicState.Value;
                    Join(ref stateWhenNotNull, ref tempStateValue);
                    if (oldState.HasValue)
                    {
                        var oldStateValue = oldState.Value;
                        Join(ref oldStateValue, ref tempStateValue);
                        oldState = oldStateValue;
                    }

                    NonMonotonicState = oldState;
                }
                else
                {
                    VisitRvalue(binary.Right);
                    Meet(ref stateWhenNotNull, ref State);
                }

                var isNullConstant = binary.Right.ConstantValueOpt?.IsNull == true;
                SetConditionalState(isNullConstant == isEquals(binary)
                    ? (State, stateWhenNotNull)
                    : (stateWhenNotNull, State));
                LeaveRegionIfNeeded(binary);

                if (stack.Count == 0)
                {
                    return;
                }

                binary = stack.Pop();
            }

            while (true)
            {
                if (!canLearnFromOperator(binary)
                    || !learnFromOperator(binary))
                {
                    Unsplit();
                    VisitRvalue(binary.Right);
                }
                LeaveRegionIfNeeded(binary);

                if (stack.Count == 0)
                {
                    break;
                }

                binary = stack.Pop();
            }

            static bool canLearnFromOperator(BoundBinaryOperator binary)
            {
                var kind = binary.OperatorKind;
                return kind.Operator() is BinaryOperatorKind.Equal or BinaryOperatorKind.NotEqual
                    && (!kind.IsUserDefined() || kind.IsLifted());
            }

            static bool isKnownNullOrNotNull(BoundExpression expr)
            {
                return expr.ConstantValueOpt is object
                    || (expr is BoundConversion { ConversionKind: ConversionKind.ExplicitNullable or ConversionKind.ImplicitNullable } conv
                        && conv.Operand.Type!.IsNonNullableValueType());
            }

            static bool isEquals(BoundBinaryOperator binary)
                => binary.OperatorKind.Operator() == BinaryOperatorKind.Equal;

            // Returns true if `binary.Right` was visited by the call.
            bool learnFromOperator(BoundBinaryOperator binary)
            {
                // `true == a?.b(out x)`
                if (isKnownNullOrNotNull(binary.Left) && TryVisitConditionalAccess(binary.Right, out var stateWhenNotNull))
                {
                    var isNullConstant = binary.Left.ConstantValueOpt?.IsNull == true;
                    SetConditionalState(isNullConstant == isEquals(binary)
                        ? (State, stateWhenNotNull)
                        : (stateWhenNotNull, State));

                    return true;
                }
                // `a && b(out x) == true`
                else if (IsConditionalState && binary.Right.ConstantValueOpt is { IsBoolean: true } rightConstant)
                {
                    var (stateWhenTrue, stateWhenFalse) = (StateWhenTrue.Clone(), StateWhenFalse.Clone());
                    Unsplit();
                    Visit(binary.Right);
                    SetConditionalState(isEquals(binary) == rightConstant.BooleanValue
                        ? (stateWhenTrue, stateWhenFalse)
                        : (stateWhenFalse, stateWhenTrue));

                    return true;
                }
                // `true == a && b(out x)`
                else if (binary.Left.ConstantValueOpt is { IsBoolean: true } leftConstant)
                {
                    Unsplit();
                    Visit(binary.Right);
                    if (IsConditionalState && isEquals(binary) != leftConstant.BooleanValue)
                    {
                        SetConditionalState(StateWhenFalse, StateWhenTrue);
                    }

                    return true;
                }

                return false;
            }
        }

        protected void VisitBinaryInterpolatedStringAddition(BoundBinaryOperator node)
        {
            Debug.Assert(node.InterpolatedStringHandlerData.HasValue);
            var parts = ArrayBuilder<BoundInterpolatedString>.GetInstance();
            var data = node.InterpolatedStringHandlerData.GetValueOrDefault();

            node.VisitBinaryOperatorInterpolatedString(
                (parts, @this: this),
                stringCallback: static (BoundInterpolatedString interpolatedString, (ArrayBuilder<BoundInterpolatedString> parts, AbstractFlowPass<TLocalState, TLocalFunctionState> @this) arg) =>
                {
                    arg.parts.Add(interpolatedString);
                    return true;
                },
                binaryOperatorCallback: (op, arg) => arg.@this.VisitInterpolatedStringBinaryOperatorNode(op));

            Debug.Assert(parts.Count >= 2);

            VisitInterpolatedStringHandlerConstructor(data.Construction);

            bool visitedFirst = false;
            bool hasTrailingHandlerValidityParameter = data.HasTrailingHandlerValidityParameter;
            bool hasConditionalEvaluation = data.UsesBoolReturns || hasTrailingHandlerValidityParameter;
            TLocalState? shortCircuitState = hasConditionalEvaluation ? State.Clone() : default;

            foreach (var part in parts)
            {
                visitedFirst |= VisitInterpolatedStringHandlerParts(part, data.UsesBoolReturns, firstPartIsConditional: visitedFirst || hasTrailingHandlerValidityParameter, ref shortCircuitState);
            }

            if (hasConditionalEvaluation)
            {
                Join(ref State, ref shortCircuitState);
            }

            parts.Free();
        }

        protected virtual void VisitInterpolatedStringBinaryOperatorNode(BoundBinaryOperator node) { }

        protected virtual bool VisitInterpolatedStringHandlerParts(BoundInterpolatedStringBase node, bool usesBoolReturns, bool firstPartIsConditional, ref TLocalState? shortCircuitState)
        {
            Debug.Assert(shortCircuitState != null || (!usesBoolReturns && !firstPartIsConditional));
            if (node.Parts.IsEmpty)
            {
                return false;
            }

            ReadOnlySpan<BoundExpression> parts;

            if (firstPartIsConditional)
            {
                parts = node.Parts.AsSpan();
            }
            else
            {
                VisitRvalue(node.Parts[0]);
                shortCircuitState = State.Clone();
                parts = node.Parts.AsSpan()[1..];
            }

            foreach (var part in parts)
            {
                VisitRvalue(part);
                if (usesBoolReturns)
                {
                    Debug.Assert(shortCircuitState != null);
                    Join(ref shortCircuitState, ref State);
                }
            }

            return true;
        }
#nullable disable

        public override BoundNode VisitUnaryOperator(BoundUnaryOperator node)
        {
            if (node.OperatorKind == UnaryOperatorKind.BoolLogicalNegation)
            {
                // We have a special case for the ! unary operator, which can operate in a boolean context (5.3.3.26)
                VisitCondition(node.Operand);
                // it inverts the sense of assignedWhenTrue and assignedWhenFalse.
                SetConditionalState(StateWhenFalse, StateWhenTrue);
            }
            else
            {
                VisitRvalue(node.Operand);
            }
            return null;
        }

        public override BoundNode VisitRangeExpression(BoundRangeExpression node)
        {
            if (node.LeftOperandOpt != null)
            {
                VisitRvalue(node.LeftOperandOpt);
            }

            if (node.RightOperandOpt != null)
            {
                VisitRvalue(node.RightOperandOpt);
            }

            return null;
        }

        public override BoundNode VisitFromEndIndexExpression(BoundFromEndIndexExpression node)
        {
            VisitRvalue(node.Operand);
            return null;
        }

        public override BoundNode VisitAwaitExpression(BoundAwaitExpression node)
        {
            VisitRvalue(node.Expression);
            PendingBranches.Add(new PendingBranch(node, this.State, null));
            return null;
        }

        public override BoundNode VisitIncrementOperator(BoundIncrementOperator node)
        {
            // TODO: should we also specially handle events?
            if (RegularPropertyAccess(node.Operand))
            {
                var left = (BoundPropertyAccess)node.Operand;
                var property = left.PropertySymbol;
                if (property.RefKind == RefKind.None)
                {
                    var readMethod = GetReadMethod(property);
                    var writeMethod = GetWriteMethod(property);
                    Debug.Assert(node.HasAnyErrors || (object)readMethod != (object)writeMethod);
                    VisitReceiverBeforeCall(left.ReceiverOpt, readMethod);
                    VisitReceiverAfterCall(left.ReceiverOpt, readMethod);
                    PropertySetter(node, left.ReceiverOpt, writeMethod); // followed by a write
                    return null;
                }
            }

            VisitRvalue(node.Operand);

            return null;
        }

        public override BoundNode VisitArrayCreation(BoundArrayCreation node)
        {
            foreach (var expr in node.Bounds)
            {
                VisitRvalue(expr);
            }

            VisitRvalue(node.InitializerOpt);
            return null;
        }

        public override BoundNode VisitForStatement(BoundForStatement node)
        {
            if (node.Initializer != null)
            {
                VisitStatement(node.Initializer);
            }
            LoopHead(node);
            TLocalState bodyState, breakState;
            if (node.Condition != null)
            {
                VisitCondition(node.Condition);
                bodyState = this.StateWhenTrue;
                breakState = this.StateWhenFalse;
            }
            else
            {
                bodyState = this.State;
                breakState = UnreachableState();
            }

            SetState(bodyState);
            VisitStatement(node.Body);
            ResolveContinues(node.ContinueLabel);
            if (node.Increment != null)
            {
                VisitStatement(node.Increment);
            }

            LoopTail(node);
            ResolveBreaks(breakState, node.BreakLabel);
            return null;
        }

        public override BoundNode VisitForEachStatement(BoundForEachStatement node)
        {
            // foreach [await] ( var v in node.Expression ) { node.Body; node.ContinueLabel: } node.BreakLabel:
            VisitForEachExpression(node);
            LoopHead(node);
            var breakState = this.State.Clone();
            VisitForEachIterationVariables(node);
            VisitStatement(node.Body);
            ResolveContinues(node.ContinueLabel);
            LoopTail(node);
            ResolveBreaks(breakState, node.BreakLabel);

            if (AwaitUsingAndForeachAddsPendingBranch && ((CommonForEachStatementSyntax)node.Syntax).AwaitKeyword != default)
            {
                PendingBranches.Add(new PendingBranch(node, this.State, null));
            }

            return null;
        }

        protected virtual void VisitForEachExpression(BoundForEachStatement node)
        {
            VisitRvalue(node.Expression);
        }

        public virtual void VisitForEachIterationVariables(BoundForEachStatement node)
        {
        }

        public override BoundNode VisitAsOperator(BoundAsOperator node)
        {
            VisitRvalue(node.Operand);
            return null;
        }

        public override BoundNode VisitIsOperator(BoundIsOperator node)
        {
            if (VisitPossibleConditionalAccess(node.Operand, out var stateWhenNotNull))
            {
                Debug.Assert(!IsConditionalState);
                SetConditionalState(stateWhenNotNull, State);
            }
            else
            {
                // `(a && b.M(out x)) is bool` should discard conditional state from LHS
                Unsplit();
            }
            return null;
        }

        public override BoundNode VisitMethodGroup(BoundMethodGroup node)
        {
            if (node.ReceiverOpt != null)
            {
                // An explicit or implicit receiver, for example in an expression such as (x.Goo is Action, or Goo is Action), is considered to be read.
                VisitRvalue(node.ReceiverOpt);
            }

            return null;
        }

#nullable enable

        public override BoundNode? VisitNullCoalescingOperator(BoundNullCoalescingOperator node)
        {
            if (IsConstantNull(node.LeftOperand))
            {
                VisitRvalue(node.LeftOperand);
                Visit(node.RightOperand);
            }
            else
            {
                TLocalState savedState;
                if (VisitPossibleConditionalAccess(node.LeftOperand, out var stateWhenNotNull))
                {
                    Debug.Assert(!IsConditionalState);
                    savedState = stateWhenNotNull;
                }
                else
                {
                    Unsplit();
                    savedState = State.Clone();
                }

                if (node.LeftOperand.ConstantValueOpt != null)
                {
                    SetUnreachable();
                }
                Visit(node.RightOperand);
                if (IsConditionalState)
                {
                    Join(ref StateWhenTrue, ref savedState);
                    Join(ref StateWhenFalse, ref savedState);
                }
                else
                {
                    Join(ref this.State, ref savedState);
                }
            }
            return null;
        }

        /// <summary>
        /// Visits a node only if it is a conditional access.
        /// Returns 'true' if and only if the node was visited.
        /// </summary>
        private bool TryVisitConditionalAccess(BoundExpression node, [NotNullWhen(true)] out TLocalState? stateWhenNotNull)
        {
            var access = node switch
            {
                BoundConditionalAccess ca => ca,
                BoundConversion { Conversion: Conversion conversion, Operand: BoundConditionalAccess ca } when CanPropagateStateWhenNotNull(conversion) => ca,
                _ => null
            };

            if (access is not null)
            {
                EnterRegionIfNeeded(access);
                Unsplit();
                VisitConditionalAccess(access, out stateWhenNotNull);
                Debug.Assert(!IsConditionalState);
                LeaveRegionIfNeeded(access);
                return true;
            }

            stateWhenNotNull = default;
            return false;
        }

        /// <summary>
        /// "State when not null" can only propagate out of a conditional access if
        /// it is not subject to a user-defined conversion whose parameter is not of a non-nullable value type.
        /// </summary>
        protected static bool CanPropagateStateWhenNotNull(Conversion conversion)
        {
            if (!conversion.IsValid)
            {
                return false;
            }

            if (!conversion.IsUserDefined)
            {
                return true;
            }

            var method = conversion.Method;
            Debug.Assert(method is object);
            Debug.Assert(method.ParameterCount is 1);
            var param = method.Parameters[0];
            return param.Type.IsNonNullableValueType();
        }

        /// <summary>
        /// Unconditionally visits an expression.
        /// If the expression has "state when not null" after visiting,
        /// the method returns 'true' and writes the state to <paramref name="stateWhenNotNull" />.
        /// </summary>
        private bool VisitPossibleConditionalAccess(BoundExpression node, [NotNullWhen(true)] out TLocalState? stateWhenNotNull)
        {
            if (TryVisitConditionalAccess(node, out stateWhenNotNull))
            {
                return true;
            }
            else
            {
                Visit(node);
                return false;
            }
        }

        private void VisitConditionalAccess(BoundConditionalAccess node, out TLocalState stateWhenNotNull)
        {
            // The receiver may also be a conditional access.
            // `(a?.b(x = 1))?.c(y = 1)`
            if (VisitPossibleConditionalAccess(node.Receiver, out var receiverStateWhenNotNull))
            {
                stateWhenNotNull = receiverStateWhenNotNull;
            }
            else
            {
                Unsplit();
                stateWhenNotNull = this.State.Clone();
            }

            if (node.Receiver.ConstantValueOpt != null && !IsConstantNull(node.Receiver))
            {
                // Consider a scenario like `"a"?.M0(x = 1)?.M0(y = 1)`.
                // We can "know" that `.M0(x = 1)` was evaluated unconditionally but not `M0(y = 1)`.
                // Therefore we do a VisitPossibleConditionalAccess here which unconditionally includes the "after receiver" state in State
                // and includes the "after subsequent conditional accesses" in stateWhenNotNull
                if (VisitPossibleConditionalAccess(node.AccessExpression, out var firstAccessStateWhenNotNull))
                {
                    stateWhenNotNull = firstAccessStateWhenNotNull;
                }
                else
                {
                    Unsplit();
                    stateWhenNotNull = this.State.Clone();
                }
            }
            else
            {
                var savedState = this.State.Clone();
                if (IsConstantNull(node.Receiver))
                {
                    SetUnreachable();
                }
                else
                {
                    SetState(stateWhenNotNull);
                }

                // We want to preserve stateWhenNotNull from accesses in the same "chain":
                // a?.b(out x)?.c(out y); // expected to preserve stateWhenNotNull from both ?.b(out x) and ?.c(out y)
                // but not accesses in nested expressions:
                // a?.b(out x, c?.d(out y)); // expected to preserve stateWhenNotNull from a?.b(out x, ...) but not from c?.d(out y)
                BoundExpression expr = node.AccessExpression;
                while (expr is BoundConditionalAccess innerCondAccess)
                {
                    Debug.Assert(innerCondAccess.Receiver is not (BoundConditionalAccess or BoundConversion));
                    // we assume that non-conditional accesses can never contain conditional accesses from the same "chain".
                    // that is, we never have to dig through non-conditional accesses to find and handle conditional accesses.
                    VisitRvalue(innerCondAccess.Receiver);
                    expr = innerCondAccess.AccessExpression;

                    // The savedState here represents the scenario where 0 or more of the access expressions could have been evaluated.
                    // e.g. after visiting `a?.b(x = null)?.c(x = new object())`, the "state when not null" of `x` is NotNull, but the "state when maybe null" of `x` is MaybeNull.
                    Join(ref savedState, ref State);
                }

                Debug.Assert(expr is BoundExpression);
                VisitRvalue(expr);

                stateWhenNotNull = State;
                State = savedState;
                Join(ref State, ref stateWhenNotNull);
            }
        }

        public override BoundNode? VisitConditionalAccess(BoundConditionalAccess node)
        {
            VisitConditionalAccess(node, stateWhenNotNull: out _);
            return null;
        }

#nullable disable

        public override BoundNode VisitLoweredConditionalAccess(BoundLoweredConditionalAccess node)
        {
            VisitRvalue(node.Receiver);

            var savedState = this.State.Clone();

            VisitRvalue(node.WhenNotNull);
            Join(ref this.State, ref savedState);

            if (node.WhenNullOpt != null)
            {
                savedState = this.State.Clone();
                VisitRvalue(node.WhenNullOpt);
                Join(ref this.State, ref savedState);
            }

            return null;
        }

        public override BoundNode VisitConditionalReceiver(BoundConditionalReceiver node)
        {
            return null;
        }

        public override BoundNode VisitComplexConditionalReceiver(BoundComplexConditionalReceiver node)
        {
            var savedState = this.State.Clone();

            VisitRvalue(node.ValueTypeReceiver);
            Join(ref this.State, ref savedState);

            savedState = this.State.Clone();
            VisitRvalue(node.ReferenceTypeReceiver);
            Join(ref this.State, ref savedState);

            return null;
        }

        public override BoundNode VisitSequence(BoundSequence node)
        {
            var sideEffects = node.SideEffects;
            if (!sideEffects.IsEmpty)
            {
                foreach (var se in sideEffects)
                {
                    VisitRvalue(se);
                }
            }

            Visit(node.Value);
            return null;
        }

        public override BoundNode VisitSequencePoint(BoundSequencePoint node)
        {
            if (node.StatementOpt != null)
            {
                VisitStatement(node.StatementOpt);
            }

            return null;
        }

        public override BoundNode VisitSequencePointExpression(BoundSequencePointExpression node)
        {
            Visit(node.Expression);
            return null;
        }

        public override BoundNode VisitSequencePointWithSpan(BoundSequencePointWithSpan node)
        {
            if (node.StatementOpt != null)
            {
                VisitStatement(node.StatementOpt);
            }

            return null;
        }

        public override BoundNode VisitModuleCancellationTokenExpression(ModuleCancellationTokenExpression node)
        {
            return null;
        }

        public override BoundNode VisitStatementList(BoundStatementList node)
        {
            return VisitStatementListWorker(node);
        }

        private BoundNode VisitStatementListWorker(BoundStatementList node)
        {
            foreach (var statement in node.Statements)
            {
                VisitStatement(statement);
            }

            return null;
        }

        public override BoundNode VisitTypeOrInstanceInitializers(BoundTypeOrInstanceInitializers node)
        {
            return VisitStatementListWorker(node);
        }

        public override BoundNode VisitUnboundLambda(UnboundLambda node)
        {
            // The presence of this node suggests an error was detected in an earlier phase.
            return VisitLambda(node.BindForErrorRecovery());
        }

        public override BoundNode VisitBreakStatement(BoundBreakStatement node)
        {
            Debug.Assert(!this.IsConditionalState);
            PendingBranches.Add(new PendingBranch(node, this.State, node.Label));
            SetUnreachable();
            return null;
        }

        public override BoundNode VisitContinueStatement(BoundContinueStatement node)
        {
            Debug.Assert(!this.IsConditionalState);
            PendingBranches.Add(new PendingBranch(node, this.State, node.Label));
            SetUnreachable();
            return null;
        }

        public override BoundNode VisitUnconvertedConditionalOperator(BoundUnconvertedConditionalOperator node)
        {
            return VisitConditionalOperatorCore(node, isByRef: false, node.Condition, node.Consequence, node.Alternative);
        }

        public override BoundNode VisitConditionalOperator(BoundConditionalOperator node)
        {
            return VisitConditionalOperatorCore(node, node.IsRef, node.Condition, node.Consequence, node.Alternative);
        }

#nullable enable

        protected virtual BoundNode? VisitConditionalOperatorCore(
            BoundExpression node,
            bool isByRef,
            BoundExpression condition,
            BoundExpression consequence,
            BoundExpression alternative)
        {
            VisitCondition(condition);
            var consequenceState = this.StateWhenTrue;
            var alternativeState = this.StateWhenFalse;
            if (IsConstantTrue(condition))
            {
                VisitConditionalOperand(alternativeState, alternative, isByRef);
                VisitConditionalOperand(consequenceState, consequence, isByRef);
            }
            else if (IsConstantFalse(condition))
            {
                VisitConditionalOperand(consequenceState, consequence, isByRef);
                VisitConditionalOperand(alternativeState, alternative, isByRef);
            }
            else
            {
                // at this point, the state is conditional after a conditional expression if:
                // 1. the state is conditional after the consequence, or
                // 2. the state is conditional after the alternative

                VisitConditionalOperand(consequenceState, consequence, isByRef);
                var conditionalAfterConsequence = IsConditionalState;
                var (afterConsequenceWhenTrue, afterConsequenceWhenFalse) = conditionalAfterConsequence ? (StateWhenTrue, StateWhenFalse) : (State, State);

                VisitConditionalOperand(alternativeState, alternative, isByRef);
                if (!conditionalAfterConsequence && !IsConditionalState)
                {
                    // simplify in the common case
                    Join(ref this.State, ref afterConsequenceWhenTrue);
                }
                else
                {
                    Split();
                    Join(ref this.StateWhenTrue, ref afterConsequenceWhenTrue);
                    Join(ref this.StateWhenFalse, ref afterConsequenceWhenFalse);
                }
            }

            return null;
        }

#nullable disable

        private void VisitConditionalOperand(TLocalState state, BoundExpression operand, bool isByRef)
        {
            SetState(state);
            if (isByRef)
            {
                VisitLvalue(operand);
                // exposing ref is a potential write
                WriteArgument(operand, RefKind.Ref, method: null);
            }
            else
            {
                Visit(operand);
            }
        }

        public override BoundNode VisitBaseReference(BoundBaseReference node)
        {
            return null;
        }

        public override BoundNode VisitDoStatement(BoundDoStatement node)
        {
            // do { statements; node.ContinueLabel: } while (node.Condition) node.BreakLabel:
            LoopHead(node);
            VisitStatement(node.Body);
            ResolveContinues(node.ContinueLabel);
            VisitCondition(node.Condition);
            TLocalState breakState = this.StateWhenFalse;
            SetState(this.StateWhenTrue);
            LoopTail(node);
            ResolveBreaks(breakState, node.BreakLabel);
            return null;
        }

        public override BoundNode VisitGotoStatement(BoundGotoStatement node)
        {
            Debug.Assert(!this.IsConditionalState);
            PendingBranches.Add(new PendingBranch(node, this.State, node.Label));
            SetUnreachable();
            return null;
        }

        protected void VisitLabel(LabelSymbol label, BoundStatement node)
        {
            node.AssertIsLabeledStatementWithLabel(label);
            ResolveBranches(label, node);
            var state = LabelState(label);
            Join(ref this.State, ref state);
            _labels[label] = this.State.Clone();
            _labelsSeen.Add(node);
        }

        protected virtual void VisitLabel(BoundLabeledStatement node)
        {
            VisitLabel(node.Label, node);
        }

        public override BoundNode VisitLabelStatement(BoundLabelStatement node)
        {
            VisitLabel(node.Label, node);
            return null;
        }

        public override BoundNode VisitLabeledStatement(BoundLabeledStatement node)
        {
            VisitLabel(node);
            VisitStatement(node.Body);
            return null;
        }

        public override BoundNode VisitLockStatement(BoundLockStatement node)
        {
            VisitRvalue(node.Argument);
            VisitStatement(node.Body);
            return null;
        }

        public override BoundNode VisitNoOpStatement(BoundNoOpStatement node)
        {
            return null;
        }

        public override BoundNode VisitNamespaceExpression(BoundNamespaceExpression node)
        {
            return null;
        }

        public override BoundNode VisitUsingStatement(BoundUsingStatement node)
        {
            if (node.ExpressionOpt != null)
            {
                VisitRvalue(node.ExpressionOpt);
            }

            if (node.DeclarationsOpt != null)
            {
                VisitStatement(node.DeclarationsOpt);
            }

            VisitStatement(node.Body);

            if (AwaitUsingAndForeachAddsPendingBranch && node.AwaitOpt != null)
            {
                PendingBranches.Add(new PendingBranch(node, this.State, null));
            }
            return null;
        }

        public abstract bool AwaitUsingAndForeachAddsPendingBranch { get; }

        public override BoundNode VisitFixedStatement(BoundFixedStatement node)
        {
            VisitStatement(node.Declarations);
            VisitStatement(node.Body);
            return null;
        }

        public override BoundNode VisitFixedLocalCollectionInitializer(BoundFixedLocalCollectionInitializer node)
        {
            VisitRvalue(node.Expression);
            return null;
        }

        public override BoundNode VisitThrowStatement(BoundThrowStatement node)
        {
            BoundExpression expr = node.ExpressionOpt;
            VisitRvalue(expr);
            SetUnreachable();
            return null;
        }

        public override BoundNode VisitYieldBreakStatement(BoundYieldBreakStatement node)
        {
            Debug.Assert(!this.IsConditionalState);
            PendingBranches.Add(new PendingBranch(node, this.State, null));
            SetUnreachable();
            return null;
        }

        public override BoundNode VisitYieldReturnStatement(BoundYieldReturnStatement node)
        {
            VisitRvalue(node.Expression);
            PendingBranches.Add(new PendingBranch(node, this.State, null));
            return null;
        }

        public override BoundNode VisitDefaultLiteral(BoundDefaultLiteral node)
        {
            return null;
        }

        public override BoundNode VisitDefaultExpression(BoundDefaultExpression node)
        {
            return null;
        }

        public override BoundNode VisitUnconvertedObjectCreationExpression(BoundUnconvertedObjectCreationExpression node)
        {
            throw ExceptionUtilities.Unreachable();
        }

        public override BoundNode VisitTypeOfOperator(BoundTypeOfOperator node)
        {
            VisitTypeExpression(node.SourceType);
            return null;
        }

        public override BoundNode VisitNameOfOperator(BoundNameOfOperator node)
        {
            var savedState = this.State;
            SetState(UnreachableState());
            Visit(node.Argument);
            SetState(savedState);
            return null;
        }

        public override BoundNode VisitAddressOfOperator(BoundAddressOfOperator node)
        {
            VisitAddressOfOperand(node.Operand, shouldReadOperand: false);
            return null;
        }

        protected void VisitAddressOfOperand(BoundExpression operand, bool shouldReadOperand)
        {
            if (shouldReadOperand)
            {
                this.VisitRvalue(operand);
            }
            else
            {
                this.VisitLvalue(operand);
            }

            this.WriteArgument(operand, RefKind.Out, null); //Out because we know it will definitely be assigned.
        }

        public override BoundNode VisitPointerIndirectionOperator(BoundPointerIndirectionOperator node)
        {
            VisitRvalue(node.Operand);
            return null;
        }

        public override BoundNode VisitPointerElementAccess(BoundPointerElementAccess node)
        {
            VisitRvalue(node.Expression);
            VisitRvalue(node.Index);
            return null;
        }

        public override BoundNode VisitSizeOfOperator(BoundSizeOfOperator node)
        {
            return null;
        }

        private BoundNode VisitStackAllocArrayCreationBase(BoundStackAllocArrayCreationBase node)
        {
            VisitRvalue(node.Count);
            VisitRvalue(node.InitializerOpt);
            return null;
        }

        public override BoundNode VisitStackAllocArrayCreation(BoundStackAllocArrayCreation node)
        {
            return VisitStackAllocArrayCreationBase(node);
        }

        public override BoundNode VisitConvertedStackAllocExpression(BoundConvertedStackAllocExpression node)
        {
            return VisitStackAllocArrayCreationBase(node);
        }

        public override BoundNode VisitAnonymousObjectCreationExpression(BoundAnonymousObjectCreationExpression node)
        {
            //  visit arguments as r-values
            VisitArguments(node.Arguments, default(ImmutableArray<RefKind>), node.Constructor, default, false);

            return null;
        }

        public override BoundNode VisitArrayLength(BoundArrayLength node)
        {
            VisitRvalue(node.Expression);
            return null;
        }

        public override BoundNode VisitConditionalGoto(BoundConditionalGoto node)
        {
            VisitCondition(node.Condition);
            Debug.Assert(this.IsConditionalState);
            if (node.JumpIfTrue)
            {
                PendingBranches.Add(new PendingBranch(node, this.StateWhenTrue, node.Label));
                this.SetState(this.StateWhenFalse);
            }
            else
            {
                PendingBranches.Add(new PendingBranch(node, this.StateWhenFalse, node.Label));
                this.SetState(this.StateWhenTrue);
            }

            return null;
        }

        public override BoundNode VisitObjectInitializerExpression(BoundObjectInitializerExpression node)
        {
            return VisitObjectOrCollectionInitializerExpression(node.Initializers);
        }

        public override BoundNode VisitCollectionInitializerExpression(BoundCollectionInitializerExpression node)
        {
            return VisitObjectOrCollectionInitializerExpression(node.Initializers);
        }

        private BoundNode VisitObjectOrCollectionInitializerExpression(ImmutableArray<BoundExpression> initializers)
        {
            foreach (var initializer in initializers)
            {
                VisitRvalue(initializer);
            }

            return null;
        }

        public override BoundNode VisitObjectInitializerMember(BoundObjectInitializerMember node)
        {
            var arguments = node.Arguments;
            if (!arguments.IsDefaultOrEmpty)
            {
                MethodSymbol method = null;

                if (node.MemberSymbol?.Kind == SymbolKind.Property)
                {
                    var property = (PropertySymbol)node.MemberSymbol;
                    method = GetReadMethod(property);
                }

                VisitArguments(node.Arguments, node.ArgumentRefKindsOpt, method, node.ArgsToParamsOpt, node.Expanded);
            }

            return null;
        }

        public override BoundNode VisitDynamicObjectInitializerMember(BoundDynamicObjectInitializerMember node)
        {
            return null;
        }

        public override BoundNode VisitCollectionElementInitializer(BoundCollectionElementInitializer node)
        {
            if (node.AddMethod.CallsAreOmitted(node.SyntaxTree))
            {
                // If the underlying add method is a partial method without a definition, or is a conditional method
                // whose condition is not true, then the call has no effect and it is ignored for the purposes of
                // flow analysis.

                TLocalState savedState = savedState = this.State.Clone();
                SetUnreachable();

                VisitArguments(node.Arguments, default(ImmutableArray<RefKind>), node.AddMethod, node.ArgsToParamsOpt, node.Expanded);

                this.State = savedState;
            }
            else
            {
                VisitArguments(node.Arguments, default(ImmutableArray<RefKind>), node.AddMethod, node.ArgsToParamsOpt, node.Expanded);
            }

            return null;
        }

        public override BoundNode VisitDynamicCollectionElementInitializer(BoundDynamicCollectionElementInitializer node)
        {
            VisitArguments(node.Arguments, default(ImmutableArray<RefKind>), method: null, default, false);
            return null;
        }

        public override BoundNode VisitImplicitReceiver(BoundImplicitReceiver node)
        {
            return null;
        }

        public override BoundNode VisitFieldEqualsValue(BoundFieldEqualsValue node)
        {
            VisitRvalue(node.Value);
            return null;
        }

        public override BoundNode VisitPropertyEqualsValue(BoundPropertyEqualsValue node)
        {
            VisitRvalue(node.Value);
            return null;
        }

        public override BoundNode VisitParameterEqualsValue(BoundParameterEqualsValue node)
        {
            VisitRvalue(node.Value);
            return null;
        }

        public override BoundNode VisitDeconstructValuePlaceholder(BoundDeconstructValuePlaceholder node)
        {
            return null;
        }

        public override BoundNode VisitObjectOrCollectionValuePlaceholder(BoundObjectOrCollectionValuePlaceholder node)
        {
            return null;
        }

        public override BoundNode VisitAwaitableValuePlaceholder(BoundAwaitableValuePlaceholder node)
        {
            return null;
        }

        public sealed override BoundNode VisitOutVariablePendingInference(OutVariablePendingInference node)
        {
            throw ExceptionUtilities.Unreachable();
        }

        public sealed override BoundNode VisitDeconstructionVariablePendingInference(DeconstructionVariablePendingInference node)
        {
            throw ExceptionUtilities.Unreachable();
        }

        public override BoundNode VisitDiscardExpression(BoundDiscardExpression node)
        {
            return null;
        }

        private static MethodSymbol GetReadMethod(PropertySymbol property) =>
            property.GetOwnOrInheritedGetMethod() ?? property.SetMethod;

        private static MethodSymbol GetWriteMethod(PropertySymbol property) =>
            property.GetOwnOrInheritedSetMethod() ?? property.GetMethod;

        public override BoundNode VisitConstructorMethodBody(BoundConstructorMethodBody node)
        {
            Visit(node.Initializer);
            VisitMethodBodies(node.BlockBody, node.ExpressionBody);
            return null;
        }

        public override BoundNode VisitNonConstructorMethodBody(BoundNonConstructorMethodBody node)
        {
            VisitMethodBodies(node.BlockBody, node.ExpressionBody);
            return null;
        }

        public override BoundNode VisitNullCoalescingAssignmentOperator(BoundNullCoalescingAssignmentOperator node)
        {
            TLocalState leftState;
            if (RegularPropertyAccess(node.LeftOperand) &&
                (BoundPropertyAccess)node.LeftOperand is var left &&
                left.PropertySymbol is var property &&
                property.RefKind == RefKind.None)
            {
                var readMethod = property.GetOwnOrInheritedGetMethod();

                VisitReceiverBeforeCall(left.ReceiverOpt, readMethod);
                VisitReceiverAfterCall(left.ReceiverOpt, readMethod);

                var savedState = this.State.Clone();
                AdjustStateForNullCoalescingAssignmentNonNullCase(node);
                leftState = this.State.Clone();
                SetState(savedState);
                VisitAssignmentOfNullCoalescingAssignment(node, left);
            }
            else
            {
                VisitRvalue(node.LeftOperand, isKnownToBeAnLvalue: true);
                var savedState = this.State.Clone();
                AdjustStateForNullCoalescingAssignmentNonNullCase(node);
                leftState = this.State.Clone();
                SetState(savedState);
                VisitAssignmentOfNullCoalescingAssignment(node, propertyAccessOpt: null);
            }

            Join(ref this.State, ref leftState);
            return null;
        }

        public override BoundNode VisitReadOnlySpanFromArray(BoundReadOnlySpanFromArray node)
        {
            VisitRvalue(node.Operand);
            return null;
        }

        public override BoundNode VisitFunctionPointerInvocation(BoundFunctionPointerInvocation node)
        {
            VisitRvalue(node.InvokedExpression);
            VisitArguments(node.Arguments, node.ArgumentRefKindsOpt, node.FunctionPointer.Signature, default, false);
            return null;
        }

        public override BoundNode VisitUnconvertedAddressOfOperator(BoundUnconvertedAddressOfOperator node)
        {
            // This is not encountered in correct programs, but can be seen if the function pointer was
            // unable to be converted and the semantic model is used to query for information.
            Visit(node.Operand);
            return null;
        }

        /// <summary>
        /// This visitor represents just the assignment part of the null coalescing assignment
        /// operator.
        /// </summary>
        protected virtual void VisitAssignmentOfNullCoalescingAssignment(
            BoundNullCoalescingAssignmentOperator node,
            BoundPropertyAccess propertyAccessOpt)
        {
            VisitRvalue(node.RightOperand);
            if (propertyAccessOpt != null)
            {
                var symbol = propertyAccessOpt.PropertySymbol;
                var writeMethod = symbol.GetOwnOrInheritedSetMethod();
                PropertySetter(node, propertyAccessOpt.ReceiverOpt, writeMethod);
            }
        }

        public override BoundNode VisitSavePreviousSequencePoint(BoundSavePreviousSequencePoint node)
        {
            return null;
        }

        public override BoundNode VisitRestorePreviousSequencePoint(BoundRestorePreviousSequencePoint node)
        {
            return null;
        }

        public override BoundNode VisitStepThroughSequencePoint(BoundStepThroughSequencePoint node)
        {
            return null;
        }

        /// <summary>
        /// This visitor represents just the non-assignment part of the null coalescing assignment
        /// operator (when the left operand is non-null).
        /// </summary>
        protected virtual void AdjustStateForNullCoalescingAssignmentNonNullCase(BoundNullCoalescingAssignmentOperator node)
        {
        }

        private void VisitMethodBodies(BoundBlock blockBody, BoundBlock expressionBody)
        {
            if (blockBody == null)
            {
                Visit(expressionBody);
                return;
            }
            else if (expressionBody == null)
            {
                Visit(blockBody);
                return;
            }

            // In error cases we have two bodies. These are two unrelated pieces of code,
            // they are not executed one after another. As we don't really know which one the developer
            // intended to use, we need to visit both. We are going to pretend that there is
            // an unconditional fork in execution and then we are converging after each body is executed.
            // For example, if only one body assigns an out parameter, then after visiting both bodies
            // we should consider that parameter is not definitely assigned.
            // Note, that today this code is not executed for regular definite assignment analysis. It is
            // only executed for region analysis.
            TLocalState initialState = this.State.Clone();
            Visit(blockBody);
            TLocalState afterBlock = this.State;
            SetState(initialState);
            Visit(expressionBody);

            Join(ref this.State, ref afterBlock);
        }
        #endregion visitors
    }

    /// <summary>
    /// The possible places that we are processing when there is a region.
    /// </summary>
    /// <remarks>
    /// This should be nested inside <see cref="AbstractFlowPass{TLocalState, TLocalFunctionState}"/> but is not due to https://github.com/dotnet/roslyn/issues/36992 .
    /// </remarks>
    internal enum RegionPlace { Before, Inside, After };
}
