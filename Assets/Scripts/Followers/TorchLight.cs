using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LightFlickerEffect))]
[RequireComponent(typeof(SphereCollider))]
public class TorchLight : MonoBehaviour
{
    [Tooltip("Les zones de lumières associées à un état safe")]
    [SerializeField] LightArea[] initialLightAreas;
    LightFlickerEffect lightFlicker;

    [Header("Decrease Light")]
    //[SerializeField]
    //float decreaseInvincibleLightRadiusPerSeconds;
    //[SerializeField]
    //float decreaseProtectedLightRadiusPerSeconds;
    //[SerializeField]
    //float decreaseUnprotectedLightRadiusPerSeconds;
    [SerializeField]
    float decreaseLightRadiusPerSecond;

    Dictionary<SafeLevel, LightArea> lightAreasDico;
    Dictionary<SafeLevel, SphereCollider> lightSphereColliders;
    Dictionary<SafeLevel, Light> lights;

    SphereCollider col;
    [SerializeField]
    LayerMask lightRefillMask;

    bool refill;

    private void Start()
    {
        lightSphereColliders = new Dictionary<SafeLevel, SphereCollider>();
        lights = new Dictionary<SafeLevel, Light>();
        lightFlicker = GetComponent<LightFlickerEffect>();
        col = GetComponent<SphereCollider>();

        //For every light area needed
        foreach (LightArea area in initialLightAreas)
        {
            //Create a gameobject to contain each sphere collider
            GameObject areaObjectToSpawn = new GameObject("Light - " + area.safeLevel.ToString());
            areaObjectToSpawn.transform.SetParent(transform);
            areaObjectToSpawn.transform.localPosition = Vector3.zero;
            areaObjectToSpawn.layer = LayerMask.NameToLayer(area.safeLevel.ToString() + "Light");

            //Create the sphere collider and assign its radius
            SphereCollider sphereLightCol = areaObjectToSpawn.AddComponent<SphereCollider>();
            sphereLightCol.isTrigger = true;
            sphereLightCol.radius = area.radius;
            lightSphereColliders.Add(area.safeLevel, sphereLightCol);

            //Create a light and assign its range and color
            Light light = areaObjectToSpawn.AddComponent<Light>();
            light.type = LightType.Point;
            light.intensity = area.lightIntensity;
            light.range = area.radius;
            light.color = area.lightColor;
            lights.Add(area.safeLevel, light);

        }
    }

    private void Update()
    {
        if (!refill)
        {
            foreach (KeyValuePair<SafeLevel, Light> light in lights)
            {
                light.Value.range -= Time.deltaTime * decreaseLightRadiusPerSecond;
                light.Value.range = Mathf.Clamp(light.Value.range, lightAreasDico[light.Key].minRadius, float.PositiveInfinity);
            }
            foreach (KeyValuePair<SafeLevel, SphereCollider> lightSphereCol in lightSphereColliders)
            {
                lightSphereCol.Value.radius -= Time.deltaTime * decreaseLightRadiusPerSecond;
                lightSphereCol.Value.radius = Mathf.Clamp(lightSphereCol.Value.radius, lightAreasDico[lightSphereCol.Key].minRadius, float.PositiveInfinity);
            }
        }
        else
        {
            foreach (KeyValuePair<SafeLevel, Light> light in lights)
            {
                light.Value.range += Time.deltaTime * decreaseLightRadiusPerSecond;
                light.Value.range = Mathf.Clamp(light.Value.range, lightAreasDico[light.Key].maxRadius, float.PositiveInfinity);
            }
            foreach (KeyValuePair<SafeLevel, SphereCollider> lightSphereCol in lightSphereColliders)
            {
                lightSphereCol.Value.radius += Time.deltaTime * decreaseLightRadiusPerSecond;
                lightSphereCol.Value.radius = Mathf.Clamp(lightSphereCol.Value.radius, lightAreasDico[lightSphereCol.Key].maxRadius, float.PositiveInfinity);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "RefillArea")
        {
            refill = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Collider[] lightRefillCols = Physics.OverlapSphere(transform.position, col.radius, lightRefillMask);

        if (lightRefillCols.Length == 0)
        {
            return;
        }

        if (other.tag == "RefillArea")
        {
            refill = false;
        }
    }

    private void OnDrawGizmos()
    {
        foreach (LightArea lightArea in initialLightAreas)
        {
            Gizmos.color = new Color(0.0f, 0.25f, 0f, 0.25f);
            Gizmos.DrawSphere(transform.position, lightArea.radius);
        }
    }
}

[System.Serializable]
public class LightArea
{
    public float radius;
    public float minRadius;
    public float maxRadius;
    public SafeLevel safeLevel;
    public float lightIntensity;
    public Color lightColor;
}

public enum SafeLevel
{
    Invincible,
    Protected,
    Unprotected
}
