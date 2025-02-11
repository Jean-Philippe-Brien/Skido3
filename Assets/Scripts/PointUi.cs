using TMPro;
using UnityEngine;

namespace DefaultNamespace
{
    public class PointUi : MonoBehaviour
    {
        public TextMeshPro tmpText;

        private void Start()
        {
            GameManager.Instance.OnMakePoint += OnMakePoint;
        }

        private void OnMakePoint(int point)
        {
            if (tmpText != null)
            {
                tmpText.text = point.ToString();
            }
        }
        
        private void OnDestroy()
        {
            GameManager.Instance.OnMakePoint -= OnMakePoint;
        }
    }
}