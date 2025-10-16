using UnityEditor;
using UnityEngine;

namespace U_GAS
{
    public class TestMagnitudeUAssetEditorTool
    {
        private const string _GEN_PATH = "GameEffect/Editor/Asset/Magnitude";

        private static string GenPath => UConst.UGAS_PATH + "/" + _GEN_PATH + "/NewTestMagnitudeUAsset.asset";
        
        [MenuItem("U-GAS/Game Effect/Magnitude/New TestMagnitudeUAsset")]
        public static void Create()
        {
            var so = ScriptableObject.CreateInstance<TestMagnitudeUAssetProvider>();
            AssetDatabase.CreateAsset(so, GenPath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Selection.activeObject = so;
        }
    }
    
    public class TestMagnitudeUAssetProvider : ModifierMagnitudeUAssetProvider, IUAssetProvider
    {
        public float k;
        
        public IUAsset GetUAsset()
        {
            var data = new TestMagnitude();
            data.k = k;
            return data;
        }
    }
}