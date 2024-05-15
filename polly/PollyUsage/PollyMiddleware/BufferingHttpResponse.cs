internal class BufferingHttpResponse : HttpResponse
{
    private readonly BufferingResponseCookies _cookies = new();
    private readonly MemoryStream _memoryStream = new();
    private readonly HttpResponse _underlying;
    private bool _hasStarted;
    private IHeaderDictionary _headers = new HeaderDictionary();
    private string? _redirectLocation;
    private bool _redirectPermanent;

    public BufferingHttpResponse(HttpContext httpContext, HttpResponse underlying)
    {
        _underlying = underlying;
        HttpContext = httpContext;
    }

    public override HttpContext HttpContext { get; }
    public override int StatusCode { get; set; }

    public override IHeaderDictionary Headers => _headers;

    public override Stream Body
    {
        get => _memoryStream;
        set => throw new NotSupportedException();
    }

    public override long? ContentLength { get; set; }
    public override string? ContentType { get; set; }
    public override IResponseCookies Cookies => _cookies;

    public override bool HasStarted => _hasStarted;

    public override void OnStarting(Func<object, Task> callback, object state)
    {
        _hasStarted = true;
    }

    public override void OnCompleted(Func<object, Task> callback, object state)
    {
    }

    public override void Redirect(string location, bool permanent)
    {
        _redirectLocation = location;
        _redirectPermanent = permanent;
    }

    public void Reset()
    {
        _cookies.Reset();
        _memoryStream.Seek(0, SeekOrigin.Begin);
        ContentLength = _underlying.ContentLength;
        ContentType = _underlying.ContentType;
        StatusCode = _underlying.StatusCode;
        _headers = new HeaderDictionary();
    }

    public async Task AcceptAsync()
    {
        if (ContentLength != null)
        {
            _underlying.ContentLength = ContentLength;
        }

        if (ContentType != null)
        {
            _underlying.ContentType = ContentType;
        }

        _cookies.Accept(_underlying.Cookies);

        foreach (var header in Headers)
        {
            _underlying.Headers.Add(header);
        }

        await _memoryStream.CopyToAsync(_underlying.Body);

        if (_redirectLocation != null)
        {
            _underlying.Redirect(_redirectLocation, _redirectPermanent);
        }
    }
}