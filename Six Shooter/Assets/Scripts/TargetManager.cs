using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using TMPro;

public class TargetManager : MonoBehaviour
{
    [SerializeField] private ParticleSystem targetParticle;
    [SerializeField] private Transform[] randPos;
    private int prevPos = 0;
    [SerializeField] private AudioSource targetHit;
    [SerializeField] private GameObject timerUI;
    [SerializeField] private GameObject endUI;
    [SerializeField] private TextMeshProUGUI hitText;
    [SerializeField] private TextMeshProUGUI accText;
    [SerializeField] private GameObject startUI;
    [SerializeField] private TextMeshProUGUI startText;
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private float timerLength = 60;
    [SerializeField] private AudioSource startAudio;
    [SerializeField] private ShootWeapon shootWeapon;
    private bool started = false;
    private bool ended = false;
    private bool gameOver = false;

    private float hit = 0;
    private float acc = 0;

    private MeshRenderer meshRenderer;
    private CapsuleCollider capsuleCollider;

    private void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        capsuleCollider = GetComponent<CapsuleCollider>();

        meshRenderer.enabled = false;
        capsuleCollider.enabled = false;

        timerUI.SetActive(false);
        endUI.SetActive(false);
        startUI.SetActive(false);

        timerText.text = timerLength.ToString();

        StartCoroutine(StartGame());
    }

    private void Update()
    {
        if (started && !ended)
        {
            if (!gameOver)
            {
                timerLength -= Time.deltaTime;
                timerText.text = timerLength.ToString("F0");

                if (timerLength <= 0)
                    gameOver = true;
            }
            else
            {
                meshRenderer.enabled = false;
                capsuleCollider.enabled = false;

                acc = Mathf.RoundToInt((hit / shootWeapon.numShots) * 100);

                hitText.text = "Targets Hit: " + hit.ToString();
                accText.text = "Accuracy: " + acc.ToString() + "%";

                timerUI.SetActive(false);
                endUI.SetActive(true);

                ended = true;
            }
        }

    }

    IEnumerator StartGame()
    {
        yield return new WaitForSeconds(3);

        startUI.SetActive(true);
        startText.text = "3";

        startAudio.Play();

        yield return new WaitForSeconds(1);

        startText.text = "2";

        yield return new WaitForSeconds(1);

        startText.text = "1";

        yield return new WaitForSeconds(1);

        timerUI.SetActive(true);
        started = true;

        int rand = Random.Range(0, randPos.Length);
        prevPos = rand;
        gameObject.transform.position = randPos[rand].position;
        meshRenderer.enabled = true;
        capsuleCollider.enabled = true;

        startText.text = "Draw";

        yield return new WaitForSeconds(1);

        startUI.SetActive(false);
    }

    public void TargetHit()
    {
        hit++;
    }

    public void SpawnTarget()
    {
        Instantiate(targetParticle, gameObject.transform.position, gameObject.transform.rotation);
        targetHit.Play();
        int rand = Random.Range(0, randPos.Length);
        while (rand == prevPos)
        {
            rand = Random.Range(0, randPos.Length);
        }

        prevPos = rand;
        gameObject.transform.position = randPos[rand].position;
    }
}
