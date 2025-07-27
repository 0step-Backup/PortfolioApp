using UnityEngine;
using UnityEngine.Tilemaps;

namespace LLOYD.Match3.Types
{
    /*
     * 참고: Unity Technologies "Gem Hunter Match"
     * class GemPlacerTile
     */

    using LLOYD.Match3.Common;

    [CreateAssetMenu(fileName = "GemTile", menuName = "LLOYD - Match3/Tile/Gem Tile")]
    public class GemTile : TileBase
    {
        [SerializeField] Sprite _sprite;
        [SerializeField] Defines.Gem _gem = Defines.Gem.NONE;

        public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
        {
            tileData.sprite = _sprite;
        }

        public override bool StartUp(Vector3Int position, ITilemap tilemap, GameObject go)
        {
            return base.StartUp(position, tilemap, go);
        }
    }
}
