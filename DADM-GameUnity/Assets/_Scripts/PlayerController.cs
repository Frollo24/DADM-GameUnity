using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Vector2 _prevPos, _currentPos, _targetPos;
    [SerializeField] private float _smoothness = 0.1f;

    private enum ControlType
    {
        PokemonLike, OneByOne
    }

    [SerializeField] private ControlType _controlType = ControlType.OneByOne;
    [SerializeField] private InputDirection _touchDirection;
    //TODO process input via command pattern
    public void OnInputTouch(InputDirection input) => _touchDirection |= input;

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
            case ControlType.PokemonLike:
                PokemonLikeTileMovementSmooth();
                break;
            case ControlType.OneByOne:
                OneByOneMovement();
                break;
        }

        CheckPlayerOnTile();
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
            //Fixes floating point innacuracies
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

    private void OneByOneMovement()
    {
        _currentPos = transform.position;

        if (_targetPos == _currentPos)
        {
            //Fixes floating point innacuracies
            transform.position = _targetPos;
            _prevPos = transform.position;

            GetHorizontalInput(out float horizontal);
            GetVerticalInput(out float vertical);

            _targetPos = MoveIfAvailable(horizontal, vertical);
            if(_touchDirection != InputDirection.None)
                _touchDirection = InputDirection.None;
        }
        else
        {
            transform.position = Vector2.MoveTowards(transform.position, _targetPos, _smoothness);
        }
    }

    private void GetHorizontalInput(out float horizontalInput)
    {
        horizontalInput = 0;
        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D) 
            || (_touchDirection & InputDirection.Right) != InputDirection.None)
        {
            horizontalInput += 1;
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A) 
            || (_touchDirection & InputDirection.Left) != InputDirection.None)
        {
            horizontalInput -= 1;
        }
    }

    private void GetVerticalInput(out float verticalInput)
    {
        verticalInput = 0;
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)
            || (_touchDirection & InputDirection.Up) != InputDirection.None)
        {
            verticalInput += 1;
        }
        if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S)
            || (_touchDirection & InputDirection.Down) != InputDirection.None)
        {
            verticalInput -= 1;
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
