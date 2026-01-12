using System;
using System.IO;
using System.Text;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using Object = UnityEngine.Object;

[CreateAssetMenu(fileName = "InputSystemConfig", menuName = "Battle/Input/InputSystemConfig")]
public class InputSystemConfig : ScriptableObject
{
    public InputActionAsset InputAsset;
    public Object GenPath;

    [Button]
    public void Gen()
    {
        for (int i = 0; i < InputAsset.actionMaps.Count; i++)
        {
            GenOneMap(InputAsset.actionMaps[i]);
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    private void GenOneMap(InputActionMap map)
    {
        var path = AssetDatabase.GetAssetPath(GenPath) + "/" + map.name + "Map.cs";
        if (File.Exists(path))
        {
            File.Delete(path);
        }

        var sb = new StringBuilder();
        sb.AppendLine("using UnityEngine.InputSystem;");
        // sb.AppendLine("namespace Meow.Runtime.HotUpdate");
        // sb.AppendLine("{");
        sb.AppendLine($"    public partial class {map.name}Map : BaseMap");
        sb.AppendLine("    {");
        foreach (var action in map.actions)
        {
            sb.AppendLine($"       public InputAction {action.name} {{ get; private set; }}");
        }
        sb.AppendLine();
        sb.AppendLine($"        public {map.name}Map(InputActionAsset inputActionAsset, string inputMap) : base(inputActionAsset, inputMap)");
        sb.AppendLine($"        {{");
        foreach (var action in map.actions)
        {
            var field = action.name;
            sb.AppendLine($"            {field} = InputActionMap.FindAction(\"{action.name}\");");
        }
        foreach (var action in map.actions)
        {
            var field = action.name;
            sb.AppendLine($"            RegisterAction({field});");
        }
        sb.AppendLine("        }");
        sb.AppendLine("        public override void Dispose()");
        sb.AppendLine("        {");
        foreach (var action in map.actions)
        {
            var field = action.name;
            sb.AppendLine($"            UnRegisterAction({field});");
        }
        {
            sb.AppendLine($"            base.Dispose();");
        }
        sb.AppendLine("        }");
        sb.AppendLine("    }");
        // sb.AppendLine("}");

        File.WriteAllText(path, sb.ToString());
        AssetDatabase.Refresh();
    }
}