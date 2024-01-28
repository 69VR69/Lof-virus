using DG.Tweening;
using UnityEngine;

public class ArrowScript : MonoBehaviour
{

    private Vector3 baseScale = new(20, 20, 20);
    private void OnEnable()
    {
        transform.localScale = Vector3.one * .1f;
        
        var seq = DOTween.Sequence();

        seq.Append(transform.DOScale(baseScale, .4f).SetEase(Ease.InOutBack));
        seq.Append(transform.DOScale(1.2f * baseScale, 0.5f).SetLoops(-1, LoopType.Yoyo));
    }
}
