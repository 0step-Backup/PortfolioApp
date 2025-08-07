using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.Tilemaps;

namespace LLOYD.Match3
{
    using Addon.Tilemap;
    using LLOYD.Match3.Types;
    using LLOYD.Match3.Common;

    public class LevelDesign : MonoBehaviour
    {
        [SerializeField] Grid _grid = null;

        [SerializeField] Tilemap TMAP_Gems = null;
        [SerializeField] Tilemap TMAP_Guide = null;

        [SerializeField] Stage _stage = null;

        // Start is called before the first frame update
        void Start()
        {
            //Debug.Log($"Board.Start(): TMAP_Gems= {TMAP_Gems.GetUsedTilesCount()}");
            ////사용하는 타일 종류의 개수

            _stage.Setup(TMAP_Gems, Check_RegenCells());

            Make_Gems();

            //TMAP_Gems.Erasse_Gem(new Vector3Int(0, 1, 0));
            //TMAP_Gems.Erasse_Gem(new Vector3Int(1, 2, 0));
        }

        void Make_Gems()
        {
            //TMAP_Gems.Loop_Tiles();//[Sprite_GreenItem] (3, 5)
            //var gems = TMAP_Gems.Loop_Tiles_byGetTile();//[Sprite_GreenItem] (-1, 1)            
            var gems = TMAP_Gems.Loop_Tiles_byGrid(_grid, typeof(GemTile));//[Sprite_GreenItem] 셀 위치: (-1, 1), 월드 위치: (-0.50, 1.50, 0.00)

            //{ return; }//DEV TEST

            //Debug.Log(gems.Count);
            TMAP_Gems.ClearAllTiles();
            
            //랜덤 잼 생성으로 처음부터 매칭된 상태 안되게 하기 위해
            foreach (var gem in gems)
            {
                var gemdata = new Addon.Tilemap.GemDesignValue() {
                    type = Get_NotMacthedGem(gem.Key),
                    pos_wolrd = gem.Value.pos_wolrd
                };
                
                _stage.Add_Gem(gem.Key, gemdata);
            }
        }

        // 매칭되지 않는 안전한 젬 타입 선택
        private Defines.Gem Get_NotMacthedGem(Vector3Int __cell)
        {
            var availableTypes = new List<Defines.Gem> { 
                Defines.Gem.blue, Defines.Gem.green, Defines.Gem.purple, 
                Defines.Gem.red, Defines.Gem.white, Defines.Gem.yellow 
            };

            // 가로/세로 인접한 2개 젬과 같은 타입 제외
            var excludeTypes = new HashSet<Defines.Gem>();

            // 왼쪽 2개 연속 체크
            Check_ExcludeType(__cell, Vector3Int.left, Vector3Int.left * 2, excludeTypes);
            // 오른쪽 2개 연속 체크  
            Check_ExcludeType(__cell, Vector3Int.right, Vector3Int.right * 2, excludeTypes);
            // 아래 2개 연속 체크
            Check_ExcludeType(__cell, Vector3Int.down, Vector3Int.down * 2, excludeTypes);
            // 위 2개 연속 체크
            Check_ExcludeType(__cell, Vector3Int.up, Vector3Int.up * 2, excludeTypes);

            // 사용 가능한 타입 중 랜덤 선택
            var type = availableTypes.Where(type => !excludeTypes.Contains(type)).ToList();
            
            return type.Count > 0 
                ? type[Random.Range(0, type.Count)] 
                : Defines.Gem.blue; // 기본값
        }

        // 특정 방향의 2개 젬이 같은 타입인지 체크하여 제외 타입에 추가
        private void Check_ExcludeType(Vector3Int cell, Vector3Int dir1, Vector3Int dir2, HashSet<Defines.Gem> excludeTypes)
        {
            var cell1 = cell + dir1;
            var cell2 = cell + dir2;
            
            var gem1 = _stage.Get_GemAt(cell1);
            var gem2 = _stage.Get_GemAt(cell2);
            
            if (gem1 != null && gem2 != null && gem1.TYPE == gem2.TYPE)
            {
                excludeTypes.Add(gem1.TYPE);
            }
        }

        Dictionary<Vector3Int, Vector3> Check_RegenCells()
        {
            var regens = TMAP_Guide.Loop_Tiles_byGrid(_grid, typeof(TileBase));
            var ret = new Dictionary<Vector3Int, Vector3>();

            foreach (var item in regens)
            {
                var pos = item.Value.pos_wolrd;
                ret.Add(item.Key, pos);
            }

            TMAP_Guide.ClearAllTiles();
            return ret;
        }

        //// Update is called once per frame
        //void Update() {}
    }
}
