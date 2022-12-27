using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DG.Tweening;
using System;

public enum PlaceCardsMode
{
    Awake,
    Start
}

public abstract class Pile : MonoBehaviour
{
    public static event Action<Card, Pile, Pile> OnCardMoved;

    [SerializeField] private Collider2D _collider;
    [SerializeField] private bool _isDraggable;
    [SerializeField] private Vector3 _cardsOffset;
    [SerializeField] private float _cardPlaceDuration = 0.5f;

    protected abstract PlaceCardsMode PlaceCardsMode { get; }

    protected virtual bool EnableColliderWhenEmpty => false;
    protected virtual bool DisableColliderWhenNotEmpty => false;

    public Deck Deck { get; private set; }
    public List<Card> Cards { get; } = new List<Card>();

    public Card TopCard => Cards.LastOrDefault();
    public bool IsPileEmpty => !Cards.Any();

    public Vector3 CardsOffset
    {
        get => _cardsOffset;
        protected set => _cardsOffset = value;
    }

    public float CardPlaceDuration => _cardPlaceDuration;


    protected virtual void Awake()
    {
        Deck = FindObjectOfType<Deck>();

        if (PlaceCardsMode == PlaceCardsMode.Awake)
        {
            PlaceInitialCards(GetInitialCards());
        }

        OnCardMoved += OnCardPlaced;
    }

    private void Start()
    {
        if (PlaceCardsMode == PlaceCardsMode.Start)
        {
            PlaceInitialCards(GetInitialCards());
        }

        SetCardFaces();

        Cards.ForEach(x => x.Draggable = false);

        if (TopCard != null)
        {
            TopCard.Draggable = _isDraggable;
        }
    }

    private void OnDestroy() => OnCardMoved -= OnCardPlaced;

    public void Shuffle() => Cards.Shuffle();

    public abstract bool CanPlaceCardOver(Card card);
    protected abstract List<Card> GetInitialCards();
    protected abstract void SetCardFaces();
    
    protected virtual void PlaceInitialCards(List<Card> cards)
    {
        foreach (Card card in cards)
        {
            PlaceCardOver(card, force: true, notify: false);
        }
    }

    protected virtual void OnCardPlaced(Card card, Pile oldPile, Pile newPile) { }

    public bool TryPlaceCardOver(Card card, bool force = false, Action onComplete = null, bool notify = true)
    {
        if (CanPlaceCardOver(card))
        {
            PlaceCardOver(card, force, onComplete, notify);
            return true;
        }

        return false;
    }

    public void PlaceCardOver(Card card, bool force = false, Action onComplete = null, bool notify = true)
    {
        Pile oldPile = card.Pile;

        if (card.Pile != null && card.Pile.Cards.Any())
        {
            card.Pile.Cards.Remove(card);
        }

        Vector3 position = GetCardPosition(Cards.Count);

        if (force)
        {
            card.transform.position = position;
        }
        else
        {
            card.Tween = card.transform
                .DOMove(position, _cardPlaceDuration)
                .OnComplete(() => onComplete())
                .SetEase(Ease.OutQuad);
        }

        card.Pile = this;
        card.Draggable = _isDraggable;
        card.transform.parent = this.transform;
        card.IndexInPile = Cards.Count;
        card.SortingOrder = Cards.Count;

        Cards.Add(card);

        if (notify)
        {
            OnCardMoved?.Invoke(card, oldPile, this);
        }
    }

    public virtual Vector3 GetCardPosition(int cardIndexInPile)
    {
        return this.transform.position + CardsOffset * cardIndexInPile;
    }

    public void ToggleCollider(bool enable)
    {
        _collider.enabled = enable;
    }
}
