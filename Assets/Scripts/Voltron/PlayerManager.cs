using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;


public class PlayerManager : MonoBehaviour
{

    Appendage playerObject;
    Vector2 inputVector = Vector2.zero;
    bool fire = false;

    // Start is called before the first frame update
    void Start()
    {
        playerObject = FindAnyObjectByType<GameManager>().registerPlayer();
        Debug.Log("Player Registered");
        if (playerObject == null)
        {
            Debug.Log("Player is null :(");
        }
        //StartCoroutine(Update_Co());
    }

    public void OnMove(InputValue value)
    {
        inputVector = value.Get<Vector2>();
    }

    public void OnFire(InputValue value)
    {
        fire = true;
    }

    private void Update()
    { 
        if (playerObject != null)
        {
            playerObject.callableUpdate(inputVector, fire);
            fire = false;
        }
            
    }

    
}
