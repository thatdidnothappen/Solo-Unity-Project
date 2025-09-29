using UnityEngine;
using UnityEngine.InputSystem;

public class Rifle : Weapon
{
    public void toggleFireMode()
    {
        currentFireMode++;

        if (currentFireMode >= fireModes)
            currentFireMode = 0;

        if (currentFireMode == 0)
            rof = 1;
        else
            rof = .25f;
    }
}
