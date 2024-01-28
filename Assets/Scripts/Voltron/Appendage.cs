using UnityEngine;

public abstract class Appendage : MonoBehaviour
{

    // Update is called once per frame
    public abstract void callableUpdate(Vector2 vec, bool fire);
}