using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public GameObject targetObject;

    private float distanceToTarget;

    private AudioSource music;

    private void Start()
    {
        distanceToTarget = transform.position.x - targetObject.transform.position.x;
        LoadVolume();        
    }
    private void Update()
    {
        float targetObjectx = targetObject.transform.position.x;
        Vector3 newCameraPosition = transform.position;
        newCameraPosition.x = targetObjectx + distanceToTarget;
        transform.position = newCameraPosition;
    }

    private void LoadVolume()
    {
        music = GetComponent<AudioSource>();
        music.volume = PlayerPrefs.GetFloat("Volume");
    }
}
