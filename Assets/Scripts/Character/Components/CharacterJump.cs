using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterJump : CharacterComponent
{
    // private
    private bool _CharacterCanJump;
    private float _TimeSinceLastJump = 0f;
    private float _LowJumpModifier = 2.5f;

    private KeyCode _JumpKeyCode = KeyCode.Space; // TODO: INPUT OVERRIDES

    // Serialized
    [SerializeField] private float _FallMultiplier = 2.5f;
    [SerializeField] private float _GravityScaled;
    [SerializeField] private float _VerticalTakeOff = 15f;
    [SerializeField] private float _TimeBetweenJumps;
    [SerializeField] private Sensor _GroundSensor;
    
    // public
    public bool CharacterCanJump {get => _CharacterCanJump; set => _CharacterCanJump = value;}
    public float FallMultiplier {get => _FallMultiplier; set => _FallMultiplier = value;}
    public float GravityScaled {get => _GravityScaled; set => _GravityScaled = value;}
    public float VerticalTakeOff {get => _VerticalTakeOff; set => _VerticalTakeOff = value;}
    public float TimeBetweenJumps {get => _TimeBetweenJumps; set => _TimeBetweenJumps = value;}
    public float LowJumpModifier {get => _LowJumpModifier; set => _LowJumpModifier = value;}

    public KeyCode JumpKeyCode {get => _JumpKeyCode; set => _JumpKeyCode = value;}

    protected override void Start(){
        base.Start();   
    }

    protected override void HandlePhysicsComponentFunction(){
        ApplyGravity();
    }

    protected override bool HandlePlayerInput(){
        if(!base.HandlePlayerInput()) return false;
        // TODO: Add lockouts
        
        if(DecideIfCharacterCanJump()) Jump();
        
        return true;
    }

    protected override bool HandleAIInput(){
        // TODO: AI JUMP
        if(!base.HandleAIInput()) return false;
        return false;
    }

    private bool DecideIfCharacterCanJump(){
        // TODO: JUMP TIMEOUT
        if(_GroundSensor.SensorActivated && JumpInput()) return true;
        return false;
    }

    private bool JumpInput(){
        if(Input.GetKeyDown(JumpKeyCode)) return true;
        return false;
    }

    private void Jump(){
        _Character.CharacterRigidBody2D.velocity = Vector2.up * VerticalTakeOff;
    }

    private void ApplyGravity(){
        
        if(_Character.CharacterRigidBody2D.velocity.y < 0f){
            // Effects rigidbody with downward force
            _Character.CharacterRigidBody2D.velocity += Vector2.up * Physics2D.gravity.y * (FallMultiplier - GravityScaled) * Time.deltaTime;
        }
        else if(_Character.CharacterRigidBody2D.velocity.y > 0f){
            // Creates "Video game" jump that has a snappier up and floaty down
            _Character.CharacterRigidBody2D.velocity += Vector2.up * Physics2D.gravity.y * (LowJumpModifier - GravityScaled) * Time.deltaTime;
        }
    }
}
