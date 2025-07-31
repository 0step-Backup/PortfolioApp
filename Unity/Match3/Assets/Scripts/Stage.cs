using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Tilemaps;

namespace LLOYD.Match3
{
    using AYellowpaper.SerializedCollections;

    using LLOYD.Match3.Common;    

    //using Addon.Tilemap;

    public class Stage : MonoBehaviour
    {
        [SerializeField] GameObject PRFB_Gem = null;
        [SerializeField] Transform TRSF_Gems = null;

        [SerializedDictionary("Defines.Gem", "스프라이트")]
        [SerializeField]
        SerializedDictionary<Defines.Gem, Sprite> DICT_Gem_Sprites = null;

        Tilemap _tilemap = null;

        Dictionary<Vector3Int, Vector3> _regenCells = null;

        Node.Gem _pickGem = null;

        // Start is called before the first frame update
        void Start()
        {
        }

        public void Setup(Tilemap __tilemap, Dictionary<Vector3Int, Vector3> __regenCells)
        {
            _tilemap = __tilemap;

            _regenCells = __regenCells;
            //{//DEV LOG
            //    string strlog = $"RegenCells {__regenCells.Count} 개";
            //    foreach (var item in __regenCells)
            //        strlog += $"\n\t [{item.Key.x}, {item.Key.y}] world 좌표 {item.Value.x}, {item.Value.y}";
            //    Debug.Log(strlog);
            //}
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

                //{
                //    //var pos_cell = _tilemap.WorldToCell(pos_world);
                //    pos_world = new Vector3(pos_world.x - 0.2f, pos_world.y + 0.2f);
                //    var pos_cell = _tilemap.Get_CellPosition_byWorldPosition(pos_world);
                //    Debug.Log($"[{pos_cell.x}, {pos_cell.y}] {type}: world position= {pos_world.x}, {pos_world.y}");
                //}
            }
        }

        //bool IsAdjacent(Vector3 pos1, Vector3 pos2) => Vector3.Distance(pos1, pos2) <= 1f;
        bool IsAdjacent(Node.Gem gem1, Node.Gem gem2)
        {
            var pos_cell1 = _tilemap.WorldToCell(gem1.transform.position);
            var pos_cell2 = _tilemap.WorldToCell(gem2.transform.position);

            bool isAdjacent = Mathf.Abs(pos_cell1.x - pos_cell2.x) +
                             Mathf.Abs(pos_cell1.y - pos_cell2.y) == 1;

            Debug.Log($"Adjacent Check: ({pos_cell1.x},{pos_cell1.y}) vs ({pos_cell2.x},{pos_cell2.y}) = {isAdjacent}");

            return isAdjacent;
        }

        public void Enter_Gem(Node.Gem __gem)
        {
            //Debug.Log($"<color=red>Stage.Enter_Gem</color>({__gem.transform.position})");

            if(_pickGem
                && _pickGem != __gem)
            {
                var pos_pick = _pickGem.transform.position;
                var pos_target = __gem.transform.position;

                if (!IsAdjacent(_pickGem, __gem))//인접한 블럭 체크
                    return;

                //matching
                _pickGem.Move(pos_target);
                __gem.Move(pos_pick);

                _pickGem = null;
                Debug.Log($"<color=cyan>target gem: {__gem.TYPE}</color>, ({__gem.transform.position.x}, {__gem.transform.position.y})");
            }
        }
        public void Out_Gem(Node.Gem __gem)
        {
            //Debug.Log($"<color=grey>Stage.Out_Gem</color>({__gem.transform.position})");
        }

        public void Push_Gem(Node.Gem __gem)
        {
            //Debug.Log($"<color=green>Stage.Push_Gem</color>({__gem.transform.position})");

            if (!_pickGem)
            {
                _pickGem = __gem;

                Debug.Log($"<color=green>pick gem: {__gem.TYPE}</color>, ({__gem.transform.position.x}, {__gem.transform.position.y})");
            }
        }

        public void Release_Gem(Node.Gem __gem)
        {
            //Debug.Log($"<color=black>Stage.Release_Gem</color>({__gem.transform.position})");

            if (_pickGem &&
                _pickGem == __gem)
            {
                _pickGem = null;
                Debug.Log($"<color=grey>pick release gem: {__gem.TYPE}</color>, ({__gem.transform.position.x}, {__gem.transform.position.y})");
            }
        }

        //// Update is called once per frame
        //void Update() {}
    }
}
