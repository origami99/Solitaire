using UnityEngine;

public class MouseInput : MonoBehaviour
{
    //[SerializeField] private float _doubleClickTime = 0.5f;

    //private IMouseInput _hovered;
    //private float _clickElapsedTime = 999f;

    //private void Update()
    //{
    //    Vector2 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    //    RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero);

    //    IMouseInput component = null;

    //    if (hit.collider != null)
    //    {
    //        component = hit.collider.GetComponent(typeof(IMouseInput)) as IMouseInput;

    //        if (component != null)
    //        {
    //            if (_hovered == null)
    //            {
    //                component.OnMouseRayEnter();
    //            }
    //        }
    //    }
    //    else
    //    {
    //        if (_hovered != null)
    //        {
    //            _hovered.OnMouseRayExit();
    //        }
    //    }

    //    _hovered = component;
    //}

    //public bool IsDoubleClicked { get; private set; }

    //private void Update()
    //{
    //    _clickElapsedTime += Time.deltaTime;
    //}

    //private void OnMouseDown()
    //{
    //    if (_clickElapsedTime < _doubleClickTime)
    //    {
    //        OnDobleClick();
    //    }
    //    else
    //    {
    //        _clikElapsedTime = 0f;
    //    }
    //}
}
