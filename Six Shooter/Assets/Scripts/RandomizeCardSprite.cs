using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RandomizeCardSprite : MonoBehaviour
{
    [SerializeField] private Sprite[] cardSprites;
    private void OnEnable()
    {
        GetComponent<Image>().sprite = cardSprites[Random.Range(0, cardSprites.Length)];
    }
}
