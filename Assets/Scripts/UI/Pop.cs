using DG.Tweening;
using DG.Tweening.Plugins.Options;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pop : MonoBehaviour
{
    [SerializeField] private bool _isSetUpdate = true;
    [SerializeField] private float _duration = .8f;

    void Start()
    {
        PopIn();
        PopIn2();
    }

    private void OnEnable()
    {
        PopIn();
    }


    private void PopIn()
    {
        transform.localScale = Vector3.zero;
        transform.DOScale(new Vector3(1, 1, 1), _duration).SetEase(Ease.InSine).SetUpdate(_isSetUpdate);
    }

    private void PopIn2()
    {
        transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
        transform.DOScale(new Vector3(1, 1, 1), _duration).SetEase(Ease.OutSine).SetLoops(-1, LoopType.Yoyo); 
    }
    
}
