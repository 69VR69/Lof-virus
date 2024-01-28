using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonMenu : MonoBehaviour
{
    [SerializeField] private int _hoverSoundId = 3;    

    public void HoverSoundEffect()
    {
        Debug.Log("sdfs");
        AudioSystem.instance.PlaySFX(_hoverSoundId);
    }

}
