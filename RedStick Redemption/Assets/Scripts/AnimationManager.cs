﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    protected Animator anim;

    private PlayerControllerScript playerControllerScript;

    public Animator getAnimator()
    { return anim; }


    void Start()
    {
        anim = GetComponent<Animator>();
        anim.SetFloat("GunTree", 0);
        anim.SetFloat("LoudWeapon", 0);
        anim.SetBool("isWalking", false);
        anim.SetBool("isRunning", false);
        anim.SetBool("isCrouching", false);
        anim.SetBool("idleClimb", false);

        playerControllerScript = GetComponent<PlayerControllerScript>();
    }

    void Update()
    {
    }

    /*
     * @Deprecated
    public Animator GetAnimator()
    {
        return this.anim;
    }*/

    public void JumpAnimation()
    {
        anim.SetBool("isJumping", true);
        anim.SetBool("isWalking", false);
    }

    public void getOnGroundAnimation()
    {
        anim.SetBool("isJumping", false);
    }

    public void TriggerTakeOff()
    {
        anim.SetTrigger("takeOff");
    }

    public void TriggerCrouching()
    {
        anim.SetTrigger("isCrouching");
    }

    public void startRunning()
    {
        anim.SetBool("isRunning", true);
    }

    public void stopRunning()
    {
        anim.SetBool("isRunning", false);
    }

    public void startClimbing()
    {
        anim.SetBool("idleClimb", false);
        anim.SetBool("isClimbing", true);
    }


    public void pauseClimbing()
    {
        anim.SetBool("idleClimb", true);
        anim.SetBool("isClimbing", false);

    }

    public void stopClimbing()
    {
        anim.SetBool("idleClimb", false);
        anim.SetBool("isClimbing", false);

    }


    public void stopWalking()
    {
        anim.SetBool("isWalking", false);
    }

    public void startWalking()
    {
        anim.SetBool("isWalking", true);
    }

    public void setCrounching()
    {
        anim.SetBool("isCrouching", true);
    }

    public void standUp()
    {
        anim.SetBool("isCrouching", false);
    }

    public void startCrouchWalking()
    {
        anim.SetBool("isCrouchWalking", true);
    }

    public void stopCrouchWalking()
    {
        anim.SetBool("isCrouchWalking", false);
    }

    public void startFlyingKick()
    {
        anim.SetTrigger("flyingKick");
    }


    public void resetTrigger(string animName)
    {
        if (animName != null && animName.Length > 0)
        {
            anim.ResetTrigger(animName);
        }
    }

    public void punchAnimation()
    {
        anim.SetBool("punch", true);
    }

    public bool getIsAttacking()
    {
        return anim.GetBool("punch") || anim.GetBool("kick") || anim.GetBool("lowKick") || anim.GetBool("uppercut");
    }
    public void kickAnimation()
    {
        anim.SetBool("kick", true);
    }

    public void lowKickAnimation()
    {
        anim.SetBool("lowKick", true);
    }

    public void uppercutAnimation()
    {
        anim.SetBool("uppercut", true);
    }

    public void setAnimationState(string animName)
    {
        anim.SetBool(animName, false);
    }

    public void armToFire()
    {
        anim.SetTrigger("armToFire");
        anim.SetBool("isFiring", true);
    }
    public void fireAnimation()
    {
        anim.SetBool("isFiring", true);
    }

    public void stopFire()
    {
        anim.SetBool("isFiring", false);
    }

}
