// Copyright (c) SharpCrafters s.r.o. See the LICENSE.md file in the root directory of this repository root for details.

internal class RestartableHttpResponse : HttpResponse
{
    private readonly BufferingResponseCookies _cookies = new();
    private readonly MemoryStream _memoryStream = new();
    private readonly HttpResponse _underlying;
    private bool _hasStarted;
    private IHeaderDictionary _headers = new HeaderDictionary();
    private string? _redirectLocation;
    private bool _redirectPermanent;

    public RestartableHttpResponse( HttpContext httpContext, HttpResponse underlying )
    {
        this._underlying = underlying;
        this.HttpContext = httpContext;
    }

    public override HttpContext HttpContext { get; }

    public override int StatusCode { get; set; }

    public override IHeaderDictionary Headers => this._headers;

    public override Stream Body
    {
        get => this._memoryStream;
        set => throw new NotSupportedException();
    }

    public override long? ContentLength { get; set; }

    public override string? ContentType { get; set; }

    public override IResponseCookies Cookies => this._cookies;

    public override bool HasStarted => this._hasStarted;

    public override void OnStarting( Func<object, Task> callback, object state )
    {
        this._hasStarted = true;
    }

    public override void OnCompleted( Func<object, Task> callback, object state ) { }

    public override void Redirect( string location, bool permanent )
    {
        this._redirectLocation = location;
        this._redirectPermanent = permanent;
    }

    public void Reset()
    {
        this._cookies.Reset();
        this._memoryStream.Seek( 0, SeekOrigin.Begin );
        this.ContentLength = this._underlying.ContentLength;
        this.ContentType = this._underlying.ContentType;
        this.StatusCode = this._underlying.StatusCode;
        this._headers = new HeaderDictionary();
    }

    public async Task AcceptAsync()
    {
        if ( this.ContentLength != null )
        {
            this._underlying.ContentLength = this.ContentLength;
        }

        if ( this.ContentType != null )
        {
            this._underlying.ContentType = this.ContentType;
        }

        this._cookies.Accept( this._underlying.Cookies );

        foreach ( var header in this.Headers )
        {
            this._underlying.Headers.Add( header );
        }

        await this._memoryStream.CopyToAsync( this._underlying.Body );

        if ( this._redirectLocation != null )
        {
            this._underlying.Redirect( this._redirectLocation, this._redirectPermanent );
        }
    }
}