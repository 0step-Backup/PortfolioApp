using UnityEngine;

namespace LLOYD.Match3.Node
{
    using DG.Tweening;

    using LLOYD.Match3.Common;
    using System;

    public class Gem : MonoBehaviour
    {
        Stage _stage = null;

        Defines.Gem _type = Defines.Gem.NONE;
        public Defines.Gem TYPE => _type;

        SpriteRenderer _sprRenderer = null;

        bool _isMoving = false;
        public bool IsMoving => _isMoving;

        const float CrashTime = 0.2f;

        public enum NewType { NONE = -1,
            normal = 0, regen = 1,
        }

        private void Awake()
        {
            _sprRenderer = this.GetComponent<SpriteRenderer>();
        }

        // Start is called before the first frame update
        void Start()
        {
        }

        public void Setup(Stage __stage, Defines.Gem __type, Sprite __sprite)
        {
            _stage = __stage;
            _type = __type;

            _sprRenderer.sprite = __sprite;
        }

        public void Update_Name(Vector3Int __pos_cell)
        {
            this.name = $"[{__pos_cell.x}, {__pos_cell.y}] {_type}";
        }

        const float SwapMovingTime = 0.2f;
        public float Move(Vector3 __pos)
        {
            _isMoving = true;
            this.transform.DOMove(__pos, SwapMovingTime)
                //.SetEase(Ease.OutBack)
                .OnComplete(() => {
                    //Debug.Log($"{_type}");
                    _isMoving = false;
                });

            return SwapMovingTime;
        }
        public float Regen(Vector3 __pos, Action<Gem> __act)
        {
            float duration = SwapMovingTime;
            { duration = 0.2f; }//DEV TEST

            _sprRenderer.color = new Color(1f, 1f, 1f, 0.15f);
            var sequence = DOTween.Sequence();
            {
                sequence.Append(this.transform.DOMove(__pos, duration));
                sequence.Join(_sprRenderer.DOFade(1f, duration));

                //sequence.SetEase(Ease.OutExpo);
                //sequence.onComplete += __act;

                sequence.OnComplete(() => {
                    if (null != __act) __act(this);
                });
            }
            sequence.Play();

            return duration;
        }
        
        public float Crash()
        {
            var sequence = DOTween.Sequence();
            {
                sequence.Append(this.transform.DOScale(0f, CrashTime).SetEase(Ease.InBack));
                sequence.Join(_sprRenderer.DOFade(0f, CrashTime));

                //sequence.SetEase(Ease.OutExpo);

                sequence.OnComplete(() => {
                    Destroy(this.gameObject);
                });
            }
            sequence.Play();

            return CrashTime;
        }

        void OnMouseEnter() => _stage.Enter_Gem(this);
        void OnMouseExit() => _stage.Out_Gem(this);
        void OnMouseDown() => _stage.Push_Gem(this);
        void OnMouseUp() => _stage.Release_Gem(this);
    }
}
