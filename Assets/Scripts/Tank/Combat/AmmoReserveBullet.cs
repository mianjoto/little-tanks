using UnityEngine;

public class AmmoReserveBullet
{
    float timeShot;
    float reloadAfterShootingDelayInSeconds;
    float reloadTimeInSeconds;

    public AmmoReserveBullet(float reloadAfterShootingDelayInSeconds, float reloadTimeInSeconds)
    {
        this.timeShot = 0;
        this.reloadAfterShootingDelayInSeconds = reloadAfterShootingDelayInSeconds;
        this.reloadTimeInSeconds = reloadTimeInSeconds;
    }

    public void Shoot(float currentTime)
    {
        this.timeShot = currentTime;
    }

    public bool CanShoot()
    {
        if (timeShot + reloadAfterShootingDelayInSeconds + reloadTimeInSeconds <= Time.time)
            return true;
        else
            return false;
    }
}
