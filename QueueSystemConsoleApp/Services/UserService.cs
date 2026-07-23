using QueueSystemConsoleApp.ObserverDesignPattern;

namespace QueueSystemConsoleApp.Services;

public class UserService(UserPublisher userPublisher)
{
  //SRP  1 +2

    public   Task Create()
    {
        Console.WriteLine("user created.");
        // create user  =1 +1+1
        userPublisher.CreateUser("ahmet16");
        Console.WriteLine("user created2.");
        return Task.CompletedTask;

    }
}