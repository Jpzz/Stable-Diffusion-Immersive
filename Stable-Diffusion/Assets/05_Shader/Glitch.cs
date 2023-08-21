using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;


[System.Serializable, VolumeComponentMenu("Post-processing/Custom/Glitch")]
public sealed class Glitch : CustomPostProcessVolumeComponent, IPostProcessComponent
{
    [Tooltip("Controls the intensity of the effect.")]
    public ClampedFloatParameter block = new ClampedFloatParameter(0, 0, 1);
    public ClampedFloatParameter drift = new ClampedFloatParameter(0, 0, 1);
    public ClampedFloatParameter jitter = new ClampedFloatParameter(0, 0, 1);
    public ClampedFloatParameter jump = new ClampedFloatParameter(0, 0, 1);
    public ClampedFloatParameter shake = new ClampedFloatParameter(0, 0, 1);
    
    Material m_Material;

    private float _prevTime;
    private float _jumpTime;
    private float _blockTime;
    private int _blockSeed1 = 71;
    private int _blockSeed2 = 113;
    private int _blockStride = 1;

    static class ShaderIDs
    {
        internal static readonly int BlockSeed1 = Shader.PropertyToID("_BlockSeed1");
        internal static readonly int BlockSeed2 = Shader.PropertyToID("_BlockSeed2");
        internal static readonly int BlockStrength = Shader.PropertyToID("_BlockStrength");
        internal static readonly int BlockStride = Shader.PropertyToID("_BlockStride");
        internal static readonly int Drift = Shader.PropertyToID("_Drift");
        internal static readonly int InputTexture = Shader.PropertyToID("_InputTexture");
        internal static readonly int Jitter = Shader.PropertyToID("_Jitter");
        internal static readonly int Jump = Shader.PropertyToID("_Jump");
        internal static readonly int Seed = Shader.PropertyToID("_Seed");
        internal static readonly int Shake = Shader.PropertyToID("_Shake");
    }
    public bool IsActive() => m_Material != null &&
                              (block.value > 0 || drift.value > 0 || jitter.value > 0 || jump.value > 0 ||
                               shake.value > 0);
    
    // Do not forget to add this post process in the Custom Post Process Orders list (Project Settings > Graphics > HDRP Settings).
    public override CustomPostProcessInjectionPoint injectionPoint => CustomPostProcessInjectionPoint.AfterPostProcess;

    const string kShaderName = "Hidden/Custom/PostProcess/Glitch";

    public override void Setup()
    {
        if (Shader.Find(kShaderName) != null)
            m_Material = new Material(Shader.Find(kShaderName));
        else
            Debug.LogError($"Unable to find shader '{kShaderName}'. Post Process Volume Glitch is unable to load.");
    }

    public override void Render(CommandBuffer cmd, HDCamera camera, RTHandle source, RTHandle destination)
    {
        if (m_Material == null)
            return;
        var time = Time.time;
        var delta = time - _prevTime;
        _jumpTime += delta * jump.value * 11.3f;
        _prevTime = time;

        var block3 = block.value * block.value * block.value;

        _blockTime += delta * 60;

        if (_blockTime > 1)
        {
            if (Random.value < 0.09f) _blockSeed1 += 251;
            if (Random.value < 0.29f) _blockSeed2 += 373;
            if (Random.value < 0.25f) _blockStride = Random.Range(1, 32);
            _blockTime = 0;
        }

        var vdrift = new Vector2(time * 606.11f % (Mathf.PI * 2), drift.value * 0.04f);

        var jv = jitter.value;
        var vjitter = new Vector3(Mathf.Max(0, 1.001f - jv * 1.2f), 0.002f + jv * jv * jv * 0.05f);

        var vjump = new Vector2(_jumpTime, jump.value);
        
        m_Material.SetInt(ShaderIDs.Seed, (int)(time * 10000));
        m_Material.SetFloat(ShaderIDs.BlockStrength, block3);
        m_Material.SetInt(ShaderIDs.BlockStride, _blockStride);
        m_Material.SetInt(ShaderIDs.BlockSeed1, _blockSeed1);
        m_Material.SetInt(ShaderIDs.BlockSeed2, _blockSeed2);
        m_Material.SetVector(ShaderIDs.Drift, vdrift);
        m_Material.SetVector(ShaderIDs.Jitter, vjitter);
        m_Material.SetVector(ShaderIDs.Jump, vjump);
        m_Material.SetFloat(ShaderIDs.Shake, shake.value * 0.2f);
        m_Material.SetTexture(ShaderIDs.InputTexture, source);

        var pass = 0;
        if (drift.value > 0 || jitter.value > 0 || jump.value > 0 || shake.value > 0) pass += 1;
        if (block.value > 0) pass += 2;
        HDUtils.DrawFullScreen(cmd, m_Material, destination, null, pass);
    }

    public override void Cleanup()
    {
        CoreUtils.Destroy(m_Material);
    }
}
