using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonActivation : MonoBehaviour
{
    public enum ButtonOptions
    {
        Play,
        Quit,
        Wave,
        Target,
        Back,
        Restart,
        Menu
    }

    [SerializeField] private ButtonController buttonController;
    [SerializeField] private ButtonOptions buttonOptions;

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Bullet")
        {
            switch (buttonOptions)
            {
                case ButtonOptions.Play:
                    buttonController.Play();
                    break;
                case ButtonOptions.Quit:
                    buttonController.Quit();
                    break;
                case ButtonOptions.Wave:
                    buttonController.WaveMode();
                    break;
                case ButtonOptions.Target:
                    buttonController.TargetMode();
                    break;
                case ButtonOptions.Back:
                    buttonController.Back();
                    break;
                case ButtonOptions.Restart:
                    buttonController.Restart();
                    break;
                case ButtonOptions.Menu:
                    buttonController.Menu();
                    break;
            }
        }
    }
}
