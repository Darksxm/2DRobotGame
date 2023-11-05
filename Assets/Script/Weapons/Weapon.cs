using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    protected Animator baseAnim;
    protected Animator weaponAnim;
    protected PlayerAttackState state;
    protected int attackCounter; 
    protected virtual void Start()
    {
        baseAnim = transform.Find("Base").GetComponent<Animator>();
        weaponAnim = transform.Find("Weapon").GetComponent<Animator>();
        gameObject.SetActive(false);
    }
    public virtual void EnterWeapon()
    {
        gameObject.SetActive(true);
        
        if(attackCounter >= 3)
        {
            attackCounter = 0;
        }
        baseAnim.SetBool("attack", true);
        weaponAnim.SetBool("attack", true);

        baseAnim.SetInteger("attackCounter", attackCounter);
        weaponAnim.SetInteger("attackCounter", attackCounter);
    }
    public virtual void ExitWeapon()
    {
        baseAnim.SetBool("attack", false);
        weaponAnim.SetBool("attack", false);

        attackCounter++;

        gameObject.SetActive(false);
    }
    #region Animation Triggers
    public virtual void AnimationFinishTrigger()
    {
        state.AnimationFinishTrigger();
    }
    #endregion
    public void InitializeWeapon(PlayerAttackState state)
    {
        this.state = state;
    }
}
