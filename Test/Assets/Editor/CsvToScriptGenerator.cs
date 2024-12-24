using UnityEditor;
using UnityEngine;
using System.IO;
using System.Text;
using UnityEngine.UIElements;
using System;
using OfficeOpenXml;

public class CsvToScriptGenerator /*: EditorWindow*/
{
    private static string outputFolder = "Scripts/Generate";

    //[MenuItem("Tools/CsvToScript Generator")]
    //public static void ShowWindow()
    //{
    //    GetWindow<CsvToScriptGenerator>(false, "CsvToScript Generator", true);
    //}

    //private void OnGUI()
    //{
    //    GUILayout.Label("CSV to Script Generator", EditorStyles.boldLabel);

    //    outputFolder = EditorGUILayout.TextField("Output Folder:", outputFolder);

    //    if (GUILayout.Button("Generate C# Scripts"))
    //    {
    //        GenerateScripts();
    //    }
    //}
    [MenuItem("Tools/CsvToScript Generator")]
    public static void GenerateScripts()
    {
        // 设置许可证上下文
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

        string streamingAssetsPath = Application.streamingAssetsPath;
        string[] excelFiles = Directory.GetFiles(streamingAssetsPath, "*.xlsx");

        foreach (string excelFilePath in excelFiles)
        {
            FileInfo excelFile = new FileInfo(excelFilePath);
            FileInfo csvFile = new FileInfo(excelFilePath.Replace(".xlsx",".csv"));

            using (ExcelPackage package = new ExcelPackage(excelFile))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets[0]; // 获取第一个工作表
                int totalRows = worksheet.Dimension.Rows;
                int totalCols = worksheet.Dimension.Columns;

                using (StreamWriter writer = new StreamWriter(csvFile.FullName))
                {
                    for (int row = 1; row <= totalRows; row++)
                    {
                        for (int col = 1; col <= totalCols; col++)
                        {
                            // 获取单元格值并写入 CSV 文件
                            object cellValue = worksheet.Cells[row, col].Value;
                            writer.Write(cellValue != null ? $"\"{cellValue.ToString()}\"" : "\"\"");

                            if (col < totalCols)
                            {
                                writer.Write(",");
                            }
                        }
                        if (row != totalRows)
                            writer.WriteLine();
                    }
                }
            }
        }

        string[] csvFiles = Directory.GetFiles(streamingAssetsPath, "*.csv");
        if (csvFiles.Length == 0)
        {
            Debug.LogError("No CSV files found in StreamingAssets folder.");
            return;
        }
        string folderPath = Path.Combine(Application.dataPath, outputFolder);

        if (Directory.Exists(folderPath))
        {
            string[] files = Directory.GetFiles(folderPath);

            foreach (string file in files)
            {
                try
                {
                    File.Delete(file);
                }
                catch (System.Exception ex)
                {
                    Debug.LogError($"Cannot delete file: {file}, Error: {ex.Message}");
                }
            }
        }
        foreach (string csvFilePath in csvFiles)
        {
            string csvContent = File.ReadAllText(csvFilePath);
            csvContent = csvContent.Replace("\r", "");
            string outputFileName = "Table" + Path.GetFileNameWithoutExtension(csvFilePath);

            string[] records = csvContent.Split('\n');
            string[] fieldName = records[0].Split(',');
            string[] typeName = records[1].Split(',');

            StringBuilder sb = new StringBuilder();
            // ... 保留原有生成脚本代码 ...

            sb.AppendLine("using System.Collections.Generic;");
            sb.AppendLine("using System;");
            sb.AppendLine("using System.IO;\nusing System.Text;");
            sb.AppendLine("using UnityEngine;");
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
            sb.AppendLine($"    private static string csvFilePath = Path.Combine(Application.streamingAssetsPath, \"{Path.GetFileNameWithoutExtension(csvFilePath)}.csv\");");
            sb.AppendLine($"    private static Dictionary<{typeName[0].Substring(1, typeName[0].Length - 2)},{outputFileName}> dic = new Dictionary<{typeName[0].Substring(1, typeName[0].Length - 2)},{outputFileName}>();");
            sb.AppendLine($"    private static {outputFileName}[] array;\r\n    public static {outputFileName}[] Array\r\n    {{\r\n        get\r\n        {{\r\n            if (array == null)\r\n                Init();\r\n            return array;\r\n        }}\r\n    }}");
            sb.AppendLine($"    private static void Init()\r\n    {{\r\n        var csvContent = File.ReadAllText(csvFilePath);\r\n        " +
                $"string[] records = csvContent.Split('\\n');\r\n        array = new {outputFileName}[records.Length - 2];\r\n        for (int i = 2;i < records.Length;i++)\r\n        {{\r\n            " +
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
            sb.AppendLine($"            dic.Add(cfg.ID, cfg);\r\n            array[i - 2] = cfg;");
            sb.AppendLine("        }");
            sb.AppendLine("    }");
            sb.AppendLine($"    public static {outputFileName} Get({typeName[0].Substring(1, typeName[0].Length - 2)} id)\r\n    {{\r\n        if (dic.Count == 0)\r\n            Init();\r\n        if (dic.ContainsKey(id))\r\n            return dic[id];\r\n        return null;\r\n    }}");
            sb.AppendLine("}");

            string outputPath = Path.Combine(Application.dataPath, outputFolder, outputFileName + ".cs");
            File.WriteAllText(outputPath, sb.ToString());
            Debug.Log("C# script generated at: " + outputPath);
        }
        AssetDatabase.Refresh();
    }
}
