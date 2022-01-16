using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Vector2 _prevPos, _currentPos, _targetPos;
    [SerializeField] private float _smoothness = 0.1f;

    private enum ControlType
    {
        OneByOneKeyboard, PokemonLike, OneByOneTouch
    }

    [SerializeField] private ControlType _controlType = ControlType.OneByOneKeyboard;

    [SerializeField] private float _horizontalInputTouch, _verticalInputTouch;

    public static PlayerController playerController;

    private void Awake()
    {
        playerController = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        _prevPos = transform.position;
        _currentPos = transform.position;
        _targetPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        switch (_controlType)
        {
            case ControlType.OneByOneKeyboard:
                OneByOneKeyboardMovement();
                break;
            case ControlType.PokemonLike:
                PokemonLikeTileMovementSmooth();
                break;
            case ControlType.OneByOneTouch:
                OneByOneTouchMovement();
                break;
        }

        CheckPlayerOnTile();

#if UNITY_EDITOR
        OneByOneKeyboardMovement();
#endif
    }

    private void CheckPlayerOnTile()
    {
        Collider2D[] results = new Collider2D[4];

        if (Physics2D.OverlapBoxNonAlloc(transform.position, Vector2.one * 0.7f, 0, results) <= 1)
        {
            TakeDeath();
        }

        if (transform.position.y > GridManager.gridManager.MaxDistanceForward + 2)
        {
            ResultMenu.HasWon = true;
        }
    }

    //Pokemon-like tile movement
    private void PokemonLikeTileMovementSmooth()
    {
        _currentPos = transform.position;

        if (_targetPos == _currentPos)
        {
            //Fixex floating point innacuracies
            transform.position = _targetPos;
            _prevPos = transform.position;

            //TODO change this to proper input from UI
            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");

            _targetPos = MoveIfAvailable(horizontal, vertical);
        }
        else
        {
            transform.position = Vector2.MoveTowards(transform.position, _targetPos, _smoothness);
        }
    }

    private void OneByOneKeyboardMovement()
    {
        _currentPos = transform.position;

        if (_targetPos == _currentPos)
        {
            //Fixex floating point innacuracies
            transform.position = _targetPos;
            _prevPos = transform.position;

            //TODO change this to proper input from UI
            GetHorizontalInput(out float horizontal);
            GetVerticalInput(out float vertical);

            _targetPos = MoveIfAvailable(horizontal, vertical);
        }
        else
        {
            transform.position = Vector2.MoveTowards(transform.position, _targetPos, _smoothness);
        }
    }

    private void OneByOneTouchMovement()
    {
        _currentPos = transform.position;

        if (_targetPos == _currentPos)
        {
            //Fixex floating point innacuracies
            transform.position = _targetPos;
            _prevPos = transform.position;

            if (_horizontalInputTouch != 0 || _verticalInputTouch != 0) 
            {
                _targetPos = MoveIfAvailable(_horizontalInputTouch, _verticalInputTouch);

                _horizontalInputTouch = _verticalInputTouch = 0;
            }
        }
        else
        {
            transform.position = Vector2.MoveTowards(transform.position, _targetPos, _smoothness);
        }
    }

    private void GetHorizontalInput(out float horizontalInput)
    {
        horizontalInput = 0;
        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            horizontalInput += 1;
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {
            horizontalInput -= 1;
        }
    }

    private void GetVerticalInput(out float verticalInput)
    {
        verticalInput = 0;
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
        {
            verticalInput += 1;
        }
        if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
        {
            verticalInput -= 1;
        }
    }

    public void UpdateInputTouch(InputDirection input)
    {
        switch (input)
        {
            case InputDirection.Up:
                _verticalInputTouch = 1;
                break;
            case InputDirection.Down:
                _verticalInputTouch = -1;
                break;

            case InputDirection.Right:
                _horizontalInputTouch = 1;
                break;
            case InputDirection.Left:
                _horizontalInputTouch = -1;
                break;
        }
    }

    private Vector2 MoveIfAvailable(float horizontal, float vertical)
    {
        Vector2 tryToMove = new Vector2(_targetPos.x + horizontal, _targetPos.y + vertical);

        Collider2D[] results = new Collider2D[4];

        if (Physics2D.OverlapBoxNonAlloc(tryToMove, Vector2.one * 0.7f, 0, results) != 0)
        {
            foreach (var collider in results)
            {
                if (collider != null && !collider.isTrigger)
                {
                    tryToMove = _currentPos;
                    break;
                }
            }
        }

        return tryToMove;
    }

    public void TakeDeath()
    {
        GameManager.gameManager.IsPlayerDead = true;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        _targetPos = _prevPos;
    }
}
