using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.InputSystem;

public class GrappleArm : Appendage
{
    [SerializeField]
    Rigidbody2D hand;

    [SerializeField]
    GrapplingHook hook;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Leg update - toggleable lock and boost, otherwise move the leg
    override public void callableUpdate(UnityEngine.Vector2 vec, bool fire)
    {
        hand.AddForce(vec * 10);

        if (fire)
        {
            Debug.Log("Hand Fire");
            hook.OnFirePressed();
        }

    }
}



