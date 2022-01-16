using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIMovementButton : MonoBehaviour, IPointerDownHandler
{
    public InputDirection direction = InputDirection.Up;

    [SerializeField] private PlayerController _playerController;

    public delegate void ButtonPressed(InputDirection direction);
    public event ButtonPressed OnButtonPressed;

    // Start is called before the first frame update
    void Start()
    {
        _playerController = PlayerController.playerController;
        OnButtonPressed += _playerController.OnInputTouch;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        OnButtonPressed?.Invoke(direction);
    }
}

public enum InputDirection
{
    None = 0, 
    Up = 1 << 0, 
    Down = 1 << 1,
    Right = 1 << 2, 
    Left = 1 << 3
}
