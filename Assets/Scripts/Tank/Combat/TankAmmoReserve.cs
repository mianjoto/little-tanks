using UnityEngine;

public class TankAmmoReserve
{
    AmmoReserveBullet[] ammoReserve;
    
    public TankAmmoReserve(TankData tankData)
    {
        ammoReserve = new AmmoReserveBullet[tankData.MaxBulletsInReserve];
        for (int i = 0; i < ammoReserve.Length; i++)
        {
            ammoReserve[i] = new AmmoReserveBullet(tankData.ReloadAfterShootingDelayInSeconds, tankData.PerBulletReloadTimeInSeconds);
        }
    }

    public bool HasAmmo
    {
        get
        {
            for (int i = 0; i < ammoReserve.Length; i++)
            {
                if (ammoReserve[i].CanShoot())
                {
                    ammoReserve[i].Shoot(Time.time);
                    return true;
                }
            }
            return false;
        }
    }
}
