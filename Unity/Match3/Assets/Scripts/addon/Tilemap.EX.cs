
using UnityEngine;
using UnityEngine.Tilemaps;

namespace LLOYD.Match3.Addon
{
    namespace Tilemap
    {
        using LLOYD.Match3.Common;
        using LLOYD.Match3.Types;
        using System.Collections.Generic;

        public class GemDesignValue
        {
            public Defines.Gem type;
            public Vector3 pos_wolrd = Vector3.zero;
        }

        static class Extensions
        {
            public static void Loop_Tiles(this UnityEngine.Tilemaps.Tilemap __timlemap)
            {
                // 타일맵 전체 크기 가져오기
                BoundsInt bounds = __timlemap.cellBounds;
                TileBase[] allTiles = __timlemap.GetTilesBlock(bounds);

                // 모든 타일 순회
                int count = 0;
                for (int x = 0; x < bounds.size.x; x++)
                {
                    for (int y = 0; y < bounds.size.y; y++)
                    {
                        var tile = allTiles[x + y * bounds.size.x] as GemTile;
                        if (tile != null)
                        {
                            //if(!tile.name.Contains("Random"))
                                Debug.Log($"타일 위치: ({x}, {y}), 타일 이름: {tile.name} (Type: {tile.TYPE})");// 타일 처리 로직 (예: 타일 정보 출력)

                            count += 1;
                        }
                    }
                }
                Debug.Log($"Stage.Loop_Tiles(): gem 개수= {count}");
            }
            public static Dictionary<Vector3Int, Defines.Gem> Loop_Tiles_byGetTile(this UnityEngine.Tilemaps.Tilemap __timlemap)
            {
                // 타일맵 전체 크기 가져오기
                BoundsInt bounds = __timlemap.cellBounds;

                var ret = new Dictionary<Vector3Int, Defines.Gem>();

                // 모든 타일 순회
                int count = 0;
                for (int x = bounds.x; x < bounds.xMax; x++)
                {
                    for (int y = bounds.y; y < bounds.yMax; y++)
                    {
                        Vector3Int cellPosition = new Vector3Int(x, y, 0);
                        var tile = __timlemap.GetTile(cellPosition) as GemTile;

                        if (tile != null)
                        {
                            ////if (!tile.name.Contains("Random"))
                            //    Debug.Log($"타일 위치: ({x}, {y}), 타일 이름: {tile.name} (Type: {tile.TYPE})");// 타일 처리 로직 (예: 타일 정보 출력)
                            count += 1;

                            ret.Add(cellPosition, tile.TYPE);
                        }
                    }
                }
                //Debug.Log($"Stage.Loop_Tiles_byGetTile(): gem 개수= {count}");
                return ret;
            }

            public static Dictionary<Vector3Int, GemDesignValue> Loop_Tiles_byGrid(this UnityEngine.Tilemaps.Tilemap __timlemap, Grid __grid)
            {
                // 타일맵 전체 크기 가져오기
                BoundsInt bounds = __timlemap.cellBounds;
                //var grid = this.GetComponent<Grid>();

                var ret = new Dictionary<Vector3Int, GemDesignValue>();

                // 월드 좌표 순회 예시
                int count = 0;
                for (int x = bounds.xMin; x < bounds.xMax; x++)
                {
                    for (int y = bounds.yMin; y < bounds.yMax; y++)
                    {
                        Vector3Int cellPosition = new Vector3Int(x, y, 0);
                        Vector3 worldPosition = __grid.GetCellCenterWorld(cellPosition);

                        // 월드 좌표에서 타일 처리 로직
                        //string strlog = $"셀 위치: ({x}, {y}), 월드 위치: {worldPosition}";

                        var tile = __timlemap.GetTile(cellPosition) as GemTile;
                        if (tile != null)
                        {
                            //if (!tile.name.Contains("Random"))
                            //    Debug.Log($"셀 위치: ({x}, {y}), 월드 위치: {worldPosition}, 타일 이름: {tile.name} (Type: {tile.TYPE})");
                            count += 1;

                            var data = new GemDesignValue() {
                                type= tile.TYPE,
                                pos_wolrd= worldPosition
                            };
                            ret.Add(cellPosition, data);
                        }

                        //Debug.Log(strlog);
                    }
                }
                //Debug.Log($"Stage.Loop_Tiles_byGrid(): gem 개수= {count}");

                return ret;
            }

            ////World 좌표값에서 Grid 기준의 Cell 좌표값 리턴
            //public static Vector3Int Get_CellPosition_byWorldPosition(this UnityEngine.Tilemaps.Tilemap __timlemap, Vector3 __worldposition)
            //    => __timlemap.WorldToCell(__worldposition);

            public static void Erasse_Gem(this UnityEngine.Tilemaps.Tilemap __timlemap, Vector3Int __pos)
            {
                //TMAP_Gems.GetTile(new Vector3Int(0, 0, 0));
                __timlemap.SetTile(__pos, null);
            }
        }
    }
}
