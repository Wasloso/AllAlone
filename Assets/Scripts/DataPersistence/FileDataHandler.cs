
using System.IO;
using UnityEngine;

public class FileDataHandler
{
    private string dataDirPath = "";


    public FileDataHandler(string dataDirPath)
    {
        this.dataDirPath = dataDirPath;
    }

    public GameData Load(string dataFileName)
    {
        string fullPath = Path.Combine(dataDirPath, dataFileName);

        GameData loadedData = null;

        if (File.Exists(fullPath))
        {
            try
            {
                string dataToLoad = "";

                using (FileStream stream = new FileStream(fullPath,FileMode.Open))
                {
                    using (StreamReader reader = new StreamReader(stream))
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
        }
        return loadedData;
    }

    public void Save(GameData data, string dataFileName)
    {
        string fullPath = Path.Combine(dataDirPath, dataFileName);

        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

            string dataToStore = JsonUtility.ToJson(data);

            using (FileStream stream = new FileStream(fullPath, FileMode.Create))
            {
                using (StreamWriter writer = new StreamWriter(stream))
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
}