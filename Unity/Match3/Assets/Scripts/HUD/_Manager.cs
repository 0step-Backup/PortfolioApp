using AYellowpaper.SerializedCollections;

using UnityEngine;
using UnityEngine.UI;

namespace LLOYD.Match3.HUD
{
    using LLOYD.Match3.Common;

    public partial class _Manager : MonoBehaviour
    {
        [SerializedDictionary("Defines.Gem", "GemCounter")]
        [SerializeField]
        SerializedDictionary<Defines.Gem, NODE.GemCounter> DICT_GemCounter = null;

        [SerializeField] Text TXT_SwapCounter = null;

        int _swapCount = 0;

        // Start is called before the first frame update
        void Start()
        {
            foreach (var gem in DICT_GemCounter.Values)
                gem.Reset_Counting();

            Reset_SwapCount();
        }

        #region [Gem Count]
        //public void Set_GemCount(Common.Defines.Gem __gem, int __count)
        //{
        //    if (DICT_GemCounter.ContainsKey(__gem))
        //        return;

        //    DICT_GemCounter[__gem].Set_Count(__count);
        //}
        public void Count_Gem(Common.Defines.Gem __gem)
        {
            if (DICT_GemCounter.ContainsKey(__gem))
                return;

            DICT_GemCounter[__gem].Counting();
        }
        public void Reset_GemCount(Common.Defines.Gem __gem)
        {
            if (DICT_GemCounter.ContainsKey(__gem))
                return;

            DICT_GemCounter[__gem].Reset_Counting();
        }
        #endregion

        #region [Swap Count]
        public void Set_SwapCount(int __count)
        {
            _swapCount = __count;
            TXT_SwapCounter.text = __count.ToString();
        }
        public void Reset_SwapCount() => Set_SwapCount(0);
        #endregion

        //// Update is called once per frame
        //void Update() {}
    }
}
