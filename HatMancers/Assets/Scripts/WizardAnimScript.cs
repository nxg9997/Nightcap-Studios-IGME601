using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Handles animating the Wizard model (attached to the Player).
/// Author(s): Michael
/// </summary>
public class WizardAnimScript : MonoBehaviour
{
    // Testing Attributes
    public bool debug;
    public bool test_isIdle = false;
    public bool test_isRunning = false;

    // Attributes
    private Animator anim;

    // Animation State Enum
    private enum AnimState
    {
        Idle,
        Running,
        Shooting
    };

    /// <summary>
    /// </summary>
    private void SetDebugVars()
    {
        // Retrieving the testing variables
        test_isIdle = anim.GetBool("isIdle");
        test_isRunning = anim.GetBool("isRunning");
    }

    /// <summary>
    /// 
    /// </summary>
    void Start()
    {
        // Retrieving the attached Animator object
        anim = GetComponent<Animator>();

        // IF debug is enabled...
        if (debug)
            SetDebugVars();
    }

    /// <summary>
    /// </summary>
    private void Update()
    {
        
    }

    /// <summary>
    /// </summary>
    private void SetAnimBools(AnimState state)
    {
        // Setting every animation boolean to false
        anim.SetBool("isIdle", false);
        anim.SetBool("isRunning", false);
        anim.SetBool("isShooting", false);

        // SWITCH for every animation state
        switch (state)
        {
            case AnimState.Idle:
                anim.SetBool("isIdle", true);
                break;
            case AnimState.Running:
                anim.SetBool("isRunning", true);
                break;
            case AnimState.Shooting:
                anim.SetBool("isShooting", true);
                break;
            default:
                Debug.LogError("Case for selected AnimState enum has not been defined!");
                break;
        }

        // IF debug is enabled...
        if (debug)
            SetDebugVars();
    }

    /// <summary>
    /// </summary>
    public void Idle()
    {
        // IF the Wizard is not already idling...
        if (!anim.GetBool("isIdle"))
        {
            // Making the idle animation play
            anim.CrossFade("Idle", 0.1f);
            SetAnimBools(AnimState.Idle);
        }
    }

    /// <summary>
    /// </summary>
    public bool IsIdling()
    {
        return anim.GetBool("isIdle");
    }

    /// <summary>
    /// </summary>
    public void Run()
    {
        // IF the Wizard is not already running...
        if (!anim.GetBool("isRunning"))
        {
            // Making the running animation play
            anim.CrossFade("Running", 0.1f);
            SetAnimBools(AnimState.Running);
        }
    }

    /// <summary>
    /// </summary>
    public bool IsRunning()
    {
        return anim.GetBool("isRunning");
    }

    /// <summary>
    /// </summary>
    public void Shoot()
    {
        // IF the Wizard is not already shooting...
        if (!anim.GetBool("isShooting"))
        {
            // Making the running animation play
            anim.CrossFade("Shooting", 0.1f);
            SetAnimBools(AnimState.Shooting);
        }
    }

    public bool IsShooting()
    {
        return anim.GetBool("isShooting");
    }

    /// <summary>
    /// </summary>
    public void Die()
    {
        // IF the Wizard is not already dying...
        if (!anim.GetBool("isDying"))
        {
            // Making the dying animation play (eventually)
        }
    }

    /// <summary>
    /// </summary>
    public bool IsDying()
    {
        return anim.GetBool("isDying");
    }

    /// <summary>
    /// </summary>
    public void Jump()
    {
        // IF the Wizard is not already jumping...
        if (!anim.GetBool("isJumping"))
        {
            // Making the jumping animation play (eventually)
        }
    }

    /// <summary>
    /// </summary>
    public bool IsJumping()
    {
        return anim.GetBool("isJumping");
    }
}
