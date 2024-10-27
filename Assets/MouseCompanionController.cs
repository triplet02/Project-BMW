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
        Debug.Log("[ParentMouse Position] : " + parentMousePosition.ToString());
        companion.transform.position = parentMousePosition - positionBias;
        Debug.Log("[Companion Position] : (Bias)" + positionBias.ToString() + " / " + companion.transform.position.ToString());

        string parentAnimatorStatus = parentAnimator.GetCurrentAnimatorClipInfo(0)[0].clip.name;
        string companionAnimationStatus = companionAnimator.GetCurrentAnimatorClipInfo(0)[0].clip.name;

        //Debug.Log("[Parent] : " + parentAnimatorStatus);
        //sDebug.Log("[Companion] : " + companionAnimationStatus);

        /*
        if (!companionAnimationStatus.Equals(parentAnimatorStatus))
        {
            companionAnimator.Play(parentAnimatorStatus);
        }
        */
    }

    public Vector3 GetPositionBias()
    {
        return positionBias;
    }
}
