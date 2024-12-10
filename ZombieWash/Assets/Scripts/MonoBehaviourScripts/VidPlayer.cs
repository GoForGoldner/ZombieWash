using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class VidPlayer : MonoBehaviour {
    [SerializeField] private string _videoFileName;

    private VideoPlayer _videoPlayer;

    private void Awake() {
        _videoPlayer = GetComponent<VideoPlayer>();
    }

    private void OnEnable() {
        Debug.Log("VidPlayer activated.");
        PlayVideo();
    }

    public void PlayVideo() {
        string videoPath = System.IO.Path.Combine(Application.streamingAssetsPath, _videoFileName);
        _videoPlayer.url = videoPath;
        _videoPlayer.Play();
    }

    private void OnDisable() {
        _videoPlayer.Stop();
    }
}
