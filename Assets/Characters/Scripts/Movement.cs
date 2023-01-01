using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    CharacterController characterController;
    Animator animator;

#region Direction
    float directionX;
    float directionY;
    float directionZ;
#endregion


#region: Horizontal Speeds
    public float walkSpeed;
    public float runSpeed;
    public float ninjaSpeed;
    public float turnSpeed;
#endregion
    public float animSpeed;
    private Vector3 rotation;


#region Jump
    public float jumpHeight = 1.0f;
    public float gravity = -9.81f;
    private float Velocity;
    private Vector3 moveVector;

#endregion
    
    private bool doRun;
    private bool doRunNinja;
    private float locoBlendSpeed;


    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        // Kalau shift key dipencet maka character akan berlari
        doRun = Input.GetKey(KeyCode.LeftShift);
        doRunNinja = Input.GetKey(KeyCode.Tab);

        applyGravity();
        MoveChar();    
    }

    /*
    Use custom gravity in an attempt to fix the unstable isGrounded (ground check feature) provided by Unity
    Character Controller component.
    */
    private void applyGravity() {
        if(charGrounded() && Velocity < 0) {
            Velocity = -1.0f;
        }
        else
        {
            Velocity += gravity * Time.deltaTime;
        }

        moveVector.y = Velocity;
    }

    private void MoveChar() {
 /*        directionX = Input.GetAxis("Horizontal");
       directionZ = Input.GetAxis("Vertical"); */
        // directionY = 0f;
        directionX = 0;
        directionZ= 1;
        // moveVector = new Vector3(x,y,z);
        moveVector.x = directionX;
        moveVector.z = directionZ;

        characterController.Move(moveVector * AppliedSpeed() * Time.deltaTime);

        // Use apply gravity instead
         // transform.position = new Vector3(transform.position.x, y, transform.position.z);    

        Debug.Log(charGrounded() ); // This is now working well at least in this project
        
        if( directionX == 0 && directionZ == 0) {
            locoBlendSpeed = 0f;
        } else if(doRun) {
            locoBlendSpeed = 1f;
        } else if(doRunNinja) {
            locoBlendSpeed = 2;
        } else {
            locoBlendSpeed = 0.5f;        
        }

        animator.SetFloat("Speed", locoBlendSpeed, animSpeed, Time.deltaTime);

        rotation = new Vector3(directionX, 0, directionZ); // Create new vector3 value for rotation to prevent y rotation on the character
        if(rotation != Vector3.zero)
            Rotation(rotation);
    }

    private void Rotation(Vector3 moveVector) {

        Quaternion targetRotation = Quaternion.LookRotation(moveVector, Vector3.up);

        Quaternion newRotation = Quaternion.Lerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);

        transform.rotation = newRotation;
    }

    private float AppliedSpeed() {
        if(doRun) {
            return runSpeed;
        } else if(doRunNinja) {
            return ninjaSpeed;
        } else {
            return walkSpeed;
        }       
    }

    private bool charGrounded() => characterController.isGrounded;
}
