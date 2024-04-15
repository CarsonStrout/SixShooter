using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainController : MonoBehaviour
{
    [SerializeField] private Transform startPoint, stopPoint, endPoint;
    [SerializeField] private float speed = 10f;
    [SerializeField] private float stopDuration = 30f; // Duration in seconds for which the train will stop
    [SerializeField] private AudioSource engineSound, trainWhistle;
    [SerializeField] private ParticleSystem smokeEffect;

    private void Start()
    {
        gameObject.transform.position = startPoint.position;
        smokeEffect.Stop();
        StartCoroutine(MoveTrain());
    }

    private IEnumerator MoveTrain()
    {
        while (true)
        {
            yield return new WaitForSeconds(15f);

            engineSound.Play();
            smokeEffect.Play();

            yield return StartCoroutine(MoveToPosition(startPoint.position, stopPoint.position, speed));

            engineSound.Stop();
            trainWhistle.Play();

            yield return new WaitForSeconds(stopDuration);

            trainWhistle.Play();

            yield return new WaitForSeconds(3f);

            engineSound.Play();

            yield return StartCoroutine(MoveToPosition(stopPoint.position, endPoint.position, speed));

            engineSound.Stop();
            smokeEffect.Stop();

            yield return new WaitForSeconds(30f);

            gameObject.transform.position = startPoint.position;
        }
    }

    private IEnumerator MoveToPosition(Vector3 start, Vector3 end, float speed)
    {
        float journeyLength = Vector3.Distance(start, end);
        float startTime = Time.time;
        float fractionOfJourney = 0f;

        while (fractionOfJourney < 1f)
        {
            float distCovered = (Time.time - startTime) * speed;
            fractionOfJourney = distCovered / journeyLength;
            gameObject.transform.position = Vector3.Lerp(start, end, fractionOfJourney);
            yield return null;
        }
    }
}
