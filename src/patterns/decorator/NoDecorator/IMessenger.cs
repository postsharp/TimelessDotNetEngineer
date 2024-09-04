// Copyright (c) SharpCrafters s.r.o. Released under the MIT License.

public interface IMessenger
{
    void Send( Message message );

    public Message Receive();
}