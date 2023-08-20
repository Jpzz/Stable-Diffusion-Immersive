using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Serialization;

public class VFXHandler : MonoBehaviour
{
    public float turValue;
    public Vector3 vecLightPos;
    public float lightRadi;
    public Texture2D[] textures;
    [HideInInspector] public Texture2D curTextures;
    [HideInInspector] public int curTextureIdx;
    [Range(0f, 120f)] public float lightXRange;
    [Range(0f, 120f)] public float lightYRange;
    
    public Volume volume;
    public Transform camTR;
    private Glitch _glitch;
    [SerializeField] private Ease ease;
    
    void Start()
    {
        volume.profile.TryGet(out _glitch);
    }

    public void Glitch(float jitterDuration, float driftDuration)
    {
        DOTween.To( () => _glitch.jitter.value, x => _glitch.jitter.value = x, 1f, jitterDuration).SetLoops(2, LoopType.Yoyo);
        DOTween.To( () => _glitch.drift.value, x => _glitch.drift.value = x, 0.5f, driftDuration).SetLoops(2, LoopType.Yoyo);
    }
    
    public void ShakeCamera(float ShakeDuration, float ShakeStrength, int ShakeVibrato, float randomness)
    {
        camTR.DOShakePosition(ShakeDuration, ShakeStrength, ShakeVibrato, randomness).SetEase(ease);
    }
}
