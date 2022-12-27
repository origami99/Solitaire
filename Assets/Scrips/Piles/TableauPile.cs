using DG.Tweening;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TableauPile : Pile
{
    [SerializeField] private int _intialCards;
    [SerializeField] private float _bottomEdgeOffset = 1f;

    protected override PlaceCardsMode PlaceCardsMode => PlaceCardsMode.Awake;

    protected override bool EnableColliderWhenEmpty => true;
    protected override bool DisableColliderWhenNotEmpty => true;

    private Vector3 _intialCardsOffset;

    protected override void Awake()
    {
        base.Awake();

        _intialCardsOffset = base.CardsOffset;
    }

    public override bool CanPlaceCardOver(Card card)
    {
        //return true;

        if (base.IsPileEmpty) return true;

        bool isThePreviousCard = card.Data.Rank == base.TopCard.Data.Rank - 1;
        bool isSameColor = card.Color == base.TopCard.Color;

        return isThePreviousCard && !isSameColor;
    }

    protected override void SetCardFaces()
    {
        for (int i = 0; i < base.Cards.Count; i++)
        {
            Card card = base.Cards[i];

            if (i == base.Cards.Count - 1)
            {
                card.ChangeFace(CardFace.Front);
            }
            else
            {
                card.ChangeFace(CardFace.Back);
            }
        }
    }

    protected override List<Card> GetInitialCards()
    {
        return base.Deck.WithdrawRandomCards(_intialCards);
    }

    protected override void OnCardPlaced(Card card, Pile oldPile, Pile newPile)
    {
        base.OnCardPlaced(card, oldPile, newPile);

        if (oldPile is TableauPile)
        {
            if (!oldPile.Cards.Any())
            {
                oldPile.ToggleCollider(true);
            }
            else if (oldPile.TopCard.Face == CardFace.Back)
            {
                oldPile.TopCard.ChangeFace(CardFace.Front, FlipAnimation.Twitch);
                oldPile.TopCard.Draggable = true;
            }

            ManageCardOffset();
        }

        if (newPile is TableauPile)
        {
            if (newPile.Cards.Count >= 1)
            {
                newPile.ToggleCollider(false);
            }

            ManageCardOffset();
        }
    }

    private void ManageCardOffset()
    {
        Vector3 offset = CalculateCardOffset();

        if (offset.y > base.CardsOffset.y || offset.y > _intialCardsOffset.y)
        {
            base.CardsOffset = offset;
            PlayCardOffsetAnimation();
        }
    }

    private Vector3 CalculateCardOffset()
    {
        float x = this.transform.position.x;

        Vector3 topEdge = this.transform.position;
        Vector3 bottomEdge = Camera.main.ScreenToWorldPoint(new Vector3(x, 0));

        float verticalRange = topEdge.y - bottomEdge.y;
        verticalRange -= _bottomEdgeOffset;

        float unit = verticalRange / (Cards.Count + 1);

        return new Vector3(base.CardsOffset.x, -unit, base.CardsOffset.z);
    }

    private void PlayCardOffsetAnimation()
    {
        foreach (Card card in base.Cards)
        {
            card.transform.DOMove(card.GetPosition(), base.CardPlaceDuration);
        }
    }
}
