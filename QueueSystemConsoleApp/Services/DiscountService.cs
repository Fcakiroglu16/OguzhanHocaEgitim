using QueueSystemConsoleApp.ObserverDesignPattern;

namespace QueueSystemConsoleApp.Services;

public class DiscountService:IUserSubscriber
{
    public async Task CreateUser(string userName)
    {
        
        await Task.Delay(3000);
        Console.WriteLine($"Discount applied to {userName}");
    }
}