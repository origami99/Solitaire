using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FoundationPile : Pile
{
    [SerializeField] private CardSuit _pileSuit;

    protected override PlaceCardsMode PlaceCardsMode => PlaceCardsMode.Awake;

    protected override bool EnableColliderWhenEmpty => true;
    protected override bool DisableColliderWhenNotEmpty => true;

    public override bool CanPlaceCardOver(Card card)
    {
        //return true;
        bool isSuitMatching = card.Data.Suit == _pileSuit;
        if (!isSuitMatching) return false;

        if (base.IsPileEmpty)
        {
            return card.IsAce;
        }

        bool isTheNextCard = card.Data.Rank == TopCard.Data.Rank + 1;
        return isTheNextCard;
    }

    protected override List<Card> GetInitialCards() => new List<Card>();

    protected override void SetCardFaces() => base.Cards.ForEach(card => card.ChangeFace(CardFace.Front));

    protected override void OnCardPlaced(Card card, Pile oldPile, Pile newPile)
    {
        base.OnCardPlaced(card, oldPile, newPile);

        if (oldPile is FoundationPile)
        {
            if (!oldPile.Cards.Any())
            {
                oldPile.ToggleCollider(true);
            }
            else
            {
                oldPile.TopCard.Draggable = true;
            }
        }

        if (newPile is FoundationPile)
        {
            if (newPile.Cards.Count == 1)
            {
                newPile.ToggleCollider(false);
            }
            else if (newPile.Cards.Count >= 2)
            {
                newPile.Cards[newPile.Cards.Count - 2].Draggable = false;
            }
        }
    }
}
