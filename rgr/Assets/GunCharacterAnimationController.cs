using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunCharacterAnimationController : MonoBehaviour
{
    [SerializeField] private Animator gunCharacterAnimation;
    public float speedThreshold = 0.1f; 

    private Vector3 lastPosition; 
    private float lastTime;

    private WaitForSeconds quartSec = new WaitForSeconds(.25f);


    private GunCharacterAnimationState AnimationState;
    void Start()
    {
        gunCharacterAnimation = GetComponent<Animator>();
        lastPosition = transform.position;  
        lastTime = Time.time;
        StartCoroutine(ControlState());
        AnimationState = GunCharacterAnimationState.isIdle;
    }
    
    public enum GunCharacterAnimationState
    {
        isIdle, 
        isRunning,
    }
    
    IEnumerator ControlState()
    {
        while (true)
        {
            float elapsedTime = Time.time - lastTime;

            float distance = Vector3.Distance(transform.position, lastPosition);

            float speed = distance / elapsedTime;

            lastTime = Time.time;
            lastPosition = transform.position;

            Debug.Log("CurrentState is => " + AnimationState);

            if (speed > speedThreshold)
            {
                if (AnimationState != GunCharacterAnimationState.isRunning)
                {
                    Debug.Log("Set to is Running, Animation State => " + AnimationState);
                    gunCharacterAnimation.Play("setRun");
                    ChangeToState("setRun");
                    AnimationState = GunCharacterAnimationState.isRunning;
                }
            }
            else if(AnimationState != GunCharacterAnimationState.isIdle)
            {
                Debug.Log("Set to is Idle =>" + AnimationState);
                gunCharacterAnimation.Play("setIdle");
                ChangeToState("setIdle");
                AnimationState = GunCharacterAnimationState.isIdle;

            }
            
            yield return quartSec;
        }
        yield return null;
    }

    void ChangeToState(string setToState)
    {
        foreach (var param in gunCharacterAnimation.parameters)
        {
            if (param.type == AnimatorControllerParameterType.Bool)
            {
                gunCharacterAnimation.SetBool(param.name, param.name == setToState);
            }
        }
    }
}
