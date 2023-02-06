using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentGenerator : MonoBehaviour
{
    public GameObject floorPiece;
    public Vector3 startPos;
    public int width;
    public int height;
    public bool generate;

    private GameObject[,] floorGrid;

    public int ToPulseX;
    public int ToPulseY;
    public bool Activate;

    private void Awake()
    {
        if (generate)
        {
            floorGrid = new GameObject[width, height];

            for (int i = 0; i < width; i++)
            {
                for (int e = 0; e < height; e++)
                {
                    floorGrid[i, e] = Instantiate(floorPiece, new Vector3(startPos.x + (i * 2), startPos.y, startPos.z - (e * 2)), Quaternion.identity);
                    floorGrid[i, e].GetComponent<BouncyCube>().startingY = startPos.y;
                    floorGrid[i, e].GetComponent<MeshRenderer>().material.color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
                    floorGrid[i, e].name = "Floor Piece - X " + i.ToString() + "  ||  Y " + e.ToString(); 
                }
            }
            print(floorGrid.Length);
        }
    }

    private void Update()
    {
        if (Activate)
        {
            floorGrid[ToPulseX, ToPulseY].GetComponent<BouncyCube>().Pulse();
            Activate = false;
        }
    }
}
