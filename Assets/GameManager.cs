using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
    public GameObject cube;
    public Transform StartCube;
    public Transform CubesParent;

    [HideInInspector]
    public bool bActive;

    private float zPos;
    private GameObject _cube;
    public int switcher;
    private float initialPos = 4.5f;

    private float originalX = 3.5f;
    private float originalZ = 3.5f;
    private float valueToChopOff;
    private Transform bottomCube;
    private float bottomCubeRightEdge;
    private float bottomCubeLeftEdge;
    private string chopAxis;
    private float leftEdgeXCoord;
    private float leftEdgeZCoord;
    private float xRef;
    private float zRef;


    [Header("Visuals")]
    public Color _color;
    private ColorChange colorChange;

    private Vector3 nextScale = new Vector3(3.5f, 0.2f, 3.5f);


    void Start()
    {
        bActive = true;
        switcher = 0;
        colorChange = GetComponent<ColorChange>();
    }

    void Update()
    {
        if (bActive) 
        {
            //Trigger on mouse click and when cube is non-existent
            if (Input.GetMouseButtonDown(0) && !_cube)
            {
                //SpawnCube(nextScale, _color);
                SpawnCube2(nextScale, _color);
                if (switcher == 0)
                    _cube.transform.position = new Vector3(0, 0, initialPos);
                else
                    _cube.transform.position = new Vector3(-initialPos, 0, 0);
            }

            else if (_cube)
            {
                if (_cube.transform.position.z < -CubesParent.GetChild(0).localScale.z)
                    Lose(_cube);

                if (switcher == 0)
                {
                    zPos = _cube.transform.position.z - 0.025f;
                    if (CubesParent.GetChild(CubesParent.childCount - 2).name == "Base Cube")
                        _cube.transform.position = new Vector3(0, CubesParent.GetChild(CubesParent.childCount - 2).localScale.y / 2 + cube.transform.localScale.y / 2, zPos);
                    else
                        _cube.transform.position = new Vector3(0, CubesParent.GetChild(CubesParent.childCount - 2).position.y + cube.transform.localScale.y, zPos);
                }
                else
                {
                    zPos = _cube.transform.position.x + 0.025f;
                    if (CubesParent.GetChild(CubesParent.childCount - 2).name == "Base Cube")
                        _cube.transform.position = new Vector3(zPos, CubesParent.GetChild(CubesParent.childCount - 2).localScale.y / 2 + cube.transform.localScale.y / 2, 0);
                    else
                        _cube.transform.position = new Vector3(zPos, CubesParent.GetChild(CubesParent.childCount - 2).position.y + cube.transform.localScale.y, 0);
                }

                if (Input.GetMouseButtonDown(0) && _cube.transform.position.z > -3.5f && _cube.transform.position.z < 3.5f) //merge these 2 ifs
                {
                    if (switcher == 0)
                        switcher = 1;
                    else
                        switcher = 0;
                    //chopOff(_cube);
                    _color = colorChange.tempColor;
                    //SpawnCube(nextScale, _color);
                    SpawnCubeChopped(_cube);
                    SpawnCube2(nextScale, _color);

                    //chopOff(_cube);
                    if (switcher == 0)
                        _cube.transform.position = new Vector3(0, 0, initialPos);
                    else
                        _cube.transform.position = new Vector3(-initialPos, 0, 0);
                }
            }
        }
    }



    public void chopOff(GameObject _parentCube)
    {
        //Coming from x direction => Chop x
        if (switcher == 0)
        {
            leftEdgeXCoord = _parentCube.transform.position.x;
            if (leftEdgeXCoord + _parentCube.transform.localScale.x > CubesParent.GetChild(CubesParent.childCount - 2).position.x + (CubesParent.GetChild(CubesParent.childCount - 2).localScale.x / 2))
            {
                chopOffRight(_parentCube, "x");
            }
            else
            {
                chopOffLeft(_parentCube, "x");
            }

        }
        //Coming from z direction => Chop z
        else
        {
            leftEdgeZCoord = _parentCube.transform.position.z;
            if (leftEdgeZCoord + _parentCube.transform.localScale.z < CubesParent.GetChild(CubesParent.childCount - 2).position.z + (CubesParent.GetChild(CubesParent.childCount - 2).localScale.z / 2))
            {
                chopOffRight(_parentCube, "z");


            }
            else
            {
                chopOffLeft(_parentCube, "z");
            }
        }

        //Add rigid body component and add useGravityOn

    }

    public void chopOffLeft(GameObject _parentCube, string chopAxis)
    {
        Debug.Log("reach1");
        bottomCube = CubesParent.GetChild(CubesParent.childCount - 2);
        if (chopAxis == "x")
        {
            bottomCubeLeftEdge = (bottomCube.position.x - bottomCube.localScale.x / 2);
            //Distance between the bottom cube's right edge and the current cube's right edge
            valueToChopOff = System.Math.Abs(bottomCubeLeftEdge - (_parentCube.transform.position.x + _parentCube.transform.localScale.x));
            //Shift right then chop
            xRef = _parentCube.transform.position.x;
            xRef += valueToChopOff;
            nextScale = new Vector3(originalX - valueToChopOff, _parentCube.transform.localScale.y, originalZ);
            _parentCube.transform.localScale = nextScale;
            originalX -= valueToChopOff;
        }
        else
        {
            bottomCubeLeftEdge = (bottomCube.position.z - bottomCube.localScale.z / 2);
            //Distance between the bottom cube's right edge and the current cube's right edge
            valueToChopOff = Math.Abs(bottomCubeLeftEdge - (_parentCube.transform.position.z + _parentCube.transform.localScale.z));
            zRef = _parentCube.transform.position.z;
            zRef += valueToChopOff;
            nextScale = new Vector3(originalX, _parentCube.transform.localScale.y, originalZ - valueToChopOff);
            originalZ -= valueToChopOff;


        }
        _parentCube.transform.localScale = nextScale;



    }

    public void chopOffRight(GameObject _parentCube, string chopAxis)
    {
        Debug.Log("reach2");
        bottomCube = CubesParent.GetChild(CubesParent.childCount - 2);
        if (chopAxis == "x")
        {
            bottomCubeRightEdge = (bottomCube.position.x + bottomCube.localScale.x);
            //Distance between the bottom cube's right edge and the current cube's right edge
            valueToChopOff = Math.Abs(bottomCubeRightEdge - (_parentCube.transform.position.x + _parentCube.transform.localScale.x));

            nextScale = new Vector3(originalX - valueToChopOff, _parentCube.transform.localScale.y, originalZ);
            originalX -= valueToChopOff;
        }
        else
        {
            bottomCubeRightEdge = (bottomCube.position.z + bottomCube.localScale.z);
            //Distance between the bottom cube's right edge and the current cube's right edge
            valueToChopOff = Math.Abs(bottomCubeRightEdge - (_parentCube.transform.position.z + _parentCube.transform.localScale.z));

            nextScale = new Vector3(originalX, _parentCube.transform.localScale.y, originalZ - valueToChopOff);
            originalZ -= valueToChopOff;


        }
        _parentCube.transform.localScale = nextScale;



    }

    public void Lose(GameObject _cube)
    {
        bActive = false;
        _cube.GetComponent<Rigidbody>().useGravity = true;
        _cube.GetComponent<Rigidbody>().isKinematic = false;
        StartCoroutine(DeleteCube(_cube));
    }

    IEnumerator DeleteCube(GameObject _cube)
    {
        yield return new WaitForSeconds(3);
        Destroy(_cube);
    }

    public void SpawnCube(Vector3 size, Color _color) //spawns a cube(tempCube) inside a parent gameobject(_cube) and handles the size and color
    {
        _cube = Instantiate(new GameObject());
        _cube.name = "test";
        Rigidbody rb = _cube.AddComponent<Rigidbody>();
        rb.isKinematic = true;
        rb.useGravity = false;
        GameObject tempCube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        tempCube.name = "Test2";
        tempCube.transform.parent = _cube.transform;
        if (switcher == 0)
            tempCube.transform.position = new Vector3(0, 0, -tempCube.transform.localScale.z / 2);
        else
            tempCube.transform.position = new Vector3(0, 0, -tempCube.transform.localScale.z / 2);
        tempCube.GetComponent<Renderer>().material.color = _color;
        _cube.transform.localScale = size;
        _cube.transform.parent = CubesParent;
    }

    public void SpawnCube2(Vector3 size, Color _color) //testing other method of spawning and chopping
    {
        /*
        _cube = Instantiate(new GameObject());
        _cube.name = "test";
        Rigidbody rb = _cube.AddComponent<Rigidbody>();
        rb.isKinematic = true;
        rb.useGravity = false;
        */
        _cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        _cube.name = "Test2";
        _cube.transform.parent = _cube.transform;
        _cube.GetComponent<Renderer>().material.color = _color;
        _cube.transform.localScale = size;
        _cube.transform.parent = CubesParent;
    }

    public void SpawnCubeChopped(GameObject currentCube)
    {
        Transform UnderCube = CubesParent.GetChild(CubesParent.childCount - 2).transform;

        if (switcher == 0)
        {
            Vector3 CurrentCubeRightEdge = new Vector3(0, currentCube.transform.position.y, currentCube.transform.localScale.z / 2 + currentCube.transform.position.z);
            Vector3 UnderCubeRightEdge = new Vector3(0, UnderCube.position.y, UnderCube.localScale.z / 2 + UnderCube.position.z);

            Vector3 CurrentCubeLeftEdge = -CurrentCubeRightEdge;
            Vector3 UnderCubeLeftEdge = -UnderCubeRightEdge;

            float DistanceBetweenRight = Vector3.Distance(CurrentCubeRightEdge, UnderCubeRightEdge);
            float DistanceBetweenLeft = Vector3.Distance(CurrentCubeLeftEdge, UnderCubeLeftEdge);

            if (DistanceBetweenRight > 0)
            {
                _cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                _cube.transform.localScale = new Vector3(currentCube.transform.localScale.x, currentCube.transform.localScale.y, currentCube.transform.localScale.z - (DistanceBetweenRight / 2));
                _cube.transform.position = new Vector3(currentCube.transform.position.x, currentCube.transform.position.y, currentCube.transform.position.z);
                _cube.transform.parent = CubesParent;
                Destroy(currentCube);
            }
        }

        else
        {
            Vector3 CurrentCubeRightEdge = new Vector3(currentCube.transform.localScale.x / 2 + currentCube.transform.position.x, currentCube.transform.position.y, 0);
            Vector3 UnderCubeRightEdge = new Vector3(UnderCube.localScale.x / 2 + UnderCube.position.x, UnderCube.position.y, 0);

            Vector3 CurrentCubeLeftEdge = -CurrentCubeRightEdge;
            Vector3 UnderCubeLeftEdge = -UnderCubeRightEdge;

            float DistanceBetweenRight = Vector3.Distance(CurrentCubeRightEdge, UnderCubeRightEdge);
            float DistanceBetweenLeft = Vector3.Distance(CurrentCubeLeftEdge, UnderCubeLeftEdge);

            if (DistanceBetweenRight > 0)
            {
                _cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                _cube.transform.localScale = new Vector3(currentCube.transform.localScale.x - (DistanceBetweenRight / 2), currentCube.transform.localScale.y, currentCube.transform.localScale.z);
                _cube.transform.position = new Vector3(currentCube.transform.position.x, currentCube.transform.position.y, currentCube.transform.position.z);
                _cube.transform.parent = CubesParent;
                Destroy(currentCube);
            }
        }

        //Destroy(currentCube);
    }
}
