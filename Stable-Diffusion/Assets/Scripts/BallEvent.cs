using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class BallEvent : MonoBehaviour
{
   public VFXHandler vfxHandler;
   public Transform trailTR;
 

   public void TrailPosEvent(Vector3 pos)
   {
      transform.position = vfxHandler.vecLightPos;
      trailTR.DOMove(pos, 1f).SetAutoKill(true);
   }
   
}
