using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaytraceRenderer : MonoBehaviour
{
    [SerializeField] Camera camera;
    [SerializeField] bool realtime;
    [SerializeField] float renderResolution = 1;

    private Texture2D renderTexture;
    private Light[] lights;

    //Collision Mask (in case we forgot mesh colliders on any objects)
    //private LayerMask collisionMask = 1 << 31;

    //Create render texture with screen size
    void Awake()
    {
        renderTexture = new Texture2D(Mathf.FloorToInt(Screen.width*renderResolution), Mathf.FloorToInt(Screen.height * renderResolution));
    }

    //Do one raytrace when we start playing
    void Start()
    {
        if (!realtime)
        {
            Raytrace();
        }
    }

    private void Update()
    {
        if (realtime)
        {
            Raytrace();
        }
    }

    //Draw the render
    private void OnGUI()
    {
        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), renderTexture);
        GUILayout.Label("fps: " + Mathf.Round(1 / Time.smoothDeltaTime));
    }

    //The function that renders the entire scene to a texture
    private void Raytrace() {

        // Gather light objects
        lights = FindObjectsOfType(typeof(Light)) as Light[];

        for (int x = 0; x < renderTexture.width; x++) {
            for (int y = 0; y < renderTexture.height; y++) {

                //Now that we have an x/y value for each pixel, we need to make that into a 3d ray
                //according to the camera we are attached to
                Ray ray = camera.ScreenPointToRay(new Vector3(x, y, 0));

                //Now lets call a function with this ray and apply it's return value to the pixel we are on
                //We will define this function afterwards
                renderTexture.SetPixel(x, y, TraceRay(ray));
            }
        }

        renderTexture.Apply();
    }

    //Trace a Ray for a singple point
    private Color TraceRay(Ray ray)
    {
        Color returnColor = Color.black;
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            Material material;

            //Set the used material
            material = hit.collider.GetComponent<Renderer>().material;
            //hit.collider.renderer.material;

            //if the material has a texture
            if (material.mainTexture)
            {
                //return the color of the pixel at the pixel coordinate of the hit
                returnColor += (material.mainTexture as Texture2D).GetPixelBilinear(hit.textureCoord.x, hit.textureCoord.y);
            }
            else
            {
                //return the material color
                returnColor += material.color;
            }

            returnColor *= TraceLight(hit.point + hit.normal*0.0001f, hit.normal);
        }

        return returnColor;
    }

    private Color TraceLight(Vector3 pos, Vector3 normal)
    {
        Color returnColor = RenderSettings.ambientLight;

        foreach (Light light in lights)
        {
            if (light.enabled)
            {
                returnColor += LightTrace(light, pos, normal);
            }
        }
        return returnColor;
    }

    private Color LightTrace(Light light, Vector3 pos, Vector3 normal)
    {
        // Directional light
        if (light.type == LightType.Directional)
        {
            float dot = Vector3.Dot(-light.transform.forward, normal);

            if (dot > 0)
            {
                // If its normal doesn't face the light, draw shadow (black)
                if (Physics.Raycast(pos, -light.transform.forward))
                {
                    return Color.black;
                }

                return light.color * light.intensity;
            }
        }
        else
        {
            Vector3 direction = (light.transform.position - pos).normalized;

            float dot = Vector3.Dot(normal, direction);
            float distance = Vector3.Distance(pos, light.transform.position);

            if (distance < light.range && dot > 0)
            {
                // Point light
                if (light.type == LightType.Point)
                {
                    if (Physics.Raycast(pos, direction, distance))
                    {
                        return Color.black;
                    }

                    return light.color * light.intensity * dot * (1 - light.range / distance);

                }
                // Spot light
                else if (light.type == LightType.Spot)
                {
                    //Get the dot product between the backwards direction of the light and our direction to it
                    float dot2 = Vector3.Dot(-light.transform.forward, normal);

                    //Only do lighting if we're within the spot light range
                    if (dot2 < (1 - light.spotAngle / 180))
                    {
                        if (Physics.Raycast(pos, direction, distance))
                        {
                            return Color.black;
                        }

                        return light.color * light.intensity * dot * (1 - light.range / distance) * ((dot2 / (1 - light.spotAngle / 180)));
                    }
                }
            }
        }
        return Color.clear;

    }

}
