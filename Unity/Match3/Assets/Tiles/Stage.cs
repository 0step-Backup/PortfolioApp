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
            TMAP_Gems.ClearAllTiles();
            Debug.Log($"Board.Start(): TMAP_Gems= {TMAP_Gems.GetUsedTilesCount()}");
        }

        // Update is called once per frame
        void Update()
        {
        }
    }
}
