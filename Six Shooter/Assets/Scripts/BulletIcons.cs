using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BulletIcons : MonoBehaviour
{
    private BulletManager bulletManager => BulletManager.Instance;
    public Image[] bulletIcons;
    [SerializeField] private Sprite[] bulletSprites;

    private void Start()
    {
        // Assign bullet icons to the sprite based off the bullet type
        for (int i = 0; i < bulletIcons.Length; i++)
        {
            bulletIcons[i].sprite = bulletSprites[(int)bulletManager.GetBulletType(i)];
        }
    }

    public void UpdateBulletIcons()
    {
        // Assign bullet icons to the sprite based off the bullet type
        for (int i = 0; i < bulletIcons.Length; i++)
        {
            bulletIcons[i].sprite = bulletSprites[(int)bulletManager.GetBulletType(i)];
        }
    }

    public void BlackOutIcon(int index)
    {
        bulletIcons[index].color = new Color(0, 0, 0, 1f);
    }

    public void LightUpIcon(int index)
    {
        bulletIcons[index].color = new Color(1, 1, 1, 1f);
    }
}
