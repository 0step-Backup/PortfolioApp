using UnityEngine;
using UnityEngine.Tilemaps;

namespace LLOYD.Match3
{
    using Addon.Tilemap;

    public class Stage : MonoBehaviour
    {
        [SerializeField] Tilemap TMAP_Gems = null;

        // Start is called before the first frame update
        void Start()
        {
            //TMAP_Gems.ClearAllTiles();
            Debug.Log($"Board.Start(): TMAP_Gems= {TMAP_Gems.GetUsedTilesCount()}");

            TMAP_Gems.Loop_Tiles();//[Sprite_GreenItem] (3, 5)
            TMAP_Gems.Loop_Tiles_byGetTile();//[Sprite_GreenItem] (-1, 1)
            TMAP_Gems.Loop_Tiles_byGrid(this.GetComponent<Grid>());//[Sprite_GreenItem] 셀 위치: (-1, 1), 월드 위치: (-0.50, 1.50, 0.00)

            //TMAP_Gems.Erasse_Gem(new Vector3Int(0, 1, 0));
            //TMAP_Gems.Erasse_Gem(new Vector3Int(1, 2, 0));
        }

        // Update is called once per frame
        void Update()
        {
        }
    }
}
