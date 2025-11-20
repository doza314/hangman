using System.Text.Json;

class APIHandler
{ 
  public async Task<string> getRandomWord()
  {
        using var client = new HttpClient();

        // download a JSON string like ["apple"]
        string json = await client.GetStringAsync("https://random-word-api.vercel.app/api?words=1");

        // convert JSON -> C# string[]
        string[] words = JsonSerializer.Deserialize<string[]>(json)!;

        return words[0];
  }
}
