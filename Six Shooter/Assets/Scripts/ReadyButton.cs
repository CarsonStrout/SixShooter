using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReadyButton : MonoBehaviour
{
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Bullet")
        {
            gameObject.transform.parent.GetComponent<MoveControlsUI>().MoveUI();
            gameObject.SetActive(false);
        }
    }
}
