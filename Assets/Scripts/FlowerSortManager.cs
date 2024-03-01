using System;
using System.Collections;
using UnityEngine;

public class FlowerSortManager : StaticInstance<FlowerSortManager> {
    private bool _gameStarted;

    private int _amountSorted;

    public int AmountSorted { 
        get { return _amountSorted; }
        set {  _amountSorted = value; }
    }

    public bool GameStarted { get => _gameStarted; set => _gameStarted = value; }

    public void AddPoints() {
        AmountSorted++;
    }
}