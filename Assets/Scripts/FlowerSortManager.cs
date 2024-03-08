using System;
using System.Collections;
using UnityEngine;

public class FlowerSortManager : StaticInstance<FlowerSortManager> {
    private bool _gameStarted;

    private int _amountSorted;
    private int _amountCrushed;

    public int AmountSorted { 
        get { return _amountSorted; }
        set {  _amountSorted = value; }
    }

    public int AmountCrushed {
        get { return _amountCrushed; }
        set { _amountCrushed = value; }
    }

    public bool GameStarted { get => _gameStarted; set => _gameStarted = value; }

    public void AddPointsSort() {
        AmountSorted++;
    }

    public void AddPointsCrushed() {
        AmountCrushed++;
    }
}