namespace Tamagotchi
{
    public class ExerciseButtonToggle : ButtonDemoToggle
    {
        public override void ButtonTurnsOn()
        {
            if (Pet.Instance.CanPlay())
            {
                ExerciseManager.Instance.Activate();
                Pet.Instance.Play();
                Target.Instance.Reset();
            }

            base.ButtonTurnsOn();
        }

        public override void ButtonTurnsOff()
        {
           ExerciseManager.Instance.Deactivate();

           base.ButtonTurnsOff();
        }
    }
}