using System.Security.Claims;
using Microsoft.AspNetCore.Http.Features;

internal class BufferingHttpContext : HttpContext
{
    private readonly BufferingHttpRequest _request;
    private readonly BufferingHttpResponse _response;
    private readonly HttpContext _underlying;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public BufferingHttpContext(HttpContext underlying)
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    {
        _underlying = underlying;
        _response = new BufferingHttpResponse(this, underlying.Response);
        _request = new BufferingHttpRequest(this, underlying.Request);
        Reset();
    }

    public override IFeatureCollection Features => _underlying.Features;
    public override HttpRequest Request => _request;
    public override HttpResponse Response => _response;
    public override ConnectionInfo Connection => _underlying.Connection;
    public override WebSocketManager WebSockets => _underlying.WebSockets;

    public override ClaimsPrincipal User { get; set; }

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
        get => _underlying.Session;
        set => throw new NotSupportedException();
    }

    public Task InitializeAsync(CancellationToken cancellationToken)
    {
        return _request.ReadRequestAsync();
    }

    public Task AcceptAsync()
    {
        _request.Accept();
        return _response.AcceptAsync();
    }

    public void Reset()
    {
        User = _underlying.User;
        Items = new Dictionary<object, object?>(_underlying.Items);
        RequestServices = _underlying.RequestServices;
        _response.Reset();
        _request.Reset();
    }


    public override void Abort()
    {
        _underlying.Abort();
    }
}