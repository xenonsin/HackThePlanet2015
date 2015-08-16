using UnityEngine;
using UnityEngine.UI;

namespace Tamagotchi
{
    public class DataBindingGUIBar : MonoBehaviour
    {
        public Scrollbar HealthBar;
        public Scrollbar HungerBar;
        public Scrollbar HappinessBar;

        void Update()
        {
            HealthBar.size = Pet.Instance.health/100;
            HungerBar.size = Pet.Instance.hunger / 100;
            HappinessBar.size = Pet.Instance.happiness / 100;

            Canvas.ForceUpdateCanvases();
        }
    }
}