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
    public string Backup;
    public string ParentFullPath;
    public string TagName;
    public TagNode Parent;
    public int Index;
    public List<TagNode> ChildTag;
}

[CreateAssetMenu(fileName = "GameAbilitySystemConfig", menuName = "Gas/GameAbilitySystemConfig")]
public class GameAbilitySystemConfig : ScriptableObject
{
    public DefaultAsset GameTagRoot;
    public DefaultAsset GameTagGenCodeRoot;

    private readonly Dictionary<string, TagNode> _tagDic = new();
    private readonly List<TagNode> _tagTree = new();
    private int _tagIdx;

    [Button]
    public void GenGameTag()
    {
        var path = AssetDatabase.GetAssetPath(GameTagRoot);
        var folder1 = Directory.GetDirectories(path, "*", SearchOption.AllDirectories);
        var folder2 = folder1.Select((s => s.Replace($"{path}\\", ""))).ToList();

        _tagDic.Clear();
        _tagTree.Clear();
        _tagIdx = 1;

        CreateTagTree(folder2);
        foreach (var tagNode in _tagTree)
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
                ParentFullPath = parentFullPath,
                TagName = split[0],
                Backup = backup,
                ChildTag = new List<TagNode>()
            };
            _tagDic.Add(f, tag);
            // 说明是根节点
            if (string.IsNullOrEmpty(parentFullPath))
            {
                _tagTree.Add(tag);
            }
        }
        // 添加父子关系
        foreach (var tag in _tagDic.Values)
        {
            if (!string.IsNullOrEmpty(tag.ParentFullPath))
            {
                var parent = _tagDic[tag.ParentFullPath];
                tag.Parent = parent;
                parent.ChildTag.Add(tag);
            }
        }
    }

    private void SetTagIndex(TagNode tagNode)
    {
        tagNode.Index = _tagIdx++;
        foreach (var tag in tagNode.ChildTag)
        {
            SetTagIndex(tag);
        }
    }

    private void GenTagEnum()
    {
        var path = AssetDatabase.GetAssetPath(GameTagGenCodeRoot) + "/EGameTag.cs";
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
        foreach (var tagNode in _tagTree)
        {
            AddTag(tagNode);
        }
        sb.AppendLine("\t}");
        // sb.AppendLine("}");
        File.WriteAllText(path, sb.ToString());
        return;

        void AddTag(TagNode tagNode)
        {
            if (!string.IsNullOrEmpty(tagNode.Backup))
            {
                var backup = GetTagFullBackup(tagNode, tagNode.Backup);
                sb.AppendLine($"\t\t/// <summary>");
                sb.AppendLine($"\t\t/// \"{backup}\"");
                sb.AppendLine($"\t\t/// </summary>");
                sb.AppendLine($"\t\t[LabelText(\"{backup}\")]");
            }
            var fullName = GetTagFullName(tagNode, tagNode.TagName);
            sb.AppendLine($"\t\t{fullName} = {tagNode.Index},");
            foreach (var tag in tagNode.ChildTag)
            {
                AddTag(tag);
            }
        }
    }

    private string GetTagFullName(TagNode tagNode, string tagName)
    {
        if (tagNode.Parent == null)
        {
            return tagName;
        }
        return GetTagFullName(tagNode.Parent, $"{tagNode.Parent.TagName}_{tagName}");
    }

    private string GetTagFullBackup(TagNode tagNode, string backup)
    {
        if (tagNode.Parent == null)
        {
            return backup;
        }
        return GetTagFullBackup(tagNode.Parent, $"{tagNode.Parent.Backup}/{backup}");
    }

    private void GenTagRegister()
    {
        var path = AssetDatabase.GetAssetPath(GameTagGenCodeRoot) + "/GameTagRegister.cs";
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
        sb.AppendLine($"\t\tpublic static readonly int s_Size = {_tagIdx};");
        sb.AppendLine("\t\tpublic static readonly int[] s_Tree =");
        sb.AppendLine("\t\t{");
        sb.AppendLine("\t\t\t0,\t// 0 Null");
        foreach (var tagNode in _tagTree)
        {
            AddTree(tagNode);
        }
        sb.AppendLine("\t\t};");
        sb.AppendLine("\t\tpublic static readonly Dictionary<string, EGameTag> s_StringToEnum = new()");
        sb.AppendLine("\t\t{");
        foreach (var tagNode in _tagDic.Values)
        {
            var fullName = GetTagFullName(tagNode, tagNode.TagName);
            sb.AppendLine($"\t\t\t{{ \"{fullName}\", EGameTag.{fullName} }},");
        }
        sb.AppendLine("\t\t};");


        var code = @"
#if UNITY_EDITOR
        static GameTagRegister()
        {
            if (s_Tree == null || s_Tree.Length != s_Size)
            {
                throw new Exception($""s_Tree.Length({s_Tree?.Length}) != s_Size({s_Size})"");
            }
            for (int i = 0; i < s_Tree.Length; i++)
            {
                int p = s_Tree[i];
                if (p < -1 || p >= s_Size)
                {
                    throw new Exception($""Illegal parent index: {i} -> {p}"");
                }
            }

            // 简单环检测
            var seen = new bool[s_Size];
            for (int i = 0; i < s_Size; i++)
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
                    cur = s_Tree[cur];
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
            var parentIndex = tagNode.Parent?.Index ?? 0;
            sb.AppendLine($"\t\t\t{parentIndex},\t// {tagNode.Index} {GetTagFullName(tagNode, tagNode.TagName)}");
            foreach (var tag in tagNode.ChildTag)
            {
                AddTree(tag);
            }
        }
    }
}