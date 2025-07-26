using UnityEngine;
using UnityEngine.Tilemaps;

namespace LLOYD.Match3
{
    public class Stage : MonoBehaviour
    {
        [SerializeField] Tilemap TMAP_Gems = null;

        // Start is called before the first frame update
        void Start()
        {
            //TMAP_Gems.ClearAllTiles();
            Debug.Log($"Board.Start(): TMAP_Gems= {TMAP_Gems.GetUsedTilesCount()}");

            Loop_Tiles();//[Sprite_GreenItem] (3, 5)
            Loop_Tiles_byGetTile();//[Sprite_GreenItem] (-1, 1)
            Loop_Tiles_byGrid();//[Sprite_GreenItem] 셀 위치: (-1, 1), 월드 위치: (-0.50, 1.50, 0.00)

            //Erasse_Gem(new Vector3Int(0, 0, 0));
        }

        void Loop_Tiles()
        {
            // 타일맵 전체 크기 가져오기
            BoundsInt bounds = TMAP_Gems.cellBounds;
            TileBase[] allTiles = TMAP_Gems.GetTilesBlock(bounds);

            // 모든 타일 순회
            int count = 0;
            for (int x = 0; x < bounds.size.x; x++)
            {
                for (int y = 0; y < bounds.size.y; y++)
                {
                    TileBase tile = allTiles[x + y * bounds.size.x];
                    if (tile != null)
                    {
                        // 타일 처리 로직 (예: 타일 정보 출력)
                        Debug.Log($"타일 위치: ({x}, {y}), 타일 이름: {tile.name}");

                        count += 1;
                    }
                }
            }
            Debug.Log($"Stage.Loop_Tiles(): gem 개수= {count}");
        }
        void Loop_Tiles_byGetTile()
        {
            // 타일맵 전체 크기 가져오기
            BoundsInt bounds = TMAP_Gems.cellBounds;

            // 모든 타일 순회
            int count = 0;
            for (int x = bounds.x; x < bounds.xMax; x++)
            {
                for (int y = bounds.y; y < bounds.yMax; y++)
                {
                    Vector3Int cellPosition = new Vector3Int(x, y, 0);
                    TileBase tile = TMAP_Gems.GetTile(cellPosition);

                    if (tile != null)
                    {
                        // 타일 처리 로직 (예: 타일 정보 출력)
                        Debug.Log($"타일 위치: ({x}, {y}), 타일 이름: {tile.name}");
                        count += 1;
                    }
                }
            }
            Debug.Log($"Stage.Loop_Tiles_byGetTile(): gem 개수= {count}");
        }
        void Loop_Tiles_byGrid()
        {
            // 타일맵 전체 크기 가져오기
            BoundsInt bounds = TMAP_Gems.cellBounds;
            var grid = this.GetComponent<Grid>();

            // 월드 좌표 순회 예시
            int count = 0;
            for (int x = bounds.xMin; x < bounds.xMax; x++)
            {
                for (int y = bounds.yMin; y < bounds.yMax; y++)
                {
                    Vector3Int cellPosition = new Vector3Int(x, y, 0);
                    Vector3 worldPosition = grid.GetCellCenterWorld(cellPosition);
                    

                    // 월드 좌표에서 타일 처리 로직
                    //string strlog = $"셀 위치: ({x}, {y}), 월드 위치: {worldPosition}";

                    TileBase tile = TMAP_Gems.GetTile(cellPosition);
                    if (tile != null)
                    {
                        //strlog += $", 타일 이름: {tile.name}";
                        Debug.Log($"셀 위치: ({x}, {y}), 월드 위치: {worldPosition}, 타일 이름: {tile.name}");
                        count += 1;
                    }

                    //Debug.Log(strlog);
                }
            }
            Debug.Log($"Stage.Loop_Tiles_byGrid(): gem 개수= {count}");
        }

        void Erasse_Gem(Vector3Int __pos)
        {
            //TMAP_Gems.GetTile(new Vector3Int(0, 0, 0));
            TMAP_Gems.SetTile(__pos, null);
        }

        // Update is called once per frame
        void Update()
        {
        }
    }
}
