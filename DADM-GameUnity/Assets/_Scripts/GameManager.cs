using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private bool _isPlayerDead = false;
    public bool IsPlayerDead {
        get { return _isPlayerDead; }
        set { _isPlayerDead = value; } 
    }

    public static GameManager gameManager;

    private void Awake()
    {
        if(gameManager == null)
        {
            gameManager = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(gameManager);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawIcon(transform.position, "GameManager Icon", true);
    }
}
