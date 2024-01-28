using UnityEngine;

public abstract class Appendage : MonoBehaviour
{

    // Update is called once per frame
    public abstract void callableUpdate(Vector3 vec, bool fire);
}