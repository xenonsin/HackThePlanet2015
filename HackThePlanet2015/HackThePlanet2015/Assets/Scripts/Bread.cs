using UnityEngine;

namespace Tamagotchi
{
    public class Bread : MonoBehaviour, IConsumable
    {
        public void Consume()
        {
                Pet.Instance.ModifyHunger(10);
                Destroy(this.gameObject);
        }
    }
}