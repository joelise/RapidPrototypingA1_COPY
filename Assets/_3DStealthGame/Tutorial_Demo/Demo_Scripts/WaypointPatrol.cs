using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace StealthGame
{
    public class WaypointPatrol : MonoBehaviour
    {
        public float moveSpeed = 1.0f;
        public Transform[] waypoints;

        private Rigidbody m_RigidBody;
        int m_CurrentWaypointIndex;

        public float ChaseSpeed;
        public PlayerMovement playerMovement;
        public Transform PlayerTransform;
        public bool PlayerVisible;
        //public UnityEvent onTriggerEnterEvent;

        void Start ()
        {
            m_RigidBody = GetComponent<Rigidbody>();
        }

        void FixedUpdate ()
        {
            //PatrolArea();
        }

        private void Update()
        {
            if (PlayerVisible)
            {
                ChasePlayer();
            }

            else
            {
                PatrolArea();
            }
        }

        public void PatrolArea()
        {
            Transform currentWaypoint = waypoints[m_CurrentWaypointIndex];
            Vector3 currentToTarget = currentWaypoint.position - m_RigidBody.position;

            if (currentToTarget.magnitude < 0.1f)
            {
                //very close to the waypoint, get to the next waypoint
                m_CurrentWaypointIndex = (m_CurrentWaypointIndex + 1) % waypoints.Length;
            }

            //find the rotation to orient the rigidbody toward the waypoint
            //This will be a sharp change toward that direction, this could be made more gradual if wanted by only rotating
            //at a given speed.
            Quaternion forwardRotation = Quaternion.LookRotation(currentToTarget);
            m_RigidBody.MoveRotation(forwardRotation);

            //move toward the waypoint at the set speed
            //currentToTarget is normalized before multiplying by speed because we only want the direction and not the length
            m_RigidBody.MovePosition(m_RigidBody.position + currentToTarget.normalized * moveSpeed * Time.deltaTime);
        }

        public void ChasePlayer()
        {
            Vector3 currentToPlayer = PlayerTransform.position - m_RigidBody.position;

            if (playerMovement.Chaseable == true)
            {
                Quaternion playerRotation = Quaternion.LookRotation(currentToPlayer);
                m_RigidBody.MoveRotation(playerRotation);

                m_RigidBody.MovePosition(m_RigidBody.position + currentToPlayer.normalized * ChaseSpeed * Time.deltaTime);
            }
        }

        public void CheckPlayer()
        {
            PlayerVisible = true;
        }

        public void PlayerNotVisible()
        {
            PlayerVisible = false;
        }
    }
}