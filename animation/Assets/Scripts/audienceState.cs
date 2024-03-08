using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class audienceState : MonoBehaviour
{
    Animator anim;
    public Camera cam;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        bool isLooked = anim.GetBool("isLooked");

        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;
        bool isHit = Physics.Raycast(ray, out hit);
        
        if (!isLooked && isHit)
            anim.SetBool("isLooked", true);
        if (isLooked && !isHit)
            anim.SetBool("isLooked", false);
    }
}
