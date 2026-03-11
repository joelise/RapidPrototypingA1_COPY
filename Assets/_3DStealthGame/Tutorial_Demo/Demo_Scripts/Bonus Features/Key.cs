using System;
using System.Collections;
using UnityEngine;

namespace StealthGame
{
    public class Key : MonoBehaviour
    {
        public string KeyName = "key1";
        public float RotateSpeed;
        public float Speed;
        public float Height;
        Vector3 pos;
       

        private void OnTriggerEnter(Collider other)
        {
            PlayerMovement player = other.gameObject.GetComponent<PlayerMovement>();

            //this wasn't a player
            if (player == null)
                return;
        
            player.AddKey(KeyName);
            gameObject.SetActive(false);
        }

        private void Update()
        {
            transform.Rotate(0, RotateSpeed * Time.deltaTime, 0);
            AnimateUp();
          
        }

        private void Start()
        {
            pos = transform.position;
        }

        public void Reset()
        {
            gameObject.SetActive(true);
        }

        public void AnimateUp()
        {
            
            float newY = Mathf.Sin(Time.time * Speed) * Height + pos.y;
            transform.position = new Vector3(transform.position.x, newY, transform.position.z);
        }

       

        
    }
}