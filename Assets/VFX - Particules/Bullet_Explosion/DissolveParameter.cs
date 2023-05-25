using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class DissolveParameter : MonoBehaviour
{
    public float blendersValue;
    public float blendValue;
    Renderer renderer;

    void Start()
    {
        renderer = GetComponent<Renderer>();
        renderer.material.shader = Shader.Find("Explosion_Fire");

    }
    void Update()
    {
    }
}
