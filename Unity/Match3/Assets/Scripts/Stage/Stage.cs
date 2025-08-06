using System.Collections.Generic;
//using System.Linq;

using UnityEngine;
using UnityEngine.Tilemaps;

namespace LLOYD.Match3
{
    using AYellowpaper.SerializedCollections;

    using LLOYD.Match3.Common;

    //using Addon.Tilemap;

    public partial class Stage : MonoBehaviour
    {
        [SerializeField] GameObject PRFB_Gem = null;
        [SerializeField] Transform TRSF_Gems = null;
        [SerializeField] Transform TRSF_UnderWorld = null;
        [SerializeField] Transform TRSF_Regens = null;

        [SerializedDictionary("Defines.Gem", "스프라이트")]
        [SerializeField]
        SerializedDictionary<Defines.Gem, Sprite> DICT_Gem_Sprites = null;

        Tilemap _tilemap = null;

        Dictionary<Vector3Int, Node.Gem> _gems = new Dictionary<Vector3Int, Node.Gem>();
        Dictionary<Vector3Int, Vector3> _regenCells = null;

        Node.Gem _pickGem = null;

        BoundsInt _gridBounds = new BoundsInt();

        // Start is called before the first frame update
        void Start()
        {
        }

        public void Setup(Tilemap __tilemap, Dictionary<Vector3Int, Vector3> __regenCells)
        {
            _tilemap = __tilemap;

            _regenCells = __regenCells;
            //Debug.Log($"RegenCells Count: {__regenCells.Count}");

            // 그리드 바운드 설정
            _gridBounds = __tilemap.cellBounds;
            Debug.Log($"Stage Setup - Grid Bounds: {_gridBounds}, Size: {_gridBounds.size}");

            _gems.Clear();
        }

        public Node.Gem Add_Gem(Vector3Int __pos_cell, Addon.Tilemap.GemDesignValue __gemvalue
            , Node.Gem.NewType __newtype = Node.Gem.NewType.normal)
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
                Transform trsf = TRSF_Gems;
                if (Node.Gem.NewType.regen == __newtype)
                    trsf = TRSF_Regens;

                var newgem = Instantiate(PRFB_Gem, trsf);
                newgem.transform.position = pos_world;
                newgem.name = $"[{__pos_cell.x}, {__pos_cell.y}] {type}";

                var cs_newgem = newgem.GetComponent<Node.Gem>();
                cs_newgem.Setup(this, type, sprite);
                cs_newgem.Update_Name(__pos_cell);

                if(Node.Gem.NewType.normal == __newtype)
                    _gems.Add(__pos_cell, cs_newgem);

                //{
                //    //var pos_cell = _tilemap.WorldToCell(pos_world);
                //    pos_world = new Vector3(pos_world.x - 0.2f, pos_world.y + 0.2f);
                //    var pos_cell = _tilemap.Get_CellPosition_byWorldPosition(pos_world);
                //    Debug.Log($"[{pos_cell.x}, {pos_cell.y}] {type}: world position= {pos_world.x}, {pos_world.y}");
                //}
                return cs_newgem;
            }
            return null;
        }

        //// Update is called once per frame
        //void Update() {}
    }
}
