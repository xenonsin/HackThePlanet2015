using UnityEngine;

namespace Tamagotchi
{
    public class MedicineDispenser : MonoBehaviour, IDispensable
    {
        public GameObject MedicineGO;

        [SerializeField]
        private bool _isActive = false;
        public bool IsActive { get { return _isActive; } set { _isActive = value; } }

        public void Dispense()
        {
            throw new System.NotImplementedException();
        }
    }
}