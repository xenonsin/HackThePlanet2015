namespace Tamagotchi
{
    public class ExerciseButtonToggle : ButtonDemoToggle
    {
        public override void ButtonTurnsOn()
        {
            if (Pet.Instance.CanPlay())
            {
                ExerciseManager.Instance.Activate();
            }

            base.ButtonTurnsOn();
        }

        public override void ButtonTurnsOff()
        {
           base.ButtonTurnsOff();
        }
    }
}