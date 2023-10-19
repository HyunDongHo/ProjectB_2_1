using UnityEngine;
using System;
using System.Text;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class CSVData
{
    public string fileName { get; private set; }
    private Dictionary<string, List<object>> fileArrays;

    public CSVData(string fileName, Dictionary<string, List<object>> value)
    {
        this.fileName = fileName;

        fileArrays = value;
    }

    public T GetFileData<T>(string dataName, int index)
    {
        return (T)fileArrays[dataName][index];
    }

    public int GetFileDataLength(string dataName)
    {
        return fileArrays[dataName].Count;
    }
}

public class CSVManager
{
    private const string LINE_SPLIT = @"\r\n|\n\r|\n|\r";
    private const string WORD_SPLIT = @",(?=(?:[^""]*""[^""]*"")*(?![^""]*""))";
    private const string DELIMITER = ",";

    private const string EMPTY_TEXT = "Empty";

    private static List<CSVData> csvDatas = new List<CSVData>();

    public static CSVData GetInEditor(TextAsset file)
    {
        return new CSVData(file.name, ConvertTextAssetToVariable(file));
    }

    public static CSVData Get(string file)
    {
        CSVData data = csvDatas.Find(csvData => csvData.fileName == file);

        if(data == null)
        {
            TextAsset textAsset = ResourceManager.instance.Load<TextAsset>(file);
            data = new CSVData(file, ConvertTextAssetToVariable(textAsset));
        }

        return data;
    }

    private static Dictionary<string, List<object>> ConvertTextAssetToVariable(TextAsset data)
    {
        Dictionary<string, List<object>> row = new Dictionary<string, List<object>>();
        string[] lines = Regex.Split(data.text, LINE_SPLIT);

        string[] keys = Regex.Split(lines[0], WORD_SPLIT);
        for (int i = 0; i < keys.Length; i++)
        {
            row.Add(keys[i], new List<object>());
        }

        List<string[]> values = new List<string[]>();
        for (int i = 1; i < lines.Length; i++)
        {
            values.Add(Regex.Split(lines[i], WORD_SPLIT));
        }

        for (int i = 0; i < values.Count; i++)
        {
            for (int j = 0; j < values[i].Length; j++)
            {
                object finalValue = values[i][j];

                if(string.IsNullOrEmpty(finalValue as string))
                {
                    continue;
                }
                else if((finalValue as string) == EMPTY_TEXT)
                {
                    finalValue = string.Empty;
                }
                else if (int.TryParse(finalValue as string, out int n))
                {
                    finalValue = n;

                }
                else if (float.TryParse(finalValue as string, out float f))
                {
                    finalValue = f;
                }

                row[keys[j]].Add(finalValue);
            }
        }

        return row;
    }
}