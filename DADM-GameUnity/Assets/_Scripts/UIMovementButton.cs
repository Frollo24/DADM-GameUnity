using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIMovementButton : MonoBehaviour, IPointerDownHandler
{
    public InputDirection direction = InputDirection.Up;

    [SerializeField] private PlayerController _playerController;

    // Start is called before the first frame update
    void Start()
    {
        _playerController = FindObjectOfType<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _playerController.UpdateInputTouch(direction);
    }
}

public enum InputDirection
{
    Up, Down, Right, Left
}
