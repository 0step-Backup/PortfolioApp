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
            Erasse_Gem(new Vector3Int(0, 0, 0));
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
