// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Symbols;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.CSharp.Test.Utilities;
using Microsoft.CodeAnalysis.Test.Utilities;
using Roslyn.Test.Utilities;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.CodeAnalysis.CSharp.UnitTests
{
    public class ConstParameterTests : ParsingTests
    {
        public ConstParameterTests(ITestOutputHelper output) : base(output) { }

        protected override SyntaxTree ParseTree(string text, CSharpParseOptions options)
        {
            return SyntaxFactory.ParseSyntaxTree(text, options);
        }

        [Fact]
        public void TestConstParameter001()
        {
            var text = "public class A { public void TestConst(const int value){ return; } }";
            var file = this.ParseFile(text);

            VerifyTestPassed(text, file);
        }

        [Fact]
        public void TestConstParameter002()
        {
            var text = "public class A { public void TestConst(ref int input, const int value){ return; } }";
            var file = this.ParseFile(text);

            VerifyTestPassed(text, file);
        }

        [Fact]
        public void TestConstParameter003()
        {
            var text = "public class A { public void TestConst<T, U>(ref int input, const int value){ return; } }";
            var file = this.ParseFile(text);

            VerifyTestPassed(text, file);
        }

        [Fact]
        public void TestConstParameter004()
        {
            var text = "public class A { public void TestConst<T, U>(ref T input, const int value, params U[] list){ return; } }";
            var file = this.ParseFile(text);

            VerifyTestPassed(text, file);
        }

        [Fact]
        public void TestConstParameter005()
        {
            var text = "public class A { public void TestConst<T>(const int input, out T value){ value = null; return; } }";
            var file = this.ParseFile(text);

            VerifyTestPassed(text, file);
        }

        [Fact]
        public void TestConstParameter006()
        {
            var text = "public class A { public async Task TestConstAsync<T>(const int input, out T value){ value = null; return; } }";
            var file = this.ParseFile(text);

            VerifyTestPassed(text, file);
        }

        [Fact]
        public void TestConstParameter007()
        {
            var text = "public class A { public async Task<U> TestConstAsync<T, U>(const int input, out T value) where U : struct { value = null; return default(U); } }";
            var file = this.ParseFile(text);

            VerifyTestPassed(text, file);
        }

        private static void VerifyTestPassed(string text, CompilationUnitSyntax file)
        {
            Assert.NotNull(file);
            Assert.Equal(text, file.ToString());
            Assert.False(file.HasErrors);
            Assert.False(file.Warnings().Length > 0);
        }
    }
}
