﻿// Copyright (c) SharpCrafters s.r.o. Released under the MIT License.

using Metalama.Framework.Aspects;

[assembly: AspectOrder( typeof(RetryAttribute), typeof(DbTransactionAttribute) )]