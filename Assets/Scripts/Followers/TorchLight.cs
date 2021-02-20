using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class TorchLight : MonoBehaviour
{
    [Tooltip("Les zones de lumières associées à un état safe")]
    [SerializeField]
    LightArea[] initialLightAreas;

    [Header("Light evolution")]
    //[SerializeField]
    //float decreaseInvincibleLightRadiusPerSeconds;
    //[SerializeField]
    //float decreaseProtectedLightRadiusPerSeconds;
    //[SerializeField]
    //float decreaseUnprotectedLightRadiusPerSeconds;
    [SerializeField]
    float decreaseLightRadiusPerSecond;
    [SerializeField]
    float increaseLightRadiusPerSecond;

    Dictionary<SafeLevel, LightArea> lightAreasDico;
    Dictionary<SafeLevel, SphereCollider> lightSphereColliders;
    Dictionary<SafeLevel, Light> lights;

    SphereCollider col;
    [SerializeField]
    LayerMask lightRefillMask;

    bool refill;

    private void Start()
    {
        lightAreasDico = new Dictionary<SafeLevel, LightArea>();

        lightSphereColliders = new Dictionary<SafeLevel, SphereCollider>();
        lights = new Dictionary<SafeLevel, Light>();
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

            lightAreasDico.Add(area.safeLevel, area);

            if (area.willFlicker)
            {
                LightFlickerEffect lightFlicker = areaObjectToSpawn.AddComponent<LightFlickerEffect>();
                lightFlicker.smoothing = area.flickSmoothness;
            }
        }
    }

    private void Update()
    {
        if (!refill)
        {
            foreach (KeyValuePair<SafeLevel, Light> light in lights)
            {
                light.Value.range -= Time.deltaTime * decreaseLightRadiusPerSecond;
                light.Value.range = Mathf.Clamp(light.Value.range, lightAreasDico[light.Key].minRadius, lightAreasDico[light.Key].maxRadius);
            }
            foreach (KeyValuePair<SafeLevel, SphereCollider> lightSphereCol in lightSphereColliders)
            {
                lightSphereCol.Value.radius -= Time.deltaTime * decreaseLightRadiusPerSecond;
                lightSphereCol.Value.radius = Mathf.Clamp(lightSphereCol.Value.radius, lightAreasDico[lightSphereCol.Key].minRadius, lightAreasDico[lightSphereCol.Key].maxRadius);
            }
        }
        else
        {
            foreach (KeyValuePair<SafeLevel, Light> light in lights)
            {
                light.Value.range += Time.deltaTime * increaseLightRadiusPerSecond;
                light.Value.range = Mathf.Clamp(light.Value.range, lightAreasDico[light.Key].minRadius, lightAreasDico[light.Key].maxRadius);
            }
            foreach (KeyValuePair<SafeLevel, SphereCollider> lightSphereCol in lightSphereColliders)
            {
                lightSphereCol.Value.radius += Time.deltaTime * increaseLightRadiusPerSecond;
                lightSphereCol.Value.radius = Mathf.Clamp(lightSphereCol.Value.radius, lightAreasDico[lightSphereCol.Key].minRadius, lightAreasDico[lightSphereCol.Key].maxRadius);
            }
        }
        foreach (KeyValuePair<SafeLevel, Light> light in lights)
        {
            if (lightAreasDico[light.Key].willFlicker)
            {
                light.Value.gameObject.GetComponent<LightFlickerEffect>().UpdateLightFlickerIntensity(
                    light.Value.intensity - lightAreasDico[light.Key].flickIntensityOffset,
                    light.Value.intensity + lightAreasDico[light.Key].flickIntensityOffset);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "RefillArea")
        {
            refill = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Collider[] lightRefillCols = Physics.OverlapSphere(transform.position, col.radius, lightRefillMask);

        if (lightRefillCols.Length > 0)
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
    public bool willFlicker;
    public int flickSmoothness;
    public float flickIntensityOffset;
}

public enum SafeLevel
{
    Invincible,
    Protected,
    Unprotected
}
