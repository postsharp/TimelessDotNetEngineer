﻿// Copyright (c) SharpCrafters s.r.o. See the LICENSE.md file in the root directory of this repository root for details.

public interface IExceptionReportingService
{
    void ReportException( string v, Exception e );
}