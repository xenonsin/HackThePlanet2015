using LMWidgets;
using UnityEngine;

namespace Tamagotchi
{
    public class ExerciseActivationManager : DataBinderToggle
    {
        [SerializeField] private ExerciseManager _exerciseManager;
        public override bool GetCurrentData()
        {
            return _exerciseManager.IsActive;
        }

        protected override void setDataModel(bool value)
        {
            _exerciseManager.IsActive = value;
        }
    }
}