// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Diagnostics;
using System.Xml;
using System.Xml.XPath;

namespace System.Xml.Xsl.XsltOld
{
    internal sealed class CommentAction : ContainerAction
    {
        internal override void Compile(Compiler compiler)
        {
            CompileAttributes(compiler);

            if (compiler.Recurse())
            {
                CompileTemplate(compiler);
                compiler.ToParent();
            }
        }

        internal override void Execute(Processor processor, ActionFrame frame)
        {
            Debug.Assert(processor != null && frame != null);

            switch (frame.State)
            {
                case Initialized:
                    if (processor.BeginEvent(XPathNodeType.Comment, string.Empty, string.Empty, string.Empty, false) == false)
                    {
                        // Come back later
                        break;
                    }

                    processor.PushActionFrame(frame);
                    frame.State = ProcessingChildren;
                    break;                              // Allow children to run

                case ProcessingChildren:
                    if (processor.EndEvent(XPathNodeType.Comment) == false)
                    {
                        break;
                    }

                    frame.Finished();
                    break;

                default:
                    Debug.Fail("Invalid IfAction execution state");
                    break;
            }
        }
    }
}
