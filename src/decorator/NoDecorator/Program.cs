// Copyright (c) SharpCrafters s.r.o. See the LICENSE.md file in the root directory of this repository root for details.

var reportingService = new ExceptionReportingService();
var messenger = new Messenger( reportingService );
messenger.Send( new Message( "Hello!" ) );
var response = messenger.Receive();
Console.WriteLine( $"Received message: {response.Text}" );