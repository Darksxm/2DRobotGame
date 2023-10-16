using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    public Vector2 RawMovementInput { get; private set; }
    public int NormalizedInputX { get; private set; }
    public int NormalizedInputY { get; private set; }
    public bool JumpInput { get; private set; }

    /// <summary>
    /// Those are the different input according to the different actions
    /// The equivalent of Input.GetKey/GetButton of the new Input System.
    /// </summary>

    /// <summary>
    /// Horizontal Movement converting the Raw Input into normalized Int value to use 
    /// in the PlayerGroundState.cs/LogicUpdate() to define xInput. It can later on be used
    /// by all its substate.
    /// </summary>
    public void OnMoveInput(InputAction.CallbackContext context)
    {
        RawMovementInput = context.ReadValue<Vector2>();
        NormalizedInputX = (int)(RawMovementInput * Vector2.right).normalized.x;
        NormalizedInputY = (int)(RawMovementInput * Vector2.up).normalized.y;
       }
    public void OnJumpInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            JumpInput = true;
        }
    }
    public void UseJumpInput() => JumpInput = false;
   
}
