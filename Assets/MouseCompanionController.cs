using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseCompanionController : MonoBehaviour
{
    [SerializeField] GameObject companion;
    Animator companionAnimator;
    [SerializeField] GameObject parentMouse;
    Animator parentAnimator;
    [SerializeField] Vector3 positionBias;
    Vector3 parentMousePosition;

    // Start is called before the first frame update
    void Start()
    {
        companionAnimator = companion.GetComponent<Animator>();
        parentAnimator = parentMouse.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        parentMousePosition = parentMouse.transform.position;
        companion.transform.position = parentMousePosition - positionBias;
    }

    public Vector3 GetPositionBias()
    {
        return positionBias;
    }
}
