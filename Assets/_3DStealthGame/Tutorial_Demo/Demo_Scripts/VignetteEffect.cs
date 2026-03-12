using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using System.Collections;
using StealthGame;

public class VignetteEffect : MonoBehaviour
{
    public Volume globalVolume;
    private Vignette vignette;
    private Coroutine currentRoutine;
    //public bool Alert;
    float normalValue = 0.3f;
    WaypointPatrol enemyView;
    public GameEnding player;
    Color Black;
    Color Red;
    //public float target;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //globalVolume.profile = Instantiate(globalVolume.profile);
         globalVolume.profile.TryGet(out vignette);
        Black = Color.black;
        Red = Color.red;
        vignette.color.value = Black;
        vignette.intensity.value = normalValue;
        //vignette.color.value = Color.red;

        //Debug.Log(globalVolume.name);
    }

    public void SetAlert(bool alert)
    {
       

        if (alert)
        {
            StartCoroutine(ChangeVignetteRed(0.85f, 2f));
            //vignette.intensity.value = Mathf.Lerp(0.3f, 0.75f, 5f);
            //currentRoutine = 
                //StartCoroutine(ChangeVignette(0.45f, 0.5f));
            
        }
        if (alert == false)
        {
            Color current = vignette.color.value;
            StartCoroutine(ChangeVignetteBlack(0.3f, 2f));
            //vignette.intensity.value = 0.3f;
            //currentRoutine = 
                //StartCoroutine(ChangeVignette(0f, 1f));
        }
    }

    

    IEnumerator ChangeVignetteRed(float target, float duration)
    {
        float start = vignette.intensity.value;
        float time = 0f;
        Color startColor = vignette.color.value;
        Color endColor = Color.red;
        while (time < duration)
        {
            time += Time.deltaTime;
            vignette.intensity.value = Mathf.Lerp(start, target, time / duration);
            vignette.color.value = Color.Lerp(startColor, endColor, vignette.intensity.value);
            yield return null;
        }

        vignette.intensity.value = target;
        //start = target;
        //Debug.Log(vignette.intensity.value);
    }

    IEnumerator ChangeVignetteBlack(float target, float duration)
    {
        float start = vignette.intensity.value;
        float time = 0f;
        Color startColor = vignette.color.value;
        Color endColor = Color.black;
        while (time < duration)
        {
            time += Time.deltaTime;
            vignette.intensity.value = Mathf.Lerp(start, target, time / duration);
            vignette.color.value = Color.Lerp(startColor, endColor, vignette.intensity.value);
            yield return null;
        }

        vignette.intensity.value = target;
        //start = target;
        //Debug.Log(vignette.intensity.value);
    }



    /*public void AlertTrue()
    {
        float start = vignette.intensity.value;
        float target = -start;

        while (Alert)
        {
            StartCoroutine(ChangeVignette(target, 1f));
        }
    }

    public void AlertFalse()
    {
        float start = vignette.intensity.value;
        float target = normalValue;

        if (Alert == false)
        {
            StartCoroutine(ChangeVignette(target, 2f));

        }
        
    }

    public void CheckEnemy()
    {
        if (enemyView.PlayerVisible == true)
        {
            Alert = true;
        }

        if (enemyView.PlayerVisible == false)
        {
            Alert = false;
        }
    }*/

    // Update is called once per frame
    void Update()
    {
        if (player.m_IsPlayerCaught)
        {
            StopAllCoroutines();
            vignette.color.value = Black;
            vignette.intensity.value = 0.3f;
        }
        //CheckEnemy();
    }
}
