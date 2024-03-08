using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class begin : MonoBehaviour
{
    private VideoPlayer beginVideo;

    // Start is called before the first frame update
    void Start()
    {
        beginVideo = GameObject.Find("VideoPlayerBegin").GetComponent<VideoPlayer>();
    }

    // Update is called once per frame
    void Update()
    {
        if ((long)beginVideo.frame == (long)(beginVideo.frameCount - 5))
        {
            SceneManager.LoadScene(1);
        }
    }
}
