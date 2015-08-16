using UnityEngine;

namespace Tamagotchi
{
    public class CandyDispenser : MonoBehaviour, IDispensable
    {
        public GameObject CandyGO;
        [SerializeField]
        private bool _isActive = false;
        public bool IsActive { get { return _isActive; } set { _isActive = value; } }

        public void Dispense()
        {
            throw new System.NotImplementedException();
        }
    }
}