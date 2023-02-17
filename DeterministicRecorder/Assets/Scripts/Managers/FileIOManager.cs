using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class FileIOManager : MonoBehaviour
{


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
        Debug.LogError("Write" + path);
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
