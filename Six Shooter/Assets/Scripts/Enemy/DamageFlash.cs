using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageFlash : MonoBehaviour
{
    [SerializeField] private GameObject visualComponent;
    [SerializeField] private Material defaultMAT;
    [SerializeField] private Material damageMAT;
    [SerializeField] private float flashDuration = 0.1f;
    [SerializeField] private float transitionDuration = 0.5f;

    private void Start()
    {
        defaultMAT = visualComponent.GetComponent<MeshRenderer>().material;
    }

    public void Flash()
    {
        StartCoroutine(FlashCoroutine());
    }

    private IEnumerator FlashCoroutine()
    {
        visualComponent.GetComponent<MeshRenderer>().material = damageMAT;
        yield return new WaitForSeconds(flashDuration);

        // Lerp from damageMAT to defaultMAT
        float t = 0;
        while (t < transitionDuration)
        {
            t += Time.deltaTime;
            visualComponent.GetComponent<MeshRenderer>().material.Lerp(damageMAT, defaultMAT, t);
            yield return null;
        }
    }
}
