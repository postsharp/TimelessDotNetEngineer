using System.Security.Claims;
using Microsoft.AspNetCore.Http.Features;

namespace PollyMiddleware;

internal class BufferingHttpContext : HttpContext
{
    private readonly HttpContext _underlying;
    private readonly BufferingHttpResponse _response;
    private readonly BufferingHttpRequest _request;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public BufferingHttpContext(HttpContext underlying)
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    {
        this._underlying = underlying;
        this._response = new BufferingHttpResponse(this, underlying.Response);
        this._request = new BufferingHttpRequest(this, underlying.Request);
        this.Reset();
    }

    public Task InitializeAsync( CancellationToken cancellationToken )
    {
        return this._request.ReadRequestAsync();
    }

    public Task AcceptAsync()
    {
        this._request.Accept();
        return this._response.AcceptAsync();
    }
    
    public void Reset()
    {
        this.User = this._underlying.User;
        this.Items = new Dictionary<object, object?>(this._underlying.Items);
        this.RequestServices = this._underlying.RequestServices;
        this._response.Reset();
        this._request.Reset();
    }

    public override IFeatureCollection Features => _underlying.Features;
    public override HttpRequest Request => this._request;
    public override HttpResponse Response => this._response;
    public override ConnectionInfo Connection => _underlying.Connection;
    public override WebSocketManager WebSockets => _underlying.WebSockets;

    public override ClaimsPrincipal User
    {
        get;
        set;
    }

    public override IDictionary<object, object?> Items { get; set; }

    public override IServiceProvider RequestServices { get; set; }
   

    public override CancellationToken RequestAborted
    {
        get => _underlying.RequestAborted;
        set => _underlying.RequestAborted = value;
    }

    public override string TraceIdentifier
    {
        get => _underlying.TraceIdentifier;
        set => _underlying.TraceIdentifier = value;
    }

    public override ISession Session
    {
        get => this._underlying.Session;
        set => throw new NotSupportedException();
    }


    public override void Abort()
        => this._underlying.Abort();
}