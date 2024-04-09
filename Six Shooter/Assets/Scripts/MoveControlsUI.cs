using System.Collections;
using UnityEngine;

public class MoveControlsUI : MonoBehaviour
{
    [SerializeField] private GameObject menuUI;
    [SerializeField] private float[] xyz;
    [SerializeField] private float rotationAngle;
    [SerializeField] private float timeToMove = 2.0f; // Total time to move and rotate

    public void MoveUI()
    {
        StartCoroutine(MoveUIRoutine());
    }

    private IEnumerator MoveUIRoutine()
    {
        Vector3 startPos = transform.position;
        Vector3 targetPos = new Vector3(xyz[0], xyz[1], xyz[2]);
        Quaternion startRot = transform.rotation;
        Quaternion targetRot = Quaternion.Euler(0, rotationAngle, 0);

        float currentTime = 0.0f;

        while (currentTime < timeToMove)
        {
            currentTime += Time.deltaTime;
            float progress = currentTime / timeToMove;

            transform.position = Vector3.Lerp(startPos, targetPos, progress);
            transform.rotation = Quaternion.Lerp(startRot, targetRot, progress);

            yield return null;
        }

        transform.position = targetPos;
        transform.rotation = targetRot;

        menuUI.SetActive(true);
    }
}
