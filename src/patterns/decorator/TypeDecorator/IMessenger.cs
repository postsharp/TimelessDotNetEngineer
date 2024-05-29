// Copyright (c) SharpCrafters s.r.o. See the LICENSE.md file in the root directory of this repository root for details.

// [<snippet IMessenger>]

public interface IMessenger
{
    void Send( Message message );

    public Message Receive();
}

// [<endsnippet IMessenger>]