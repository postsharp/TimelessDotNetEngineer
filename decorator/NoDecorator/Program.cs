var reportingService = new ExceptionReportingService();
var messenger = new Messenger(reportingService);
messenger.Send(new Message("Hello!"));
var response = messenger.Receive();
Console.WriteLine($"Received message: {response.Text}");