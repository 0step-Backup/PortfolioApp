using UnityEngine;

namespace LLOYD.Match3
{
    using AYellowpaper.SerializedCollections;
    using LLOYD.Match3.Common;

    public class Stage : MonoBehaviour
    {
        [SerializeField] GameObject PRFB_Gem = null;
        [SerializeField] Transform TRSF_Gems = null;

        [SerializedDictionary("Defines.Gem", "스프라이트")]
        [SerializeField]
        SerializedDictionary<Defines.Gem, Sprite> DICT_Gem_Sprites = null;

        bool _is_picked = false;
        Vector3 _cellv3_pick = Vector3.zero;

        // Start is called before the first frame update
        void Start()
        {
        }

        public void Add_Gem(Vector3Int __pos_cell, Addon.Tilemap.GemDesignValue __gemvalue)
        {
            Sprite sprite = null;

            var type = __gemvalue.type;
            var pos_world = __gemvalue.pos_wolrd;

            if (Defines.Gem.random == type)
            {
                type = (Defines.Gem)Random.Range(1, (int)Defines.Gem.yellow + 1);
            }

            sprite = DICT_Gem_Sprites[type];

            if (null != sprite)
            {
                var newgem = Instantiate(PRFB_Gem, TRSF_Gems);
                newgem.transform.position = pos_world;
                newgem.name = $"[{__pos_cell.x}, {__pos_cell.y}] {type}";

                newgem.GetComponent<Node.Gem>().Setup(this, type, sprite);
            }
        }

        public void Enter_Gem(Node.Gem __gem)
        {
            //Debug.Log($"<color=red>Stage.Enter_Gem</color>({__gem.transform.position})");

            if(_is_picked
                && _cellv3_pick != __gem.transform.position
                )
            {
                //if(인접한 블럭이면)
                //matching

                _is_picked = false;
                Debug.Log($"<color=cyan>target gem: {__gem.TYPE}</color>, ({_cellv3_pick.x}, {_cellv3_pick.y})");
            }
        }
        public void Out_Gem(Node.Gem __gem)
        {
            //Debug.Log($"<color=grey>Stage.Out_Gem</color>({__gem.transform.position})");
        }

        public void Push_Gem(Node.Gem __gem)
        {
            //Debug.Log($"<color=green>Stage.Push_Gem</color>({__gem.transform.position})");

            if (!_is_picked)
            {
                _is_picked = true;

                _cellv3_pick = __gem.transform.position;
                Debug.Log($"<color=green>pick gem: {__gem.TYPE}</color>, ({_cellv3_pick.x}, {_cellv3_pick.y})");
            }
        }

        public void Release_Gem(Node.Gem __gem)
        {
            //Debug.Log($"<color=black>Stage.Release_Gem</color>({__gem.transform.position})");

            if(_is_picked &&
                _cellv3_pick == __gem.transform.position)
            {
                _is_picked = false;
                Debug.Log($"<color=grey>pick release gem: {__gem.TYPE}</color>, ({_cellv3_pick.x}, {_cellv3_pick.y})");
            }
        }

        // Update is called once per frame
        void Update() {}
    }
}
