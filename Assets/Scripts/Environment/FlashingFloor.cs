using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashingFloor : MonoBehaviour
{
    private MeshRenderer meshRenderer;
    private Material instanceMaterial;
    private bool alphaIncrease;
    [Range(0f, 1f)]
    [SerializeField] private float speed;

    void Update()
    {
        if (meshRenderer == null)
        {
            GetMaterials();
        }
        else
        {
            UpdateMaterial();
        }
    }

    private void GetMaterials()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        instanceMaterial = meshRenderer.material;
    }

    private void UpdateMaterial()
    {
        if (alphaIncrease)
        {
            Color currentColour = instanceMaterial.color;
            currentColour.a += Time.deltaTime * speed;
            meshRenderer.material.color = currentColour;

            if (currentColour.a > .45f)
            {
                alphaIncrease = !alphaIncrease;
            }
        }
        else
        {
            Color currentColour = instanceMaterial.color;
            currentColour.a -= Time.deltaTime * speed;
            meshRenderer.material.color = currentColour;

            if (currentColour.a < .25f)
            {
                alphaIncrease = !alphaIncrease;
            }
        }
    }
}
