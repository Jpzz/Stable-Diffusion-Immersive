using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class CamEvent : MonoBehaviour
{
    public float rotDuration;
    private Transform _camTR;
    public bool ingEvent;
    void Start()
    {
        _camTR = GetComponent<Transform>();
    }


    public void CamOrbitEvent()
    {
        ingEvent = true;
        _camTR.DORotate(new Vector3(0f, 360f, 0f), rotDuration, RotateMode.WorldAxisAdd).OnComplete(delegate
        {
            ingEvent = false;
        });
    }
}
