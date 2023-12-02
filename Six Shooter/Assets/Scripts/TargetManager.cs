using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class TargetManager : MonoBehaviour
{
    [SerializeField] private ParticleSystem targetParticle;
    [SerializeField] private Transform[] randPos;
    [SerializeField] private AudioSource targetHit;

    private void Start()
    {
        SpawnTarget();
    }

    public void SpawnTarget()
    {
        Instantiate(targetParticle, gameObject.transform.position, gameObject.transform.rotation);
        targetHit.Play();
        int rand = Random.Range(0, randPos.Length);
        gameObject.transform.position = randPos[rand].position;
    }
}
