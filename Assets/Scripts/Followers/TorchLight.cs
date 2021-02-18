using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorchLight : MonoBehaviour
{
    [Tooltip("Les zones de lumières associées à un état safe")]
    [SerializeField] LightArea[] lightAreas;

    private void Start()
    {
        //For every light area needed
        foreach (LightArea area in lightAreas)
        {
            //Create a gameobject to contain each sphere collider
            GameObject areaObjectToSpawn = new GameObject("Light - " + area.safeLevel.ToString());
            areaObjectToSpawn.transform.SetParent(transform);
            areaObjectToSpawn.transform.localPosition = Vector3.zero;
            areaObjectToSpawn.tag = area.safeLevel.ToString();

            //Create the sphere collider and assign its radius
            SphereCollider sphereLightCol = areaObjectToSpawn.AddComponent<SphereCollider>();
            sphereLightCol.isTrigger = true;
            sphereLightCol.radius = area.radius;
        }
    }

    private void OnDrawGizmos()
    {
        foreach (LightArea lightArea in lightAreas)
        {
            Gizmos.color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f), 0.25f);
            Gizmos.DrawSphere(transform.position, lightArea.radius);
        }
    }
}

[System.Serializable]
public class LightArea
{
    public float radius;
    public SafeLevel safeLevel;
}

public enum SafeLevel
{
    Invincible,
    Protected,
    Unprotected
}
