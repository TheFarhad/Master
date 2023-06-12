namespace Master.Utilities.Services.Abstraction.MessageBus;

public interface ISendMessageBus
{
    void Send(Parcel source);
    void SendCommand<TCommandData>(string destinationService, string commandName, TCommandData source);
    void SendCommandTo<TCommandData>(string destinationService, string commandName, string correlationId, TCommandData commandData);
    void Publish<TInput>(TInput input);
}
