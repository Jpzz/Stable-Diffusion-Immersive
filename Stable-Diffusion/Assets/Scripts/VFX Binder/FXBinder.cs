using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.VFX;
using UnityEngine.VFX.Utility;

[AddComponentMenu("VFX/Property Binders/FX Binder")]
[VFXBinder("VFX/FX Binder")]
public class FXBinder : VFXBinderBase
{
    public VFXHandler vfxHandler;
    
    public string turValue
    {
        get => (string)_turValue;
        set => _turValue = value;
    }

    public string vecPosLight
    {
        get => (string)_vecPosLight;
        set => _vecPosLight = value;
    }

    public string lightRadi
    {
        get => (string)_lightRadi;
        set => _lightRadi = value;
    }
    
    [VFXPropertyBinding("UnityEngine.Float32"), SerializeField]
    private ExposedProperty _turValue = "TurValue";

    [VFXPropertyBinding("UnityEngine.Vector3"), SerializeField]
    private ExposedProperty _vecPosLight = "VecPosLight";

    [VFXPropertyBinding("UnityEngine.Float32"), SerializeField]
    private ExposedProperty _lightRadi = "LightRadi";

    #region Method

    public override bool IsValid(VisualEffect component)
        => component.HasFloat(_turValue) && component.HasVector3(_vecPosLight);

    public override void UpdateBinding(VisualEffect component)
    {
        component.SetFloat(_turValue, vfxHandler.turValue);
        component.SetVector3(_vecPosLight, vfxHandler.vecLightPos);
        component.SetFloat(_lightRadi, vfxHandler.lightRadi);
    }

    #endregion
    
}
