using UnityEngine;
using UnityEngine.SceneManagement;

namespace BallRunner.Core
{
    public sealed class BootLoader : MonoBehaviour
    {
        [SerializeField] private string nextScene = SceneIds.MainMenu;

        private void Start()
        {
            SceneManager.LoadScene(nextScene);
        }
    }
}
