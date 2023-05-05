using System.IO;
using System.Threading.Tasks;
using UnityEngine;

public class LogManager : MonoBehaviour
{
    public static LogManager Instance 
    {
        get; private set;
    }
    public string Path
    {
        get
        {
            return Application.dataPath +"/Log.txt";
        }
    }

    private void Awake()
    {
        if(Instance == null)
            Instance = this;
        DontDestroyOnLoad(this);
        using var fileStream = new FileStream(Path, FileMode.Create);
        fileStream.Close();
        _ = SaveLog("Start Game");

    }
    public async Task SaveLog(string Input)
    {
        var input ="["+ System.DateTime.Now.ToString() + "] " + Input;
        Debug.Log(input);
        await using (var fileStream = new FileStream(Path, FileMode.Append))
        {
            await using (var stredmWriter = new StreamWriter(fileStream))
            {
                await stredmWriter.WriteLineAsync(input);
            }
        }
    }
    private void OnApplicationQuit()
    {
        _ = SaveLog("Exit Game");
    }
}
