// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CSharp.Symbols;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.CSharp.Test.Utilities;
using Roslyn.Test.Utilities;
using Xunit;

namespace Microsoft.CodeAnalysis.CSharp.UnitTests
{
    public class ConstantParameterTests : CompilingTestBase
    {
        [Fact]
        public void TestConstantParameter001()
        {
            string source = @"
public class C
{
    public void TestConstant(const int value) { return; }
    public void TestConstant(int value) { return; }
    public void M() { TestConstant(const 32 - 1); }
}
";
            var actual = ConstantTests.ParseAndGetConstantFoldingSteps(source);
            var expected = "32 - 1 --> 31";
            Assert.Equal(expected, actual);
        }
    }
}
