using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaitPoint : MonoBehaviour
{
    private bool startCounting;
    private float score;
    [SerializeField] private CapsuleCollider _collider;
    [SerializeField] private MeshRenderer _renderer;
    [SerializeField] private MeshRenderer _secondRenderer;
    [SerializeField] private Material insideMaterial;
    [SerializeField] private Material outsideMaterial;
    public bool inTutorial = false;
    [SerializeField] private Slider captureSlider;
    [SerializeField] private AudioSource captureSound;
    [SerializeField] private AudioClip endSound;

    private void Start()
    {
        captureSound.Play();
        captureSound.Pause();
    }

    void Update()
    {
        if (startCounting)
        {
            score += Time.deltaTime;
            captureSlider.value = Mathf.InverseLerp(0, 6, score);

            if (score >= 6)
            {
                score = 0;
                captureSound.PlayOneShot(endSound, 1);
                StartCapture(false);
                ActivatePoint(false); 

                if (inTutorial == false)
                {
                    FindObjectOfType<GameManager>().IncreaseDownloadTime();
                }
                else
                {
                    FindObjectOfType<TutorialManager>().ActivateNextTask();
                }
            }
        }           
    }

    public void StartCapture(bool on)
    {
        startCounting = on;
        if (on)
        {
            _renderer.material = insideMaterial;
            captureSound.UnPause();
        }
        else
        {
            _renderer.material = outsideMaterial;
            captureSound.Pause();
        } 
    }

    public void ActivatePoint(bool on)
    {
        _collider.enabled = on;
        _renderer.enabled = on;
        _secondRenderer.enabled = on;
        captureSlider.gameObject.SetActive(on);
        captureSlider.value = Mathf.InverseLerp(0, 6, score);
    }
}
