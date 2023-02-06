using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CapturePoint : MonoBehaviour
{
    [SerializeField] private CapsuleCollider _collider;
    [SerializeField] private MeshRenderer _renderer;
    [SerializeField] private MeshRenderer _secondRenderer;
    [SerializeField] private GameObject particleObject;
    [SerializeField] private AudioSource collectAudio;

    public void ActivatePoint(bool on)
    {
        _collider.enabled = on;
        _renderer.enabled = on;
        _secondRenderer.enabled = on;
        particleObject.SetActive(on);
    }

    public void PlaySound()
    {
        collectAudio.Play();
    }
}
