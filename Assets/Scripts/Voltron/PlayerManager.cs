using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;


public class PlayerManager : MonoBehaviour
{

    Appendage playerObject;

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
        Debug.Log($"{playerObject == null} {value == null}");

        if (playerObject != null)
            playerObject.callableUpdate(value.Get<Vector2>(), false);
    }

    // Update is called once per frame
    //private IEnumerator Update_Co()
    //{
    //    while (true)
    //    {
    //        float h = Input.GetAxis("Horizontal");
    //        float v = Input.GetAxis("Vertical");
    //        Vector3 tempVect = new Vector3(h, v, 0);
    //        playerObject.callableUpdate(tempVect, Input.GetButtonDown("Fire1"));
    //        yield return null;
    //    }
    //}
}
