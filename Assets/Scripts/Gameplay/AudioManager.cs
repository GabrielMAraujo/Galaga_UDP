using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioClip projectileClip;
    public AudioClip explosionClip;

    private AudioSource source;

    private void Start()
    {
        source = GetComponent<AudioSource>();
    }

    public void PlayProjectile()
    {
        source.PlayOneShot(projectileClip);
    }

    public void PlayExplosion()
    {
        source.PlayOneShot(explosionClip);
    }
}
