namespace Tamagotchi
{
    public interface IDispensable
    {
        bool IsActive { get; set; }
        float SpawnCooldown { get; set; }
        void Dispense();
    }
}