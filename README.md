# GenericCharacterController

A generic character with multiple states like "Idle, Walk, Running, Air"

## Add a new state

1) In the class PlayerState, add the new state to the enum
 public enum State { Idle, Walk, Running, Air, NewState };
 
 2) Create a new class called PlayerStateNewState which inherit from PlayerState
 
 3) In the inspector, locate the "stateTransform" of the StateMachine and add your new state
 as a component
 
 4) In PlayerStateMachine, in the function InitialiseStateDictionary(), add
InitialiseState(PlayerState.State.NewState, stateTransform.GetComponent<PlayerStateNewState>());

5) In other state, call SetState(PlayerState.NewState) when you see fit!

## Addition

"PlayerInput" will convert your input to match the view of the camera. Pressing "Up" will make you walk forward
considering the camera view.

"PlayerAnimationListener" will bind animations with state, this is helpful if you want your code in the state to stay
clean and cohesive.
