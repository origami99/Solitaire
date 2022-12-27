using DG.Tweening;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DiscardPile : Pile
{
    [SerializeField] private StockPile _stockPile;

    private const int MAX_DISPLAYED_CARDS = 3;

    protected override PlaceCardsMode PlaceCardsMode => PlaceCardsMode.Awake;

    public override bool CanPlaceCardOver(Card card) => false;

    protected override List<Card> GetInitialCards() => new List<Card>();

    protected override void SetCardFaces() => base.Cards.ForEach(card => card.ChangeFace(CardFace.Front));

    public override Vector3 GetCardPosition(int cardNumInPile) => cardNumInPile < MAX_DISPLAYED_CARDS
        ? base.GetCardPosition(cardNumInPile)
        : base.GetCardPosition(MAX_DISPLAYED_CARDS - 1);

    protected override void OnCardPlaced(Card card, Pile oldPile, Pile newPile)
    {
        base.OnCardPlaced(card, oldPile, newPile);

        if (oldPile is DiscardPile && oldPile.Cards.Any())
        {
            oldPile.TopCard.Draggable = true;

            if (oldPile.Cards.Count >= 3)
            {
                Card firstCard = oldPile.Cards[oldPile.Cards.Count - 1];
                Card secondCard = oldPile.Cards[oldPile.Cards.Count - 2];

                Vector3 firstCardNewPos = oldPile.transform.position + oldPile.CardsOffset * 2;
                Vector3 secondCardNewPos = oldPile.transform.position + oldPile.CardsOffset;

                firstCard.Tween = firstCard.transform
                    .DOMove(firstCardNewPos, CardPlaceDuration)
                    .SetEase(Ease.OutQuad);

                secondCard.Tween = secondCard.transform
                    .DOMove(secondCardNewPos, CardPlaceDuration)
                    .SetEase(Ease.OutQuad);
            }
        }
    }
}
