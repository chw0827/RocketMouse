using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxScroll : MonoBehaviour
{
    public Renderer backGround;
    public Renderer foreGround;
    public float backgroundSpeed;
    public float foregroundSpeed;
    public float offset;

    private void Update()
    {
                                 
        float backgroundOffset = offset * backgroundSpeed;
        float foregroundOffset = offset * foregroundSpeed;

        backGround.material.mainTextureOffset = new Vector2(backgroundOffset, 0);
        foreGround.material.mainTextureOffset = new Vector2(foregroundOffset, 0);
    }
}
