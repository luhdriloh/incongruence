using UnityEngine;

using Cinemachine;

public class CinemachineCameraFunctions : MonoBehaviour
{
    public static CinemachineCameraFunctions _cameraFunctions;
    public CinemachineVirtualCamera _cinemachineCamera;

    public float _noiseAmplitudeGain;
    public float _noiseFrequencyGain;


    private CinemachineBasicMultiChannelPerlin _perlinNoise;

    private void Awake()
    {
        if (_cameraFunctions == null)
        {
            _cameraFunctions = this;
        }
        else if (this != _cameraFunctions)
        {
            Destroy(this);
        }
    }

    private void Start()
    {
        _perlinNoise = _cinemachineCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        StopCameraShake();
    }

    public void StartCameraShake()
    {
        _perlinNoise.m_AmplitudeGain = _noiseAmplitudeGain;
        _perlinNoise.m_FrequencyGain = _noiseFrequencyGain;
    }

    public void StopCameraShake()
    {
        _perlinNoise.m_AmplitudeGain = 0;
        _perlinNoise.m_FrequencyGain = 0;
    }
}
