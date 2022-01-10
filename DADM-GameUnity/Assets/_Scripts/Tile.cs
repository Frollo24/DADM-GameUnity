using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Tile : MonoBehaviour
{
    [SerializeField] private Color _baseColor, _offsetColor;
    [SerializeField] private SpriteRenderer _renderer;
    [SerializeField] private BoxCollider2D _boxCollider2D;

    public void Init(bool isOffset, bool isStartOrEnd, uint mapWidth = 9, uint mapHeight = 55)
    {
        _renderer.color = isOffset ? _offsetColor : _baseColor;
        if (isStartOrEnd && 
            (transform.position.x == 0 || transform.position.x == mapWidth - 1
            || transform.position.y == 0 || transform.position.y == mapHeight - 1))
        {
            _boxCollider2D.isTrigger = false;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        if (!_boxCollider2D.isTrigger)
        {
            Gizmos.DrawWireCube(transform.position, Vector3.one * 0.9f);
        }
    }
}
