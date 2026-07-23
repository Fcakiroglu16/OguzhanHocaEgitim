namespace QueueSystemConsoleApp.ObserverDesignPattern;

public class UserPublisher
{
    private List<IUserSubscriber> _subscribers = new List<IUserSubscriber>();

    public void Subscribe(IUserSubscriber subscriber)
    {
        _subscribers.Add(subscriber);
    }

    public void Unsubscribe(IUserSubscriber subscriber)
    {
        _subscribers.Remove(subscriber);
    }

    public async Task CreateUser(string userName)
    {
        foreach (var subscriber in _subscribers)
        {
             subscriber.CreateUser(userName).ContinueWith(t =>
             {
                 if(t.IsFaulted)
                 {
                     Console.WriteLine($"Error occurred while notifying subscriber: {t.Exception}");
                 }
             });
        }
    }
}


public interface IUserSubscriber
{
    Task CreateUser(string userName);
}