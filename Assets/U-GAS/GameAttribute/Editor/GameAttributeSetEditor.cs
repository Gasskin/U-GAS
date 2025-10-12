using System.Collections.Generic;
using System.IO;
using System.Text;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace U_GAS.Editor
{
    public class GameAttributeSetEditor : ScriptableObject
    {
        private const string _SERIALIZE_TARGET = "GameAttribute/Editor/GameAttributeSetEditor.asset";
        private const string _GEN_PATH = "GameAttribute/Gen";

        private static string SerializeTarget => UConst.UGAS_PATH + "/" + _SERIALIZE_TARGET;

        private static string GenEnumPath => UConst.UGAS_PATH + "/" + _GEN_PATH + "/" + "EGameAttributeSet.cs";
        private static string GenAttributeSetsPath => UConst.UGAS_PATH + "/" + _GEN_PATH + "/" + "GameAttributeSets.cs";

        private static string GenRegisterPath => UConst.UGAS_PATH + "/" + _GEN_PATH + "/" + "GameAttributeSetRegister.cs";

        [MenuItem("U-GAS/GameAttributeSet")]
        public static void Get()
        {
            var so = AssetDatabase.LoadAssetAtPath<GameAttributeSetEditor>(SerializeTarget);
            if (so == null)
            {
                so = CreateInstance<GameAttributeSetEditor>();
                AssetDatabase.CreateAsset(so, SerializeTarget);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }
            Selection.activeObject = so;
        }

        public List<BaseGameAttributeSet> attributeSets = new();

        [Button]
        public void Gen()
        {
            GenEnum();
            GenAttributes();
            GenRegister();

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        private void GenEnum()
        {
            if (File.Exists(GenEnumPath))
            {
                File.Delete(GenEnumPath);
            }

            var sb = new StringBuilder();
            sb.AppendLine("using Sirenix.OdinInspector;");
            sb.AppendLine("namespace U_GAS");
            sb.AppendLine("{");
            sb.AppendLine("\tpublic enum EGameAttributeSet");
            sb.AppendLine("\t{");
            foreach (var set in attributeSets)
            {
                sb.AppendLine("\t\t/// <summary>");
                sb.AppendLine($"\t\t/// {set.BackUp}");
                sb.AppendLine("\t\t/// </summary>");
                sb.AppendLine($"\t\t[LabelText(\"{set.BackUp}\")]");
                sb.AppendLine($"\t\t{set.Key},");
                sb.AppendLine("\t\t");
            }
            sb.AppendLine("\t}");
            sb.AppendLine("}");

            File.WriteAllText(GenEnumPath, sb.ToString());
        }

        private void GenAttributes()
        {
            if (File.Exists(GenAttributeSetsPath))
            {
                File.Delete(GenAttributeSetsPath);
            }

            var sb = new StringBuilder();
            sb.AppendLine("namespace U_GAS");
            sb.AppendLine("{");
            var attrSet = new HashSet<EGameAttribute>();
            foreach (var set in attributeSets)
            {
                sb.AppendLine($"\tpublic class GameAttributeSet_{set.Key} : BaseGameAttributeSet");
                sb.AppendLine("\t{");
                sb.AppendLine($"\t\tpublic GameAttributeSet_{set.Key}()");
                sb.AppendLine("\t\t{");
                sb.AppendLine("\t\t\tattributes = new()");
                sb.AppendLine("\t\t\t{");
                foreach (var selector in set.attributeSelectors)
                {
                    if (attrSet.Add(selector))
                    {
                        sb.AppendLine($"\t\t\t\t{{ EGameAttribute.{selector}, new GameAttribute_{selector}() }},");
                    }
                }
                sb.AppendLine("\t\t\t};");
                sb.AppendLine("\t\t}");
                sb.AppendLine("\t}");
            }
            sb.AppendLine("}");

            File.WriteAllText(GenAttributeSetsPath, sb.ToString());
        }

        private void GenRegister()
        {
            if (File.Exists(GenRegisterPath))
            {
                File.Delete(GenRegisterPath);
            }

            var sb = new StringBuilder();
            sb.AppendLine("using System;");
            sb.AppendLine("using System.Collections.Generic;");
            sb.AppendLine("namespace U_GAS");
            sb.AppendLine("{");
            sb.AppendLine("\tpublic static class GameAttributeSetRegister");
            sb.AppendLine("\t{");
            sb.AppendLine("\t\tpublic static BaseGameAttributeSet New(EGameAttributeSet set)");
            sb.AppendLine("\t\t{");
            sb.AppendLine("\t\t\tswitch (set)");
            sb.AppendLine("\t\t\t{");
            foreach (var set in attributeSets)
            {
                sb.AppendLine($"\t\t\t\tcase EGameAttributeSet.{set.Key}:");
                sb.AppendLine($"\t\t\t\t\treturn new GameAttributeSet_{set.Key}();");
            }
            sb.AppendLine("\t\t\t}");
            sb.AppendLine("\t\t\treturn null;");
            sb.AppendLine("\t\t}");
            sb.AppendLine("\t}");
            sb.AppendLine("}");

            File.WriteAllText(GenRegisterPath, sb.ToString());
        }
    }
}