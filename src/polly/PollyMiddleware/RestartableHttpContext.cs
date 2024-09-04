// Copyright (c) SharpCrafters s.r.o. Released under the MIT License.

using System.Security.Claims;
using Microsoft.AspNetCore.Http.Features;

internal class RestartableHttpContext : HttpContext
{
    private readonly RestartableHttpRequest _request;
    private readonly RestartableHttpResponse _response;
    private readonly HttpContext _underlying;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public RestartableHttpContext( HttpContext underlying )
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    {
        this._underlying = underlying;
        this._response = new RestartableHttpResponse( this, underlying.Response );
        this._request = new RestartableHttpRequest( this, underlying.Request );
        this.Reset();
    }

    public override IFeatureCollection Features => this._underlying.Features;

    public override HttpRequest Request => this._request;

    public override HttpResponse Response => this._response;

    public override ConnectionInfo Connection => this._underlying.Connection;

    public override WebSocketManager WebSockets => this._underlying.WebSockets;

    public override ClaimsPrincipal User { get; set; }

    public override IDictionary<object, object?> Items { get; set; }

    public override IServiceProvider RequestServices { get; set; }

    public override CancellationToken RequestAborted
    {
        get => this._underlying.RequestAborted;
        set => this._underlying.RequestAborted = value;
    }

    public override string TraceIdentifier
    {
        get => this._underlying.TraceIdentifier;
        set => this._underlying.TraceIdentifier = value;
    }

    public override ISession Session
    {
        get => this._underlying.Session;
        set => throw new NotSupportedException();
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
        this.Items = new Dictionary<object, object?>( this._underlying.Items );
        this.RequestServices = this._underlying.RequestServices;
        this._response.Reset();
        this._request.Reset();
    }

    public override void Abort()
    {
        this._underlying.Abort();
    }
}