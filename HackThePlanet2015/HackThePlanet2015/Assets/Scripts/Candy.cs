using UnityEngine;

namespace Tamagotchi
{
    public class Candy : MonoBehaviour, IConsumable
    {
        public void Consume()
        {
                Pet.Instance.ModifyHappiness(10);
                Pet.Instance.ModifyHealth(-4);
                Pet.Instance.ModifyHunger(1);
                Destroy(this.gameObject);
        }
    }
}