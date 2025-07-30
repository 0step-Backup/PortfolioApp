using UnityEngine;

namespace LLOYD.Match3.Node
{
    using DG.Tweening;

    using LLOYD.Match3.Common;

    public class Gem : MonoBehaviour
    {
        Stage _stage = null;

        Defines.Gem _type = Defines.Gem.NONE;
        public Defines.Gem TYPE => _type;

        // Start is called before the first frame update
        void Start()
        {
        }

        public void Setup(Stage __stage, Defines.Gem __type, Sprite __sprite)
        {
            _stage = __stage;
            _type = __type;

            this.GetComponent<SpriteRenderer>().sprite = __sprite;
        }

        public void Move(Vector3 __pos)
        {
            this.transform.DOMove(__pos, 0.2f)
                .OnComplete(() => {
                    //Debug.Log($"{_type}");
                });
        }

        void OnMouseEnter() => _stage.Enter_Gem(this);
        void OnMouseExit() => _stage.Out_Gem(this);
        void OnMouseDown() => _stage.Push_Gem(this);
        void OnMouseUp() => _stage.Release_Gem(this);
    }
}
