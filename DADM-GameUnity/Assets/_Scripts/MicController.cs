using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MicController : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSource;

    private string _selectedMic;

    public static float[] samples = new float[128];

    public static AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        if (Microphone.devices.Length > 0)
        {
            _selectedMic = Microphone.devices[0].ToString();

            if (_audioSource == null) _audioSource = GetComponent<AudioSource>();

            _audioSource.clip = Microphone.Start(_selectedMic, true, 100, AudioSettings.outputSampleRate);
            _audioSource.loop = true;
            audioSource = _audioSource;

            while (!(Microphone.GetPosition(_selectedMic) > 0))
            {
                _audioSource.Play();
            }
        }
        else
        {
            Debug.LogWarning("No mics available");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
