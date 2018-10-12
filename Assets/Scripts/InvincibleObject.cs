using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvincibleObject : MonoBehaviour {

    [SerializeField] private int invincibleTime = 5;
    private bool isUsed = false;


    public int GetInvincibleTime()
    {
        if(isUsed)
        {
            invincibleTime = 0;
        }
        isUsed = true;
        return invincibleTime;
    }
}
