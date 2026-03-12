 using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using System.Collections.Generic;
using UnityEngine.UI;
using Image = UnityEngine.UI.Image;
using UnityEngine.Rendering;
using System.IO.Pipes;

namespace StealthGame
{
    public class GameEnding : MonoBehaviour
    {
        public float fadeDuration = 1f;
        public float displayImageDuration = 1f;
        public GameObject player;
        public UIDocument uiDocument;
        public UIDocument addedUI;
        public AudioSource exitAudio;
        public AudioSource caughtAudio;

        bool m_IsPlayerAtExit;
        public bool m_IsPlayerCaught;
        float m_Timer;
        bool m_HasAudioPlayed;

        private VisualElement m_EndScreen;
        private VisualElement m_CaughtScreen;
    
        //DEMO ADDITION
        private float m_Demo_GameTimer;
        private bool m_Demo_GameTimerIsTicking;
        private Label m_Demo_GameTimerLabel;

        // Added
        public GameObject[] levelKeys;
        public int KeyAmount;
        public bool HasAllKeys;
        public PlayerMovement playerMovement;
        //public GameObject[] KeyCameras;
        public List<GameObject> KeyCams;
        public float CamDelay;
        public bool GameStarted;
        public GameObject PlayerCam;
        public Transform PlayerStart;
        public GameObject PlayerMesh;

        public bool SkipPressed;
        public bool CamSeq;
        //public GameObject BlackUI;
        //public GameObject Fade;
        public Image FadeUI;
        public bool LevelReset;
        private Label m_Skip;
        //private Label KeyUI;
        private Label KeyCollected;
        private Label KeysTotal;
        private Label Middle;
        private Label KeysLabel;

        private Label timeTaken;

        public VignetteEffect vignette;
        public bool AllReset;

        void Start()
        {
            AllReset = false;
            //BlackUI.SetActive(false);
            KeyCollected = addedUI.rootVisualElement.Q<Label>("KeyCollected");
            KeysTotal = addedUI.rootVisualElement.Q<Label>("AllKeys");
            Middle = addedUI.rootVisualElement.Q<Label>("middle");
            KeysLabel = addedUI.rootVisualElement.Q<Label>("Keys");
            m_Skip = addedUI.rootVisualElement.Q<Label>("Skip");
            timeTaken = uiDocument.rootVisualElement.Q<Label>("TimeTaken");

            HideKeyUI();
            //Fade.SetActive(false);
            PlayerStart = player.transform;
            playerMovement.DisableMovement();
            //player.GetComponent<PlayerMovement>().enabled = false;
            GameStarted = false;
            KeyAmount = levelKeys.Length;
            HasAllKeys = false;
            PlayerCam.gameObject.SetActive(true);
            StartCoroutine(CameraCycle());
            
            m_EndScreen = uiDocument.rootVisualElement.Q<VisualElement>("EndScreen");
            m_CaughtScreen = uiDocument.rootVisualElement.Q<VisualElement>("CaughtScreen");

            m_Demo_GameTimerLabel = uiDocument.rootVisualElement.Q<Label>("Demo_TimerLabel");
            //KeyUI = uiDocument.rootVisualElement.Q<Label>("KeyNumber");
            HideTimerUI();
            //m_Demo_GameTimerLabel.style.opacity = 0;
            m_Demo_GameTimer = 0.0f;
            m_Demo_GameTimerIsTicking = true;
            Demo_UpdateTimerLabel();
            UpdateKeyUI();
           







        }

        void HideKeyUI()
        {
            KeyCollected.style.opacity = 0;
            KeysTotal.style.opacity = 0;
            Middle.style.opacity = 0;
            KeysLabel.style.opacity = 0;
        }

        void ShowKeyUI()
        {
            KeyCollected.style.opacity = 1;
            KeysTotal.style.opacity = 1;
            Middle.style.opacity = 1;
            KeysLabel.style.opacity = 1;
        }

        void HideTimerUI()
        {
            m_Demo_GameTimerLabel.style.opacity = 0;
            timeTaken.style.opacity = 0;
        }

        void ShowTimerUI()
        {
            m_Demo_GameTimerLabel.style.opacity = 1;
            timeTaken.style.opacity = 1;
        }

        private void Awake()
        {
            Color fade = FadeUI.color;
            fade.a = 0f;
        }

        void OnTriggerEnter (Collider other)
        {
            if (other.gameObject == player)
            {
                m_IsPlayerAtExit = true;
            }
        }

        public void CaughtPlayer ()
        {
            m_IsPlayerCaught = true;
        }

        void Update ()
        {
            UpdateKeyUI();
            if (m_Demo_GameTimerIsTicking)
            {
                m_Demo_GameTimer += Time.deltaTime;
                Demo_UpdateTimerLabel();
            }
        
            if (m_IsPlayerAtExit)
            {
                KeyCheck();
                if (HasAllKeys)
                {
                    EndLevel(m_EndScreen, true, exitAudio);
                }
                
            }
            if (m_IsPlayerCaught)
            {
                // ResetLevel();
                //PlayerCaught(m_CaughtScreen, caughtAudio);
                // m_IsPlayerCaught = false;
                //player.GetComponent<PlayerMovement>().enabled = false;
                StartCoroutine(FadeInRoutine());
                StartCoroutine(FadeOut());
                //BlackUI.SetActive(false);
                //playerMovement.EnableMovement();
                m_IsPlayerCaught = false;
            }

            if (AllReset)
            {
                playerMovement.EnableMovement();
                AllReset = false;
            }

            CheckPressed();
            if (SkipPressed)
            {
                
            }
        }

        void EndLevel (VisualElement element, bool doRestart, AudioSource audioSource)
        {
            m_Demo_GameTimerIsTicking = false;
        
            if (!m_HasAudioPlayed)
            {
                audioSource.Play();
                m_HasAudioPlayed = true;
            }
            
            m_Timer += Time.deltaTime;
            element.style.opacity = m_Timer / fadeDuration;
            ShowTimerUI();
            if (m_Timer > fadeDuration + displayImageDuration)
            {
                Application.Quit();
                //Time.timeScale = 0;

                /*if (doRestart)
                {
                    SceneManager.LoadScene("DemoScene");
                }
                else
                {
                    Application.Quit();
                    Time.timeScale = 0;
                }*/
            }
        }

        void Demo_UpdateTimerLabel()
        {
            m_Demo_GameTimerLabel.text = m_Demo_GameTimer.ToString("0.00");
        }

        void UpdateKeyUI()
        {
            KeyCollected.text = playerMovement.m_OwnedKeys.Count.ToString();
            KeysTotal.text = KeyAmount.ToString();
        }

        public void KeyCheck()
        {
            if (playerMovement.m_OwnedKeys.Count == KeyAmount)
            {
                HasAllKeys = true;
            }
        }

        public IEnumerator CameraCycle()
        {
            
            //PlayerMesh.SetActive(true);
            CamSeq = true;
            //SkipUI.SetActive(true);
            PlayerCam.SetActive(false);
            playerMovement.DisableMovement();
            foreach (GameObject cam in KeyCams)
            {
                cam.SetActive(false);
            }

            foreach (GameObject cam in KeyCams)
            {
                cam.SetActive(true);
                yield return new WaitForSeconds(CamDelay);
                cam.SetActive(false);
            }

            PlayerCam.SetActive(true);
            GameStarted = true;
            playerMovement.EnableMovement();
            //player.GetComponent<PlayerMovement>().enabled = true;
            CamSeq = false;
            //SkipUI.SetActive(false);
            m_Skip.style.opacity = 0f;
            ShowKeyUI();
            //PlayerMesh.SetActive(false);

        }

       

        public void CheckPressed()
        {
            if (CamSeq)
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    SkipPressed = true;
                    
                    foreach (GameObject cam in KeyCams)
                    {
                        cam.SetActive(false);
                    }
                    StopAllCoroutines();
                    m_Skip.style.opacity = 0;
                    //SkipUI.SetActive(false);
                    PlayerCam.SetActive(true);
                    GameStarted = true;
                    //player.GetComponent<PlayerMovement>().enabled = true;
                    CamSeq = false;
                    playerMovement.EnableMovement();
                    ShowKeyUI();
                    //PlayerMesh.SetActive(false);
                }
            }
           
        }

        public void ResetLevel()
        {
            foreach (GameObject k in levelKeys)
            {
                k.SetActive(true);
            }

            playerMovement.ResetPlayer();
            LevelReset = true;
            
        }

        public void PlayerCaught(VisualElement element, AudioSource audioSource)
        {
            Color fade = FadeUI.color;
            fade.a = 0f;
            //Fade.SetActive(true);
            float fadeTimer = 0f;
            float fadeTime = 2f;
            if (!m_HasAudioPlayed)
            {
                audioSource.Play();
                m_HasAudioPlayed = true;
            }

            m_Timer += Time.deltaTime;
            element.style.opacity = m_Timer / fadeDuration;

            if (m_Timer > fadeDuration + displayImageDuration)
            {
                //m_Timer = 0;
                fadeTimer += Time.deltaTime;
                fade.a = Mathf.Lerp(0, 1, fadeTimer / fadeTime);
                element.style.opacity = 0f;

                

            }

            if (LevelReset)
            {
                element.style.opacity = 0f;
                fadeTimer = 0f;
                fadeTimer += Time.deltaTime;
                fade.a = Mathf.Lerp(1, 0, fadeTimer / fadeTime);
                LevelReset = false;

            }
        }

        public IEnumerator FadeInRoutine()
        {
            
            playerMovement.DisableMovement();
            Color c = FadeUI.color;
            c.a = 0f;
            //Fade.SetActive(true);
            float t = 0f;
            float fadeT = 2f;
            //float delay = 2f;

            m_Timer += Time.deltaTime;
            m_CaughtScreen.style.opacity = m_Timer / fadeDuration;

            while (t < fadeT)
            {
                t += Time.deltaTime;
                c.a = Mathf.Lerp(0, 1, t / fadeT);
                FadeUI.color = c;
                yield return null;
                

            }

            m_CaughtScreen.style.opacity = 0;
            t = 0f;
            
            ResetLevel();
            
            //m_IsPlayerCaught = false;
            //vignette.globalVolume.
            //yield return new WaitForSeconds(3f);
            //BlackUI.SetActive(true);
            yield return null;
            /*

            while (t < fadeT)
            {
                t += Time.deltaTime;
                c.a = Mathf.Lerp(1, 0, t / fadeT);
                FadeUI.color = c;
               
                yield return null;
            }

            playerMovement.EnableMovement();*/
        }

        public IEnumerator FadeOut()
        {
            float t = 0f;
            float fadeT = 2f;

            Color c = FadeUI.color;
            //c.a = 0f;
            while (t < fadeT)
            {
                t += Time.deltaTime;
                c.a = Mathf.Lerp(1, 0, t / fadeT);
                FadeUI.color = c;

                
            }
            //BlackUI.SetActive(false);
            yield return null;

            AllReset = true;
        }

    }
}