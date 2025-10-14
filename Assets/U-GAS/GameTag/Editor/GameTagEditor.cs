using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace U_GAS.Editor
{
    public class TagNode
    {
        public string backup;
        public string parentFullPath;
        public string tagName;
        public TagNode parent;
        public int index;
        public List<TagNode> childTag;
    }

    [InitializeOnLoad]
    public static class GameTagEditor
    {
        private const string _TAG_PATH = "GameTag/Tags";
        private const string _GEN_TAG_PATH = "GameTag/Gen";

        private static string TagPath => UConst.UGAS_PATH + "/" + _TAG_PATH;
        private static string GenTagPath => UConst.UGAS_PATH + "/" + _GEN_TAG_PATH;

        private static readonly Dictionary<string, TagNode> _tagDic = new();
        private static readonly List<TagNode> _tagTree = new();
        private static int _tagIdx;

        private static string[] _gameTagValueDropdown;

        static GameTagEditor()
        {
            EditorApplication.projectWindowItemOnGUI += OnProjectWindowItemGUI;
        }

        private static void OnProjectWindowItemGUI(string guid, Rect rect)
        {
            if (string.IsNullOrEmpty(guid))
                return;

            if (Selection.assetGUIDs is not { Length: 1 } || Selection.assetGUIDs[0] != guid)
                return;

            string path = AssetDatabase.GUIDToAssetPath(guid);
            if (!AssetDatabase.IsValidFolder(path))
                return;

            if (!path.EndsWith(TagPath))
                return;

            // 列表视图：行右侧靠右画一个小按钮
            var btnRect = new Rect(rect.xMax - 65f, rect.y, 60, rect.height);
            if (GUI.Button(btnRect, "GenTag", EditorStyles.miniButton))
            {
                GenTag();
                AssetDatabase.Refresh();
            }
        }

        public static IEnumerable<string> GameTagValueDropdown()
        {
            if (_gameTagValueDropdown == null)
            {
                _gameTagValueDropdown = new string [GameTagRegister.Size];
                _gameTagValueDropdown[0] = "NULL";
                for (int i = 1; i < GameTagRegister.Size; i++)
                {
                    _gameTagValueDropdown[i] = ((EGameTag)i).ToString();
                }
            }
            return _gameTagValueDropdown;
        }

        private static void GenTag()
        {
            if (Directory.Exists(GenTagPath))
            {
                Directory.Delete(GenTagPath, true);
            }
            Directory.CreateDirectory(GenTagPath);


            var folder1 = Directory.GetDirectories(TagPath, "*", SearchOption.AllDirectories);
            var folder2 = folder1.Select((s => s.Replace($"{TagPath}\\", ""))).ToList();

            _tagDic.Clear();
            _tagTree.Clear();
            _tagIdx = 1;

            CreateTagTree(folder2);
            foreach (var tagNode in _tagTree)
            {
                SetTagIndex(tagNode);
            }

            GenTagEnum();
            GenTagTree();
        }

        private static void CreateTagTree(List<string> allPath)
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
                var backup = split.Length > 1 ? split[1] : "";
                var tag = new TagNode()
                {
                    parentFullPath = parentFullPath,
                    tagName = split[0],
                    backup = backup,
                    childTag = new List<TagNode>()
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
                if (!string.IsNullOrEmpty(tag.parentFullPath))
                {
                    var parent = _tagDic[tag.parentFullPath];
                    tag.parent = parent;
                    parent.childTag.Add(tag);
                }
            }
        }

        private static void CreateTagTree(string f)
        {
            var tags = f.Split("\\");
            // 创建Tag链上的所有Tag并建立父子关系
            for (int i = 0; i < tags.Length - 1; i++)
            {
                var parent = GetOrCreateTag(tags[i]);
                var child = GetOrCreateTag(tags[i + 1]);

                child.parent = parent;

                if (!parent.childTag.Contains(child))
                {
                    parent.childTag.Add(child);
                }

                // 第一个节点说明是根节点
                if (i == 0 && !_tagTree.Contains(parent))
                {
                    _tagTree.Add(parent);
                }
            }

            TagNode GetOrCreateTag(string tagFullName)
            {
                var param = tagFullName.Split("@");
                var tagName = param[0];
                var tagBackup = param.Length > 1 ? param[1] : "";

                if (!_tagDic.TryGetValue(tagName, out var tagNode))
                {
                    tagNode = new TagNode();
                    tagNode.childTag = new();
                    tagNode.tagName = tagName;
                    tagNode.backup = tagBackup;
                    _tagDic.Add(tagName, tagNode);
                }

                return tagNode;
            }
        }

        private static void SetTagIndex(TagNode tagNode)
        {
            tagNode.index = _tagIdx++;
            foreach (var tag in tagNode.childTag)
            {
                SetTagIndex(tag);
            }
        }

        private static void GenTagEnum()
        {
            var sb = new StringBuilder();
            sb.AppendLine("using Sirenix.OdinInspector;");
            sb.AppendLine("namespace U_GAS");
            sb.AppendLine("{");
            sb.AppendLine("\tpublic enum EGameTag");
            sb.AppendLine("\t{");
            foreach (var tagNode in _tagTree)
            {
                AddTag(tagNode);
            }
            sb.AppendLine("\t}");
            sb.AppendLine("}");

            File.WriteAllText(GenTagPath + "/EGameTag.cs", sb.ToString());

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

        private static string GetTagFullName(TagNode tagNode, string tagName)
        {
            if (tagNode.parent == null)
            {
                return tagName;
            }
            return GetTagFullName(tagNode.parent, $"{tagNode.parent.tagName}_{tagName}");
        }

        private static string GetTagFullBackup(TagNode tagNode, string backup)
        {
            if (tagNode.parent == null)
            {
                return backup;
            }
            return GetTagFullBackup(tagNode.parent, $"{tagNode.parent.backup}/{backup}");
        }


        private static void GenTagTree()
        {
            var sb = new StringBuilder();
            sb.AppendLine("using System.Collections.Generic;");
            sb.AppendLine("namespace U_GAS");
            sb.AppendLine("{");
            sb.AppendLine("\tpublic static class GameTagRegister");
            sb.AppendLine("\t{");
            sb.AppendLine($"\t\tpublic static readonly int Size = {_tagIdx};");
            sb.AppendLine("\t\tpublic static readonly int[] Tree =");
            sb.AppendLine("\t\t{");
            sb.AppendLine("\t\t\t0, // 0 Null");
            foreach (var tagNode in _tagTree)
            {
                AddTree(tagNode);
            }
            sb.AppendLine("\t\t};");
            sb.AppendLine("\t\tpublic static readonly Dictionary<string, EGameTag> String2Enum = new()");
            sb.AppendLine("\t\t{");
            foreach (var tagNode in _tagDic.Values)
            {
                var fullName = GetTagFullName(tagNode, tagNode.tagName);
                sb.AppendLine($"\t\t\t{{\"{fullName}\", EGameTag.{fullName}}},");
            }
            sb.AppendLine("\t\t};");
            sb.AppendLine("\t}");
            sb.AppendLine("}");

            File.WriteAllText(GenTagPath + "/GameTagRegister.cs", sb.ToString());

            void AddTree(TagNode tagNode)
            {
                var parentIndex = tagNode.parent?.index ?? 0;
                sb.AppendLine($"\t\t\t{parentIndex}, // {tagNode.index} {GetTagFullName(tagNode, tagNode.tagName)}");
                foreach (var tag in tagNode.childTag)
                {
                    AddTree(tag);
                }
            }
        }
    }
}