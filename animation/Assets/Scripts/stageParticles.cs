using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class stageParticles : MonoBehaviour
{
    ParticleSystem particleSystem;
    ParticleSystem.Particle[] particles;

    AudioSource audioSource;

    static int radius = 36;
    public float[] _samples = new float[2048];
    int sampleindex = 0;
    
    void OnEnable()
    {
        particleSystem = GetComponent<ParticleSystem>();
        ParticleSystem.EmitParams ep = new ParticleSystem.EmitParams();

        audioSource = GameObject.Find("musicSource").GetComponent<AudioSource>();
        
        for (int r = 0; r < radius; r++)
        {
            for (int j = 0; j < 360; j += 4)
            {
                Vector3 position;
                position.x = r * Mathf.Cos((float)j * Mathf.PI / 180f);
                position.y = 0.25f;
                position.z = r * Mathf.Sin((float)j * Mathf.PI / 180f);

                ep.position = position;
                particleSystem.Emit(ep, 1);
            }
        }

        particles = new ParticleSystem.Particle[radius * 90];
        particleSystem.GetParticles(particles);
    }

    void Update()
    {
        audioSource.GetSpectrumData(_samples, 0, FFTWindow.Blackman);

        for (int r = 0; r < radius; r++)  
        {
            for (int j = 0; j < 90; j++)
            {
                float Xpos = r * Mathf.Cos((float)j * Mathf.PI / 45f);
                float Zpos = r * Mathf.Sin((float)j * Mathf.PI / 45f);

                if (r * 90 + j >= 2048)
                    sampleindex = r * 90 + j - 2048;
                else sampleindex = r * 90 + j;
                float Ypos = _samples[sampleindex] * 90f;

                particles[r * 90 + j].position = new Vector3(Xpos, Mathf.PerlinNoise(Xpos * 0.1f, Zpos * 0.1f) * 6f - Ypos + 25f, Zpos);
                
            }
        }
        particleSystem.SetParticles(particles);
    }
}
