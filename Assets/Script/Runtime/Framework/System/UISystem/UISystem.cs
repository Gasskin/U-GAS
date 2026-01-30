using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Script.Runtime.Framework.ObjectPool;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Script.Runtime.Framework.System
{
    public class OnUIOpenEvent : BaseEventMessage
    {
        public ulong Uid;
        public UIConfig Config;

        public override void Release()
        {
            Pool<OnUIOpenEvent>.Release(this);
        }
        
        public override void OnRelease()
        {
            Uid = 0;
            Config = null;
        }
    }


    public class UISystem : BaseSystem, ITickSystem
    {
        private const int UiLayerInterval = 50;

        // 所有的UI
        private Dictionary<ulong, UILogic> _uiLogics = new();

        // 激活的UI
        // private List<UILogic> _activeUiLogics = new();
        private Dictionary<EUILayer, List<UILogic>> _activeUiLogicDict = new();

        public override async UniTask Initialize()
        {
            var enums = Enum.GetNames(typeof(EUILayer));
            foreach (var e in enums)
            {
                Enum.TryParse(e, out EUILayer layer);
                _activeUiLogicDict.Add(layer, new List<UILogic>());
            }
            await UniTask.Yield();
        }

        public override void Destroy()
        {
        }

        public void Tick(float dt)
        {
            foreach (var uiLogics in _activeUiLogicDict.Values)
            {
                for (int i = 0; i < uiLogics.Count; i++)
                {
                    uiLogics[i].Tick(dt);
                }
            }
        }

        public UILogic OpenWindow(UIConfig config)
        {
            if (!config.CanMultiSpawn)
            {
                // 如果已经打开，则重新置顶
                var layer = _activeUiLogicDict[config.Layer];
                for (int i = 0; i < layer.Count; i++)
                {
                    if (layer[i].Config == config)
                    {
                        PopupUi(layer[i], layer);
                        SetLayerUiDepth(config.Layer);
                        RefreshLayerVisibility(layer);
                        return layer[i];
                    }
                }

                // 如果正在打开中，返回，不进行操作
                foreach (var uiLogicsValue in _uiLogics.Values)
                {
                    if (uiLogicsValue.Config == config)
                    {
                        return uiLogicsValue;
                    }
                }
            }

            // 否则加载新UI
            var uiLogic = Pool<UILogic>.Get();
            uiLogic.Uid = UILogic.GetUid();
            uiLogic.Config = config;
            _uiLogics.Add(uiLogic.Uid, uiLogic);
            LoadUIAsync(uiLogic).Forget();

            return uiLogic;
        }

        public void CloseWindow(ulong uid)
        {
            if (!_uiLogics.TryGetValue(uid, out var uiLogic))
            {
                return;
            }
            // 说明已经打开
            if (_activeUiLogicDict[uiLogic.Config.Layer].Remove(uiLogic))
            {
                uiLogic.Destroy();
                SetLayerUiDepth(uiLogic.Config.Layer);
                RefreshLayerVisibility(_activeUiLogicDict[uiLogic.Config.Layer]);
                Object.Destroy(uiLogic.Window.gameObject);
            }
            _uiLogics.Remove(uid);
            Pool<UILogic>.Release(uiLogic);
        }

        private async UniTaskVoid LoadUIAsync(UILogic uiLogic)
        {
            var root = SystemDriver.Instance.UIRoot.UIRoots.GetValueOrDefault(uiLogic.Config.Layer, null);
            // 层级异常
            if (root == null)
            {
                Debug.LogError($"layer is null: {uiLogic.Config.Layer}");
                Pool<UILogic>.Release(uiLogic);
                return;
            }
            var prefab = await SystemDriver.YooSystem.InitializeGameObjectAsync(root, uiLogic.Config.Path);
            // 资源异常
            if (prefab == null)
            {
                Pool<UILogic>.Release(uiLogic);
                return;
            }
            // 还没打开又被关了
            if (!_uiLogics.ContainsKey(uiLogic.Uid))
            {
                Object.Destroy(prefab);
                Pool<UILogic>.Release(uiLogic);
                return;
            }
            var wnd = prefab.GetComponent<BaseWindow>();
            // 没有挂载UI类
            if (wnd == null)
            {
                Object.Destroy(prefab);
                Pool<UILogic>.Release(uiLogic);
                return;
            }
            uiLogic.Window = wnd;
            prefab.gameObject.SetActive(true);
            prefab.transform.SetAsLastSibling();

            _activeUiLogicDict[uiLogic.Config.Layer].Add(uiLogic);
            SetLayerUiDepth(uiLogic.Config.Layer);
            RefreshLayerVisibility(_activeUiLogicDict[uiLogic.Config.Layer]);

            uiLogic.Create();

            var msg = Pool<OnUIOpenEvent>.Get();
            msg.Uid = uiLogic.Uid;
            msg.Config = uiLogic.Config;
            msg.Send();

            await UniTask.Yield();
        }

        private void PopupUi(UILogic target, List<UILogic> uiLogics)
        {
            uiLogics.Remove(target);
            uiLogics.Add(target);

            target.Window.transform.SetAsLastSibling();
        }

        // 从上往下找到第一个全屏界面，显示他和他上面的所有ui
        private void RefreshLayerVisibility(List<UILogic> layer)
        {
            var fullScreenIndex = -1;
            for (int i = layer.Count - 1; i >= 0; i--)
            {
                if (layer[i].Config.FullScreen)
                {
                    fullScreenIndex = i;
                    break;
                }
            }

            for (int i = 0; i < layer.Count; i++)
            {
                var logic = layer[i];

                // 没有全屏窗口，全部显示
                if (fullScreenIndex == -1)
                {
                    logic.Show();
                }
                else
                {
                    if (i >= fullScreenIndex)
                    {
                        logic.Show();
                    }
                    else
                    {
                        logic.Hide();
                    }
                }
            }
        }

        private void SetLayerUiDepth(EUILayer layer)
        {
            var root = SystemDriver.Instance.UIRoot.UIRoots.GetValueOrDefault(layer, null);
            if (root == null)
            {
                return;
            }
            var rootDepth = (int)layer;
            var targetLayer = _activeUiLogicDict[layer];
            for (int i = 0; i < targetLayer.Count; i++)
            {
                var depth = rootDepth + i * UiLayerInterval;

                targetLayer[i].Window.Canvas.overrideSorting = true;
                targetLayer[i].Window.Canvas.sortingOrder = depth;

                targetLayer[i].Window.OnDepthChange(depth);
            }
        }
    }
}