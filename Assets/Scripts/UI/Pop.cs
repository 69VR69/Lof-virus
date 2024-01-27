using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pop : MonoBehaviour
{
    void Start()
    {
        PopIn();
    }

    private void OnEnable()
    {
        PopIn();
    }


    private void PopIn()
    {
        transform.localScale = Vector3.zero;
        transform.DOScale(new Vector3(1, 1, 1), .8f).SetEase(Ease.InSine).SetUpdate(true);
    }
    
}
