using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeManager : MonoBehaviour
{
    public GameObject sessionManager;
    public GameObject CubePiecePref;
    Transform CubeTransf;
    List<GameObject> AllCubePieces = new List<GameObject>();
    GameObject CubeCenterPiece;
    bool CanRotate = true,
        CanShufle = true;

    List<GameObject> UpPieces
    { get { return AllCubePieces.FindAll(x => Mathf.Round(x.transform.localPosition.y) == 0); } }
    List<GameObject> DownPieces
    { get { return AllCubePieces.FindAll(x => Mathf.Round(x.transform.localPosition.y) == -2); } }
    List<GameObject> FrontPieces
    { get { return AllCubePieces.FindAll(x => Mathf.Round(x.transform.localPosition.x) == 0); } }
    List<GameObject> BackPieces
    { get { return AllCubePieces.FindAll(x => Mathf.Round(x.transform.localPosition.x) == -2); } }
    List<GameObject> LeftPieces
    { get { return AllCubePieces.FindAll(x => Mathf.Round(x.transform.localPosition.z) == 0); } }
    List<GameObject> RightPieces
    { get { return AllCubePieces.FindAll(x => Mathf.Round(x.transform.localPosition.z) == 2); } }
    List<GameObject> UpHorizontalPieces
    { get { return AllCubePieces.FindAll(x => Mathf.Round(x.transform.localPosition.x) == -1); } }
    List<GameObject> UpVerticalPieces
    { get { return AllCubePieces.FindAll(x => Mathf.Round(x.transform.localPosition.z) == 1); } }
    List<GameObject> FrontHorizontalPieces
    { get { return AllCubePieces.FindAll(x => Mathf.Round(x.transform.localPosition.y) == -1); } }
    Vector3[] RotationVectors =
    {
        new Vector3(0,1,0), new Vector3(0,-1,0),
        new Vector3(0,0,-1), new Vector3(0,0,1),
        new Vector3(1,0,0), new Vector3(-1,0,0)
    };
    GameObject[,] UpDesk
    {
        get
        {
            return new GameObject[7, 7] {
            {
                null,null,
                AllCubePieces.Find(p => (Mathf.Round(p.transform.localPosition.x) == -2) && (Mathf.Round(p.transform.localPosition.y) == -2) && (Mathf.Round(p.transform.localPosition.z) == 0)),
                AllCubePieces.Find(p => (Mathf.Round(p.transform.localPosition.x) == -2) && (Mathf.Round(p.transform.localPosition.y) == -2) && (Mathf.Round(p.transform.localPosition.z) == 1)),
                AllCubePieces.Find(p => (Mathf.Round(p.transform.localPosition.x) == -2) && (Mathf.Round(p.transform.localPosition.y) == -2) && (Mathf.Round(p.transform.localPosition.z) == 2)),
                null,null
            },
            {
                null,null,
                AllCubePieces.Find(p => (Mathf.Round(p.transform.localPosition.x) == -2) && (Mathf.Round(p.transform.localPosition.y) == -1) && (Mathf.Round(p.transform.localPosition.z) == 0)),
                AllCubePieces.Find(p => (Mathf.Round(p.transform.localPosition.x) == -2) && (Mathf.Round(p.transform.localPosition.y) == -1) && (Mathf.Round(p.transform.localPosition.z) == 1)),
                AllCubePieces.Find(p => (Mathf.Round(p.transform.localPosition.x) == -2) && (Mathf.Round(p.transform.localPosition.y) == -1) && (Mathf.Round(p.transform.localPosition.z) == 2)),
                null,null
            },
            {
                AllCubePieces.Find(p => (Mathf.Round(p.transform.localPosition.x) == -2) && (Mathf.Round(p.transform.localPosition.y) == -2) && (Mathf.Round(p.transform.localPosition.z) == 0)),
                AllCubePieces.Find(p => (Mathf.Round(p.transform.localPosition.x) == -2) && (Mathf.Round(p.transform.localPosition.y) == -1) && (Mathf.Round(p.transform.localPosition.z) == 0)),
                AllCubePieces.Find(p => (Mathf.Round(p.transform.localPosition.x) == -2) && (Mathf.Round(p.transform.localPosition.y) == 0) && (Mathf.Round(p.transform.localPosition.z) == 0)),
                AllCubePieces.Find(p => (Mathf.Round(p.transform.localPosition.x) == -2) && (Mathf.Round(p.transform.localPosition.y) == 0) && (Mathf.Round(p.transform.localPosition.z) == 1)),
                AllCubePieces.Find(p => (Mathf.Round(p.transform.localPosition.x) == -2) && (Mathf.Round(p.transform.localPosition.y) == 0) && (Mathf.Round(p.transform.localPosition.z) == 2)),
                AllCubePieces.Find(p => (Mathf.Round(p.transform.localPosition.x) == -2) && (Mathf.Round(p.transform.localPosition.y) == -1) && (Mathf.Round(p.transform.localPosition.z) == 2)),
                AllCubePieces.Find(p => (Mathf.Round(p.transform.localPosition.x) == -2) && (Mathf.Round(p.transform.localPosition.y) == -2) && (Mathf.Round(p.transform.localPosition.z) == 2))
            },
            {
                AllCubePieces.Find(p => (Mathf.Round(p.transform.localPosition.x) == -1) && (Mathf.Round(p.transform.localPosition.y) == -2) && (Mathf.Round(p.transform.localPosition.z) == 0)),
                AllCubePieces.Find(p => (Mathf.Round(p.transform.localPosition.x) == -1) && (Mathf.Round(p.transform.localPosition.y) == -1) && (Mathf.Round(p.transform.localPosition.z) == 0)),
                AllCubePieces.Find(p => (Mathf.Round(p.transform.localPosition.x) == -1) && (Mathf.Round(p.transform.localPosition.y) == 0) && (Mathf.Round(p.transform.localPosition.z) == 0)),
                AllCubePieces.Find(p => (Mathf.Round(p.transform.localPosition.x) == -1) && (Mathf.Round(p.transform.localPosition.y) == 0) && (Mathf.Round(p.transform.localPosition.z) == 1)),
                AllCubePieces.Find(p => (Mathf.Round(p.transform.localPosition.x) == -1) && (Mathf.Round(p.transform.localPosition.y) == 0) && (Mathf.Round(p.transform.localPosition.z) == 2)),
                AllCubePieces.Find(p => (Mathf.Round(p.transform.localPosition.x) == -1) && (Mathf.Round(p.transform.localPosition.y) == -1) && (Mathf.Round(p.transform.localPosition.z) == 2)),
                AllCubePieces.Find(p => (Mathf.Round(p.transform.localPosition.x) == -1) && (Mathf.Round(p.transform.localPosition.y) == -2) && (Mathf.Round(p.transform.localPosition.z) == 2))
            },
            { 
                AllCubePieces.Find(p => (Mathf.Round(p.transform.localPosition.x) == 0) && (Mathf.Round(p.transform.localPosition.y) == -2) && (Mathf.Round(p.transform.localPosition.z) == 0)),
                AllCubePieces.Find(p => (Mathf.Round(p.transform.localPosition.x) == 0) && (Mathf.Round(p.transform.localPosition.y) == -1) && (Mathf.Round(p.transform.localPosition.z) == 0)),
                AllCubePieces.Find(p => (Mathf.Round(p.transform.localPosition.x) == 0) && (Mathf.Round(p.transform.localPosition.y) == 0) && (Mathf.Round(p.transform.localPosition.z) == 0)),
                AllCubePieces.Find(p => (Mathf.Round(p.transform.localPosition.x) == 0) && (Mathf.Round(p.transform.localPosition.y) == 0) && (Mathf.Round(p.transform.localPosition.z) == 1)),
                AllCubePieces.Find(p => (Mathf.Round(p.transform.localPosition.x) == 0) && (Mathf.Round(p.transform.localPosition.y) == 0) && (Mathf.Round(p.transform.localPosition.z) == 2)),
                AllCubePieces.Find(p => (Mathf.Round(p.transform.localPosition.x) == 0) && (Mathf.Round(p.transform.localPosition.y) == -1) && (Mathf.Round(p.transform.localPosition.z) == 2)),
                AllCubePieces.Find(p => (Mathf.Round(p.transform.localPosition.x) == 0) && (Mathf.Round(p.transform.localPosition.y) == -2) && (Mathf.Round(p.transform.localPosition.z) == 2))
                },
            {
                null,null,
                AllCubePieces.Find(p => (Mathf.Round(p.transform.localPosition.x) == 0) && (Mathf.Round(p.transform.localPosition.y) == -1) && (Mathf.Round(p.transform.localPosition.z) == 0)),
                AllCubePieces.Find(p => (Mathf.Round(p.transform.localPosition.x) == 0) && (Mathf.Round(p.transform.localPosition.y) == -1) && (Mathf.Round(p.transform.localPosition.z) == 1)),
                AllCubePieces.Find(p => (Mathf.Round(p.transform.localPosition.x) == 0) && (Mathf.Round(p.transform.localPosition.y) == -1) && (Mathf.Round(p.transform.localPosition.z) == 2)),
                null,null },
            {
                null,null,
                AllCubePieces.Find(p => (Mathf.Round(p.transform.localPosition.x) == 0) && (Mathf.Round(p.transform.localPosition.y) == -2) && (Mathf.Round(p.transform.localPosition.z) == 0)),
                AllCubePieces.Find(p => (Mathf.Round(p.transform.localPosition.x) == 0) && (Mathf.Round(p.transform.localPosition.y) == -2) && (Mathf.Round(p.transform.localPosition.z) == 1)),
                AllCubePieces.Find(p => (Mathf.Round(p.transform.localPosition.x) == 0) && (Mathf.Round(p.transform.localPosition.y) == -2) && (Mathf.Round(p.transform.localPosition.z) == 2)),
                null,null}
            };
        }
    }
    GameObject[,] DownDesk
    {
        get
        {
            return new GameObject[7, 7] {
            {
                null,null,
                AllCubePieces.Find(p => (Mathf.Round(p.transform.localPosition.x) == 0) && (Mathf.Round(p.transform.localPosition.y) == 0) && (Mathf.Round(p.transform.localPosition.z) == 0)),
                AllCubePieces.Find(p => (Mathf.Round(p.transform.localPosition.x) == 0) && (Mathf.Round(p.transform.localPosition.y) == 0) && (Mathf.Round(p.transform.localPosition.z) == 1)),
                AllCubePieces.Find(p => (Mathf.Round(p.transform.localPosition.x) == 0) && (Mathf.Round(p.transform.localPosition.y) == 0) && (Mathf.Round(p.transform.localPosition.z) == 2)),
                null,null
            },
            {
                null,null,
                AllCubePieces.Find(p => (Mathf.Round(p.transform.localPosition.x) == 0) && (Mathf.Round(p.transform.localPosition.y) == -1) && (Mathf.Round(p.transform.localPosition.z) == 0)),
                AllCubePieces.Find(p => (Mathf.Round(p.transform.localPosition.x) == 0) && (Mathf.Round(p.transform.localPosition.y) == -1) && (Mathf.Round(p.transform.localPosition.z) == 1)),
                AllCubePieces.Find(p => (Mathf.Round(p.transform.localPosition.x) == 0) && (Mathf.Round(p.transform.localPosition.y) == -1) && (Mathf.Round(p.transform.localPosition.z) == 2)),
                null,null
            },
            {
                AllCubePieces.Find(p => (Mathf.Round(p.transform.localPosition.x) == 0) && (Mathf.Round(p.transform.localPosition.y) == 0) && (Mathf.Round(p.transform.localPosition.z) == 0)),
                AllCubePieces.Find(p => (Mathf.Round(p.transform.localPosition.x) == 0) && (Mathf.Round(p.transform.localPosition.y) == -1) && (Mathf.Round(p.transform.localPosition.z) == 0)),
                AllCubePieces.Find(p => (Mathf.Round(p.transform.localPosition.x) == 0) && (Mathf.Round(p.transform.localPosition.y) == -2) && (Mathf.Round(p.transform.localPosition.z) == 0)),
                AllCubePieces.Find(p => (Mathf.Round(p.transform.localPosition.x) == 0) && (Mathf.Round(p.transform.localPosition.y) == -2) && (Mathf.Round(p.transform.localPosition.z) == 1)),
                AllCubePieces.Find(p => (Mathf.Round(p.transform.localPosition.x) == 0) && (Mathf.Round(p.transform.localPosition.y) == -2) && (Mathf.Round(p.transform.localPosition.z) == 2)),
                AllCubePieces.Find(p => (Mathf.Round(p.transform.localPosition.x) == 0) && (Mathf.Round(p.transform.localPosition.y) == -1) && (Mathf.Round(p.transform.localPosition.z) == 2)),
                AllCubePieces.Find(p => (Mathf.Round(p.transform.localPosition.x) == 0) && (Mathf.Round(p.transform.localPosition.y) == 0) && (Mathf.Round(p.transform.localPosition.z) == 2))
            },
            {
                AllCubePieces.Find(p => (Mathf.Round(p.transform.localPosition.x) == -1) && (Mathf.Round(p.transform.localPosition.y) == 0) && (Mathf.Round(p.transform.localPosition.z) == 0)),
                AllCubePieces.Find(p => (Mathf.Round(p.transform.localPosition.x) == -1) && (Mathf.Round(p.transform.localPosition.y) == -1) && (Mathf.Round(p.transform.localPosition.z) == 0)),
                AllCubePieces.Find(p => (Mathf.Round(p.transform.localPosition.x) == -1) && (Mathf.Round(p.transform.localPosition.y) == -2) && (Mathf.Round(p.transform.localPosition.z) == 0)),
                AllCubePieces.Find(p => (Mathf.Round(p.transform.localPosition.x) == -1) && (Mathf.Round(p.transform.localPosition.y) == -2) && (Mathf.Round(p.transform.localPosition.z) == 1)),
                AllCubePieces.Find(p => (Mathf.Round(p.transform.localPosition.x) == -1) && (Mathf.Round(p.transform.localPosition.y) == -2) && (Mathf.Round(p.transform.localPosition.z) == 2)),
                AllCubePieces.Find(p => (Mathf.Round(p.transform.localPosition.x) == -1) && (Mathf.Round(p.transform.localPosition.y) == -1) && (Mathf.Round(p.transform.localPosition.z) == 2)),
                AllCubePieces.Find(p => (Mathf.Round(p.transform.localPosition.x) == -1) && (Mathf.Round(p.transform.localPosition.y) == 0) && (Mathf.Round(p.transform.localPosition.z) == 2))
            },
            {
                AllCubePieces.Find(p => (Mathf.Round(p.transform.localPosition.x) == -2) && (Mathf.Round(p.transform.localPosition.y) == 0) && (Mathf.Round(p.transform.localPosition.z) == 0)),
                AllCubePieces.Find(p => (Mathf.Round(p.transform.localPosition.x) == -2) && (Mathf.Round(p.transform.localPosition.y) == -1) && (Mathf.Round(p.transform.localPosition.z) == 0)),
                AllCubePieces.Find(p => (Mathf.Round(p.transform.localPosition.x) == -2) && (Mathf.Round(p.transform.localPosition.y) == -2) && (Mathf.Round(p.transform.localPosition.z) == 0)),
                AllCubePieces.Find(p => (Mathf.Round(p.transform.localPosition.x) == -2) && (Mathf.Round(p.transform.localPosition.y) == -2) && (Mathf.Round(p.transform.localPosition.z) == 1)),
                AllCubePieces.Find(p => (Mathf.Round(p.transform.localPosition.x) == -2) && (Mathf.Round(p.transform.localPosition.y) == -2) && (Mathf.Round(p.transform.localPosition.z) == 2)),
                AllCubePieces.Find(p => (Mathf.Round(p.transform.localPosition.x) == -2) && (Mathf.Round(p.transform.localPosition.y) == -1) && (Mathf.Round(p.transform.localPosition.z) == 2)),
                AllCubePieces.Find(p => (Mathf.Round(p.transform.localPosition.x) == -2) && (Mathf.Round(p.transform.localPosition.y) == 0) && (Mathf.Round(p.transform.localPosition.z) == 2))
                },
            {
                null,null,
                AllCubePieces.Find(p => (Mathf.Round(p.transform.localPosition.x) == -2) && (Mathf.Round(p.transform.localPosition.y) == -1) && (Mathf.Round(p.transform.localPosition.z) == 0)),
                AllCubePieces.Find(p => (Mathf.Round(p.transform.localPosition.x) == -2) && (Mathf.Round(p.transform.localPosition.y) == -1) && (Mathf.Round(p.transform.localPosition.z) == 1)),
                AllCubePieces.Find(p => (Mathf.Round(p.transform.localPosition.x) == -2) && (Mathf.Round(p.transform.localPosition.y) == -1) && (Mathf.Round(p.transform.localPosition.z) == 2)),
                null,null
            },
            {
                null,null,
                AllCubePieces.Find(p => (Mathf.Round(p.transform.localPosition.x) == -2) && (Mathf.Round(p.transform.localPosition.y) == 0) && (Mathf.Round(p.transform.localPosition.z) == 0)),
                AllCubePieces.Find(p => (Mathf.Round(p.transform.localPosition.x) == -2) && (Mathf.Round(p.transform.localPosition.y) == 0) && (Mathf.Round(p.transform.localPosition.z) == 1)),
                AllCubePieces.Find(p => (Mathf.Round(p.transform.localPosition.x) == -2) && (Mathf.Round(p.transform.localPosition.y) == 0) && (Mathf.Round(p.transform.localPosition.z) == 2)),
                null,null}
            };
        }
    }
    GameObject[,] FrontDesk
    {
        get
        {
            return new GameObject[7, 7] {
            {
                null,null,
                AllCubePieces.Find(p => (Mathf.Round(p.transform.localPosition.x) == -2) && (Mathf.Round(p.transform.localPosition.y) == 0) && (Mathf.Round(p.transform.localPosition.z) == 0)),
                AllCubePieces.Find(p => (Mathf.Round(p.transform.localPosition.x) == -2) && (Mathf.Round(p.transform.localPosition.y) == 0) && (Mathf.Round(p.transform.localPosition.z) == 1)),
                AllCubePieces.Find(p => (Mathf.Round(p.transform.localPosition.x) == -2) && (Mathf.Round(p.transform.localPosition.y) == 0) && (Mathf.Round(p.transform.localPosition.z) == 2)),
                null,null
            },
            {
                null,null,
                AllCubePieces.Find(p => (Mathf.Round(p.transform.localPosition.x) == -1) && (Mathf.Round(p.transform.localPosition.y) == 0) && (Mathf.Round(p.transform.localPosition.z) == 0)),
                AllCubePieces.Find(p => (Mathf.Round(p.transform.localPosition.x) == -1) && (Mathf.Round(p.transform.localPosition.y) == 0) && (Mathf.Round(p.transform.localPosition.z) == 1)),
                AllCubePieces.Find(p => (Mathf.Round(p.transform.localPosition.x) == -1) && (Mathf.Round(p.transform.localPosition.y) == 0) && (Mathf.Round(p.transform.localPosition.z) == 2)),
                null,null
            },
            {
                AllCubePieces.Find(p => (Mathf.Round(p.transform.localPosition.x) == -2) && (Mathf.Round(p.transform.localPosition.y) == 0) && (Mathf.Round(p.transform.localPosition.z) == 0)),
                AllCubePieces.Find(p => (Mathf.Round(p.transform.localPosition.x) == -1) && (Mathf.Round(p.transform.localPosition.y) == 0) && (Mathf.Round(p.transform.localPosition.z) == 0)),
                AllCubePieces.Find(p => (Mathf.Round(p.transform.localPosition.x) == 0) && (Mathf.Round(p.transform.localPosition.y) == 0) && (Mathf.Round(p.transform.localPosition.z) == 0)),
                AllCubePieces.Find(p => (Mathf.Round(p.transform.localPosition.x) == 0) && (Mathf.Round(p.transform.localPosition.y) == 0) && (Mathf.Round(p.transform.localPosition.z) == 1)),
                AllCubePieces.Find(p => (Mathf.Round(p.transform.localPosition.x) == 0) && (Mathf.Round(p.transform.localPosition.y) == 0) && (Mathf.Round(p.transform.localPosition.z) == 2)),
                AllCubePieces.Find(p => (Mathf.Round(p.transform.localPosition.x) == -1) && (Mathf.Round(p.transform.localPosition.y) == 0) && (Mathf.Round(p.transform.localPosition.z) == 2)),
                AllCubePieces.Find(p => (Mathf.Round(p.transform.localPosition.x) == -2) && (Mathf.Round(p.transform.localPosition.y) == 0) && (Mathf.Round(p.transform.localPosition.z) == 2))
            },
            {
                AllCubePieces.Find(p => (Mathf.Round(p.transform.localPosition.x) == -2) && (Mathf.Round(p.transform.localPosition.y) == -1) && (Mathf.Round(p.transform.localPosition.z) == 0)),
                AllCubePieces.Find(p => (Mathf.Round(p.transform.localPosition.x) == -1) && (Mathf.Round(p.transform.localPosition.y) == -1) && (Mathf.Round(p.transform.localPosition.z) == 0)),
                AllCubePieces.Find(p => (Mathf.Round(p.transform.localPosition.x) == 0) && (Mathf.Round(p.transform.localPosition.y) == -1) && (Mathf.Round(p.transform.localPosition.z) == 0)),
                AllCubePieces.Find(p => (Mathf.Round(p.transform.localPosition.x) == 0) && (Mathf.Round(p.transform.localPosition.y) == -1) && (Mathf.Round(p.transform.localPosition.z) == 1)),
                AllCubePieces.Find(p => (Mathf.Round(p.transform.localPosition.x) == 0) && (Mathf.Round(p.transform.localPosition.y) == -1) && (Mathf.Round(p.transform.localPosition.z) == 2)),
                AllCubePieces.Find(p => (Mathf.Round(p.transform.localPosition.x) == -1) && (Mathf.Round(p.transform.localPosition.y) == -1) && (Mathf.Round(p.transform.localPosition.z) == 2)),
                AllCubePieces.Find(p => (Mathf.Round(p.transform.localPosition.x) == -2) && (Mathf.Round(p.transform.localPosition.y) == -1) && (Mathf.Round(p.transform.localPosition.z) == 2))
            },
            {
                AllCubePieces.Find(p => (Mathf.Round(p.transform.localPosition.x) == -2) && (Mathf.Round(p.transform.localPosition.y) == -2) && (Mathf.Round(p.transform.localPosition.z) == 0)),
                AllCubePieces.Find(p => (Mathf.Round(p.transform.localPosition.x) == -1) && (Mathf.Round(p.transform.localPosition.y) == -2) && (Mathf.Round(p.transform.localPosition.z) == 0)),
                AllCubePieces.Find(p => (Mathf.Round(p.transform.localPosition.x) == 0) && (Mathf.Round(p.transform.localPosition.y) == -2) && (Mathf.Round(p.transform.localPosition.z) == 0)),
                AllCubePieces.Find(p => (Mathf.Round(p.transform.localPosition.x) == 0) && (Mathf.Round(p.transform.localPosition.y) == -2) && (Mathf.Round(p.transform.localPosition.z) == 1)),
                AllCubePieces.Find(p => (Mathf.Round(p.transform.localPosition.x) == 0) && (Mathf.Round(p.transform.localPosition.y) == -2) && (Mathf.Round(p.transform.localPosition.z) == 2)),
                AllCubePieces.Find(p => (Mathf.Round(p.transform.localPosition.x) == -1) && (Mathf.Round(p.transform.localPosition.y) == -2) && (Mathf.Round(p.transform.localPosition.z) == 2)),
                AllCubePieces.Find(p => (Mathf.Round(p.transform.localPosition.x) == -2) && (Mathf.Round(p.transform.localPosition.y) == -2) && (Mathf.Round(p.transform.localPosition.z) == 2))
                },
            {
                null,null,
                AllCubePieces.Find(p => (Mathf.Round(p.transform.localPosition.x) == -1) && (Mathf.Round(p.transform.localPosition.y) == -2) && (Mathf.Round(p.transform.localPosition.z) == 0)),
                AllCubePieces.Find(p => (Mathf.Round(p.transform.localPosition.x) == -1) && (Mathf.Round(p.transform.localPosition.y) == -2) && (Mathf.Round(p.transform.localPosition.z) == 1)),
                AllCubePieces.Find(p => (Mathf.Round(p.transform.localPosition.x) == -1) && (Mathf.Round(p.transform.localPosition.y) == -2) && (Mathf.Round(p.transform.localPosition.z) == 2)),
                null,null
            },
            {
                null,null,
                AllCubePieces.Find(p => (Mathf.Round(p.transform.localPosition.x) == -2) && (Mathf.Round(p.transform.localPosition.y) == -2) && (Mathf.Round(p.transform.localPosition.z) == 0)),
                AllCubePieces.Find(p => (Mathf.Round(p.transform.localPosition.x) == -2) && (Mathf.Round(p.transform.localPosition.y) == -2) && (Mathf.Round(p.transform.localPosition.z) == 1)),
                AllCubePieces.Find(p => (Mathf.Round(p.transform.localPosition.x) == -2) && (Mathf.Round(p.transform.localPosition.y) == -2) && (Mathf.Round(p.transform.localPosition.z) == 2)),
                null,null}
            };
        }
    }
    GameObject[,] BackDesk
    {
        get
        {
            return new GameObject[7, 7] {
            {
                null,null,
                AllCubePieces.Find(p => (Mathf.Round(p.transform.localPosition.x) == 0) && (Mathf.Round(p.transform.localPosition.y) == 0) && (Mathf.Round(p.transform.localPosition.z) == 2)),
                AllCubePieces.Find(p => (Mathf.Round(p.transform.localPosition.x) == 0) && (Mathf.Round(p.transform.localPosition.y) == 0) && (Mathf.Round(p.transform.localPosition.z) == 1)),
                AllCubePieces.Find(p => (Mathf.Round(p.transform.localPosition.x) == 0) && (Mathf.Round(p.transform.localPosition.y) == 0) && (Mathf.Round(p.transform.localPosition.z) == 0)),
                null,null
            },
            {
                null,null,
                AllCubePieces.Find(p => (Mathf.Round(p.transform.localPosition.x) == -1) && (Mathf.Round(p.transform.localPosition.y) == 0) && (Mathf.Round(p.transform.localPosition.z) == 2)),
                AllCubePieces.Find(p => (Mathf.Round(p.transform.localPosition.x) == -1) && (Mathf.Round(p.transform.localPosition.y) == 0) && (Mathf.Round(p.transform.localPosition.z) == 1)),
                AllCubePieces.Find(p => (Mathf.Round(p.transform.localPosition.x) == -1) && (Mathf.Round(p.transform.localPosition.y) == 0) && (Mathf.Round(p.transform.localPosition.z) == 0)),
                null,null
            },
            {
                AllCubePieces.Find(p => (Mathf.Round(p.transform.localPosition.x) == 0) && (Mathf.Round(p.transform.localPosition.y) == 0) && (Mathf.Round(p.transform.localPosition.z) == 2)),
                AllCubePieces.Find(p => (Mathf.Round(p.transform.localPosition.x) == -1) && (Mathf.Round(p.transform.localPosition.y) == 0) && (Mathf.Round(p.transform.localPosition.z) == 2)),
                AllCubePieces.Find(p => (Mathf.Round(p.transform.localPosition.x) == -2) && (Mathf.Round(p.transform.localPosition.y) == 0) && (Mathf.Round(p.transform.localPosition.z) == 2)),
                AllCubePieces.Find(p => (Mathf.Round(p.transform.localPosition.x) == -2) && (Mathf.Round(p.transform.localPosition.y) == 0) && (Mathf.Round(p.transform.localPosition.z) == 1)),
                AllCubePieces.Find(p => (Mathf.Round(p.transform.localPosition.x) == -2) && (Mathf.Round(p.transform.localPosition.y) == 0) && (Mathf.Round(p.transform.localPosition.z) == 0)),
                AllCubePieces.Find(p => (Mathf.Round(p.transform.localPosition.x) == -1) && (Mathf.Round(p.transform.localPosition.y) == 0) && (Mathf.Round(p.transform.localPosition.z) == 0)),
                AllCubePieces.Find(p => (Mathf.Round(p.transform.localPosition.x) == 0) && (Mathf.Round(p.transform.localPosition.y) == 0) && (Mathf.Round(p.transform.localPosition.z) == 0))
            },
            {
                AllCubePieces.Find(p => (Mathf.Round(p.transform.localPosition.x) == 0) && (Mathf.Round(p.transform.localPosition.y) == -1) && (Mathf.Round(p.transform.localPosition.z) == 2)),
                AllCubePieces.Find(p => (Mathf.Round(p.transform.localPosition.x) == -1) && (Mathf.Round(p.transform.localPosition.y) == -1) && (Mathf.Round(p.transform.localPosition.z) == 2)),
                AllCubePieces.Find(p => (Mathf.Round(p.transform.localPosition.x) == -2) && (Mathf.Round(p.transform.localPosition.y) == -1) && (Mathf.Round(p.transform.localPosition.z) == 2)),
                AllCubePieces.Find(p => (Mathf.Round(p.transform.localPosition.x) == -2) && (Mathf.Round(p.transform.localPosition.y) == -1) && (Mathf.Round(p.transform.localPosition.z) == 1)),
                AllCubePieces.Find(p => (Mathf.Round(p.transform.localPosition.x) == -2) && (Mathf.Round(p.transform.localPosition.y) == -1) && (Mathf.Round(p.transform.localPosition.z) == 0)),
                AllCubePieces.Find(p => (Mathf.Round(p.transform.localPosition.x) == -1) && (Mathf.Round(p.transform.localPosition.y) == -1) && (Mathf.Round(p.transform.localPosition.z) == 0)),
                AllCubePieces.Find(p => (Mathf.Round(p.transform.localPosition.x) == 0) && (Mathf.Round(p.transform.localPosition.y) == -1) && (Mathf.Round(p.transform.localPosition.z) == 0))
            },
            {
                AllCubePieces.Find(p => (Mathf.Round(p.transform.localPosition.x) == 0) && (Mathf.Round(p.transform.localPosition.y) == -2) && (Mathf.Round(p.transform.localPosition.z) == 2)),
                AllCubePieces.Find(p => (Mathf.Round(p.transform.localPosition.x) == -1) && (Mathf.Round(p.transform.localPosition.y) == -2) && (Mathf.Round(p.transform.localPosition.z) == 2)),
                AllCubePieces.Find(p => (Mathf.Round(p.transform.localPosition.x) == -2) && (Mathf.Round(p.transform.localPosition.y) == -2) && (Mathf.Round(p.transform.localPosition.z) == 2)),
                AllCubePieces.Find(p => (Mathf.Round(p.transform.localPosition.x) == -2) && (Mathf.Round(p.transform.localPosition.y) == -2) && (Mathf.Round(p.transform.localPosition.z) == 1)),
                AllCubePieces.Find(p => (Mathf.Round(p.transform.localPosition.x) == -2) && (Mathf.Round(p.transform.localPosition.y) == -2) && (Mathf.Round(p.transform.localPosition.z) == 0)),
                AllCubePieces.Find(p => (Mathf.Round(p.transform.localPosition.x) == -1) && (Mathf.Round(p.transform.localPosition.y) == -2) && (Mathf.Round(p.transform.localPosition.z) == 0)),
                AllCubePieces.Find(p => (Mathf.Round(p.transform.localPosition.x) == 0) && (Mathf.Round(p.transform.localPosition.y) == -2) && (Mathf.Round(p.transform.localPosition.z) == 0))
                },
            {
                null,null,
                AllCubePieces.Find(p => (Mathf.Round(p.transform.localPosition.x) == -1) && (Mathf.Round(p.transform.localPosition.y) == -2) && (Mathf.Round(p.transform.localPosition.z) == 2)),
                AllCubePieces.Find(p => (Mathf.Round(p.transform.localPosition.x) == -1) && (Mathf.Round(p.transform.localPosition.y) == -2) && (Mathf.Round(p.transform.localPosition.z) == 1)),
                AllCubePieces.Find(p => (Mathf.Round(p.transform.localPosition.x) == -1) && (Mathf.Round(p.transform.localPosition.y) == -2) && (Mathf.Round(p.transform.localPosition.z) == 0)),
                null,null
            },
            {
                null,null,
                AllCubePieces.Find(p => (Mathf.Round(p.transform.localPosition.x) == 0) && (Mathf.Round(p.transform.localPosition.y) == -2) && (Mathf.Round(p.transform.localPosition.z) == 2)),
                AllCubePieces.Find(p => (Mathf.Round(p.transform.localPosition.x) == 0) && (Mathf.Round(p.transform.localPosition.y) == -2) && (Mathf.Round(p.transform.localPosition.z) == 1)),
                AllCubePieces.Find(p => (Mathf.Round(p.transform.localPosition.x) == 0) && (Mathf.Round(p.transform.localPosition.y) == -2) && (Mathf.Round(p.transform.localPosition.z) == 0)),
                null,null}
            };
        }
    }

    void Start()
    {
        CubeTransf = transform;
        CreateCube();
    }
    void Update()
    {
        if (CanRotate)
            CheckInput();
    }


    void CreateCube()
    {
        if (AllCubePieces.Count != 27)
        {
            for (int x = 0; x < 3; x++)
                for (int y = 0; y < 3; y++)
                    for (int z = 0; z < 3; z++)
                    {
                        GameObject go = Instantiate(CubePiecePref, CubeTransf, false);
                        go.transform.localPosition = new Vector3(-x, -y, z);
                        go.GetComponent<CubePieceScript>().SetColor(-x, -y, z);
                        AllCubePieces.Add(go);

                    }
            CubeCenterPiece = AllCubePieces[13];
        }
    }
    void CheckInput()
    {
        if (Input.GetKeyDown(KeyCode.W))
        { StartCoroutine(Rotate(UpPieces, new Vector3(0, 1, 0), 5)); }
        else if (Input.GetKeyDown(KeyCode.S))
        { StartCoroutine(Rotate(DownPieces, new Vector3(0, -1, 0), 5)); }
        else if (Input.GetKeyDown(KeyCode.A))
        { StartCoroutine(Rotate(LeftPieces, new Vector3(0, 0, 1), 5)); }
        else if (Input.GetKeyDown(KeyCode.D))
        { StartCoroutine(Rotate(RightPieces, new Vector3(0, 0, -1), 5)); }
        else if (Input.GetKeyDown(KeyCode.F))
        { StartCoroutine(Rotate(FrontPieces, new Vector3(1, 0, 0), 5)); }
        else if (Input.GetKeyDown(KeyCode.G))
        { StartCoroutine(Rotate(BackPieces, new Vector3(-1, 0, 0), 5)); }

        else if (Input.GetKeyDown(KeyCode.R) && CanShufle)
        { StartCoroutine(Shufle()); }
    }
    IEnumerator Shufle()
    {
        CanShufle = false;

        for (int moveCount = Random.Range(15,30); moveCount >= 0; moveCount--)
        {
            int edge = Random.Range(0, 6);
            List<GameObject> edgePieces = new List<GameObject>();

            switch (edge)
            {
                case 0: edgePieces = UpPieces; break;
                case 1: edgePieces = DownPieces; break;
                case 2: edgePieces = LeftPieces; break;
                case 3: edgePieces = RightPieces; break;
                case 4: edgePieces = FrontPieces; break;
                case 5: edgePieces = BackPieces; break;
            }
            StartCoroutine(Rotate(edgePieces, RotationVectors[edge], 5));
            yield return new WaitForSeconds(.3f);
        }
        CanShufle = true;
    }
    IEnumerator Rotate(List<GameObject> pieces, Vector3 rotationVector, int speed = 5)
    {
        sessionManager.GetComponent<SessionManagerScript>().CanMakeMove = false;
        CanRotate = false;
        int angle = 0;
        while (angle < 90)
        {
            foreach (GameObject go in pieces)
            {
                go.transform.RotateAround(CubeCenterPiece.transform.position, rotationVector, speed);
            }

            angle += speed;
            yield return null;
        }
        CanRotate = true;
        CheckGameEnd();
    }
    public void DetectRorate(List<GameObject> pieces, List<GameObject> planes)
    {
        if (!CanRotate || !CanShufle)
        {
            return;
        }

        if (UpVerticalPieces.Exists(x => x == pieces[0]) &&
            UpVerticalPieces.Exists(x => x == pieces[1]))
            StartCoroutine(Rotate(UpVerticalPieces, new Vector3(0, 0, 1 * DetectLeftMidleRightSign(pieces)), 5));

        else if (UpHorizontalPieces.Exists(x => x == pieces[0]) &&
            UpHorizontalPieces.Exists(x => x == pieces[1]))
            StartCoroutine(Rotate(UpHorizontalPieces, new Vector3(1 * DetectFrontMidleBackSign(pieces), 0, 0), 5));

        else if (FrontHorizontalPieces.Exists(x => x == pieces[0]) &&
            FrontHorizontalPieces.Exists(x => x == pieces[1]))
            StartCoroutine(Rotate(FrontHorizontalPieces, new Vector3(0, 1 * DetectUpMidleDownkSign(pieces), 0), 5));

        else if (DetectSide(planes, new Vector3(1, 0, 0), new Vector3(0, 0, 1), UpPieces))
            StartCoroutine(Rotate(UpPieces, new Vector3(0, 1 * DetectUpMidleDownkSign(pieces), 0), 5));
        else if (DetectSide(planes, new Vector3(1, 0, 0), new Vector3(0, 0, 1), DownPieces))
            StartCoroutine(Rotate(DownPieces, new Vector3(0, 1 * DetectUpMidleDownkSign(pieces), 0), 5));
        else if (DetectSide(planes, new Vector3(0, 0, 1), new Vector3(0, 1, 0), FrontPieces))
            StartCoroutine(Rotate(FrontPieces, new Vector3(1 * DetectFrontMidleBackSign(pieces), 0, 0), 5));
        else if (DetectSide(planes, new Vector3(0, 0, 1), new Vector3(0, 1, 0), BackPieces))
            StartCoroutine(Rotate(BackPieces, new Vector3(1 * DetectFrontMidleBackSign(pieces), 0, 0), 5));
        else if (DetectSide(planes, new Vector3(1, 0, 0), new Vector3(0, 1, 0), LeftPieces))
            StartCoroutine(Rotate(LeftPieces, new Vector3(0, 0, 1 * DetectLeftMidleRightSign(pieces)), 5));
        else if (DetectSide(planes, new Vector3(1, 0, 0), new Vector3(0, 1, 0), RightPieces))
            StartCoroutine(Rotate(RightPieces, new Vector3(0, 0, 1 * DetectLeftMidleRightSign(pieces)), 5));
    }
    bool DetectSide(List<GameObject> planes, Vector3 firstDirection, Vector3 secondDiraction, List<GameObject> side)
    {
        GameObject centerPiece = side.Find(x => x.GetComponent<CubePieceScript>().Planes.FindAll(y => y.activeInHierarchy).Count == 1);

        List<RaycastHit> hit1 = new List<RaycastHit>(Physics.RaycastAll(planes[1].transform.position, firstDirection)),
                         hit2 = new List<RaycastHit>(Physics.RaycastAll(planes[0].transform.position, firstDirection)),
                         hit1_m = new List<RaycastHit>(Physics.RaycastAll(planes[1].transform.position, -firstDirection)),
                         hit2_m = new List<RaycastHit>(Physics.RaycastAll(planes[0].transform.position, -firstDirection)),

                         hit3 = new List<RaycastHit>(Physics.RaycastAll(planes[1].transform.position, secondDiraction)),
                         hit4 = new List<RaycastHit>(Physics.RaycastAll(planes[0].transform.position, secondDiraction)),
                         hit3_m = new List<RaycastHit>(Physics.RaycastAll(planes[1].transform.position, -secondDiraction)),
                         hit4_m = new List<RaycastHit>(Physics.RaycastAll(planes[0].transform.position, -secondDiraction));

        return hit1.Exists(x => x.collider.gameObject == centerPiece) ||
               hit2.Exists(x => x.collider.gameObject == centerPiece) ||
               hit1_m.Exists(x => x.collider.gameObject == centerPiece) ||
               hit1_m.Exists(x => x.collider.gameObject == centerPiece) ||

               hit3.Exists(x => x.collider.gameObject == centerPiece) ||
               hit4.Exists(x => x.collider.gameObject == centerPiece) ||
               hit3_m.Exists(x => x.collider.gameObject == centerPiece) ||
               hit4_m.Exists(x => x.collider.gameObject == centerPiece);
    }
    float DetectLeftMidleRightSign(List<GameObject> pieces)
    {
        float sign = 0;
        if (Mathf.Round(pieces[1].transform.position.y) != Mathf.Round(pieces[0].transform.position.y))
        {
            if (Mathf.Round(pieces[0].transform.position.x) == -2)
                sign = Mathf.Round(pieces[0].transform.position.y) - Mathf.Round(pieces[1].transform.position.y);
            else
                sign = Mathf.Round(pieces[1].transform.position.y) - Mathf.Round(pieces[0].transform.position.y);
        }
        else
        { 
            if (Mathf.Round(pieces[0].transform.position.y) == -2)
                sign = Mathf.Round(pieces[1].transform.position.x) - Mathf.Round(pieces[0].transform.position.x);
            else
                sign = Mathf.Round(pieces[0].transform.position.x) - Mathf.Round(pieces[1].transform.position.x);
        }


        return sign;
    }
    float DetectFrontMidleBackSign(List<GameObject> pieces)
    {
        float sign = 0;
        if (Mathf.Round(pieces[1].transform.position.z) != Mathf.Round(pieces[0].transform.position.z))
        {
            if (Mathf.Round(pieces[0].transform.position.y) == 0)
                sign = Mathf.Round(pieces[1].transform.position.z) - Mathf.Round(pieces[0].transform.position.z);
            else
                sign = Mathf.Round(pieces[0].transform.position.z) - Mathf.Round(pieces[1].transform.position.z);
        }
        else
        {
            if (Mathf.Round(pieces[0].transform.position.z) == 0)
                sign = Mathf.Round(pieces[1].transform.position.y) - Mathf.Round(pieces[0].transform.position.y);
            else
                sign = Mathf.Round(pieces[0].transform.position.y) - Mathf.Round(pieces[1].transform.position.y);
        }


        return sign;
    }
    float DetectUpMidleDownkSign(List<GameObject> pieces)
    {
        float sign = 0;
        if (Mathf.Round(pieces[1].transform.position.z) != Mathf.Round(pieces[0].transform.position.z))
        {
            if (Mathf.Round(pieces[0].transform.position.x) == -2)
                sign = Mathf.Round(pieces[1].transform.position.z) - Mathf.Round(pieces[0].transform.position.z);
            else
                sign = Mathf.Round(pieces[0].transform.position.z) - Mathf.Round(pieces[1].transform.position.z);
        }
        else
        {
            if (Mathf.Round(pieces[0].transform.position.z) == 0)
                sign = Mathf.Round(pieces[0].transform.position.x) - Mathf.Round(pieces[1].transform.position.x);
            else
                sign = Mathf.Round(pieces[1].transform.position.x) - Mathf.Round(pieces[0].transform.position.x);
        }


        return sign;
    }

    public void SetPieceMark(GameObject piece)
    {
        GameObject go = AllCubePieces.Find(x => x.Equals(piece));
        if (go.GetComponent<CubePieceScript>().CanMark)
        {
            if (sessionManager.GetComponent<SessionManagerScript>().SessionOption == 1)
                go.GetComponent<CubePieceScript>().SetMark(sessionManager.GetComponent<SessionManagerScript>().PlayerTurn);
            else 
            if (sessionManager.GetComponent<SessionManagerScript>().SessionOption == 2)
                if (sessionManager.GetComponent<SessionManagerScript>().sequence == 1)
                    go.GetComponent<CubePieceScript>().SetMark(true);
                else
                    go.GetComponent<CubePieceScript>().SetMark(false);
            CheckGameEnd();
        }
    }
    void CheckGameEnd()
    {
        sessionManager.GetComponent<SessionManagerScript>().CanMakeMove = false;
        List<GameObject[,]> desks = new List<GameObject[,]> { UpDesk, DownDesk, FrontDesk, BackDesk };
        bool isPlayer1Win = false,
            isPlayer2Win = false;

        foreach (GameObject[,] desk in desks)
        {
            if (desk != null)
            {
                for (int y = 1; y < 6; y++)
                {
                    GameObject[,] miniDesk = new GameObject[3, 3] {
                    { desk[y - 1, 2], desk[y - 1, 3], desk[y - 1, 4] },
                    { desk[y, 2], desk[y, 3], desk[y, 4] },
                    { desk[y + 1, 2], desk[y + 1, 3], desk[y + 1, 4] }};
                    if (CheckMarkMatches(miniDesk, 1))
                        isPlayer1Win = true;
                    if (CheckMarkMatches(miniDesk, 2))
                        isPlayer2Win = true;
                }
                for (int x = 1; x < 6; x++)
                {
                    GameObject[,] miniDesk = new GameObject[3, 3] {
                    { desk[2, x-1], desk[2, x], desk[2, x+1] },
                    { desk[3, x-1], desk[3, x], desk[3, x+1] },
                    { desk[4, x-1], desk[4, x], desk[4, x+1] }};
                    if (CheckMarkMatches(miniDesk, 1))
                        isPlayer1Win = true;
                    if (CheckMarkMatches(miniDesk, 2))
                        isPlayer2Win = true;
                }
            }
        }

        sessionManager.GetComponent<SessionManagerScript>().TurnEnd();

        int vinner = -1;
        if (isPlayer1Win && isPlayer2Win)
            vinner = 3;
        if (isPlayer1Win)
            vinner = 1;
        if (isPlayer2Win)
            vinner = 2;
        if (vinner > -1)
            ToGameEnd(vinner);
    }
    bool CheckMarkMatches(GameObject[,] desk, int mark)
    {
        bool result = false;

        if (desk[0, 0].GetComponent<CubePieceScript>().Mark == mark)
            if (desk[1, 0].GetComponent<CubePieceScript>().Mark == mark)
                if (desk[2, 0].GetComponent<CubePieceScript>().Mark == mark)
                    result = true; // левая вертикаль

        if (desk[0, 1].GetComponent<CubePieceScript>().Mark == mark)
            if (desk[1, 1].GetComponent<CubePieceScript>().Mark == mark)
                if (desk[2, 1].GetComponent<CubePieceScript>().Mark == mark)
                    result = true; // средняя вертикаль

        if (desk[0, 2].GetComponent<CubePieceScript>().Mark == mark)
            if (desk[1, 2].GetComponent<CubePieceScript>().Mark == mark)
                if (desk[2, 2].GetComponent<CubePieceScript>().Mark == mark)
                    result = true; // правая вертикаль

        if (desk[0, 0].GetComponent<CubePieceScript>().Mark == mark)
            if (desk[0, 1].GetComponent<CubePieceScript>().Mark == mark)
                if (desk[0, 2].GetComponent<CubePieceScript>().Mark == mark)
                    result = true; // верхняя горизонталь

        if (desk[1, 0].GetComponent<CubePieceScript>().Mark == mark)
            if (desk[1, 1].GetComponent<CubePieceScript>().Mark == mark)
                if (desk[1, 2].GetComponent<CubePieceScript>().Mark == mark)
                    result = true; // средняя горизонталь

        if (desk[2, 0].GetComponent<CubePieceScript>().Mark == mark)
            if (desk[2, 1].GetComponent<CubePieceScript>().Mark == mark)
                if (desk[2, 2].GetComponent<CubePieceScript>().Mark == mark)
                    result = true; // нижняя горизонталь

        if (desk[0, 0].GetComponent<CubePieceScript>().Mark == mark)
            if (desk[1, 1].GetComponent<CubePieceScript>().Mark == mark)
                if (desk[2, 2].GetComponent<CubePieceScript>().Mark == mark)
                    result = true; // лева диагональ

        if (desk[0, 2].GetComponent<CubePieceScript>().Mark == mark)
            if (desk[1, 1].GetComponent<CubePieceScript>().Mark == mark)
                if (desk[2, 0].GetComponent<CubePieceScript>().Mark == mark)
                    result = true; // правая диагональ

        return result;
    }


    public string CubePack()
    {
        string result = string.Empty;
        //foreach(GameObject g in AllCubePieces)
        foreach (GameObject g in GetPiecesList())
        {
            result += g.GetComponent<CubePieceScript>().Mark;
        }
        return result;
    }

    public void CubeUnpack(string state)
    {
        List<GameObject> list = GetPiecesList();
        foreach (GameObject g in list)
            g.GetComponent<CubePieceScript>().Clean();

        for (int i = 0; i < state.Length; i++)
            if (state[i] == '1')
                list[i].GetComponent<CubePieceScript>().SetMark(true);
            else
                if (state[i] == '2')
                list[i].GetComponent<CubePieceScript>().SetMark(false);

        CheckGameEnd();
    }
    public void CubeUnpackStart(string state)
    {
        Start();
        List<GameObject> list = GetPiecesList();
        foreach (GameObject g in list)
            g.GetComponent<CubePieceScript>().Clean();
        for (int i = 0; i < state.Length; i++)
            if (state[i] == '1')
                list[i].GetComponent<CubePieceScript>().SetMark(true);
            else
                if (state[i] == '2')
                list[i].GetComponent<CubePieceScript>().SetMark(false);
    }
    public List<GameObject> GetPiecesList()
    {
        List<GameObject> result = new List<GameObject>();
        foreach (GameObject go in UpDesk)
            if (go != null && !result.Exists(x => x == go))
                result.Add(go);
        result.Add(DownDesk[3,3]);
        return result;
    }
    public void ToGameEnd(int vinner)
    {
        sessionManager.GetComponent<SessionManagerScript>().vinner = vinner;
        sessionManager.GetComponent<SessionManagerScript>().GameEnd();
    }
}
