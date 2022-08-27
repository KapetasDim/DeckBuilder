using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public abstract class Load_Image
{
    public static Texture GetImage(string imageName)
    {
        Texture2D _texture = new Texture2D(2, 2);
        
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/Pokemon_Images/" + imageName;

        if (File.Exists(path))
        {
            byte[] dataByte = null;
            dataByte = File.ReadAllBytes(path);
            _texture.LoadImage(dataByte);
        }
        else
        {
            Debug.LogError("Path not found: " + path);
        }

        return _texture;
    }
}
