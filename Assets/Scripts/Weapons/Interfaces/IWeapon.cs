public interface IWeapon 
{
    void Shoot();

    void SetWeaponStrategy(WeaponStrategy strategy);

    void OnTriggerHold();

    void OnTriggerRelease();
}
