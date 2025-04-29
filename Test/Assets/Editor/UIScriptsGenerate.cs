using UnityEditor;
using UnityEngine;
using System.IO;
using System.Text;
using System;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;

public class UIScriptsGenerate /*: EditorWindow*/
{
    private static List<OneUIClass> uIClasses = new List<OneUIClass>();
    class OneUIClass
    {
        public StringBuilder sbClass = new StringBuilder();
        public StringBuilder sbField = new StringBuilder();
        public StringBuilder sbMethod = new StringBuilder();
    }
    private static HashSet<string> recordName;
    private static string outputFolder = "Scripts/UIBase";
    private static string rootName;
    [MenuItem("Tools/UIScript Generate")]
    public static void GenerateScripts()
    {
        if (Selection.gameObjects == null || Selection.gameObjects.Length == 0)
        {
            Debug.LogError("请选中一个UI物体的根节点");
            return;
        }

        string fullFolderPath = Path.Combine(Application.dataPath, outputFolder);

        // 确保目录存在
        if (!Directory.Exists(fullFolderPath)) 
        {
            Directory.CreateDirectory(fullFolderPath);
        }
        uIClasses = new List<OneUIClass>() { new OneUIClass() };
        recordName = new HashSet<string>();

        var sbClass = uIClasses[0].sbClass;
        var sbMethod = uIClasses[0].sbMethod;
        foreach (GameObject rootObj in Selection.gameObjects)
        {
            rootName = rootObj.name.Replace("-", "");
            sbClass.AppendLine("using System.Collections;");
            sbClass.AppendLine("using System.Collections.Generic;");
            sbClass.AppendLine("using UnityEngine;");
            sbClass.AppendLine("using UnityEngine.UI;");
            sbClass.AppendLine("using TMPro;");
            sbClass.AppendLine($"public class {rootName}Base : UILogicBase");
            sbClass.AppendLine("{");
            sbMethod.AppendLine("public override void OnInit()");
            sbMethod.AppendLine("{");
            sbMethod.AppendLine("base.OnInit();");
            StringBuilder path = new StringBuilder();
            StringBuilder methodPath = new StringBuilder();
            DFS(rootObj.transform, path, methodPath,0,true,true);
            int classCnt = uIClasses.Count;
            for (int i = 0;i < classCnt;i++)
            {
                uIClasses[i].sbMethod.AppendLine("}");
            }
            for (int i = 0; i < classCnt; i++)
            {
                uIClasses[i].sbClass.Append(uIClasses[i].sbField);
                uIClasses[i].sbClass.Append(uIClasses[i].sbMethod);
                uIClasses[i].sbClass.Append("}");
            }
            for (int i = 1; i < classCnt; i++)
            {
                uIClasses[0].sbClass.Append(uIClasses[i].sbClass);
            }

            string outputPath = Path.Combine(fullFolderPath, rootName + "Base.cs");
            File.WriteAllText(outputPath, uIClasses[0].sbClass.ToString());
            AssetDatabase.Refresh();
            Debug.Log("UI C# script generated at: " + outputPath);
        }
    }
    /// <summary>
    /// 递归生成字段和方法
    /// </summary>
    /// <param name="t"></param>
    /// <param name="fullPath"></param>
    private static void DFS(Transform t,StringBuilder fullPath, StringBuilder methodPath, int index,bool firstName,
        bool firstXiegang,bool preIsTemplate = false)
    {
        var sbClass = uIClasses[index].sbClass;
        var sbMethod = uIClasses[index].sbMethod;
        var sbField = uIClasses[index].sbField;

        fullPath.Append(t.name);
        if (firstName) firstName = false;
        else methodPath.Append(t.name);

        string curPathStr = fullPath.ToString();
        string methodPathStr = methodPath.ToString();
        if (!preIsTemplate)
        {
            if (t.name.StartsWith("e_"))
            {
                StringBuilder name = new StringBuilder($"{t.name.Replace("-", "").Replace(" ", "")}");
                while (recordName.Contains(name.ToString()))
                {
                    name.Append('1');
                }
                string onlyName = name.ToString();
                recordName.Add(onlyName);

                if (t.TryGetComponent<ToggleGroup>(out var component))
                {
                    sbField.AppendLine($"protected ToggleGroup {onlyName};//{curPathStr}");
                    sbMethod.AppendLine($"{onlyName} = GetUIComponentInchildren<ToggleGroup>(\"{methodPathStr}\");");
                }
                else if (t.TryGetComponent<Toggle>(out var toggle))
                {
                    sbField.AppendLine($"protected Toggle {onlyName};//{curPathStr}");
                    sbMethod.AppendLine($"{onlyName} = GetUIComponentInchildren<Toggle>(\"{methodPathStr}\");");
                }
                else if (t.TryGetComponent<Button>(out var btn))
                {
                    sbField.AppendLine($"protected Button {onlyName};//{curPathStr}");
                    sbMethod.AppendLine($"{onlyName} = GetUIComponentInchildren<Button>(\"{methodPathStr}\");");
                }
                else if (t.TryGetComponent<Slider>(out var slider))
                {
                    sbField.AppendLine($"protected Slider {onlyName};//{curPathStr}");
                    sbMethod.AppendLine($"{onlyName} = GetUIComponentInchildren<Slider>(\"{methodPathStr}\");");
                }
                else if (t.TryGetComponent<TMP_InputField>(out var inputField))
                {
                    sbField.AppendLine($"protected TMP_InputField {onlyName};//{curPathStr}");
                    sbMethod.AppendLine($"{onlyName} = GetUIComponentInchildren<TMP_InputField>(\"{methodPathStr}\");");
                }
                else if (t.TryGetComponent<Image>(out var img))
                {
                    sbField.AppendLine($"protected Image {onlyName};//{curPathStr}");
                    sbMethod.AppendLine($"{onlyName} = GetUIComponentInchildren<Image>(\"{methodPathStr}\");");
                }
                else if (t.TryGetComponent<TextMeshProUGUI>(out var txt))
                {
                    sbField.AppendLine($"protected TextMeshProUGUI {onlyName};//{curPathStr}");
                    sbMethod.AppendLine($"{onlyName} = GetUIComponentInchildren<TextMeshProUGUI>(\"{methodPathStr}\");");
                }
                else if (t.TryGetComponent<RectTransform>(out var rectTransform))
                {
                    sbField.AppendLine($"protected RectTransform {onlyName};//{curPathStr}");
                    sbMethod.AppendLine($"{onlyName} = GetUIComponentInchildren<RectTransform>(\"{methodPathStr}\");");
                }
                else
                {
                    sbField.AppendLine($"protected Transform {onlyName};//{curPathStr}");
                    sbMethod.AppendLine($"{onlyName} = GetUIComponentInchildren<Transform>(\"{methodPathStr}\");");
                }

            }
            else if (t.name.Equals("Template"))
            {
                uIClasses.Add(new OneUIClass());
                index = uIClasses.Count - 1;
                sbClass = uIClasses[index].sbClass;
                sbMethod = uIClasses[index].sbMethod;

                StringBuilder name = new StringBuilder($"{rootName}ContentBase");
                while (recordName.Contains(name.ToString()))
                {
                    name.Append('1');
                }
                string onlyName = name.ToString();
                recordName.Add(onlyName);

                sbClass.AppendLine($"public class {onlyName} : UITemplateBase");
                sbClass.AppendLine("{");
                sbMethod.AppendLine("public override void OnInit()");
                sbMethod.AppendLine("{");
                sbMethod.AppendLine("base.OnInit();");
                var p =  new StringBuilder(fullPath.ToString());
                p.Length -= 8;
                DFS(t, p, new StringBuilder(), index, true, true, true);
                fullPath.Length -= 8;
                methodPath.Length -= 8;
                return;
            }
        }

        int childCount = t.childCount;
        if (childCount > 0)
        {
            fullPath.Append("/");
            if (firstXiegang) firstXiegang = false;
            else methodPath.Append("/");
            for (int i = 0;i < childCount;i++)
            {
                DFS(t.GetChild(i), fullPath,methodPath,index,firstName,firstXiegang);
            }
            if (methodPath.Length - t.name.Length - 1 >= 0) methodPath.Length -= t.name.Length + 1;
            if (fullPath.Length - t.name.Length - 1 >= 0) fullPath.Length -= t.name.Length + 1;
        }
        else
        {
            if (methodPath.Length - t.name.Length >= 0) methodPath.Length -= t.name.Length;
            if (fullPath.Length - t.name.Length >= 0) fullPath.Length -= t.name.Length;
        }
    }
}
