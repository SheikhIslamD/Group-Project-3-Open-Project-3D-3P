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

    private Selectable[] selectables;

    private void Start()
    {
        eventSystem = GetComponent<EventSystem>();
        ui = GetComponent<InputSystemUIInputModule>();
        //selectables = FindObjectsOfType<Selectable>();

        defaultButton = eventSystem.firstSelectedGameObject.GetComponent<Selectable>();
        defaultButton.Select();

        //ui.move.action.performed += ctx => GrabCursor(ctx);
        //ui.point.action.performed += ctx => MouseMoved(ctx);
    }

    private void GrabCursor(InputAction.CallbackContext ctx)
    {
        if (eventSystem.currentSelectedGameObject == null && defaultButton != null) defaultButton.Select();

    }
    


    /*
    private void MouseMoved(InputAction.CallbackContext ctx)
    {

    }
     */
}
