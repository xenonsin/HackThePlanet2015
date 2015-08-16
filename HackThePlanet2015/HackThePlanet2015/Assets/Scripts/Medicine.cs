using UnityEngine;

namespace Tamagotchi
{
    public class Medicine : MonoBehaviour, IConsumable
    {
        public void Consume()
        {
            Pet.Instance.ModifyHealth(10);
            Pet.Instance.ModifyHappiness(-5);
            Destroy(this.gameObject);

        }
    }
}