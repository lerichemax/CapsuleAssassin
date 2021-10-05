using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stabbing : MonoBehaviour
{

    private AudioSource _sound;

    private void Awake()
    {
        _sound = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Target"))
        {
            if (other.GetComponent<WanderAroundSpawn>().enabled)
            {
                _sound.Play();
                other.gameObject.GetComponent<Target>().GetKilled();
            }
        }
    }
}
