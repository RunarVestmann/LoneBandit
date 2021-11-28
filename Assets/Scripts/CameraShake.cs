using System.Collections;
using UnityEngine;
using Cinemachine;

public class CameraShake : MonoBehaviour
{
    [SerializeField] float intensity;
    [SerializeField] float time;
    
    CinemachineVirtualCamera cinemachineCamera;
    CinemachineBasicMultiChannelPerlin cinemachineNoise;

    void Awake()
    {
        cinemachineCamera = GetComponent<CinemachineVirtualCamera>();
        cinemachineNoise = cinemachineCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    public void Shake() => StartCoroutine(ShakeRoutine());

    IEnumerator ShakeRoutine()
    {
        cinemachineNoise.m_AmplitudeGain = intensity;
        yield return new WaitForSeconds(time);
        cinemachineNoise.m_AmplitudeGain = 0f;
    }
    
    
}