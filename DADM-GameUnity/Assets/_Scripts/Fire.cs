using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Fire : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _rigidbody2D;

    private AudioSource _micAudioSource;

    private PlayerController _player;

    // Start is called before the first frame update
    void Start()
    {
        if (_rigidbody2D == null) _rigidbody2D = GetComponent<Rigidbody2D>();

        _micAudioSource = MicController.audioSource;

        _player = PlayerController.playerController;
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(transform.position, _player.transform.position) < 2.0f)
            GetInputFromPlayer();
    }

    void GetInputFromPlayer()
    {
        GetOutputData();
    }

    void GetOutputData()
    {
        var samples = MicController.samples;

        _micAudioSource.GetOutputData(samples, 0);

        float vals = 0.0f;

        for (int i = 0; i < 128; i++)
        {
            vals += Mathf.Abs(samples[i]);
        }
        vals /= 128.0f;

        if (vals >= 0.1f)
        {
            Debug.Log(vals);
            Destroy(gameObject);
        }
    }

    //TODO create base class for all traps
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController.playerController.TakeDeath();
        }
    }
}
