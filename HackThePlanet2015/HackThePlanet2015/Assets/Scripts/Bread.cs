using UnityEngine;

namespace Tamagotchi
{
    public class Bread : MonoBehaviour, IConsumable
    {
        public void Consume()
        {
            Destroy(this);
        }
    }
}