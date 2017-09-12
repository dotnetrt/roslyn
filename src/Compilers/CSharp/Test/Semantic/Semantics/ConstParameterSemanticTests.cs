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
    public class ConstParameterSemanticTests : CompilingTestBase
    {
        [Fact]
        public void ConstantParameterFolding()
        {
            string source = @"
public class C
{
    public void TestConstant(const int value) { return; }
    public void M() { TestConstant(const 32 - 1); }
}
";
            var actual = ParseAndGetConstantFoldingSteps(source,
                TestOptions.Regular.WithExperimental(MessageID.IDS_FeatureConstParameters));
            var expected = "32 - 1 --> 31";
            Assert.Equal(expected, actual);
        }

        private static string ParseAndGetConstantFoldingSteps(string source, CSharpParseOptions parseOptions = null)
        {
            return ParseAndGetConstantFoldingSteps(
                source,
                node => node.Kind != BoundKind.Literal && node.Kind != BoundKind.Local,
                parseOptions);
        }

        private static string ParseAndGetConstantFoldingSteps(string source, Func<BoundNode, bool> predicate, CSharpParseOptions parseOptions = null)
        {
            var block = ParseAndBindMethodBody(source, parseOptions: parseOptions);
            var constants = BoundTreeSequencer.GetNodes(block).
                Where(predicate).
                OfType<BoundExpression>().
                Where(node => node.ConstantValue != null).
                Select(node => node.Syntax.ToFullString().Trim() + " --> " + ExtractValue(node.ConstantValue));
            var result = string.Join(Environment.NewLine, constants);
            return result;
        }

        private static object ExtractValue(ConstantValue constantValue)
        {
            if (constantValue.IsBad)
            {
                return "BAD";
            }

            if (constantValue.IsChar && char.IsControl(constantValue.CharValue))
            {
                return "control character";
            }

            // return constantValue.Value ?? "null";
            if (constantValue.Value == null)
                return "null";

            return TestHelpers.GetCultureInvariantString(constantValue.Value);
        }

        [Fact]
        public void OverloadResolutionWithConstParAndPar()
        {
            string source = @"
public class C
{
    public void TestConstant(const int value) { return; }
    public void TestConstant(int value) { return; }
    public void M() { TestConstant(const 1); }
}
";
            // TODO: Enable verification after implementing CodeGen, Emit and symbol import
            var actual = CompileAndVerifyExperimental(source, MessageID.IDS_FeatureConstParameters, verify: false);
            Assert.False(actual.Diagnostics.HasAnyErrors());
        }

        [Fact(Skip = "Emit not implemented for constparameter feature")]
        public void OverloadResolutionWithRefParConstParAndPar()
        {
            string source = @"
public class C
{
    public void TestConstant(ref int value) { return; }
    public void TestConstant(const int value) { return; }
    public void TestConstant(int value) { return; }
    public void M() { TestConstant(const 1); }
}
";
            // TODO: Enable verification after implementing CodeGen, Emit and symbol import
            var actual = CompileAndVerifyExperimental(source, MessageID.IDS_FeatureConstParameters, verify: false);
            Assert.False(actual.Diagnostics.HasAnyErrors());
        }
    }
}
