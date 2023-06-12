namespace Master.Utilities.Services.Abstraction.MessageBus;

public interface IReceiveMessageBus
{
    void Subscribe(string serviceId, string eventName);
    void Receive(string commandName);
}