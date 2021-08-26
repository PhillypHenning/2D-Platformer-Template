using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateController : MonoBehaviour
{   
    // Brain of the AI

    [Header("State")]
    [SerializeField] private AIState currentState;
    [SerializeField] private AIState remainState;

    public CharacterMovement _CharacterMovement { get; set; }

    public Transform _Target { get; set; }
    public Path _Path { get; set; }

    private void Awake() {
        _CharacterMovement = GetComponent<CharacterMovement>();
        _Path = GetComponent<Path>();
    }

    private void Update() {
        currentState.EvaluateState(this);
    }

    public void TransitionToState(AIState nextState){
        if(nextState != remainState){
            currentState = nextState;
        }
    }
}
