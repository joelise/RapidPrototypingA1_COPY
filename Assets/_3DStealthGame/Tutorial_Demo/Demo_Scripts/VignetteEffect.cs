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
    float normalValue = 0.2f;
    WaypointPatrol enemyView;
    //public float target;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //globalVolume.profile = Instantiate(globalVolume.profile);
         globalVolume.profile.TryGet(out vignette);
        //vignette.color.value = Color.red;

        Debug.Log(globalVolume.name);
    }

    public void SetAlert(bool alert)
    {
       

        if (alert)
        {
            StartCoroutine(VignetteIn(alert));
            //vignette.intensity.value = Mathf.Lerp(0.3f, 0.75f, 5f);
            //currentRoutine = 
                //StartCoroutine(ChangeVignette(0.45f, 0.5f));
            
        }
        if (alert == false)
        {
            vignette.intensity.value = 0.3f;
            //currentRoutine = 
                //StartCoroutine(ChangeVignette(0f, 1f));
        }
    }

    

    IEnumerator ChangeVignette(float target, float duration)
    {
        float start = vignette.intensity.value;
        float time = 0f;

        while (time < duration)
        {
            time += Time.deltaTime;
            vignette.intensity.value = Mathf.Lerp(start, target, time / duration);
            yield return null;
        }

        vignette.intensity.value = target;
        Debug.Log(vignette.intensity.value);
    }

    IEnumerator VignetteIn(bool alert)
    {
        float start = vignette.intensity.value;
        float target = -start;

        float timer = 0f;
        float fadeTime = 2f;

        timer += Time.deltaTime;

        while (alert)
        {
            vignette.intensity.value = Mathf.Lerp(start, target, 1f);
        }

        yield return null;
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
        
        //CheckEnemy();
    }
}
