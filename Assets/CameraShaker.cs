using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using DG.Tweening;

public class CameraShaker : MonoBehaviour
{
    [SerializeField] private Transform camera;
    [SerializeField] private float shakeTime;
    [SerializeField] private Vector3 positionStrength;
    [SerializeField] private Vector3 rotationStrength;

    private static event Action Shake;

    public static void Invoke()
    {
        Shake?.Invoke();
    }

    private void OnEnable()
    {
        Shake += CameraShake;
    }

    private void OnDisable()
    {
        Shake -= CameraShake;
    }

    private void CameraShake()
    {
        camera.DOComplete();
        camera.DOShakePosition(shakeTime, positionStrength);
        camera.DOShakeRotation(shakeTime, rotationStrength);
    }
}
