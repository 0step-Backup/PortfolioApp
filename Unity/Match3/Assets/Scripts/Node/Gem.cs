using UnityEngine;

namespace LLOYD.Match3.Node
{
    using LLOYD.Match3.Common;

    public class Gem : MonoBehaviour
    {
        Defines.Gem _type = Defines.Gem.NONE;
        public Defines.Gem TYPE => _type;

        // Start is called before the first frame update
        void Start()
        {
        }

        public void Setup(Defines.Gem __type, Sprite __sprite)
        {
            _type = __type;

            this.GetComponent<SpriteRenderer>().sprite = __sprite;
        }

        void OnMouseEnter()
        {
            //Debug.Log($"OnMouseEnter({_type})");
        }

        void OnMouseDown() => Debug.Log($"OnMouseDown({_type})");

        void OnMouseUp() => Debug.Log($"OnMouseUp({_type})");
    }
}
