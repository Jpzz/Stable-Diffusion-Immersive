using System;
using System.Collections;
using System.Collections.Generic;
using OscSimpl;
using UnityEditor.VersionControl;
using UnityEngine;
using Random = UnityEngine.Random;

public class OSCHandler : MonoBehaviour
{
   public VFXHandler vfxHandler;
   public CamEvent camEvent;
   public BallEvent ballEvent;
   /// <summary>
   /// Scale Value
   /// </summary>
   /// <param name="value"></param>
   public void OnReceivedScale(float value)
   {
      vfxHandler.turValue = value;
   }

   public void OnReceiveXY(OscMessage message)
   {
      var msg = message.ToString();
      var split = msg.Split(' ');
      var strY = split[^2].Replace("f", "");
      var strX = split[^1].Replace("f", "");
      var x = float.Parse(strX);
      var y = float.Parse(strY);
      var pos = Vector3.Scale(new Vector3(x, y,0f), 
         new Vector3(vfxHandler.lightXRange, vfxHandler.lightYRange, 0f));
      vfxHandler.vecLightPos = pos;
      ballEvent.TrailPosEvent(pos);
      OscPool.Recycle(message);
   }

   public void OnReceivedRadi(float value)
   {
      vfxHandler.lightRadi = value;
   }

   public void OnReceivedCam(float value)
   {
      if(value==1 && !camEvent.ingEvent)
      {
         camEvent.CamOrbitEvent();
      }
   }

   public void OnReceivedGlitch(float value)
   {
      if (value == 1)
      {
         vfxHandler.curTextureIdx++;
         vfxHandler.curTextures = vfxHandler.textures[vfxHandler.curTextureIdx % vfxHandler.textures.Length];
         vfxHandler.Glitch(1f, 0.3f);
         float rand = Random.Range(0, 1f);
         vfxHandler.ShakeCamera(3f, 0.5f, 10, rand);
      }
   }
}
