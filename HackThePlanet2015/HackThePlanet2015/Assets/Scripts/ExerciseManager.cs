using UnityEngine;

namespace Tamagotchi
{
    public class ExerciseManager : MonoBehaviour
    {
        public static ExerciseManager Instance;
        public GameObject GroupGO;

        void OnEnable()
        {
            Instance = this;
        }

        void OnDisable()
        {
            Instance = null;
        }

        public void Activate()
        {
            GroupGO.SetActive(true);
        }

        public void Deactivate()
        {
            GroupGO.SetActive(false);
        }
    }
}