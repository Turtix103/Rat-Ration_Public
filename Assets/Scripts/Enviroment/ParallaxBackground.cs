using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    private float length, height, startPosX, startPosY;
    public GameObject camera;
    public float parallaxEffectX, parallaxEffectY;

    void Start()
    {
        startPosX = transform.position.x;
        startPosY = transform.position.y;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
        height = GetComponent<SpriteRenderer>().bounds.size.y;
    }

    void Update()
    {
        float temp = (camera.transform.position.x * (1 - parallaxEffectX));
        float temp2 = (camera.transform.position.y * (1 - parallaxEffectY));
        float distX = (camera.transform.position.x * parallaxEffectX);
        float distY = (camera.transform.position.y * parallaxEffectY);

        transform.position = new Vector3(startPosX + distX, startPosY + distY, transform.position.z);

        if (temp > startPosX + length)
            startPosX += length;
        else if (temp < startPosX - length)
            startPosX -= length;

        if (temp2 > startPosY + height)
            startPosY += height;
        else if (temp2 < startPosY - height)
            startPosY -= height;
    }
}
