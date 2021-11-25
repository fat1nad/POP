// Author: Fatima Nadeem

using UnityEngine;

public class Locker : MonoBehaviour
/*
    This class is used lock/unlock the object it is attached to, to 
    freeze/unfreeze the game. Useful for non-physics oriented actions.
*/
{
    bool unlocked;

    void Start()
    {
        Unlock();
    }

    public void Lock()
    {
        unlocked = false;
    }

    public void Unlock()
    {
        unlocked = true;
    }

    public bool IsUnlocked()
    {
        return unlocked;
    }
}
