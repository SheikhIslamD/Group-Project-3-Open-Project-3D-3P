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
        if (pause.WasPressedThisFrame()) GameplayPauseManager.TogglePause();
    }

    public InputAction movement => input.Gameplay.Movement;
    public Vector2 movementVector2 => input.Gameplay.Movement.ReadValue<Vector2>();
    public bool movementTouched => input.Gameplay.Movement.ReadValue<Vector2>() != Vector2.zero;
    public InputAction aim => input.Gameplay.Aim;
    public Vector2 aimVector2 => input.Gameplay.Aim.ReadValue<Vector2>();
    public InputAction jump => input.Gameplay.Jump;
    public InputAction shoot => input.Gameplay.Shoot;
    public InputAction melee => input.Gameplay.Melee;
    public InputAction sprint => input.Gameplay.Sprint;
    public InputAction pause => input.Gameplay.Pause;
    public InputAction heal => input.Gameplay.Heal;





}
