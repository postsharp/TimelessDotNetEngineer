﻿// Copyright (c) SharpCrafters s.r.o. See the LICENSE.md file in the root directory of this repository root for details.

using Metalama.Framework.Aspects;
using Metalama.Framework.Code;

[CompileTime]
internal static class SensitiveParameterFilter
{
    private static readonly string[] _sensitiveNames = new[] { "password", "credential", "pwd" };

    public static bool IsSensitive( IParameter parameter )
    {
        if ( parameter.Attributes.OfAttributeType( typeof(NotLoggedAttribute) ).Any() )
        {
            return true;
        }

        if ( _sensitiveNames.Any( n => parameter.Name.ToLowerInvariant().Contains( n ) ) )
        {
            return true;
        }

        return false;
    }
}