using UnityEngine.SceneManagement;

namespace LLOYD.Match3.HUD
{
    public partial class _Manager
    {
        public void DEV_OnClick_Reload()
        {
            SceneManager.LoadScene("1. Level 1");
        }
    }
}