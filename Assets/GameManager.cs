using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
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

   // public bool emergingFromX;
   // public bool emergingFromZ;




    void Start()
    {
        bActive = true;
        switcher = 0;
    }

    void Update()
    {
        if (bActive) 
        {
            //Trigger on mouse click and when cube is non-existent
            if (Input.GetMouseButtonDown(0) && !_cube)
            {
                _cube = Instantiate(cube, CubesParent); //use CreatePrimitive here instead
            }

            if (_cube)
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

                if (Input.GetMouseButtonDown(0)) //merge these 2 ifs
                {
                    
                    if (_cube.transform.position.z > -3.5f && _cube.transform.position.z < 3.5f)
                    {
                        /*if (emergingFromX)
                        {
                            emergingFromX = false;
                            emergingFromZ = true;
                        }
                        else
                        {
                            emergingFromZ = false;
                            emergingFromX = true;
                        }*/
                        //0 = EmergingFromZ
                        //1 = EmergingFromX
                        if (switcher == 0)
                            switcher = 1;
                        else
                            switcher = 0;
                        chopOff(_cube);
                        _cube = Instantiate(cube, CubesParent); //use CreatePrimitive here instead
                        chopOff(_cube);
                        if (switcher == 0)
                            _cube.transform.position = new Vector3(0, 0, initialPos);
                        else
                            _cube.transform.position = new Vector3(-initialPos, 0, 0);

                    }
                }
            }
        }
    }



    public void chopOff(GameObject _cube)
    {
        //Coming from x direction => Chop x
        if (switcher == 0) {
            _cube.transform.localScale = new Vector3(originalX - 1f, _cube.transform.localScale.y, originalZ);
            originalX -= 1f;
        } else {
            _cube.transform.localScale = new Vector3(originalX, _cube.transform.localScale.y, originalZ - 1f);
            originalZ -= 1f;
        }
        //Add rigid body component and add useGravityOn


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
}
