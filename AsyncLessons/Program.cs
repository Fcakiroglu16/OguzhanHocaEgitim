using System.Runtime.InteropServices.JavaScript;

Console.WriteLine("Hello, World!");

//Sync Programming 
// db operation => dbcontext.products.ToList();
// File I/O
// Network calls
// Managed/Unmanaged resources

// Async Programming
// db operation => dbcontext.products.ToListAsync();

//   async-await => pattern
// Task /ValueTask


// 1. example
var httpClient = new HttpClient();
await Call2();
Console.WriteLine("call2'den sonra çalışan kod");

Console.ReadLine();


foreach (var item in GetWithYield())
{
}


async Task Call()
{
    Console.WriteLine("Call Çalıştı");
    var httpResponseMessageAsTask = await httpClient.GetAsync("https://jsonplaceholder.typicode.com/todos/1"); // 10 sn


    var content = await httpResponseMessageAsTask.Content.ReadAsStringAsync(); // 1sn


    Console.WriteLine(content);
}

async Task Call2()
{
    Console.WriteLine("Call2 Çalıştı");
    httpClient.GetAsync("https://jsonplaceholde.typicode.com/todos/1").ContinueWith(async httpResponseMessageAsTask =>
    {
        if (httpResponseMessageAsTask.IsFaulted)
        {
            Console.WriteLine($"Hata oluştu: {httpResponseMessageAsTask.Exception?.Message}");
        }
        else
        {
            var content = await httpResponseMessageAsTask.Result.Content.ReadAsStringAsync();

            Console.WriteLine(content);
        }
    }); // 10 sn
}

async Task Call3()
{
    var httpResponse1 = httpClient.GetAsync("https://jsonplaceholde.typicode.com/todos/1");
    var httpResponse2 = httpClient.GetAsync("https://jsonplaceholde.typicode.com/todos/1");

    if (httpResponse1.IsCompletedSuccessfully)
    {
        var response1 = httpResponse1.Result;
    }
    else
    {
        Console.WriteLine(httpResponse1.Exception.Message);
    }
}

async Task Call4()
{
    CancellationTokenSource cts = new CancellationTokenSource();
    cts.CancelAfter(TimeSpan.FromSeconds(30));

    try
    {
        var httpResponse1 = httpClient.GetAsync("https://jsonplaceholde.typicode.com/todos/1", cts.Token);
    }
    catch (OperationCanceledException ex)
    {
        Console.WriteLine("Operation was canceled.");
    }


    var x = 1;

    if (x == 1)
    {
        cts.Cancel();
    }
}

async Task Call5()
{
    var httpResponse1 = httpClient.GetAsync("https://jsonplaceholde.typicode.com/todos/1");
    var httpResponse2 = httpClient.GetAsync("https://jsonplaceholde.typicode.com/todos/1");


    Task.WaitAll(httpResponse1, httpResponse2);

    var responseWhenAll = await Task.WhenAll(httpResponse1, httpResponse2);


    var responseWhenAny = await Task.WhenAny(httpResponse1, httpResponse2);

    var variableName = responseWhenAny == httpResponse1 ? nameof(httpResponse1) : nameof(httpResponse2);
    Console.WriteLine($"İlk tamamlanan: {variableName}");

    await foreach (var completedTask in Task.WhenEach(httpResponse1, httpResponse2))
    {
    }
}


List<string> GetNamesFromDb()
{
    return ["ali", "veli", "deli"];
}

IEnumerable<string> GetWithNoYield()
{
    List<string> names = GetNamesFromDb();

    List<string> newNames = new List<string>();

    foreach (var name in names)
    {
        if (name.StartsWith("a"))
        {
            newNames.Add(name);
        }
    }

    return newNames;
}

IEnumerable<string> GetWithYield()
{
    List<string> names = GetNamesFromDb();


    foreach (var name in names)
    {
        if (name.StartsWith("a"))
        {
            yield return name;
        }
    }
}





