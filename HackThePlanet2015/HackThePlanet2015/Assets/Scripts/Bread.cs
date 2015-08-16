using UnityEngine;

namespace Tamagotchi
{
    public class Bread : MonoBehaviour, IConsumable
    {
        public void Consume()
        {
            Pet.Instance.ModifyHunger(5);
            Destroy(this.gameObject);
        }
    }
}