using UnityEngine;

namespace Tamagotchi
{
    public class Medicine : MonoBehaviour, IConsumable
    {
        public void Consume()
        {
            Destroy(this);

        }
    }
}