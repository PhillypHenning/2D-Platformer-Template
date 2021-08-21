using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : CharacterComponent
{
    public float _MovementSpeed;
    
    private float _HorizontalMovement;

    public float HorizontalMovement => _HorizontalMovement;

    public bool _MovementSurpressed;


    // TODO: This component will need to know which way the sprite is facing

    protected override void Start()
    {
        base.Start();
        SetToDefault();
    }

    protected override void HandlePhysicsAbility()
    {   
        // Technique #1: Most Basic Movement
        // _CharacterRigidBody2D.velocity = new Vector2(_MovementSpeed * _HorizontalInput, 0);

        // Techinique #2: Adding Force to the Rigidbody to move in a more gradual way
        // It can feel like sliding on ice. Be cognitive that the Rigidbody mass is weighed in to the function.
        //_CharacterRigidBody2D.AddForce(new Vector2(_MovementSpeed * _HorizontalInput, 0));
        //_CharacterRigidBody2D.AddForce(new Vector2(_MovementSpeed * _HorizontalInput, 0), ForceMode2D.Force); // <-- Default, gradual force build up applied
        if (CanMove())
        {
            _Character.RigidBody2D.AddForce(new Vector2(_MovementSpeed * _HorizontalMovement, 0), ForceMode2D.Impulse); // <-- Specified, Immediate force applied        
        }
    }
    
    protected override void HandleInput()
    {
        if (CanMove())
        {
            _HorizontalMovement = Input.GetAxisRaw("Horizontal");
            _Character.IsMoving = _HorizontalMovement != 0;
            if (_Character.IsMoving)
            {
                if ((_Character.FacingRight && _HorizontalMovement < 0) || !_Character.FacingRight && _HorizontalMovement > 0)
                {
                    FlipCharacter();
                }
            }

            _CharacterAnimation.Movement();
        }
    }

    protected override void SetToDefault()
    {
        _MovementSpeed = 200f;
    }

    private void FlipCharacter()
    {
        // TODO: flip entire character instead of just the sprite
        var character = _Character.CharacterSprite.transform;
        _Character.FacingRight = !_Character.FacingRight;
        character.localRotation = Quaternion.Euler(character.rotation.x, _Character.FacingRight ? 0 : -180, character.rotation.z);
    }

    public void IncreaseMovementSpeed(float amount, float abilitylength,  bool lockout = true){
       // TODO: Might not need. 
    }

    public void MovePosition(Vector2 newPosition){
        // TODO: (Potentially) add option to disable animations while position is being moved 
        Debug.Log(newPosition);
        _Character.RigidBody2D.MovePosition(newPosition);
    }

    public bool CanMove()
    {
        return !_Character.IsLocked;
    }

    public void Stop(){

    }

    public void LockMovement(){

    }

    public void UnlockMovement(){
        
    }
}
