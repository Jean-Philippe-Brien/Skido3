using UnityEngine;

namespace DefaultNamespace
{
    public class Ball : MonoBehaviour
    {
        private bool isHollTopTriggered = false;
        private Vector3 startPosition;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("HollTop"))
            {
                isHollTopTriggered = true;
            }
            else if (other.CompareTag("HollCenter"))
            {
                CheckIfPointValid();
            }
        }

        private void CheckIfPointValid()
        {
            if (isHollTopTriggered)
            {
                isHollTopTriggered = false;
                GameManager.Instance.MakePoint();
            }
        }

        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.CompareTag("FloorWall"))
            {
                isHollTopTriggered = false;
            }
        }
    }
}