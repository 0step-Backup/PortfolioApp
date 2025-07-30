using UnityEngine;

namespace LLOYD.Match3
{
    using AYellowpaper.SerializedCollections;
    using LLOYD.Match3.Common;

    public class Stage : MonoBehaviour
    {
        [SerializeField] GameObject PRFB_Gem = null;

        [SerializedDictionary("Defines.Gem", "스프라이트")]
        [SerializeField]
        SerializedDictionary<Defines.Gem, Sprite> DICT_Gem_Sprites = null;

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
                ////prefab = DICT_PRFB_Tiles[Defines.Gem.random];
                //var rnd = Random.Range(1, (int)Defines.Gem.yellow + 1);
                //sprite = DICT_Gem_Sprites[(Defines.Gem)rnd];

                type = (Defines.Gem)Random.Range(1, (int)Defines.Gem.yellow + 1);
            }
            //else
            //    sprite = DICT_Gem_Sprites[type];

            sprite = DICT_Gem_Sprites[type];

            if (null != sprite)
            {
                var newgem = Instantiate(PRFB_Gem, this.transform);
                newgem.transform.position = pos_world;
                newgem.name = $"[{__pos_cell.x}, {__pos_cell.y}] {type}";

                newgem.GetComponent<Node.Gem>().Setup(type, sprite);
            }
        }

        // Update is called once per frame
        void Update() {}
    }
}
