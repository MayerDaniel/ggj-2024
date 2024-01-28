using UnityEngine;

public class Bullet : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Enemy>())
            GunSword.DestroyEnemy(collision.gameObject);

        Destroy(gameObject);
    }
}
