using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubePieceScript : MonoBehaviour
{
    public GameObject UpPlane, DownPlane, LeftPlane, RightPlane, FrontPlane, BackPlane;
    public List<GameObject> Planes = new List<GameObject>();
    public Material crosses;
    public Material noughts;
    public Material empty;
    public bool CanMark { get; set; }
    public int Mark { get; set; }
    private void Start()
    {
        CanMark = true;
        Mark = 0;
    }
    public void SetColor(int x, int y, int z)
    {
        if (y == 0)
            Planes[0].SetActive(true);
        else if (y == -2)
            Planes[1].SetActive(true);
        if (z == 0)
            Planes[2].SetActive(true);
        else if (z == 2)
            Planes[3].SetActive(true);
        if (x == 0)
            Planes[4].SetActive(true);
        else if (x == -2)
            Planes[5].SetActive(true);
    }

    [System.Obsolete]
    public void SetMark(bool turn)
    {
        if (CanMark)
        {
            Material material;
            if (turn)
            {
                material = crosses;
                Mark = 1;
            }
            else
            {
                material = noughts;
                Mark = 2;
            }

            foreach (GameObject plane in Planes.FindAll(x => x.active == true))
            {
                plane.GetComponent<MeshRenderer>().material = material;
            }
            CanMark = false;
        }
    }

    public void Clean()
    {
        foreach (GameObject plane in Planes.FindAll(x => x.active == true))
        {
            plane.GetComponent<MeshRenderer>().material = empty;
        }
        CanMark = true;
        Mark = 0;
    }
}
