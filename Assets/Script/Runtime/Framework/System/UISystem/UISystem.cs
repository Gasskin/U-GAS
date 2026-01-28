using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Script.Runtime.Framework.ObjectPool;

namespace Script.Runtime.Framework.System
{
    public class UISystem : BaseSystem, ITickSystem
    {
        private Dictionary<ulong, UILogic> _uiLogics = new();
        private List<UILogic> _activeUiLogics = new();
        
        public override async UniTask Initialize()
        {
            await UniTask.Yield();
        }

        public override void Destroy()
        {
        }

        public void Tick(float dt)
        {
            for (int i = 0; i < _activeUiLogics.Count; i++)
            {
                _activeUiLogics[i].Tick(dt);
            }
        }

        public UILogic OpenWindow(UIConfig config)
        {
            //can multi spawn?
            var logic = Pool<UILogic>.Get();
            logic.Uid = UILogic.GetUid();
            _uiLogics.Add(logic.Uid, logic);
            
            return logic;
        }

        public void CloseWindow(ulong uid)
        {
            if (!_uiLogics.TryGetValue(uid,out var uiLogic))
            {
                return;
            }
            // is open
            if (uiLogic.IsActive)
            {
                uiLogic.IsActive = false;
                uiLogic.Close();
            }
            else
            {
                _uiLogics.Remove(uid);
                Pool<UILogic>.Release(uiLogic);
            }
        }

        private void InternalClose(UILogic logic)
        {
            
        }
    }
}