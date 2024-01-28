using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Leg : MonoBehaviour
{
    [SerializeField]
    Rigidbody2D foot;

    [SerializeField]
    GameObject hip;

    [SerializeField]
    GameObject knee;

    public HingeJoint2D[] kneeJoints;
    public HingeJoint2D[] hipJoints;

    JointAngleLimits2D limits;

    bool thrustFlag = false;
    bool firstThrustFlag = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        hipJoints = hip.GetComponents<HingeJoint2D>();
        kneeJoints = knee.GetComponents<HingeJoint2D>();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            thrustFlag = !thrustFlag;
        }

        if (thrustFlag)
        {

            if (firstThrustFlag)
            {
                foreach (HingeJoint2D j in hipJoints)
                {

                    limits.max = j.jointAngle + 1;
                    limits.min = j.jointAngle - 1;
                    j.limits = limits;
                    j.useLimits = true;
                }

                foreach (HingeJoint2D j in kneeJoints)
                {
                    limits.max = j.jointAngle + 1;
                    limits.min = j.jointAngle - 1;
                    j.limits = limits;
                    j.useLimits = true;
                }
            }
            foot.AddRelativeForce(Vector3.up * 20);
            firstThrustFlag = false;
        } else
        {
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");

            foreach (HingeJoint2D j in hipJoints)
            {
                j.useLimits = false;
            }

            foreach (HingeJoint2D j in kneeJoints)
            {
                j.useLimits = false;
            }

            Vector3 tempVect = new Vector3(h, v, 0);
            foot.AddForce(tempVect);
            firstThrustFlag = true;

        }

        
  

}
}



