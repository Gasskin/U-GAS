using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace U_GAS.Editor
{
    public class GameEffectEditor: ScriptableObject
    {
        private const string _GEN_PATH = "GameEffect/Editor";

        private static string GenPath => UConst.UGAS_PATH + "/" + _GEN_PATH + "/NewGameEffect.asset";

        public GameEffect gameEffect;
        
        [MenuItem("U-GAS/Game Effect/Create New")]
        public static void Create()
        {
            var so = CreateInstance<GameEffectEditor>();
            AssetDatabase.CreateAsset(so, GenPath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Selection.activeObject = so;
        }

        [MenuItem("U-GAS/Game Effect/Gen All")]
        public static void GenAll()
        {
            
        }
    }
}