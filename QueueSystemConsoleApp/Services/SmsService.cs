using QueueSystemConsoleApp.ObserverDesignPattern;

namespace QueueSystemConsoleApp.Services;

public interface ISmsService
{
    Task Send(string userName);
}

public class SmsService : ISmsService,IUserSubscriber
{
    public async Task CreateUser(string userName)
    {
        await Send(userName);
    }

    public async Task Send(string userName)
    {    await Task.Delay(1000);
        Console.WriteLine($"Sms sent to {userName}");

    }
}