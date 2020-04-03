using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerlinNoise : MonoBehaviour
{

    [Header("3D Perlin Noise Standard")]
    [SerializeField] private float AB = 0f;
    [SerializeField] private float BC = 0f;
    [SerializeField] private float AC = 0f;

    [Header("3D Perlin Noise Reversed")]
    [SerializeField] private float BA = 0f;
    [SerializeField] private float CB = 0f;
    [SerializeField] private float CA = 0f;

    [Header("Modifier Settings")]
    [SerializeField] private float Scalar = 0.15f;
    [SerializeField] private float Amplitude = 0.6f;
    [SerializeField] private float worldSeed = 0f;


    public float CalculateDensity(Vector3 pos)
    {
        AB = Mathf.PerlinNoise((pos.x + (worldSeed * Amplitude)) * Scalar, ((pos.y + (worldSeed * Amplitude)) * Scalar));
        BC = Mathf.PerlinNoise((pos.y + (worldSeed * Amplitude)) * Scalar, ((pos.z + (worldSeed * Amplitude)) * Scalar));
        AC = Mathf.PerlinNoise((pos.x + (worldSeed * Amplitude)) * Scalar, ((pos.z + (worldSeed * Amplitude)) * Scalar));

        BA = Mathf.PerlinNoise((pos.y - (worldSeed / Amplitude)) / Scalar, ((pos.y - (worldSeed / Amplitude)) / Scalar));
        CB = Mathf.PerlinNoise((pos.z - (worldSeed / Amplitude)) / Scalar, ((pos.y - (worldSeed / Amplitude)) / Scalar));
        CA = Mathf.PerlinNoise((pos.z - (worldSeed / Amplitude)) / Scalar, ((pos.x - (worldSeed / Amplitude)) / Scalar));

        float ABC = AB + BC + AC + BA + CB + CA;
        return ABC / 6f;
    }

    public float CalculateHeightMap(Vector3 pos)
    {
        float Perlin = Mathf.PerlinNoise(pos.x / 10.1f, pos.z / 10.1f);
        Perlin *= 10;
        return Perlin;
    }

}
