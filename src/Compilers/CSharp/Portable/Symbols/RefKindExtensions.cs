﻿// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.Diagnostics;
using Roslyn.Utilities;

namespace Microsoft.CodeAnalysis.CSharp.Symbols
{
    internal static partial class RefKindExtensions
    {
        public static bool IsManagedReference(this RefKind refKind)
        {
            if (refKind == RefKind.Const)
            {
                return false;
            }

            Debug.Assert(refKind <= RefKind.RefReadOnly);

            return refKind != RefKind.None;
        }

        public static RefKind GetRefKind(this SyntaxKind syntaxKind)
        {
            switch (syntaxKind)
            {
                case SyntaxKind.RefKeyword:
                    return RefKind.Ref;
                case SyntaxKind.OutKeyword:
                    return RefKind.Out;
                case SyntaxKind.ConstKeyword:
                    return RefKind.Const;
                case SyntaxKind.None:
                    return RefKind.None;
                default:
                    throw ExceptionUtilities.UnexpectedValue(syntaxKind);
            }
        }
    }
}
