using UnityEngine;
using UnityEngine.Tilemaps;

namespace LLOYD.Match3
{
    using Addon.Tilemap;
    using AYellowpaper.SerializedCollections;
    using LLOYD.Match3.Common;

    public class Stage : MonoBehaviour
    {
        [SerializeField] Tilemap TMAP_Gems = null;

        [SerializeField] Transform TRSF_Gems = null;

        [SerializedDictionary("Defines.Gem", "Prefab")] [SerializeField]
        SerializedDictionary<Defines.Gem, GameObject> DICT_PRFB_Tiles = null;

        // Start is called before the first frame update
        void Start()
        {
            Debug.Log($"Board.Start(): TMAP_Gems= {TMAP_Gems.GetUsedTilesCount()}");

            //TMAP_Gems.Loop_Tiles();//[Sprite_GreenItem] (3, 5)
            var gems = TMAP_Gems.Loop_Tiles_byGetTile();//[Sprite_GreenItem] (-1, 1)
            //TMAP_Gems.Loop_Tiles_byGrid(this.GetComponent<Grid>());//[Sprite_GreenItem] 셀 위치: (-1, 1), 월드 위치: (-0.50, 1.50, 0.00)

            //Debug.Log(gems.Count);
            TMAP_Gems.ClearAllTiles();
            {
                foreach (var gem in gems)
                {
                    GameObject prefab = null;

                    if (Defines.Gem.random == gem.Value)
                        prefab = DICT_PRFB_Tiles[Defines.Gem.random];
                    else
                        prefab = DICT_PRFB_Tiles[gem.Value];

                    var newgem = Instantiate(prefab, TRSF_Gems);
                    newgem.transform.position = new Vector3(gem.Key.x + 0.5f, gem.Key.y + 0.5f, 0f);
                    newgem.name = $"[{gem.Key.x}, {gem.Key.y}] {gem.Value}";
                }
            }

            //TMAP_Gems.Erasse_Gem(new Vector3Int(0, 1, 0));
            //TMAP_Gems.Erasse_Gem(new Vector3Int(1, 2, 0));
        }

        // Update is called once per frame
        void Update()
        {
        }
    }
}
