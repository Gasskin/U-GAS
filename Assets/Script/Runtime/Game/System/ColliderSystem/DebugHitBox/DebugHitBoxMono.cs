#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Script.Runtime.Game.ColliderSystem
{
    [ExecuteInEditMode]
    public class DebugHitBoxMono : MonoBehaviour
    {
    #region Menu
        private const string ShowHitBox = "Tools/Battle/ShowHitBox";
        public static bool NeedShowHitBox;

        [MenuItem(ShowHitBox)]
        private static void CommonSkillDamageMagnitude_Menu()
        {
            NeedShowHitBox = !NeedShowHitBox;
        }

        [MenuItem(ShowHitBox, true)]
        private static bool CommonSkillDamageMagnitude_Validate()
        {
            Menu.SetChecked(ShowHitBox, NeedShowHitBox);
            return true;
        }
    #endregion

    #region Static
        private static List<GameObject> _boxPool = new();

        private static GameObject _timelineBox;

        public static void ShowBoxHit(Vector2 pos, Vector2 size, float angle)
        {
            if (!NeedShowHitBox)
            {
                return;
            }
            var box = GetBox();
            box.transform.position = new Vector3(pos.x, pos.y, 0f);
            box.transform.localScale = new Vector3(size.x, size.y, 1f);
            box.transform.rotation = Quaternion.Euler(0f, 0f, angle);

            var mono = box.GetComponent<DebugHitBoxMono>();
            mono._aliveTime = AliveTime;
        }

        public static void TimelineShowBoxHit(Vector2 pos, Vector2 size, float angle)
        {
            var root = GameObject.Find("TimelinePreviewRoot");
            if (root == null) 
            {
                return;
            }
            _timelineBox ??= GetBox();
            _timelineBox.transform.SetParent(root.transform);
            _timelineBox.GetComponent<DebugHitBoxMono>()._isSingle = true;
            _timelineBox.transform.position = new Vector3(pos.x, pos.y, 0f);
            _timelineBox.transform.localScale = new Vector3(size.x, size.y, 1f);
            _timelineBox.transform.rotation = Quaternion.Euler(0f, 0f, angle);
        }

        public static void ClearSingleBoxHit()
        {
            if (_timelineBox != null)
            {
                DestroyImmediate(_timelineBox);
                _timelineBox = null;
            }
        }

        private static GameObject GetBox()
        {
            if (_boxPool.Count > 0)
            {
                var box = _boxPool[^1];
                box.SetActive(true);
                _boxPool.RemoveAt(_boxPool.Count - 1);
                return box;
            }
            var newBoxAsset = AssetDatabase.LoadAssetAtPath<GameObject>(BoxRedPath);
            var newBox = Instantiate(newBoxAsset);
            newBox.AddComponent<DebugHitBoxMono>();
            return newBox;
        }
    #endregion

        public const string BoxRedPath = "Assets/__EditorResources/DebugHitBox/HitBoxRed.prefab";
        public const float AliveTime = 0.5f;

        private bool _isSingle;
        private float _aliveTime;

        private void Update()
        {
            if (_isSingle)
            {
                return;
            }

            _aliveTime -= Time.deltaTime;
            if (_aliveTime <= 0f)
            {
                gameObject.SetActive(false);
                _boxPool.Add(gameObject);
            }
        }
    }
}
#endif