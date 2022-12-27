public interface IMouseInput 
{
    void OnClick();
    void OnDoubleClick();

    void OnMouseRayEnter();
    void OnMouseRayExit();

    void OnDragStart();
    void OnDragEnd();
}