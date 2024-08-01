// Copyright (c) SharpCrafters s.r.o. Released under the MIT License.

namespace Memento.Step2;

[AttributeUsage( AttributeTargets.Field | AttributeTargets.Property )]
internal sealed class MementoIgnoreAttribute : Attribute { }