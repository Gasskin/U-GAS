// using System;
// using System.IO;
// using System.Text;
// using ProtoBuf;
//
// namespace U_GAS.Editor
// {
//     public static class UAssetGenerator
//     {
//         public static void Gen(string path, IUAsset asset)
//         {
//             var assetInfo = asset.GetUAssetInfo();
//             var className = $"{assetInfo.ownerType}_{assetInfo.selfKey}";
//             var fileName = $"{className}.cs";
//             var filePath = $"{path}/{fileName}";
//             if (File.Exists(filePath))
//             {
//                 File.Delete(filePath);
//             }
//             
//             using var ms = new MemoryStream();
//             Serializer.Serialize(ms, asset);
//             var bytes = ms.ToArray();
//             var base64 = Convert.ToBase64String(bytes);
//             
//             var sb = new StringBuilder();
//             sb.Append(@"
// using System;
// using System.IO;
// using ProtoBuf;
// ");
//             sb.AppendLine("namespace U_GAS");
//             sb.AppendLine("{");
//             sb.AppendLine($"\tpublic class {className} : {assetInfo.ownerType}");
//             sb.AppendLine("\t{");
//             sb.AppendLine($"\t\tprivate const string _U_ASSET_DATA = \"{base64}\";");
//             sb.AppendLine($"\t\tprivate static byte[] _s_Bytes;");
//             sb.AppendLine($"\t\tstatic {className}()");
//             sb.AppendLine("\t\t{");
//             sb.AppendLine("\t\t\t_s_Bytes = Convert.FromBase64String(_U_ASSET_DATA);");
//             sb.AppendLine("\t\t}");
//             sb.AppendLine($"\t\tpublic {className}()");
//             sb.AppendLine("\t\t{");
//             sb.AppendLine($"\t\t\t{assetInfo.deSerializeField} = Serializer.Deserialize<{asset.GetType().Name}>(new MemoryStream(_s_Bytes));");
//             sb.AppendLine("\t\t\tOnInit();");
//             sb.AppendLine("\t\t}");
//             sb.AppendLine("\t}");
//             sb.AppendLine("}");
//             
//             File.WriteAllText(filePath, sb.ToString());
//         }
//     }
// }