using UnityEngine;

namespace Tamagotchi
{
    public class BreadDispenser : MonoBehaviour, IDispensable
    {
        public GameObject BreadGO;
        [SerializeField]
        private float _spawnCooldown = 1f;
        public float SpawnCooldown { get { return _spawnCooldown; } set { _spawnCooldown = value; } }
        [SerializeField]
        private bool _isActive = false;
        public bool IsActive { get { return _isActive; } set { _isActive = value; } }

        public void Dispense()
        {
            throw new System.NotImplementedException();
        }
    }
}