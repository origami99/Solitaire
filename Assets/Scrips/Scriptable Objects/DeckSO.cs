using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "Deck", menuName = "ScriptableObjects/Deck", order = 1)]
public class DeckSO : ScriptableObject
{
    [SerializeField] private Sprite _backFaceSprite;

    [SerializeField] private CardColor _clubsColor;
    [SerializeField] private CardColor _spadesColor;
    [SerializeField] private CardColor _heartsColor;
    [SerializeField] private CardColor _diamondsColor;

    [SerializeField] private List<GameObject> _cardPrefabs;

    public List<Card> Cards => _cardPrefabs.Select(x => x.GetComponent<Card>()).ToList(); 

    public Sprite BackFaceSprite => _backFaceSprite; 
    
    public CardColor ClubsColor => _clubsColor;
    public CardColor SpadesColor => _spadesColor;
    public CardColor HeartsColor => _heartsColor; 
    public CardColor DiamondsColor => _diamondsColor;
}