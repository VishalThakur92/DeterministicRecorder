using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class FileIOManager : MonoBehaviour
{


    //Check if a file exists in the given directory
    public bool DoesFileExist(string directory, string fileName) {
        if (File.Exists(directory + "/"+ fileName))
            return true;
        else
            return false;
    }


    //Write Memory Stream Data to a file
    public void WriteMemorySteamToFile(MemoryStream memoryStream, string path) {
        using (FileStream file = new FileStream(path, FileMode.Create, FileAccess.ReadWrite))
        {
            memoryStream.WriteTo(file);
        }
    }

}
