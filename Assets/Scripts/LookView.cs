using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookView : MonoBehaviour {
    public GameObject CanMoveHere;
    // Will look at this object when we move to CanMoveHere
    public GameObject WillLookHere;
    public bool PlayerLookingHere { get; set; }

    [SerializeField] private bool _conditionalMove;
    public bool ConditionalMove {
        get { return _conditionalMove; }
        set { _conditionalMove = value; }
    }
}