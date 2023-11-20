using UnityEngine;
using System.IO;

public static class FileWriter
{
    public static void WriteHighscoreToFile(int content)
    {
        string path = Path.Combine(Directory.GetCurrentDirectory(), "highscore.txt");
        // Use StreamWriter to write content to the file
        using (StreamWriter writer = new StreamWriter(path))
        {
            try { 
                writer.Write(content); 
            } 
            catch { }
        }
    }

    public static int ReadHighscoreFromFile()
    {
        string path = Path.Combine(Directory.GetCurrentDirectory(), "highscore.txt");
        string content;

        if (!File.Exists(path))
        {
            return 0;
        }

        using (StreamReader reader = new StreamReader(path))
        {
            content = reader.ReadToEnd();
        }

        if (int.TryParse(content, out int num))
        {
            return num;
        }
        else
        {
            return 0;
        }
    }
}