using UnityEngine;

namespace Tamagotchi
{
    public class Player : MonoBehaviour
    {
        public static Player Instance;

        void OnEnable()
        {
            Instance = this;
        }

        void OnDisable()
        {
            Instance = null;
        }
    }
}