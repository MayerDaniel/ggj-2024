using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Leg : MonoBehaviour
{
    [SerializeField]
    Rigidbody2D foot;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var gamepad = Gamepad.current;
        if (gamepad == null)
            return; // No gamepad connected.

        if (gamepad.rightTrigger.wasPressedThisFrame)
        {
            foot.AddRelativeForce(Vector3.forward);
        }

        Vector2 move = gamepad.leftStick.ReadValue();
        
    }
}



