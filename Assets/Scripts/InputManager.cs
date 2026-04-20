using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance;

    [SerializeField] private InputActionAsset inputActions;
    private InputActionMap playerInput;
    public InputAction moveAction;
    public InputAction lookAction;
    public InputAction jumpAction;
    public InputAction interactAction;
    public InputAction fireAction;

    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(Instance );
        }
        else
        {
            Instance = this;
        }

        //DontDestroyOnLoad(gameObject);

        playerInput = inputActions.FindActionMap("Player");

        moveAction = playerInput.FindAction("Move");
        lookAction = playerInput.FindAction("Look");
        jumpAction = playerInput.FindAction("Jump");
        interactAction = playerInput.FindAction("Interact");
        fireAction = playerInput.FindAction("Fire");
    }

    private void OnEnable()
    {
        moveAction.Enable();
        lookAction.Enable();
        jumpAction.Enable();
        interactAction.Enable();
        fireAction.Enable();
    }

    private void OnDisable()
    {
        moveAction.Disable();
        lookAction.Disable();
        jumpAction.Disable();
        interactAction.Disable();
        fireAction.Disable();
    }
}