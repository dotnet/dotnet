// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Microsoft.Css.Parser.Classify;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Css.Parser.Test.Classify
{
    [TestClass]
    public class ClassifyContextTest : CssUnitTestBase
    {
        [TestMethod]
        public void GetClassificationNameTest()
        {
            // For every single classifier context, make sure that the name returned by
            // IClassifierContext.GetClassificationName is correct.

            int length = (int) CssClassifierContextType.CssClassifierContextTypeCount;

            for (int i = 0; i < length; i++)
            {
                CssClassifierContextType contextType = (CssClassifierContextType) i;
                IClassifierContext context = CssClassifierContextCache.FromTypeEnum(contextType);

                // Make sure I got the right thing from the cache
                Assert.IsNotNull(context);
                Assert.AreEqual((int)contextType, context.ContextValue);

                Assert.IsTrue(context.ClassificationName == "Default" ||
                    context.ClassificationName == "Css" + contextType.ToString());
            }
        }
    }
}
