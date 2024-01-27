using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Leg : MonoBehaviour
{
    [SerializeField]
    Rigidbody2D foot;

    [SerializeField]
    GameObject pelvis;

    [SerializeField]
    GameObject thigh;

    [SerializeField]
    GameObject calf;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        if (Input.GetKeyDown(KeyCode.Space))
        {

            if (!thigh.TryGetComponent<FixedJoint2D>(out FixedJoint2D thighJoint))
            {
                thigh.AddComponent<FixedJoint2D>();
                thigh.GetComponent<FixedJoint2D>().connectedBody = pelvis.GetComponent<Rigidbody2D>();
            }

            if (!calf.TryGetComponent<FixedJoint2D>(out FixedJoint2D calfJoint))
            {
                calf.AddComponent<FixedJoint2D>();
                calf.GetComponent<FixedJoint2D>().connectedBody = thigh.GetComponent<Rigidbody2D>();
            }


            

            foot.AddRelativeForce(Vector3.forward * 50);
        } else
        {
            Destroy(thigh.GetComponent<FixedJoint2D>());
            Destroy(calf.GetComponent<FixedJoint2D>());

            Vector3 tempVect = new Vector3(h, v, 0);
            tempVect = tempVect.normalized * 10 * Time.deltaTime;
            foot.MovePosition(foot.transform.position + tempVect);
        }

        
  

}
}



