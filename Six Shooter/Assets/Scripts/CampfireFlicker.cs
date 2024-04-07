using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CampfireFlicker : MonoBehaviour
{
    public float minIntensity = 1f;
    public float maxIntensity = 5f;
    public float flickerSpeed = 0.1f;

    private Light fireLight;

    void Start()
    {
        fireLight = GetComponent<Light>();
        StartCoroutine(Flicker());
    }

    IEnumerator Flicker()
    {
        while (true)
        {
            fireLight.intensity = Random.Range(minIntensity, maxIntensity);
            yield return new WaitForSeconds(flickerSpeed);
        }
    }
}
