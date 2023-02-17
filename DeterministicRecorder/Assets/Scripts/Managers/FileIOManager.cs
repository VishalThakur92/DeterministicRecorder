using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

public class FileIOManager : MonoBehaviour
{


    //---For Testing Only
    //All files in the Location are displayed here, this lets user know what all files are there to load
    [SerializeField]
    Text allRecordingsText;
    string filesData;
    private void Update()
    {
        DirectoryInfo info = new DirectoryInfo(Application.persistentDataPath);
        var fileInfo = info.GetFiles();
        filesData = null;
        foreach (FileInfo file in fileInfo)
        {
            if (file.Name.Contains(".bin"))
            {
                filesData += file.Name.Split('.')[0] + "\n";
            }
        }

        allRecordingsText.text = filesData;
    }
    //-----------



    #region Core
    //Check if a file exists in the given directory
    public bool DoesFileExist(string filePath) {
        if (File.Exists(filePath))
            return true;
        else
            return false;
    }


    //Write Memory Stream Data to a file
    public void WriteMemorySteamToFile(MemoryStream memoryStream, string path)
    {
        using (FileStream file = new FileStream(path, FileMode.Create, FileAccess.ReadWrite))
        {
            memoryStream.WriteTo(file);
        }
    }

    //Write Memory Stream Data to a file
    public BinaryReader ReadSteamFromFile(string path)
    {
        using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
        {
            using (BinaryReader r = new BinaryReader(fs))
            {
                return r;
            }
        }
    }
    #endregion
}
