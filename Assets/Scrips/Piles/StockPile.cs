using DG.Tweening;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StockPile : Pile
{
    [SerializeField] private DiscardPile _discardPile;

    public override bool CanPlaceCardOver(Card card) => false;

    protected override PlaceCardsMode PlaceCardsMode => PlaceCardsMode.Start;

    protected override List<Card> GetInitialCards() => base.Deck.WithdrawRemainingCards();

    protected override void SetCardFaces() => base.Cards.ForEach(card => card.ChangeFace(CardFace.Back));

    private void OnMouseDown()
    {
        if (_discardPile.Cards.Any())
        {
            _discardPile.TopCard.Draggable = false;
        }

        if (base.Cards.Any())
        {
            base.TopCard.ChangeFace(CardFace.Front, FlipAnimation.Horizontal);

            if (_discardPile.Cards.Count >= 3)
            {
                Card firstCard = _discardPile.Cards[_discardPile.Cards.Count - 1];
                Card secondCard = _discardPile.Cards[_discardPile.Cards.Count - 2];

                Vector3 firstCardNewPos = _discardPile.transform.position + _discardPile.CardsOffset;
                Vector3 secondCardNewPos = _discardPile.transform.position;

                firstCard.Tween = firstCard.transform
                    .DOMove(firstCardNewPos, CardPlaceDuration)
                    .SetEase(Ease.OutQuad);

                secondCard.Tween = secondCard.transform
                    .DOMove(secondCardNewPos, CardPlaceDuration)
                    .SetEase(Ease.OutQuad);
            }

            _discardPile.PlaceCardOver(base.TopCard, onComplete: () => _discardPile.TopCard.Draggable = true);
        }
        else
        {
            for (int i = _discardPile.Cards.Count - 1; i >= 0; i--)
            {
                Card card = _discardPile.Cards[i];

                base.PlaceCardOver(card, notify: false);
            }

            SetCardFaces();
        }
    }
}
