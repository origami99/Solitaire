using DG.Tweening;
using System;
using UnityEngine;

public enum FlipAnimation
{
    None,
    Horizontal,
    Vertical,
    Twitch
}

public class Card : MonoBehaviour
{
    [SerializeField] private CardSO _data;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private Collider2D _collider;
    [SerializeField] private CardDrag _drag;

    private Sprite _frontFaceSprite;
    private Sprite _backFaceSprite;

    private Deck _deck;

    private bool _draggable;

    public CardDrag Drag => _drag;
    public Pile Pile { get; set; }
    public int IndexInPile { get; set; }

    public bool Draggable
    {
        get => _draggable;
        set
        {
            _collider.enabled = value;
            _drag.enabled = value;
        }
    }

    public int SortingOrder
    {
        get => _spriteRenderer.sortingOrder;
        set => _spriteRenderer.sortingOrder = value;
    }

    public Tween Tween { get; set; }

    public CardSO Data => _data;

    public CardColor Color
    {
        get
        {
            switch (_data.Suit)
            {
                case CardSuit.Clubs: return _deck.Data.ClubsColor;
                case CardSuit.Spades: return _deck.Data.SpadesColor;
                case CardSuit.Hearts: return _deck.Data.HeartsColor;
                case CardSuit.Diamonds: return _deck.Data.DiamondsColor;
            }

            throw new Exception();
        }
    }

    public CardFace Face { get; private set; }

    public bool IsAce => Data.Rank == 1;
    public bool IsKing => Data.Rank == 13;

    private void Awake()
    {
        _deck = FindObjectOfType<Deck>();

        _frontFaceSprite = this.Data.FrontFaceSprite;
        _backFaceSprite = _deck.Data.BackFaceSprite;
    }

    // ChangeFace(CardFace face, CardFlip cardFlip = CardFlip.Horizontal, bool force = false)
    public void ChangeFace(CardFace face, FlipAnimation animation = FlipAnimation.None)
    {
        Face = face;

        switch (animation)
        {
            case FlipAnimation.None: SetFaceSprite(face); break;
            case FlipAnimation.Horizontal: HorizontalAnimation(face); break;
            case FlipAnimation.Vertical: VerticalAnimation(face); break;
            case FlipAnimation.Twitch: TwitchAnimation(face); break;
        }
    }

    private void SetFaceSprite(CardFace face)
    {
        switch (face)
        {
            case CardFace.Front: _spriteRenderer.sprite = _frontFaceSprite; break;
            case CardFace.Back: _spriteRenderer.sprite = _backFaceSprite; break;
        }
    }

    public Vector3 GetPosition() => Pile.GetCardPosition(IndexInPile);

    private void HorizontalAnimation(CardFace face)
    {
        float origScaleX = this.transform.localScale.x;

        DOTween.Sequence()
            .Append
            (
                this.transform.DOScaleX(0, 0.25f)
                    .SetEase(Ease.InSine)
                    .OnComplete(() =>
                    {
                        SetFaceSprite(face);
                    })
            )
            .Append
            (
                this.transform
                    .DOScaleX(origScaleX, 0.25f)
                    .SetEase(Ease.InSine)
            );
    }

    private void VerticalAnimation(CardFace face)
    {
        float origScaleY = this.transform.localScale.y;

        DOTween.Sequence()
            .Append
            (
                this.transform.DOScaleY(0, 0.25f)
                    .SetEase(Ease.InSine)
                    .OnComplete(() =>
                    {
                        SetFaceSprite(face);
                    })
            )
            .Append
            (
                this.transform
                    .DOScaleY(origScaleY, 0.25f)
                    .SetEase(Ease.InSine)
            );
    }

    private void TwitchAnimation(CardFace face)
    {
        float origPosX = this.transform.position.x;

        DOTween.Sequence()
            .Append
            (
                this.transform.DOMoveX(origPosX + 0.25f, 0.10f)
                    .SetEase(Ease.Linear)
                    .OnComplete(() =>
                    {
                        SetFaceSprite(face);
                    })
            )
            .Append
            (
                this.transform
                    .DOMoveX(origPosX, 0.10f)
                    .SetEase(Ease.Linear)
            );
    }
}
