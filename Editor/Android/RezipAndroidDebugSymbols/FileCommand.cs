using System.IO;
using UnityEngine;

public static class FileCommand{

    public static void Cleanup(string path)
    {
        if (Directory.Exists(path))
        {
            Debug.Log($"Delete {path}");
            Directory.Delete(path, true);
        }

        if (File.Exists(path))
        {
            Debug.Log($"Delete {path}");
            File.Delete(path);
        }
    }
}