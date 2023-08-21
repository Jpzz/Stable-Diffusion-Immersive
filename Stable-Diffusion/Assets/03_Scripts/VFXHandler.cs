using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;
using UnityEngine.Serialization;

public class VFXHandler : MonoBehaviour
{
    public float turValue;
    public Vector3 vecLightPos;
    public float lightRadi;
    public Texture2D[] textures;
    public CubemapParameter[] skyCubeMaps;
    public VolumeProfile[] profiles;
    [HideInInspector] public Texture2D curTextures;
    [HideInInspector] public int curTextureIdx;
    [Range(0f, 120f)] public float lightXRange;
    [Range(0f, 120f)] public float lightYRange;
    
    public Volume volume;
    public Transform camTR;
    private Glitch[] _glitch;
    [SerializeField] private Ease ease;
    
    void Start()
    {
        _glitch = new Glitch[profiles.Length];
        SetOutGlitch();
    }

    public void Glitch(int idx, float jitterDuration, float driftDuration)
    {
        DOTween.To( () => _glitch[idx].jitter.value, x => _glitch[idx].jitter.value = x, 1f, jitterDuration).SetLoops(2, LoopType.Yoyo);
        DOTween.To( () => _glitch[idx].drift.value, x => _glitch[idx].drift.value = x, 0.5f, driftDuration).SetLoops(2, LoopType.Yoyo);
    }
    
    public void ShakeCamera(float ShakeDuration, float ShakeStrength, int ShakeVibrato, float randomness)
    {
        camTR.DOShakePosition(ShakeDuration, ShakeStrength, ShakeVibrato, randomness).SetEase(ease);
    }

    public void ChangeVolumeProfile(int idx)
    {
        volume.profile = profiles[idx];
    }

    private void SetOutGlitch()
    {
        for (int i = 0; i < profiles.Length; i++)
        {
            volume.profile = profiles[i];
            volume.profile.TryGet(out _glitch[i]);
        }

        volume.profile = profiles[0];
    }
    
    
}
