using UnityEngine;

public class CardDoubleClick : MonoBehaviour
{
    [SerializeField] private Card _card;
    [SerializeField] private float _doubleClickTime = 0.5f;

    private float _elapsedTime = 999f;

    public bool IsDoubleClicked { get; private set; }

    private void Update()
    {
        _elapsedTime += Time.deltaTime;
    }

    private void OnMouseDown()
    {
        if (_elapsedTime < _doubleClickTime)
        {
            OnDobleClick();
        }
        else
        {
            _elapsedTime = 0f;
        }
    }

    private void OnDobleClick()
    {
        foreach (FoundationPile foundationPile in PileManager.Instance.FoundationPiles)
        {
            bool success = foundationPile.TryPlaceCardOver(_card, 
                onComplete: () => IsDoubleClicked = false);

            if (success)
            {
                IsDoubleClicked = true;
                break;
            }
        }
    }
}
