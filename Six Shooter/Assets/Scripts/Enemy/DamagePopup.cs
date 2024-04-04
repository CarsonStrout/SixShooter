using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class DamagePopup : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private float disappearTime = 1f;

    [SerializeField] private TextMeshProUGUI textMesh;
    private Color textMeshColor;

    private void Start()
    {
        textMeshColor = textMesh.color;
    }

    private void Update()
    {
        transform.position += new Vector3(0, speed, 0) * Time.deltaTime;

        disappearTime -= Time.deltaTime;

        if (disappearTime < 0)
        {
            // start disappearing
            float disappearSpeed = 3f;
            textMeshColor.a -= disappearSpeed * Time.deltaTime;
            textMesh.color = textMeshColor;

            if (textMeshColor.a < 0)
            {
                Destroy(gameObject);
            }
        }
    }

    private void LateUpdate()
    {
        transform.rotation = Quaternion.LookRotation(transform.position - Camera.main.transform.position);
    }

    public void SetDamage(int damage)
    {
        textMesh.text = damage.ToString();
    }
}
