namespace Tamagotchi
{
    public interface IDispensable
    {
        bool IsActive { get; set; }

        void Dispense();
    }
}