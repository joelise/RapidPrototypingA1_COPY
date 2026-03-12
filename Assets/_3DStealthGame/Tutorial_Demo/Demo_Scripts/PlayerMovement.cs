using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;
using Unity.VisualScripting;
using UnityEngine.UI;

namespace StealthGame
{
    public enum Direction { North, East, South, West, Default };

    public class PlayerMovement : MonoBehaviour
    {
        
        public InputAction MoveAction;
        public InputAction JumpAction;

        public InputAction LookAction;

        public InputAction NorthControl;
        public InputAction EastControl;
        public InputAction SouthControl;
        public InputAction WestControl;
        public InputAction DefaultControl;

        public string[] checkTags;
        public Direction dir;
        public UnityEvent onTriggerEnterEvent;

        public float walkSpeed = 1.0f;
        public float turnSpeed = 20f;
        public float jumpStrength = 50f;

        public Animator m_Animator;
        Rigidbody m_Rigidbody;
        AudioSource m_AudioSource;
        Vector3 m_Movement;
        Quaternion m_Rotation = Quaternion.identity;

        public bool IsVisible;
        public bool Chaseable;
        //public bool isWalking;
        public bool CanWalk;

        public GameEnding gameEnding;
        private Vector3 startPos;


        /*[Header("New Movement")]
        public float moveSpeed;
        public InputAction MoveAction;
        public Transform cameraTarget;
        private Rigidbody rb;
        private Vector2 moveInput;

        public GameObject playerMesh;*/

        
      
    
        // DEMO
        public List<string> m_OwnedKeys = new();

        private void Awake()
        {
            
        }

        

        private void OnEnable()
        {
            //MoveAction.Enable();
        }

        private void OnDisable()
        {
            //MoveAction.Disable();
        }

        void Start ()
        {

                startPos = transform.position;
                m_Animator = GetComponent<Animator>();
                m_Rigidbody = GetComponent<Rigidbody>();
                m_AudioSource = GetComponent<AudioSource>();

                //MoveAction = DefaultControl;
                // LookAction.Enable();

                MoveAction.Enable();
                //JumpAction.Enable();
                dir = Direction.Default;
            
            
        }
        public void ResetPlayer()
        {
            transform.position = startPos;
            
            //foreach (Key k in m_OwnedKeys)
            //    k.Reset();

            m_OwnedKeys.Clear();
        }

        private void Update()
        {
            if (JumpAction.WasPressedThisFrame())
                print("Jump");

            //ChangeInputMap();

            //moveInput = MoveAction.ReadValue<Vector2>();

            //Debug.Log("Move Input: " + moveInput);
        }

        void FixedUpdate ()
        {
            if (CanWalk == false)
            {
                m_Animator.Play("Idle");
            }

            if (CanWalk)
            {
                var pos = MoveAction.ReadValue<Vector2>();

                float horizontal = pos.x;
                float vertical = pos.y;

                m_Movement.Set(horizontal, 0f, vertical);
                m_Movement.Normalize();

                bool hasHorizontalInput = !Mathf.Approximately(horizontal, 0f);
                bool hasVerticalInput = !Mathf.Approximately(vertical, 0f);
                bool isWalking = hasHorizontalInput || hasVerticalInput;
                m_Animator.SetBool("IsWalking", isWalking);

                if (isWalking)
                {
                    if (!m_AudioSource.isPlaying)
                    {
                        m_AudioSource.Play();
                    }
                }
                else
                {
                    m_AudioSource.Stop();
                }


                Vector3 desiredForward = Vector3.RotateTowards(transform.forward, m_Movement, turnSpeed * Time.deltaTime, 0f);
                m_Rotation = Quaternion.LookRotation(desiredForward);

                m_Rigidbody.MoveRotation(m_Rotation);
                m_Rigidbody.MovePosition(m_Rigidbody.position + m_Movement * walkSpeed * Time.deltaTime);
            }
            


            /*Vector3 forward = cameraTarget.forward;
            Vector3 right = cameraTarget.right;

            forward.y = 0;
            right.y = 0;

            Vector3 move = forward * moveInput.y + right * moveInput.x;
            rb.MovePosition(rb.position + move * moveSpeed * Time.fixedDeltaTime);*/

           
        }

        public void AddKey(string keyName)
        {
            m_OwnedKeys.Add(keyName);
        }

        public bool OwnKey(string keyName)
        {
            return m_OwnedKeys.Contains(keyName);
        }

        public void OnTriggerEnter(Collider other)
        {
           // if (other.gameObject.CompareTag("EnemyView"))
           // {
           //     IsVisible = true;
          //  }

          //  else
           // {
            //    IsVisible = false;
           // }

           // if (other.gameObject.CompareTag("ChaseZone"))
         //   {
           //     Chaseable = true;
           // }

           // else
            //{
            //    Chaseable = false;
                
            //}


               /* if (other.gameObject.CompareTag("North"))
                {
                    Debug.Log("North");
                    dir = Direction.North;
                }

                else if (other.gameObject.CompareTag("East"))
                {
                    Debug.Log("East");
                    dir = Direction.East;
                }

                else if (other.gameObject.CompareTag("South"))
                {
                    Debug.Log("South");
                    dir = Direction.South;
                }

                else if ( other.gameObject.CompareTag("West"))
                {
                    Debug.Log("West");
                    dir = Direction.West;
                }
            
           // else (dir = Direction.Default)*/
        }

        public void ChangeInputMap()
        {
            //NorthControl.Disable();
            //EastControl.Disable();
            //SouthControl.Disable();
            //WestControl.Disable();

            switch (dir)
            {
                case Direction.North:
                    if (MoveAction.WasReleasedThisFrame())
                    {
                        NorthControl.Enable();
                        MoveAction = NorthControl;
                    }
                   
                  
                    break;

                case Direction.East:
                   
                    if (MoveAction.WasReleasedThisFrame())
                    {
                        EastControl.Enable();
                        MoveAction = EastControl;
                    }
                    
                
                    break;

                case Direction.South:
                   
                    if (MoveAction.WasReleasedThisFrame())
                    {
                        SouthControl.Enable();
                        MoveAction = SouthControl;
                    }
                    
                
                    break;

                case Direction.West:
                    if (MoveAction.WasReleasedThisFrame())
                    {
                        WestControl.Enable();
                        MoveAction = WestControl;
                    }
                    
                    
                    break;

                case Direction.Default:
                    DefaultControl.Enable();
                    MoveAction = DefaultControl;
                    break;

                default:
                    MoveAction = DefaultControl;
                    break;
            }
        }

        public void EnterChaseZone()
        {
            Chaseable = true;
        }

        public void ExitChaseZone()
        {
            Chaseable = false;
        }

        public void EnableMovement()
        {
            //MoveAction.Enable();
            CanWalk = true;
           

        }

        public void DisableMovement()
        {
            //MoveAction.Disable();
            CanWalk = false;

        }

        
    }
}