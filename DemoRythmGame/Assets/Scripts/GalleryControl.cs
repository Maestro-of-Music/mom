using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class GalleryControl{

    public static Texture2D LoadTexture(string filePath){

        Texture2D tex = null;
        byte[] fileData;

        if(File.Exists(filePath)){
            fileData = File.ReadAllBytes(filePath);
            tex = new Texture2D(2, 2);
            tex.LoadImage(fileData);
            Debug.Log("Load Image! filePath : " + filePath);

        }
        return tex;
    }
}
