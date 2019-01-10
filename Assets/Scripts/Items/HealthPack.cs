using UnityEngine;

public class HealthPack : MonoBehaviour
{
    public int _healthRegained;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        collision.GetComponent<Player>().HealthChange(_healthRegained);
        Destroy(gameObject);
    }
}
