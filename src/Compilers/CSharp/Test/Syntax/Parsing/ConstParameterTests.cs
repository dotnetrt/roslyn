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
        public void MethodReturnsVoidWithConstPar()
        {
            var text =
@"public class A
{
    public void TestConst(const int value)
    {
        return;
    }
}
";
            var file = this.ParseFileExperimental(text, MessageID.IDS_FeatureConstParameters);

            VerifyTestPassed(text, file);
        }

        [Fact]
        public void MethodReturnsVoidWithRefParConstPar()
        {
            var text =
@"public class A
{
    public void TestConst(ref int input, const int value)
    {
        return;
    }
}
";
            var file = this.ParseFileExperimental(text, MessageID.IDS_FeatureConstParameters);

            VerifyTestPassed(text, file);
        }

        [Fact]
        public void GenericMethodReturnsVoidWithRefParConstPar()
        {
            var text =
@"public class A
{
    public void TestConst<T, U>(ref int input, const int value)
    {
        return;
    }
}
";
            var file = this.ParseFileExperimental(text, MessageID.IDS_FeatureConstParameters);

            VerifyTestPassed(text, file);
        }

        [Fact]
        public void GenericMethodReturnsVoidWithRefGenericParConstParAndGenericParams()
        {
            var text =
@"public class A
{
    public void TestConst<T, U>(ref T input, const int value, params U[] list)
    {
        return;
    }
}
";
            var file = this.ParseFileExperimental(text, MessageID.IDS_FeatureConstParameters);

            VerifyTestPassed(text, file);
        }

        [Fact]
        public void GenericMethodReturnsVoidWithConstParAndGenericOutPar()
        {
            var text =
@"public class A
{
    public void TestConst<T>(const int input, out T value)
    {
        value = null;
        return;
    }
}
";
            var file = this.ParseFileExperimental(text, MessageID.IDS_FeatureConstParameters);

            VerifyTestPassed(text, file);
        }

        [Fact]
        public void AsyncGenericMethodReturnsTaskWithConstParAndGenericOutPar()
        {
            var text =
@"public class A
{
    public async Task TestConstAsync<T>(const int input, out T value)
    {
        value = null;
        return;
    }
}
";
            var file = this.ParseFileExperimental(text, MessageID.IDS_FeatureConstParameters);

            VerifyTestPassed(text, file);
        }

        [Fact]
        public void AsyncGenericMethodReturnsTaskUWithConstParAndGenericOutPar()
        {
            var text =
@"public class A
{
    public async Task<U> TestConstAsync<T, U>(const int input, out T value) where U : struct
    {
        value = null;
        return default(U);
    }
}
";
            var file = this.ParseFileExperimental(text, MessageID.IDS_FeatureConstParameters);

            VerifyTestPassed(text, file);
        }

        [Fact]
        public void DelegateWithRefParConstPar()
        {
            var text =
@"public class A
{
    public delegate void DelegateWithConst(ref int input, const int value);
}
";
            var file = this.ParseFileExperimental(text, MessageID.IDS_FeatureConstParameters);

            VerifyTestPassed(text, file);
        }

        [Fact]
        public void ConstructorWithRefParConstPar()
        {
            var text =
@"public class A
{
    public A(ref int input, const int value)
    {
    }
}
";
            var file = this.ParseFileExperimental(text, MessageID.IDS_FeatureConstParameters);

            VerifyTestPassed(text, file);
        }

        [Fact]
        public void AnonymousMethodWithConstParConstPar()
        {
            var text =
@"public class A
{
    public delegate void DelegateWithConst(const int errorCode, const string errorMessage);
    public event DelegateWithConst PrintError = delegate (const int errorId, const string messageTxt) { WriteMessage(errorId, messageTxt); };
    private void WriteMessage(int value, string message) {}
}
";
            var file = this.ParseFileExperimental(text, MessageID.IDS_FeatureConstParameters);

            VerifyTestPassed(text, file);
        }

        [Fact]
        public void LambdaWithExplicitlyTypedConstParConstPar()
        {
            var text =
@"public class A
{
    public delegate void DelegateWithConst(const int errorCode, const string errorMessage);
    public event DelegateWithConst PrintError = (const int errorId, const string messageTxt) => { WriteMessage(errorId, messageTxt); };
    private void WriteMessage(int value, string message) {}
}
";
            var file = this.ParseFileExperimental(text, MessageID.IDS_FeatureConstParameters);

            VerifyTestPassed(text, file);
        }

        [Fact]
        public void ExplicitConversionOperatorWithParConstPar()
        {
            var text =
@"public class A
{
}

public class B
{
    public B(A value, const int shift)
    {
    }
    public explicit operator B(A value, const byte shift)
    {
        return new B(A, const shift);
    }
}
";
            var file = this.ParseFileExperimental(text, MessageID.IDS_FeatureConstParameters);

            VerifyTestPassed(text, file);
        }

        [Fact]
        public void LocalMethodParConstPar()
        {
            var text =
@"public class A
{
    public A()
    {
        InitializeA(new Object(), const -1);
        void InitializeA(object o, const sbyte value)
        {
            return;
        }
    }
}
";
            var file = this.ParseFileExperimental(text, MessageID.IDS_FeatureConstParameters);

            VerifyTestPassed(text, file);
        }

        [Fact]
        public void LocalMethodRefParConstPar()
        {
            var text =
@"public class A
{
    public A()
    {
        var obj = new Object();
        InitializeA(ref obj, const -1);
        void InitializeA(ref object o, const sbyte value)
        {
            return;
        }
    }
}
";
            var file = this.ParseFileExperimental(text, MessageID.IDS_FeatureConstParameters);

            VerifyTestPassed(text, file);
        }

        [Fact]
        public void MethodConstParDefoultVal()
        {
            var text =
@"public class A
{
    public void TestConst(const int value = -1)
    {
        return;
    }
}
";
            var file = this.ParseFileExperimental(text, MessageID.IDS_FeatureConstParameters);

            VerifyTestPassed(text, file);
        }

        [Fact]
        public void MethodOutParConstParDefoultVal()
        {
            var text =
@"public class A
{
    public void TestConst(out object obj, const int value = -1)
    {
        obj = null;
        return;
    }
}
";
            var file = this.ParseFileExperimental(text, MessageID.IDS_FeatureConstParameters);

            VerifyTestPassed(text, file);
        }

        [Fact]
        public void GenericMethodOutParConstParDefoultValWhereClause()
        {
            var text =
@"public class A
{
    public void TestConst<T>(out T obj, const int value = -1) where T : class
    {
        obj = null;
        return;
    }
}
";
            var file = this.ParseFileExperimental(text, MessageID.IDS_FeatureConstParameters);

            VerifyTestPassed(text, file);
        }

        [Fact]
        public void MethodsAllConstParTypesDefaultVal()
        {
            var text =
@"public class A
{
    public void TestByteConst(const byte value = 255)
    {
        return;
    }
    public void TestSByteConst(const sbyte value = -128)
    {
        return;
    }
    public void TestShortConst(const short value = short.MinValue)
    {
        return;
    }
    public void TestUShortConst(const ushort value = ushort.MaxValue)
    {
        return;
    }
    public void TestIntConst(const int value = -1)
    {
        return;
    }
    public void TestUIntConst(const uint value = 67987)
    {
        return;
    }
    public void TestLongConst(const long value = long.MinValue)
    {
        return;
    }
    public void TestULongConst(const ulong value = long.MaxValue)
    {
        return;
    }
    public void TestFloatConst(const float value = 1.1f)
    {
        return;
    }
    public void TestDoubleConst(const double value = -1.1)
    {
        return;
    }
    public void TestDecimalConst(const decimal value = 1.1f)
    {
        return;
    }
    public void TestCharConst(const char value = '\t')
    {
        return;
    }
    public void TestStringConst(const string value = String.Empty)
    {
        return;
    }
    public void TestBoolConst(const bool value = true)
    {
        return;
    }
}
";
            var file = this.ParseFileExperimental(text, MessageID.IDS_FeatureConstParameters);

            VerifyTestPassed(text, file);
        }

        private static void VerifyTestPassed(string text, CompilationUnitSyntax file)
        {
            Assert.NotNull(file);
            Assert.Equal(text, file.ToString());
            var error = file.Errors().FirstOrDefault();
            Assert.False(file.HasErrors, $"Compiler Error {error?.MessageIdentifier}: {error?.MessageProvider.LoadMessage((int)error?.Code, System.Globalization.CultureInfo.InstalledUICulture)}");
            Assert.False(file.Warnings().Length > 0, $"Compiler Warning {file.Warnings().FirstOrDefault()?.MessageIdentifier}");
        }
    }
}
