using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sing : MonoBehaviour
{
    public AudioSource audioSource;
    public float[] samples;
    public float[] _samples;
    public GameObject[] cubeLight;
    float[] _spectrum;

    AudioClip singClip;
    Material material0;
    Material material1;
    Material material2;
    Material material3;

    string usedMic = null;
    const int sampleCount = 1024;
    float rms;
    float dB;

    // Start is called before the first frame update
    void Start()
    {
        samples = new float[sampleCount];
        _samples = new float[sampleCount];
        _spectrum = new float[sampleCount];
        
        audioSource = GetComponent<AudioSource>();

        material0 = cubeLight[0].GetComponent<MeshRenderer>().materials[0];
        material1 = cubeLight[1].GetComponent<MeshRenderer>().materials[0];
        material2 = cubeLight[2].GetComponent<MeshRenderer>().materials[0];
        material3 = cubeLight[2].GetComponent<MeshRenderer>().materials[1];

        //if (Microphone.devices.Length > 3)
        //{
             usedMic = Microphone.devices[0].ToString();
             singClip = Microphone.Start(usedMic, true, 10, AudioSettings.outputSampleRate);
             audioSource.clip = singClip;

             while (!(Microphone.GetPosition(usedMic) > 0)) { }
             audioSource.Play();
        //}
        
    }

    // Update is called once per frame
    void Update()
    {
        print("name" + usedMic);
        print("yyy"+GetPitch().ToString("F0"));
        print("w" + Microphone.GetPosition(usedMic));

        float pitchScale = GetPitch() / 1000;
        if (pitchScale < 0.5f)
            pitchScale += 0.2f;

        Color lightColor = new Color(1f, pitchScale, 0.208f);
        material0.SetColor("_EmissionColor", lightColor);
        material1.SetColor("_EmissionColor", lightColor);
        material2.SetColor("_EmissionColor", lightColor);
        material3.SetColor("_EmissionColor", lightColor);
    }

    float GetPitch()
    {
        audioSource.GetOutputData(samples, 0);

        float sum = 0f;
        for (int i = 0; i < sampleCount; i++)
        {
            _samples[i] = samples[i];
            _samples[i] *= 10;
            sum += _samples[i] * _samples[i];
        }
        rms = Mathf.Sqrt(sum / sampleCount);
        dB = 20 * Mathf.Log10(rms / 0.1f);
        if (dB < -160)
            dB = -160;

        audioSource.GetSpectrumData(_spectrum, 0, FFTWindow.Blackman);

        float maxSpectrum = 0;
        var maxIndex = 0;
        for (int i = 0; i < sampleCount; i++)
        {
            if (!(_spectrum[i] > maxSpectrum) || !(_spectrum[i] > 0.02f))
                continue;
            maxSpectrum = _spectrum[i];
            maxIndex = i;
        }

        float freq = maxIndex;
        if (maxIndex > 0 && maxIndex < sampleCount - 1)
        {
            float dL = _spectrum[maxIndex - 1] / _spectrum[maxIndex];
            float dR = _spectrum[maxIndex + 1] / _spectrum[maxIndex];
            freq += 0.5f * (dR * dR - dL * dL);
        }
        
        return freq * (AudioSettings.outputSampleRate / 2) / sampleCount;
    }
}
