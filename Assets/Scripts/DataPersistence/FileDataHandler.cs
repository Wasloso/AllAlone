using System.IO;
using UnityEngine;

public class FileDataHandler
{
    private readonly string dataDirPath = "";


    public FileDataHandler(string dataDirPath)
    {
        this.dataDirPath = dataDirPath;
    }

    public GameData Load(string dataFileName)
    {
        var fullPath = Path.Combine(dataDirPath, dataFileName);

        GameData loadedData = null;

        if (File.Exists(fullPath))
            try
            {
                var dataToLoad = "";

                using (var stream = new FileStream(fullPath, FileMode.Open))
                {
                    using (var reader = new StreamReader(stream))
                    {
                        dataToLoad = reader.ReadToEnd();
                    }
                }

                loadedData = JsonUtility.FromJson<GameData>(dataToLoad);
            }
            catch
            {
                Debug.LogError("Error at loading game");
            }

        return loadedData;
    }

    public void Save(GameData data, string dataFileName)
    {
        var fullPath = Path.Combine(dataDirPath, dataFileName);

        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

            var dataToStore = JsonUtility.ToJson(data);

            using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                using (var writer = new StreamWriter(stream))
                {
                    writer.Write(dataToStore);
                }
            }
        }
        catch
        {
            Debug.LogError("Error at saving game");
        }
    }

    public void Delete(string dataFileName)
    {
        var fullPath = Path.Combine(dataDirPath, dataFileName);
        if (File.Exists(fullPath))
        {
            File.Delete(fullPath);
            Debug.Log($"Deleted save file at: {fullPath}");
        }
    }

    public bool SaveExists(string dataFileName)
    {
        var fullPath = Path.Combine(dataDirPath, dataFileName);
        return File.Exists(fullPath);
    }
}