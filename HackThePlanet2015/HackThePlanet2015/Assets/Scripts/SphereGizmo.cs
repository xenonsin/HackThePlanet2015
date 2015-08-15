using UnityEngine;

namespace Tamagotchi
{
    public class SphereGizmo : MonoBehaviour
    {

        public float radius = 1.2f;
        void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, radius);
        }
    }
}