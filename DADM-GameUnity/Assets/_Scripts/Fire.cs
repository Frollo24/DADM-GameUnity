using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(AudioSource))]
public class Fire : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _rigidbody2D;

    private AudioSource _audioSource;
    private string _selectedMic;

    private PlayerController _player;
    private float[] samples = new float[128];

    // Start is called before the first frame update
    void Start()
    {
        if (_rigidbody2D == null) _rigidbody2D = GetComponent<Rigidbody2D>();
        
        _audioSource = GetComponent<AudioSource>();

        Debug.Log(Microphone.devices.Length);

        if (Microphone.devices.Length > 0)
        {
            _selectedMic = Microphone.devices[0].ToString();
        }
        _audioSource.clip = Microphone.Start(_selectedMic, true, 100, AudioSettings.outputSampleRate);
        _audioSource.loop = true;
        while (!(Microphone.GetPosition(_selectedMic) > 0))
        {
            _audioSource.Play();
        }

        _player = FindObjectOfType<PlayerController>();
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
        //BS avoiding 0 output from samples
        foreach(var source in FindObjectsOfType<AudioSource>())
        {
            source.GetOutputData(samples, 0);
            if (samples[0] != 0f)
            {
                break;
            }
        }

        float vals = 0.0f;

        for (int i = 0; i < 128; i++)
        {
            vals += Mathf.Abs(samples[i]);
        }
        vals /= 128.0f;

        if (vals >= 0.01f)
        {
            Debug.Log(vals);
            Destroy(gameObject);
        }
        //gameObject.transform.localScale = new Vector3(1.0f + (vals * 10.0f), 1.0f, 1.0f);
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerController>()?.TakeDeath();
        }
    }
}
