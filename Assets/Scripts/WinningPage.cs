using UnityEngine;

namespace DefaultNamespace
{
    public class WinningPage : MonoBehaviour
    {
        public Canvas Canvas;
        private void Start()
        {
            GameManager.Instance.OnWinGame += OnWinGame;
        }

        private void OnWinGame()
        {
            if (Canvas != null)
                Canvas.enabled = true;
        }

        private void OnDestroy()
        {
            GameManager.Instance.OnWinGame -= OnWinGame;
        }
    }
}