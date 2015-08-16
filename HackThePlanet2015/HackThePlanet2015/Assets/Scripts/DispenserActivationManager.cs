using LMWidgets;
using UnityEngine;

namespace Tamagotchi
{
    public class DispenserActivationManager : DataBinderToggle
    {
        [SerializeField] private Dispenser dispenser;
        public override bool GetCurrentData()
        {
            return dispenser.IsActive;
        }

        protected override void setDataModel(bool value)
        {
            dispenser.IsActive = value;
        }
    }
}