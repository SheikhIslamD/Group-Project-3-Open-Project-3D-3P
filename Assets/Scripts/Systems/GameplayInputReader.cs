using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameplayInputReader : Singleton<GameplayInputReader>
{
    public InputActions input;
    //private PlayerInput playerInput;


    protected override void OnAwake()
    {
        input = new InputActions();
        input.Enable();
        //playerInput = GetComponent<PlayerInput>();
        aimOutput = new Vector2(Screen.width / 2, Screen.height / 2);
        ((InputActionMap)input.Gameplay).actionTriggered += AimTypeChanged;

    }

    private void Update()
    {
        //usingKeyboardForAim = playerInput.currentControlScheme == "Keyboard+Mouse";

        if (pause.WasPressedThisFrame()) GameplayStateManager.inst.TogglePause();

        if (usingKeyboardForAim)
        {
            aimOutput = aim.ReadValue<Vector2>();
        }
        else
        {
            aimOutput += aimDelta.ReadValue<Vector2>() * SettingsSave.save.GetThumbstickSensitivity();
        }
        
        aimOutput.x = Mathf.Clamp(aimOutput.x, 0, Screen.width - 1);
        aimOutput.y = Mathf.Clamp(aimOutput.y, 0, Screen.height - 1);

        




    }

    void AimTypeChanged(InputAction.CallbackContext ctx)
    {
        InputDevice device = ctx.control.device;
        if      (input.KeyboardMouseScheme.SupportsDevice(device)) usingKeyboardForAim = true;
        else if (input.GamepadScheme      .SupportsDevice(device)) usingKeyboardForAim = false;

        //InputControlScheme.FindControlSchemeForDevice
    }


    bool usingKeyboardForAim = true;
    InputAction aim => input.Gameplay.Aim;
    InputAction aimDelta => input.Gameplay.AimDelta;

    Vector2 aimVector2 => input.Gameplay.AimDelta.ReadValue<Vector2>();

    [HideInInspector] public Vector2 aimOutput;
    public InputAction movement => input.Gameplay.Movement;
    [HideInInspector] public Vector2 movementVector2 => input.Gameplay.Movement.ReadValue<Vector2>();
    public bool movementTouched => input.Gameplay.Movement.ReadValue<Vector2>() != Vector2.zero;
    public InputAction jump => input.Gameplay.Jump;
    public InputAction shoot => input.Gameplay.Shoot;
    public InputAction melee => input.Gameplay.Melee;
    public InputAction sprint => input.Gameplay.Sprint;
    public InputAction pause => input.Gameplay.Pause;
    public InputAction heal => input.Gameplay.Heal;

    InputControlScheme keyboard => input.KeyboardMouseScheme;
    InputControlScheme gamepad => input.GamepadScheme;
}
