using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[Serializable]
public class MessageLogger
{
    public readonly string Path;

    [SerializeField] private List<string> m_TextLog;

    public MessageLogger(string path)
    {
        Path = path;
        m_TextLog = new List<string>();
        if (File.Exists(path))
        {
            Debug.Log(path + " already exists");
        }
        else
        {
            File.Create(path);
            Debug.Log(string.Format("File created at {0}", path));
        }
    }

    ~MessageLogger()
    {
        Write();
    }

    public void Clear()
    {
        Debug.Log("Cleared Log");
        File.WriteAllText(Path, String.Empty);
    }

    public void Log(string text)
    {
        m_TextLog.Add(string.Format("[{0}] {1}", DateTime.Now, text));
    }

    public void Write()
    {
        StreamWriter streamWriter = new StreamWriter(Path, true);
        foreach (string line in m_TextLog)
        {
            streamWriter.WriteLine(line);
        }
        streamWriter.Close();
    }
}