using UnityEngine;

namespace Tamagotchi
{
    public class ResetInput : MonoBehaviour
    {
        void Update()
        {
            //Restart Current Level
            if (Input.GetKeyDown(KeyCode.R))
                Application.LoadLevel(Application.loadedLevel);

            if (Input.GetKeyDown(KeyCode.Q))
                Application.LoadLevel("mainmenu");
        }
    }
}