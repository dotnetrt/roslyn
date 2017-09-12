// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.Collections.Generic;
using Microsoft.CodeAnalysis.CSharp.Symbols;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.CSharp.Test.Utilities;
using Microsoft.CodeAnalysis.Text;
using Roslyn.Test.Utilities;
using Xunit;

namespace Microsoft.CodeAnalysis.CSharp.UnitTests.CodeGen
{
    public class CodeGenConstParameterTests : CSharpTestBase
    {
        [Fact]
        public void TestConstParamSignature001()
        {
            var source = @"
class C
{
    void M(const int x)
    {
    }
}";
            CompileAndVerify(source,
                expectedSignatures: new[]
                {
                    Signature("C", "M", ".method private hidebysig instance System.Void M([const] System.Int32 x) cil managed")
                },
                parseOptions: TestOptions.Regular.WithExperimental(MessageID.IDS_FeatureConstParameters));
        }

        [Fact]
        public void TestConstParamSignature002()
        {
            var source = @"
class C
{
    void M(out object instance, const int x)
    {
    }
}";
            CompileAndVerify(source,
                expectedSignatures: new[]
                {
                    Signature("C", "M", ".method private hidebysig instance System.Void M([out] System.Object instance, [const] System.Int32 x) cil managed")
                },
                parseOptions: TestOptions.Regular.WithExperimental(MessageID.IDS_FeatureConstParameters));
        }
    }
}
