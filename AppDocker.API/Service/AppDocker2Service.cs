namespace AppDocker.API.Service
{
    public class AppDocker2Service(HttpClient client)
    {
        public async Task<string> CheckTest()
        {
            var result = await client.GetAsync("test");

            return await result.Content.ReadAsStringAsync();
        }
    }
}
