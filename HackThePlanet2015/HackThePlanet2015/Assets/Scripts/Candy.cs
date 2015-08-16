using UnityEngine;

namespace Tamagotchi
{
    public class Candy : MonoBehaviour, IConsumable
    {
        public void Consume()
        {
            Destroy(this);

        }
    }
}