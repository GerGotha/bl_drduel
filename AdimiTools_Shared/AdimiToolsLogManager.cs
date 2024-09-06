using System.Text;
using TaleWorlds.MountAndBlade;

namespace AdimiToolsShared;

internal class AdimiToolsLogManager
{
    private static AdimiToolsLogManager _current = default!;
    private readonly string _sanitizedServername = default!;

    public static AdimiToolsLogManager Instance
    {
        get
        {
            if (_current == null)
            {
                _current = new AdimiToolsLogManager();
            }

            return _current;
        }
    }

    public string LogPath { get; private set; }

    private AdimiToolsLogManager()
    {
        LogPath = Path.Combine("..", "..", "Logs");

        try
        {
            if (!Directory.Exists(LogPath))
            {
                Directory.CreateDirectory(LogPath);
                AdimiToolsConsoleLog.Log("Created log directory");
            }
            else
            {
                AdimiToolsConsoleLog.Log("Log directory already exists");
            }

            string servername = MultiplayerOptions.OptionType.ServerName.GetStrValue();

            if (servername == null)
            {
                return;
            }

            char[] invalids = Path.GetInvalidFileNameChars();
            string newName = string.Join("_", servername.Split(invalids, StringSplitOptions.RemoveEmptyEntries)).TrimEnd('.');
            _sanitizedServername = newName;
            string fullServerLogPath = Path.Combine(LogPath, _sanitizedServername!);
            if (!Directory.Exists(fullServerLogPath))
            {
                Directory.CreateDirectory(fullServerLogPath);
                AdimiToolsConsoleLog.Log("Created servername log directory");
            }
            else
            {
                AdimiToolsConsoleLog.Log("Servername log directory already exists");
            }
        }
        catch (Exception e)
        {
            AdimiToolsConsoleLog.Log(string.Empty);
            AdimiToolsConsoleLog.Log("An error occured!");
            AdimiToolsConsoleLog.Log(e.Message);
        }
    }

    public async Task Log(string message)
    {
        try
        {
            if (_sanitizedServername == null)
            {
                return;
            }

            DateTime now = DateTime.Now;
            string currentDatePrefix = $"{now:yyyy_MM_dd}";
            string fileName = $"serverlog_{currentDatePrefix}.txt";
            string outputPath = Path.Combine(LogPath, _sanitizedServername, fileName);
            string finalMsg = $"[{now:HH:mm:ss}]: {message}";
            using (FileStream stream = new(outputPath, FileMode.Append, FileAccess.Write, FileShare.None, bufferSize: 4096, useAsync: true))
            using (StreamWriter sw = new(stream, Encoding.UTF8))
            {
                await sw.WriteLineAsync(finalMsg);
            }

            AdimiToolsConsoleLog.Log(finalMsg, TaleWorlds.Library.Debug.DebugColor.Magenta);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Failed to log message: {ex.Message}");
        }
    }
}
