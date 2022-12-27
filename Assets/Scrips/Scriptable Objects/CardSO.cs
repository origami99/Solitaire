using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "Card", menuName = "ScriptableObjects/Card", order = 1)]
public class CardSO : ScriptableObject
{
    [SerializeField] private int _rank;
    [SerializeField] private CardSuit _suit;
    [SerializeField] private Sprite _frontFaceSprite;

    public int Rank => _rank;
    public CardSuit Suit => _suit;
    public Sprite FrontFaceSprite => _frontFaceSprite;
}