using UnityEngine;

namespace Tamagotchi
{
    public class BreadDispenser : MonoBehaviour, IDispensable
    {
        public GameObject BreadGO;
        [SerializeField]
        private bool _isActive = false;
        public bool IsActive { get { return _isActive; } set { _isActive = value; } }

        public void Dispense()
        {
            throw new System.NotImplementedException();
        }
    }
}