using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Deck : MonoBehaviour
{
    [SerializeField] private DeckSO _initialDeck;

    public DeckSO Data => _initialDeck;

    public List<Card> RemainingCardPrefabs { get; private set; }

    private void Awake()
    {
        RemainingCardPrefabs = _initialDeck.Cards.ToList();
        RemainingCardPrefabs.Shuffle();
    }

    public List<Card> WithdrawRandomCards(int count)
    {
        if (count > RemainingCardPrefabs.Count)
        {
            Debug.LogError("Not enough cards to withdraw.");
        }

        List<Card> cardInstances = RemainingCardPrefabs.Take(count).Select(card => Instantiate(card)).ToList();

        RemainingCardPrefabs = RemainingCardPrefabs.Skip(count).ToList();

        return cardInstances;
    }

    public List<Card> WithdrawRemainingCards()
    {
        List<Card> cardsInstances = RemainingCardPrefabs.Select(card => Instantiate(card)).ToList();

        RemainingCardPrefabs.Clear();

        return cardsInstances;
    }

    #region IShuffle

    public void Shuffle() => RemainingCardPrefabs.Shuffle();

    #endregion
}
