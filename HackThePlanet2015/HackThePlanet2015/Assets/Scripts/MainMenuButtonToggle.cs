using System.Collections;
using UnityEngine;

namespace Tamagotchi
{
    public class MainMenuButtonToggle : ButtonDemoToggle
    {
        private bool pressed = false;
        public override void ButtonTurnsOn()
        {
            if (!pressed)
            {
                pressed = true;
                StartCoroutine("StartGame");

            }
            base.ButtonTurnsOn();
        }

        public override void ButtonTurnsOff()
        {

            base.ButtonTurnsOff();
        }

        IEnumerator StartGame()
        {
            yield return new WaitForSeconds(3f);

            Application.LoadLevel("test");
        }
    }
}