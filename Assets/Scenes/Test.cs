using U_GAS;
using UnityEngine;

namespace Scenes
{
    public class Test : MonoBehaviour
    {
        public EGameTag tag;
        
        private void Start()
        {
            var e = EntitySystem.Instance.CreateEntity();
        }
    }
}