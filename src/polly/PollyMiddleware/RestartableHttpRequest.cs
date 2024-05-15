// Copyright (c) SharpCrafters s.r.o. See the LICENSE.md file in the root directory of this repository root for details.

internal class RestartableHttpRequest : HttpRequest
{
    private readonly MemoryStream _requestStream = new();
    private readonly HttpRequest _underlying;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public RestartableHttpRequest( HttpContext httpContext, HttpRequest underlying )
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    {
        this._underlying = underlying;
        this.HttpContext = httpContext;
    }

    public override HttpContext HttpContext { get; }

    public override string Method { get; set; }

    public override string Scheme { get; set; }

    public override bool IsHttps { get; set; }

    public override HostString Host { get; set; }

    public override PathString PathBase { get; set; }

    public override PathString Path { get; set; }

    public override QueryString QueryString { get; set; }

    public override IQueryCollection Query { get; set; }

    public override string Protocol { get; set; }

    public override IHeaderDictionary Headers => this._underlying.Headers;

    public override IRequestCookieCollection Cookies { get; set; }

    public override long? ContentLength { get; set; }

    public override string? ContentType { get; set; }

    public override Stream Body { get; set; }

    public override bool HasFormContentType => this._underlying.HasFormContentType;

    public override IFormCollection Form { get; set; }

    public void Reset()
    {
        Copy( this._underlying, this );
        this.Body = this._requestStream;
        this._requestStream.Seek( 0, SeekOrigin.Begin );
    }

    public void Accept()
    {
        Copy( this, this._underlying );
    }

    public async Task ReadRequestAsync()
    {
        await this._underlying.Body.CopyToAsync( this._requestStream );
        this._requestStream.Seek( 0, SeekOrigin.Begin );

        if ( this._underlying.HasFormContentType )
        {
            this.Form = await this._underlying.ReadFormAsync();
        }
    }

    private static void Copy( HttpRequest from, HttpRequest to )
    {
        to.Method = from.Method;
        to.Scheme = from.Scheme;
        to.IsHttps = from.IsHttps;
        to.Host = from.Host;
        to.PathBase = from.PathBase;
        to.Path = from.Path;
        to.Query = from.Query;
        to.QueryString = from.QueryString;
        to.Protocol = from.Protocol;
        to.Cookies = from.Cookies;
        to.ContentLength = from.ContentLength;
        to.ContentType = from.ContentType;
    }

    public override Task<IFormCollection> ReadFormAsync( CancellationToken cancellationToken )
    {
        return Task.FromResult( this.Form );
    }
}