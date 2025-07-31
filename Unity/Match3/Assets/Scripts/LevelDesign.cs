using UnityEngine;
using UnityEngine.Tilemaps;

namespace LLOYD.Match3
{
    using Addon.Tilemap;

    public class LevelDesign : MonoBehaviour
    {
        [SerializeField] Tilemap TMAP_Gems = null;

        [SerializeField] Stage _stage = null;

        // Start is called before the first frame update
        void Start()
        {
            //Debug.Log($"Board.Start(): TMAP_Gems= {TMAP_Gems.GetUsedTilesCount()}");
            ////사용하는 타일 종류의 개수

            _stage.Setup(TMAP_Gems);

            Make_Gems();

            //TMAP_Gems.Erasse_Gem(new Vector3Int(0, 1, 0));
            //TMAP_Gems.Erasse_Gem(new Vector3Int(1, 2, 0));
        }

        void Make_Gems()
        {
            //TMAP_Gems.Loop_Tiles();//[Sprite_GreenItem] (3, 5)
            //var gems = TMAP_Gems.Loop_Tiles_byGetTile();//[Sprite_GreenItem] (-1, 1)
            var gems = TMAP_Gems.Loop_Tiles_byGrid(this.GetComponent<Grid>());//[Sprite_GreenItem] 셀 위치: (-1, 1), 월드 위치: (-0.50, 1.50, 0.00)

            //{ return; }//DEV TEST

            //Debug.Log(gems.Count);
            TMAP_Gems.ClearAllTiles();
            {
                foreach (var gem in gems)
                {
                    var type = gem.Value.type;
                    var pos = gem.Value.pos_wolrd;

                    _stage.Add_Gem(gem.Key, gem.Value);
                }
            }
        }

        // Update is called once per frame
        void Update()
        {
        }
    }
}
