using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using UnityEngine.VFX.Utility;

[VFXBinder("VFX/Light Binder")]
public class LightBinder : VFXBinderBase
{
    public VFXHandler vfxHandler;
    public BallEvent ballEvent;
    
    public string vecLight
    {
        get => (string)_vecLight;
        set => _vecLight = value;
    }

    public string vecLate
    {
        get => (string)_vecLate;
        set => _vecLate = value;
    }
    
    [VFXPropertyBinding("UnityEngine.Vector3"), SerializeField]
    private ExposedProperty _vecLight = "vecLight";
    [VFXPropertyBinding("UnityEngine.Vector3"), SerializeField]
    private ExposedProperty _vecLate = "vecLate";

    public override bool IsValid(VisualEffect component)
        => component.HasVector3(_vecLight) && component.HasVector3(_vecLate);

    public override void UpdateBinding(VisualEffect component)
    {
        component.SetVector3(_vecLight, vfxHandler.vecLightPos);
        component.SetVector3(_vecLate, ballEvent.trailTR.position);
    }
}
