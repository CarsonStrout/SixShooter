using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthObject : MonoBehaviour
{
    [SerializeField] private float spinSpeed = 50f;
    [SerializeField] private float floatSpeed = 0.5f;
    [SerializeField] private float floatAmount = 0.5f;
    [SerializeField] private int healAmount = 10;
    [SerializeField] private float disappearTime = 5f;
    [SerializeField] private float lerpSpeed = 5f;
    private Vector3 startPos;

    private void Start()
    {
        startPos = transform.position;
    }

    private void Update()
    {
        transform.Rotate(Vector3.up, spinSpeed * Time.deltaTime);
        transform.position = startPos + new Vector3(0, Mathf.Sin(Time.time * floatSpeed) * floatAmount, 0);

        disappearTime -= Time.deltaTime;

        if (disappearTime < 0)
        {
            StartCoroutine(Disappear());
        }
    }

    private IEnumerator Disappear()
    {
        float disappearSpeed = 3f;
        float disappearAmount = 0.5f;

        while (disappearAmount < 1)
        {
            disappearAmount += Time.deltaTime * disappearSpeed;
            transform.localScale = Vector3.Lerp(Vector3.one, Vector3.zero, disappearAmount);
            yield return null;
        }

        Destroy(gameObject);
    }

    public int GetHealAmount()
    {
        return healAmount;
    }
}
