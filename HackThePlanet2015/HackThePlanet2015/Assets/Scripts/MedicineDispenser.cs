﻿using UnityEngine;

namespace Tamagotchi
{
    public class MedicineDispenser : MonoBehaviour, IDispensable
    {
        public GameObject MedicineGO;
        [SerializeField]
        private float _spawnCooldown = 1f;
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
            GameObject medicine = GameObject.Instantiate(MedicineGO, transform.position, Quaternion.identity) as GameObject;
            //candy.transform.parent = transform;
            medicine.transform.rotation = Quaternion.Euler(Random.Range(0.0f, 360.0f), Random.Range(0.0f, 360.0f), Random.Range(0.0f, 360.0f));
            medicine.SetActive(true);

            Vector3 modifier = new Vector3(Random.value, Random.value, Random.value);
            medicine.GetComponent<Rigidbody>().AddForce(transform.forward + modifier, ForceMode.Impulse);
        }
    }
}