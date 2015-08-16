using UnityEngine;

namespace Tamagotchi
{
    public class Candy : MonoBehaviour, IConsumable
    {
        public void Consume()
        {
            Pet.Instance.ModifyHappiness(5);
            Pet.Instance.ModifyHealth(-2);
            Pet.Instance.ModifyHunger(1);
            Destroy(this.gameObject);

        }
    }
}