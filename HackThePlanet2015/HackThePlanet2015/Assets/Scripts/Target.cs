using UnityEngine;
using UnityEngine.UI;

namespace Tamagotchi
{
    public class Target : MonoBehaviour
    {
        public static Target Instance;

        public Text scoreText;
        public int score = 0;

        void OnEnable()
        {
            Instance = this;
        }

        void OnDisable()
        {
            Instance = null;
        }

        void Update()
        {
            scoreText.text = score.ToString();
            
        }

        public void Reset()
        {
            score = 0;
        }
        void OnTriggerEnter(Collider other)
        {
            score++;
        }
    }
}