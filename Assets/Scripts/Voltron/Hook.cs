using UnityEngine;

public class Hook : MonoBehaviour
{
    [SerializeField] private GrapplingHook _hookManager;
    [SerializeField] private AudioSource _audioSource;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        _hookManager.GrabHook();
        transform.SetParent(collision.gameObject.transform);
        _audioSource.Play();
    }
}
