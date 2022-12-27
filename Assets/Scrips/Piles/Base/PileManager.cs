using System.Collections.Generic;
using UnityEngine;

public class PileManager : SingletonBehaviour<PileManager>
{
    [SerializeField] private StockPile _stockPile;
    [SerializeField] private DiscardPile _discardPile;
    [SerializeField] private List<FoundationPile> _foundationPiles;
    [SerializeField] private List<TableauPile> _tableauPiles;

    public List<Pile> AllPiles { get; private set; }

    public StockPile StockPile => _stockPile;
    public DiscardPile DiscardPile => _discardPile;
    public List<FoundationPile> FoundationPiles => _foundationPiles;
    public List<TableauPile> TableauPiles => _tableauPiles;

    protected override void Awake()
    {
        base.Awake();

        this.AllPiles = new List<Pile>();

        this.AllPiles.Add(StockPile);
        this.AllPiles.Add(DiscardPile);
        this.AllPiles.AddRange(FoundationPiles);
        this.AllPiles.AddRange(TableauPiles);
    }
}
