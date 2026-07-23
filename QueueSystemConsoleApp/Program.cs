using QueueSystemConsoleApp.ObserverDesignPattern;
using QueueSystemConsoleApp.Services;

Console.WriteLine("Queue System");

var userPublisher= new UserPublisher();

userPublisher.Subscribe(new EmailService());
userPublisher.Subscribe(new SmsService());
userPublisher.Subscribe(new DiscountService());

var userService= new UserService(userPublisher);

userService.Create();


Console.ReadLine();