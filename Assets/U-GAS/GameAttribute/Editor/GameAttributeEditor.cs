using System.Collections.Generic;
using System.IO;
using System.Text;
using PlasticPipe.PlasticProtocol.Messages;
using ProtoBuf;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace U_GAS.Editor
{
    public class GameAttributeEditor : ScriptableObject
    {
        private const string _SERIALIZE_TARGET = "GameAttribute/Editor/GameAttribute.asset";
        private const string _GEN_PATH = "GameAttribute/Gen";
        private static string SerializeTarget => UConst.UGAS_PATH + "/" + _SERIALIZE_TARGET;

        private static string GenEnumPath => UConst.UGAS_PATH + "/" + _GEN_PATH + "/" + "EGameAttribute.cs";

        private static string GenRegisterPath => UConst.UGAS_PATH + "/" + _GEN_PATH + "/" + "GameAttributeRegister.cs";

        [MenuItem("U-GAS/GameAttribute")]
        public static void Get()
        {
            var so = AssetDatabase.LoadAssetAtPath<GameAttributeEditor>(SerializeTarget);
            if (so == null)
            {
                so = CreateInstance<GameAttributeEditor>();
                AssetDatabase.CreateAsset(so, SerializeTarget);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }
            Selection.activeObject = so;
        }

        public List<GameAttributeUAsset> attribute;


        [Button]
        public void Gen()
        {
            GenEnum();
            foreach (var a in attribute)
            {
                UAssetGenerator.Gen(UConst.UGAS_PATH + "/" + _GEN_PATH,a);
            }
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
            sb.AppendLine("\tpublic enum EGameAttribute");
            sb.AppendLine("\t{");
            foreach (var attributeValue in attribute)
            {
                sb.AppendLine("\t\t/// <summary>");
                sb.AppendLine($"\t\t/// {attributeValue.backUp}");
                sb.AppendLine("\t\t/// </summary>");
                sb.AppendLine($"\t\t[LabelText(\"{attributeValue.backUp}\")]");
                sb.AppendLine($"\t\t{attributeValue.key},");
                sb.AppendLine("\t\t");
            }
            sb.AppendLine($"\t\tMax = {attribute.Count},");
            sb.AppendLine("\t}");
            sb.AppendLine("}");

            File.WriteAllText(GenEnumPath, sb.ToString());
        }

        // private void GenAttribute(GameAttribute attr)
        // {
        //     var path = UConst.UGAS_PATH + "/" + _GEN_PATH + $"/GameAttribute_{attr.Key}.cs";
        //     if (File.Exists(path))
        //     {
        //         File.Delete(path);
        //     }
        //
        //     var sb = new StringBuilder();
        //     sb.AppendLine("namespace U_GAS");
        //     sb.AppendLine("{");
        //     sb.AppendLine($"\tpublic class GameAttribute_{attr.Key} : GameAttribute");
        //     sb.AppendLine("\t{");
        //     sb.AppendLine($"\t\tpublic GameAttribute_{attr.Key}()");
        //     sb.AppendLine("\t\t{");
        //     sb.AppendLine($"\t\t\tminValue = {attr.MinValue}f;");
        //     sb.AppendLine($"\t\t\tmaxValue = {attr.MaxValue}f;");
        //     sb.AppendLine($"\t\t\teCalculateMode = ECalculateMode.{attr.ECalculateMode};");
        //     sb.AppendLine($"\t\t\tattributeType = EGameAttribute.{attr.Key};");
        //     sb.AppendLine("\t\t}");
        //     sb.AppendLine("\t}");
        //     sb.AppendLine("}");
        //
        //     File.WriteAllText(path, sb.ToString());
        // }

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
            sb.AppendLine("\tpublic static class GameAttributeRegister");
            sb.AppendLine("\t{");
            sb.AppendLine("\t\tpublic static GameAttribute New(EGameAttribute attribute)");
            sb.AppendLine("\t\t{");
            sb.AppendLine("\t\t\tswitch (attribute)");
            sb.AppendLine("\t\t\t{");
            foreach (var attr in attribute)
            {
                sb.AppendLine($"\t\t\t\tcase EGameAttribute.{attr.key}:");
                sb.AppendLine($"\t\t\t\t\treturn new GameAttribute_{attr.key}();");
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