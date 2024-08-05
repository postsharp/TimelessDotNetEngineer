// Copyright (c) SharpCrafters s.r.o. Released under the MIT License.

// [<snippet IMessenger>]

public interface IMessenger
{
    void Send( Message message );

    public Message Receive();
}

// [<endsnippet IMessenger>]