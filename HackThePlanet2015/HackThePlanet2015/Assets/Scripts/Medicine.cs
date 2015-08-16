using UnityEngine;

namespace Tamagotchi
{
    public class Medicine : MonoBehaviour, IConsumable
    {
        public void Consume()
        {
            Pet.Instance.ModifyHealth(10);
            Destroy(this.gameObject);

        }
    }
}