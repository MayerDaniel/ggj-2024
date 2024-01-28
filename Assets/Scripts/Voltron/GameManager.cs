using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    int playerCount = 0;

    [SerializeField]
    Appendage rightHand;

    [SerializeField]
    Appendage leftHand;

    [SerializeField]
    Appendage rightFoot;

    [SerializeField]
    Appendage leftFoot;

    Queue<Appendage> objectList = new Queue<Appendage>();


    // Start is called before the first frame update
    void Start()
    {
        objectList.Enqueue(rightFoot);
        objectList.Enqueue(leftFoot);
        objectList.Enqueue(rightHand);
        objectList.Enqueue(leftHand);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // registers a player when a new controller is plugged in, and returns the position of the 
    public Appendage registerPlayer()
    {
        Debug.Log("Player Registered");
        return objectList.Dequeue();
    }
}
