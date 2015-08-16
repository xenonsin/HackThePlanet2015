using UnityEngine;

namespace Tamagotchi
{
    public class DrawForwardVector : MonoBehaviour
    {
        void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(transform.position, transform.position + transform.forward * 0.5f);
        }
    }
}