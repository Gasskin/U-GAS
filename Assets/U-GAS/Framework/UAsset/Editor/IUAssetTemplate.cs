using System;
using System.IO;
using System.Text;
using ProtoBuf;

namespace U_GAS
{
    public interface IUAssetTemplate
    {
        string GetGenPath();
        string GetSaveName();
        string GetSaveType();
    }

    public static class UAssetTemplate
    {
        public static void Gen(object o)
        {
            if (o is not IUAssetProvider provider)
            {
                return;
            }
            if (o is not IUAssetTemplate template)
            {
                return;
            }

            var data = provider.GetUAsset();
            using var ms = new MemoryStream();
            Serializer.Serialize(ms, data);
            var base64 = Convert.ToBase64String(ms.ToArray());

            var sb = new StringBuilder();
            sb.AppendLine("using System;");
            sb.AppendLine("using System.IO;");
            sb.AppendLine("using ProtoBuf;");
            sb.AppendLine("namespace U_GAS");
            sb.AppendLine("{");
            sb.AppendLine($"    public static class {template.GetSaveName()}Template");
            sb.AppendLine("    {");
            sb.AppendLine($"        private static string _base64 = \"{base64}\";");
            sb.AppendLine($"        private static byte[] _base64Bytes;");
            sb.AppendLine($"        public static {template.GetSaveType()} New()");
            sb.AppendLine("        {");
            sb.AppendLine($"            _base64Bytes ??= Convert.FromBase64String(_base64);");
            sb.AppendLine($"            using var ms = new MemoryStream(_base64Bytes);");
            sb.AppendLine($"            return Serializer.Deserialize<{template.GetSaveType()}>(ms);");
            sb.AppendLine("        }");
            sb.AppendLine("    }");
            sb.AppendLine("}");

            var path = template.GetGenPath() + "/" + template.GetSaveName() + "Template.cs";
            if (File.Exists(path))
            {
                File.Delete(path);
            }
            File.WriteAllText(path, sb.ToString());
        }
    }
}