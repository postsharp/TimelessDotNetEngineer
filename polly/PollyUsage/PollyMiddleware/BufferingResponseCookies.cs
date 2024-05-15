internal class BufferingResponseCookies : IResponseCookies
{
    private readonly List<Action<IResponseCookies>> _operations = new();

    public void Append(string key, string value)
    {
        _operations.Add(cookies => cookies.Append(key, value));
    }

    public void Append(string key, string value, CookieOptions options)
    {
        _operations.Add(cookies => cookies.Append(key, value, options));
    }

    public void Delete(string key)
    {
        _operations.Add(cookies => cookies.Delete(key));
    }

    public void Delete(string key, CookieOptions options)
    {
        _operations.Add(cookies => cookies.Delete(key, options));
    }

    public void Reset()
    {
        _operations.Clear();
    }

    public void Accept(IResponseCookies cookies)
    {
        foreach (var operation in _operations)
        {
            operation(cookies);
        }
    }
}