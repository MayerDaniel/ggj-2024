using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunSword : MonoBehaviour
{
    [Header("Gun")]
    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private Transform _bulletSpawnPoint;
    [SerializeField] private float _bulletSpeed;

#if UNITY_EDITOR
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha8))
            OnFirePressed();
    }
#endif

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Enemy>() != null)
        {
            DestroyEnemy(collision.gameObject);
        }
    }

    public void OnFirePressed()
    {
        var bullet = Instantiate(_bulletPrefab, _bulletSpawnPoint.position, Quaternion.identity);
        var dir = (_bulletSpawnPoint.position - transform.position).normalized;
        bullet.GetComponent<Rigidbody2D>().velocity = _bulletSpeed * dir;
    }

    public static void DestroyEnemy(GameObject enemy)
    {
        for (int i = 0; i < enemy.transform.childCount; i++)
        {
            var child = enemy.transform.GetChild(i);
            if (child.name.Contains("ook"))
                child.SetParent(null);
        }
    }
}
