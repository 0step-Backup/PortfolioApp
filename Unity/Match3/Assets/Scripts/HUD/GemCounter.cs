using UnityEngine;
using UnityEngine.UI;

namespace LLOYD.Match3.HUD.NODE
{
    public class GemCounter : MonoBehaviour
    {
        [SerializeField] Text TXT = null;

        int _count = 0;

        // Start is called before the first frame update
        void Start()
        {
            Set_Count(0);
        }

        /*public*/ void Set_Count(int __count)
        {
            _count = __count;
            TXT.text = __count.ToString();
        }

        public void Reset_Counting() => Set_Count(0);

        public void Counting() => Set_Count(_count + 1);
    }
}
