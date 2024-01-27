using System;
using UnityEngine;
using UnityEngine.Events;

public class Clickable : MonoBehaviour
{

    [SerializeField] private OnClick _onClick = new();

    private void Start()
    {
        // _onClick.AddListener(e => Debug.Log("Clicked"));
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // Check if the mouse is over this object
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out var hit, 100))
            {
                if (hit.transform == transform)
                {
                    _onClick.Invoke(new OnClickEvent());
                }
            }
        }
    }
}

#region Events
[Serializable] public class OnClick : UnityEvent<OnClickEvent> { }

public class OnClickEvent
{
    public float ClickTime;
    public OnClickEvent()
    {
        ClickTime = Time.time;
    }
}
#endregion