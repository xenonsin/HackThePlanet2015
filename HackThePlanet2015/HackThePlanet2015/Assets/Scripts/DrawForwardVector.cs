using UnityEngine;

namespace Tamagotchi
{
    public class DrawForwardVector : MonoBehaviour
    {
        public float range = 0.5f;

        void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(transform.position, transform.position + transform.forward * range);
        }
    }
}