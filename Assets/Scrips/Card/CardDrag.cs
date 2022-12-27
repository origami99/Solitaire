using DG.Tweening;
using System.Linq;
using UnityEngine;
using System.Collections.Generic;

public class CardDrag : MonoBehaviour
{
    [SerializeField] private Card _card;
    [SerializeField] private CardDoubleClick _cardDoubleClick;
    [SerializeField] private Color _mouseOverColor;
    [SerializeField] private float _returnDuration;

    private static bool _draggingAnyCard = false;

    private Camera _camera;

    private Material _material;
    private Color _originalColor;

    private List<Card> _selectedCards = new List<Card>();

    private bool _dragging = false;

    private float _distance;
    private Vector3 _mouseOffset;

    private Tween _returnTween;

    private List<Component> _hoveredElements = new List<Component>();

    private Pile _pileOnStartDragging;

    public bool Dragging => _dragging;

    public bool IsReturnTweenActive => _returnTween != null && _returnTween.active;

    private void Awake()
    {
        _camera = Camera.main;
        _material = GetComponent<Renderer>().material;
        _originalColor = _material.color;
    }

    private void Update()
    {
        if (_dragging)
        {
            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
            Vector3 rayPoint = ray.GetPoint(_distance);

            int i = 0;
            foreach (Card card in _selectedCards)
            {
                card.transform.position = rayPoint + _mouseOffset + card.Pile.CardsOffset * i;
                i++;
            }
        }
    }

    private void OnMouseEnter()
    {
        //print("enter try: " + this.name);

        if (!this.enabled) return;
        if (_draggingAnyCard) return;

        _selectedCards = _card.Pile.Cards
            .Skip(_card.IndexInPile)
            .ToList();

        _selectedCards.ForEach(x => x.Drag.Select());

        //print("enter success: " + this.name);
    }

    private void OnMouseExit()
    {
        //print("exit try: " + this.name);

        if (!this.enabled) return;
        if (_dragging) return;
        _selectedCards.ForEach(x => x.Drag.Deselect());

        _selectedCards.Clear();

        //print("exit success: " + this.name);
    }

    private void OnMouseDown()
    {
        if (!this.enabled) return;
        KillReturnTween();

        int movingCardsCount = 0;
        foreach (TableauPile pile in PileManager.Instance.TableauPiles)
        {
            if (pile.Cards.Any(x => x.Drag.IsReturnTweenActive))
            {
                movingCardsCount++;
            }
        }

        _distance = Vector3.Distance(this.transform.position, _camera.transform.position);
        _mouseOffset = this.transform.position - _camera.ScreenPointToRay(Input.mousePosition).GetPoint(_distance);

        _dragging = true;
        _draggingAnyCard = true;

        for (int i = 0; i < _selectedCards.Count; i++)
        {
            Card card = _selectedCards[i];

            if (card.Drag.IsReturnTweenActive)
            {
                card.Drag.KillReturnTween();
                card.transform.position = card.GetPosition();
            }

            if (i != 0)
            {
                card.transform.SetParent(_selectedCards[i - 1].transform);
            }

            card.SortingOrder = 999 + (movingCardsCount * 100) + i;
        }

        _pileOnStartDragging = _card.Pile;
    }

    private void OnMouseUp()
    {
        if (!this.enabled) return;

        _dragging = false;
        _draggingAnyCard = false;

        Pile bestPile = (Pile)_hoveredElements
            .Where(x =>
            {
                switch (x)
                {
                    case Pile pile: return pile.CanPlaceCardOver(_card);
                    case Card card: return card.Pile.CanPlaceCardOver(_card);
                    default: return false;
                }
            })
            .OrderBy(x => Vector3.Distance(x.transform.position, _card.transform.position))
            .Select(x =>
            {
                switch (x)
                {
                    case Pile pile: return pile;
                    case Card card: return card.Pile;
                    default: return x;
                }
            })
            .FirstOrDefault();

        if (bestPile != null && _pileOnStartDragging != bestPile)
        {
            foreach (Card card in _selectedCards)
            {
                bestPile.TryPlaceCardOver(card);
            }
        }

        if (_pileOnStartDragging == _card.Pile)
        {
            _returnTween = this.transform
                .DOMove(_card.GetPosition(), _returnDuration)
                .SetEase(Ease.OutQuad)
                .OnKill(() =>
                {
                    int i = 0;
                    foreach (Card card in _card.Pile.Cards.Skip(_card.IndexInPile).ToList())
                    {
                        card.SortingOrder = card.IndexInPile;
                        card.transform.SetParent(card.Pile.transform);
                        i++;

                        if (card.Drag.Dragging || card.Drag.IsReturnTweenActive)
                        {
                            break;
                        }
                    }
                });
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_dragging)
        {
            if (collision.TryGetComponent(out Pile pile))
            {
                _hoveredElements.Add(pile);
            }
            if (collision.TryGetComponent(out Card card))
            {
                _hoveredElements.Add(card);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Pile pile))
        {
            _hoveredElements.Remove(pile);
        }
        if (collision.TryGetComponent(out Card card))
        {
            _hoveredElements.Remove(card);
        }
    }

    public void KillReturnTween()
    {
        if (IsReturnTweenActive)
        {
            _returnTween.Kill();
        }
    }

    public void Select()
    {
        _material.color = _mouseOverColor;
    }

    public void Deselect()
    {
        _material.color = _originalColor;
    }
}
