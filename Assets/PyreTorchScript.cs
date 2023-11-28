using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class PyreTorchScript : MonoBehaviour
{
    //[SerializeField] private AudioManager audioManagerScript;
    [SerializeField] private LevelChanger levelChangerScript;
    
    [SerializeField] private AudioSource torchAudioSource;
    [SerializeField] private AudioClip soundOfFireStarting;
    [SerializeField] private AudioClip loopingFireSound;
    
    private bool playerTorchIsLit = false;

    private float timeTillLitTorch = 1.75f;

    [SerializeField] private GameObject fx_smoke;
    [SerializeField] private GameObject fx_fire;
    [SerializeField] private GameObject fx_sparks;
    [SerializeField] private GameObject PointLight;

    //private List<GameObject> listOfChildren;
    
    
    // Start is called before the first frame update
    void Start()
    {
        fx_smoke.SetActive(false);
        fx_fire.SetActive(false);
        fx_sparks.SetActive(false);
        PointLight.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("TorchFireHere") && playerTorchIsLit == false)
        {
            StartCoroutine(StartSettingFireToTorch());
        }

        if (other.CompareTag("IgniteFuneralPyreTag") && playerTorchIsLit == true)
        {
            levelChangerScript.FuneralPyreLit = true;       //The player has ignited the funeral pyre
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("TorchFireHere") && playerTorchIsLit == false)
        {
            torchAudioSource.Stop();
            StopAllCoroutines();
        }
    }

    IEnumerator StartSettingFireToTorch()
    {
        //torchAudioSource.soundOfFireStarting.Play();  
        torchAudioSource.clip = soundOfFireStarting;
        torchAudioSource.Play();
        yield return new WaitForSeconds(torchAudioSource.clip.length);
        
        fx_smoke.SetActive(true);
        fx_fire.SetActive(true);
        fx_sparks.SetActive(true);
        PointLight.SetActive(true);
        
        torchAudioSource.Stop();

        torchAudioSource.clip = loopingFireSound;
        torchAudioSource.Play();
        
        playerTorchIsLit = true;
    }
}
