using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleScript : MonoBehaviour
{
    ParticleSystem particle;
    private void Start()
    {
        particle = GetComponent<ParticleSystem>();
        particle.Play();

        Destroy(particle, 1f);

    }
}
