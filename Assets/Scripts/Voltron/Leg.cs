using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Leg : Appendage
{
    [SerializeField]
    Rigidbody2D foot;

    [SerializeField]
    GameObject hip;

    [SerializeField]
    GameObject knee;

    [SerializeField]
    GameObject boost;

    public HingeJoint2D[] kneeJoints;
    public HingeJoint2D[] hipJoints;

    JointAngleLimits2D limits;

    bool thrustFlag = false;
    bool firstThrustFlag = true;

    private SpriteRenderer sr;

    private int toggleCount;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Awake()
    {
        sr = boost.GetComponent<SpriteRenderer>();
    }

    // Leg update - toggleable lock and boost, otherwise move the leg
    override public void callableUpdate(Vector2 vec, bool fire)
    {

        hipJoints = hip.GetComponents<HingeJoint2D>();
        kneeJoints = knee.GetComponents<HingeJoint2D>();

        if (fire)
        {
            Debug.Log("Leg Fire");
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
            sr.enabled = true;

            toggleCount += 1;
            if (toggleCount == 10)
            {
                sr.flipX = !sr.flipX;
                toggleCount = 0;
            }
            foot.AddRelativeForce(Vector3.up * 500);
            firstThrustFlag = false;
        } else
        {

            sr.enabled = false;
            foreach (HingeJoint2D j in hipJoints)
            {
                j.useLimits = false;
            }

            foreach (HingeJoint2D j in kneeJoints)
            {
                j.useLimits = false;
            }

            foot.AddForce(vec * 10);
            firstThrustFlag = true;

        }

        
  

}
}



