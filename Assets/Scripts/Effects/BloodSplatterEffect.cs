using UnityEngine;
using System.Collections;

public class BloodSplatterEffect : MonoBehaviour
{
    public static BloodSplatterEffect _bloodSplatterEffect;
    private ParticleSystem _bloodSplatterParticleSystem;

    private void Awake()
    {
        if (_bloodSplatterEffect == null)
        {
            _bloodSplatterParticleSystem = GetComponentInChildren<ParticleSystem>();
            _bloodSplatterEffect = this;
        }
        else if (this != _bloodSplatterEffect)
        {
            Destroy(this);
        }
    }

    public void SpawnBloodSplatter(Vector2 location, float rotation)
    {
        transform.position = location;
        transform.eulerAngles = new Vector3(0f, 0f, rotation);
        _bloodSplatterParticleSystem.Stop();
        _bloodSplatterParticleSystem.Play();
    }
}
