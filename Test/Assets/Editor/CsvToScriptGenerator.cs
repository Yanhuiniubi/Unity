using UnityEditor;
using UnityEngine;
using System.IO;
using System.Text;
using UnityEngine.UIElements;

public class CsvToScriptGenerator : EditorWindow
{
    private string outputFolder = "Scripts";

    [MenuItem("Tools/CsvToScript Generator")]
    public static void ShowWindow()
    {
        GetWindow<CsvToScriptGenerator>(false, "CsvToScript Generator", true);
    }

    private void OnGUI()
    {
        GUILayout.Label("CSV to Script Generator", EditorStyles.boldLabel);

        outputFolder = EditorGUILayout.TextField("Output Folder:", outputFolder);

        if (GUILayout.Button("Generate C# Scripts"))
        {
            GenerateScripts();
        }
    }

    private void GenerateScripts()
    {
        string streamingAssetsPath = Application.streamingAssetsPath;
        string[] csvFiles = Directory.GetFiles(streamingAssetsPath, "*.csv");

        if (csvFiles.Length == 0)
        {
            Debug.LogError("No CSV files found in StreamingAssets folder.");
            return;
        }

        foreach (string csvFilePath in csvFiles)
        {
            string csvContent = File.ReadAllText(csvFilePath);
            string outputFileName = "Table" + Path.GetFileNameWithoutExtension(csvFilePath);

            string[] records = csvContent.Split('\n');
            string[] fieldName = records[0].Split(',');
            string[] typeName = records[1].Split(',');

            StringBuilder sb = new StringBuilder();
            // ... 保留原有生成脚本代码 ...

            sb.AppendLine("using System.Collections.Generic;");
            sb.AppendLine("using System;");
            sb.AppendLine("using System.IO;\nusing System.Text;");
            sb.AppendLine();
            sb.AppendLine($"public class {outputFileName}");
            sb.AppendLine("{");

            for (int i = 0; i < fieldName.Length; i++)
            {
                sb.AppendLine($"    public {typeName[i].Substring(1, typeName[i].Length - 2)} {fieldName[i].Substring(1, fieldName[i].Length - 2)};");
            }
            sb.AppendLine("}");

            sb.AppendLine($"public class {outputFileName}Mod");
            sb.AppendLine("{");
            sb.AppendLine($"    private static string csvFilePath = \"{csvFilePath.Replace('\\','/')}\";");
            sb.AppendLine($"    private static Dictionary<{typeName[0].Substring(1, typeName[0].Length - 2)},{outputFileName}> dic = new Dictionary<{typeName[0].Substring(1, typeName[0].Length - 2)},{outputFileName}>();");
            sb.AppendLine($"    private static void Init()\r\n    {{\r\n        var csvContent = File.ReadAllText(csvFilePath);\r\n        " +
                $"string[] records = csvContent.Split('\\n');\r\n        for (int i = 2;i < records.Length;i++)\r\n        {{\r\n            " +
                $"{outputFileName} cfg = new {outputFileName}();\r\n            var values = records[i].Split(',');\r\n            " +
                $"for (int j = 0;j < values.Length;j++)\r\n                values[j] = values[j].Replace(\"\\\"\", \"\");");
            for (int index = 0;index < typeName.Length;index++)
            {
                if (typeName[index].IndexOf("int") != -1)
                    sb.AppendLine($"            cfg.{fieldName[index].Substring(1, fieldName[index].Length - 2)} = int.Parse(values[{index}]);");
                else if (typeName[index].IndexOf("float") != -1)
                    sb.AppendLine($"            cfg.{fieldName[index].Substring(1, fieldName[index].Length - 2)} = float.Parse(values[{index}]);");
                else if (typeName[index].IndexOf("string") != -1)
                    sb.AppendLine($"            cfg.{fieldName[index].Substring(1, fieldName[index].Length - 2)} = values[{index}];");
            }
            sb.AppendLine($"            dic.Add(cfg.ID, cfg);");
            sb.AppendLine("        }");
            sb.AppendLine("    }");
            sb.AppendLine($"    public static {outputFileName} Get(int id)\r\n    {{\r\n        if (dic.Count == 0)\r\n            Init();\r\n        if (dic.ContainsKey(id))\r\n            return dic[id];\r\n        return null;\r\n    }}");
            sb.AppendLine("}");

            string outputPath = Path.Combine(Application.dataPath, outputFolder, outputFileName + ".cs");
            File.WriteAllText(outputPath, sb.ToString());
            Debug.Log("C# script generated at: " + outputPath);
        }
        AssetDatabase.Refresh();
    }
}
