using UnityEngine;

namespace Tamagotchi
{
    public class Dispenser : MonoBehaviour, IDispensable
    {
        public GameObject consumable;
        [SerializeField]
        private float _spawnCooldown = 2f;
        public float SpawnCooldown { get { return _spawnCooldown; } set { _spawnCooldown = value; } }
        [SerializeField]
        private bool _isActive = false;
        public bool IsActive { get { return _isActive; } set { _isActive = value; } }

        private float spawnTime = 0f;

        void Update()
        {
            if (!IsActive) return;

            if (Time.time > spawnTime + _spawnCooldown)
            {
                spawnTime = Time.time;
                Dispense();
            }

        }

        public void Dispense()
        {
            GameObject go = GameObject.Instantiate(consumable,transform.position, Quaternion.identity) as GameObject;
            //candy.transform.parent = transform;
            go.transform.rotation = Quaternion.Euler(Random.Range(0.0f, 360.0f), Random.Range(0.0f, 360.0f), Random.Range(0.0f, 360.0f));
            go.SetActive(true);

            Vector3 modifier = new Vector3(Random.value,Random.value,Random.value);
            go.GetComponent<Rigidbody>().AddForce(transform.forward + modifier, ForceMode.Impulse);
        }
    }
}