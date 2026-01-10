using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

public class TagNode
{
    public string backup;
    public string parentFullPath;
    public string tagName;
    public TagNode parent;
    public int index;
    public List<TagNode> childTag;
}

[CreateAssetMenu(fileName = "GameAbilitySystemConfig", menuName = "Gas/GameAbilitySystemConfig")]
public class GameAbilitySystemConfig : ScriptableObject
{
    public DefaultAsset gameTagRoot;
    public DefaultAsset gameTagGenCodeRoot;

    private readonly Dictionary<string, TagNode> s_TagDic = new();
    private readonly List<TagNode> s_TagTree = new();
    private int s_TagIdx;

    [Button]
    public void GenGameTag()
    {
        var path = AssetDatabase.GetAssetPath(gameTagRoot);
        var folder1 = Directory.GetDirectories(path, "*", SearchOption.AllDirectories);
        var folder2 = folder1.Select((s => s.Replace($"{path}\\", ""))).ToList();

        s_TagDic.Clear();
        s_TagTree.Clear();
        s_TagIdx = 1;

        CreateTagTree(folder2);
        foreach (var tagNode in s_TagTree)
        {
            SetTagIndex(tagNode);
        }

        GenTagEnum();
        GenTagRegister();
        
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    private void CreateTagTree(List<string> allPath)
    {
        // 创建所有节点
        foreach (var f in allPath)
        {
            var tagName = f;
            var parentFullPath = "";
            var idx = tagName.LastIndexOf("\\", StringComparison.Ordinal);
            if (idx != -1)
            {
                tagName = tagName.Substring(idx + 1);
                parentFullPath = f.Replace($"\\{tagName}", "");
            }
            var split = tagName.Split("@");
            var backup = split.Length > 1 ? split[1] : "空";
            var tag = new TagNode()
            {
                parentFullPath = parentFullPath,
                tagName = split[0],
                backup = backup,
                childTag = new List<TagNode>()
            };
            s_TagDic.Add(f, tag);
            // 说明是根节点
            if (string.IsNullOrEmpty(parentFullPath))
            {
                s_TagTree.Add(tag);
            }
        }
        // 添加父子关系
        foreach (var tag in s_TagDic.Values)
        {
            if (!string.IsNullOrEmpty(tag.parentFullPath))
            {
                var parent = s_TagDic[tag.parentFullPath];
                tag.parent = parent;
                parent.childTag.Add(tag);
            }
        }
    }

    private void SetTagIndex(TagNode tagNode)
    {
        tagNode.index = s_TagIdx++;
        foreach (var tag in tagNode.childTag)
        {
            SetTagIndex(tag);
        }
    }

    private void GenTagEnum()
    {
        var path = AssetDatabase.GetAssetPath(gameTagGenCodeRoot) + "/EGameTag.cs";
        if (File.Exists(path))
        {
            File.Delete(path);
        }
        
        var sb = new StringBuilder();
        sb.AppendLine("using Sirenix.OdinInspector;");
        // sb.AppendLine("namespace Meow.Runtime.HotUpdate");
        // sb.AppendLine("{");
        sb.AppendLine("\tpublic enum EGameTag");
        sb.AppendLine("\t{");
        sb.AppendLine($"\t\tNone = 0,");
        foreach (var tagNode in s_TagTree)
        {
            AddTag(tagNode);
        }
        sb.AppendLine("\t}");
        // sb.AppendLine("}");
        File.WriteAllText(path, sb.ToString());
        return;

        void AddTag(TagNode tagNode)
        {
            if (!string.IsNullOrEmpty(tagNode.backup))
            {
                var backup = GetTagFullBackup(tagNode, tagNode.backup);
                sb.AppendLine($"\t\t/// <summary>");
                sb.AppendLine($"\t\t/// \"{backup}\"");
                sb.AppendLine($"\t\t/// </summary>");
                sb.AppendLine($"\t\t[LabelText(\"{backup}\")]");
            }
            var fullName = GetTagFullName(tagNode, tagNode.tagName);
            sb.AppendLine($"\t\t{fullName} = {tagNode.index},");
            foreach (var tag in tagNode.childTag)
            {
                AddTag(tag);
            }
        }
    }

    private string GetTagFullName(TagNode tagNode, string tagName)
    {
        if (tagNode.parent == null)
        {
            return tagName;
        }
        return GetTagFullName(tagNode.parent, $"{tagNode.parent.tagName}_{tagName}");
    }

    private string GetTagFullBackup(TagNode tagNode, string backup)
    {
        if (tagNode.parent == null)
        {
            return backup;
        }
        return GetTagFullBackup(tagNode.parent, $"{tagNode.parent.backup}/{backup}");
    }

    private void GenTagRegister()
    {
        var path = AssetDatabase.GetAssetPath(gameTagGenCodeRoot) + "/GameTagRegister.cs";
        if (File.Exists(path))
        {
            File.Delete(path);
        }

        var sb = new StringBuilder();
        sb.AppendLine("using System;");
        sb.AppendLine("using System.Collections.Generic;");
        sb.AppendLine("// ReSharper disable InconsistentNaming");
        // sb.AppendLine("namespace Meow.Runtime.HotUpdate");
        // sb.AppendLine("{");
        sb.AppendLine("\tpublic static class GameTagRegister");
        sb.AppendLine("\t{");
        sb.AppendLine($"\t\tpublic static readonly int Size = {s_TagIdx};");
        sb.AppendLine("\t\tpublic static readonly int[] Tree =");
        sb.AppendLine("\t\t{");
        sb.AppendLine("\t\t\t0,\t// 0 Null");
        foreach (var tagNode in s_TagTree)
        {
            AddTree(tagNode);
        }
        sb.AppendLine("\t\t};");
        sb.AppendLine("\t\tpublic static readonly Dictionary<string, EGameTag> StringToEnum = new()");
        sb.AppendLine("\t\t{");
        foreach (var tagNode in s_TagDic.Values)
        {
            var fullName = GetTagFullName(tagNode, tagNode.tagName);
            sb.AppendLine($"\t\t\t{{ \"{fullName}\", EGameTag.{fullName} }},");
        }
        sb.AppendLine("\t\t};");


        var code = @"
#if UNITY_EDITOR
        static GameTagRegister()
        {
            if (Tree == null || Tree.Length != Size)
            {
                throw new Exception($""Tree.Length({Tree?.Length}) != Size({Size})"");
            }
            for (int i = 0; i < Tree.Length; i++)
            {
                int p = Tree[i];
                if (p < -1 || p >= Size)
                {
                    throw new Exception($""Illegal parent index: {i} -> {p}"");
                }
            }

            // 简单环检测
            var seen = new bool[Size];
            for (int i = 0; i < Size; i++)
            {
                Array.Clear(seen, 0, seen.Length);
                int cur = i;
                while (cur > 0)
                {
                    if (seen[cur])
                    {
                        throw new Exception($""Cycle detected at {i} (via {cur})"");
                    }
                    seen[cur] = true;
                    cur = Tree[cur];
                }
            }
        }
#endif";
        sb.AppendLine(code);
        sb.AppendLine("\t}");
        // sb.AppendLine("}");

        File.WriteAllText(path, sb.ToString());

        void AddTree(TagNode tagNode)
        {
            var parentIndex = tagNode.parent?.index ?? 0;
            sb.AppendLine($"\t\t\t{parentIndex},\t// {tagNode.index} {GetTagFullName(tagNode, tagNode.tagName)}");
            foreach (var tag in tagNode.childTag)
            {
                AddTree(tag);
            }
        }
    }
}