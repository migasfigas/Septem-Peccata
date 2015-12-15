using UnityEngine;
using System.Collections;

public class FlickeringLight : MonoBehaviour
{
    Light light;
    float rand;

    float timer = 0;

    // Use this for initialization
    void Start()
    {
        light = GetComponent<Light>();
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (timer > 0.05f)
        {
            rand = Random.value * 100;

            light.intensity = 1.5f * Mathf.PerlinNoise(rand + 2f + Time.time, rand + 1 + Time.time * 1);
            timer = 0;
        }
    }
}