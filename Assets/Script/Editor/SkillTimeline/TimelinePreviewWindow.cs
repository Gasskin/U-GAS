using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace Script.Editor
{
    public class TimelinePreviewWindow : OdinEditorWindow
    {
        [MenuItem("Tools/Battle/TimelinePreview")]
        public static void ShowExample()
        {
            var wnd = GetWindow<TimelinePreviewWindow>();
            wnd.titleContent = new GUIContent("TimelinePreview");
            wnd.minSize = new Vector2(500, 200);
        }


        public GameObject HeroPrefab;

        public TimelineAsset Timeline;

        private GameObject _prefab;

        private GameObject _root;


        protected override void OnDisable()
        {
            base.OnDisable();
            DestroyImmediate(_root);
        }

        [Button]
        public void Refresh()
        {
            if (HeroPrefab == null || Timeline == null) 
            {
                return;
            }

            if (_root == null)
            {
                _root = new GameObject("TimelinePreviewRoot");
            }

            if (_prefab != null)
            {
                DestroyImmediate(_prefab);
            }

            _prefab = Instantiate(HeroPrefab, _root.transform, false);
            _prefab.transform.position = Vector3.zero;
            _prefab.SetActive(true);
            var animator = _prefab.GetComponentInChildren<Animator>(true);

            if (animator == null) 
            {
                DestroyImmediate(_prefab);
                return;
            }

            var pd = animator.gameObject.AddComponent<PlayableDirector>();
            pd.playableAsset = Timeline;
            
            foreach (var track in Timeline.GetRootTracks())
            {
                if (track is AnimationTrack)
                {
                    pd.SetGenericBinding(track, animator);
                }
            }

            Selection.activeGameObject = animator.gameObject;
        }
        
    }
}