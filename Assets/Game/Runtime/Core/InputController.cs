using UnityEngine;
using UnityEngine.InputSystem;

public class InputController : MonoBehaviour
{
    [SerializeField]
    private GameController gameController;
    [SerializeField]
    private UIController ui;
    private GameInput input;

    private void Awake()
    {
        input = new();
    }

    private void OnEnable()
    {
        input.Enable();
        input.UI.Cancel.performed += OnEscape;
    }


    private void OnEscape(InputAction.CallbackContext ctx)
    {
        Debug.Log("Close this please.");
        ui.EscapePressed();
    }

}
