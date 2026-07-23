using QueueSystemConsoleApp.ObserverDesignPattern;

namespace QueueSystemConsoleApp.Services;


public interface IEmailService
{
    Task Send(string userName);
}


public class EmailService : IEmailService,IUserSubscriber
{
    public Task Send(string userName)
    {
        Console.WriteLine($"Email sent to {userName}");
        return Task.CompletedTask;
    }

    public async Task CreateUser(string userName)
    {
        await Task.Delay(2000);
         await  Send(userName);
    }
}