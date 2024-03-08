using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class camera : MonoBehaviour
{
    private Transform cameraHead;
    // Start is called before the first frame update
    void Start()
    {
        cameraHead = transform.GetChild(2);
    }

    // Update is called once per frame
    void Update()
    {
        print(cameraHead.rotation * Vector3.forward);
    }
}
