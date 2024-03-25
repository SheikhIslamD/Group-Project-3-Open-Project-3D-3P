using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameplayInputReader : Singleton<GameplayInputReader>
{
    public InputActions input;


    protected override void OnAwake()
    {
        input = new InputActions();
        input.Enable();
    }

    private void Update()
    {
        if (pause.WasPressedThisFrame()) GameplayStateManager.inst.TogglePause();
    }





    [HideInInspector] public InputAction movement => input.Gameplay.Movement;
    [HideInInspector] public Vector2 movementVector2 => input.Gameplay.Movement.ReadValue<Vector2>();
    [HideInInspector] public bool movementTouched => input.Gameplay.Movement.ReadValue<Vector2>() != Vector2.zero;
    [HideInInspector] public InputAction aim => input.Gameplay.Aim;
    [HideInInspector] public Vector2 aimVector2 => input.Gameplay.Aim.ReadValue<Vector2>();
    [HideInInspector] public InputAction jump => input.Gameplay.Jump;
    [HideInInspector] public InputAction shoot => input.Gameplay.Shoot;
    [HideInInspector] public InputAction melee => input.Gameplay.Melee;
    [HideInInspector] public InputAction sprint => input.Gameplay.Sprint;
    [HideInInspector] public InputAction pause => input.Gameplay.Pause;
    [HideInInspector] public InputAction heal => input.Gameplay.Heal;





}
