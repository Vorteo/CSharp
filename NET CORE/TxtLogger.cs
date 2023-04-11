using System.Text.Json;

namespace cv6
{
    public interface ILogger
    {
        Task Log(string message);
    }

    public class TxtLogger : ILogger
    {
        public async Task Log(string message)
        {
            await File.AppendAllTextAsync("log.txt", message);
        }

    }

    public class JsonLogger : ILogger
    {
        public async Task Log(string message)
        {
            string jsonFile = "log.json";
            List<string> msgs;
            if(File.Exists(jsonFile))
            {
                string json = await File.ReadAllTextAsync(jsonFile);
                msgs = JsonSerializer.Deserialize<List<string>>(json);
            }
            else
            {
                msgs = new List<string>();
            }
            msgs.Add(message);
            await File.WriteAllTextAsync(jsonFile, JsonSerializer.Serialize(msgs));
        }
    }
}
