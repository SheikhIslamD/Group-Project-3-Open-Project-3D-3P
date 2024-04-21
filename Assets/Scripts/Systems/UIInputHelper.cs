using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;

public class UIInputHelper : MonoBehaviour
{
    private InputSystemUIInputModule ui;
    private EventSystem eventSystem;
    private Selectable defaultButton;

    private void Start()
    {
        eventSystem = GetComponent<EventSystem>();
        ui = GetComponent<InputSystemUIInputModule>();

        defaultButton = eventSystem.firstSelectedGameObject.GetComponent<Selectable>();

        ui.move.action.performed += ctx => GrabCursor(ctx);
    }

    private void GrabCursor(InputAction.CallbackContext ctx)
    {
        if (eventSystem.currentSelectedGameObject == null && defaultButton != null) defaultButton.Select();

    }
}
