using UnityEngine;

namespace RobotFactory
{
    /// Automatic sun intensity script.
    /// Based on https://unity3d.com/learn/tutorials/modules/intermediate/graphics/realtime-gi-day-night-cycle
    public class SunAutoIntensity : MonoBehaviour
    {
        public Gradient nightDayColor;

        public float maxIntensity = 3f;
        public float minIntensity = 0f;
        public float minPoint = -0.2f;

        public float maxAmbient = 1f;
        public float minAmbient = 0f;
        public float minAmbientPoint = -0.2f;

        public Vector3 dayRotateSpeed;
        public Vector3 nightRotateSpeed;

        float skySpeed = 1;

        Light mainLight;
        Skybox sky;

        private void Start()
        {
            mainLight = GetComponent<Light>();
        }

        private void Update()
        {
            float tRange = 1 - minPoint;
            float dot = Mathf.Clamp01((Vector3.Dot(mainLight.transform.forward, Vector3.down) - minPoint)/tRange);
            float i = ((maxIntensity - minIntensity)*dot) + minIntensity;

            mainLight.intensity = i;

            tRange = 1 - minAmbientPoint;
            dot = Mathf.Clamp01((Vector3.Dot(mainLight.transform.forward, Vector3.down) - minAmbientPoint)/tRange);
            i = ((maxAmbient - minAmbient)*dot) + minAmbient;
            RenderSettings.ambientIntensity = i;

            mainLight.color = nightDayColor.Evaluate(dot);
            RenderSettings.ambientLight = mainLight.color;

            if (dot > 0)
            {
                transform.Rotate(dayRotateSpeed*Time.deltaTime*skySpeed);
            }
            else
            {
                transform.Rotate(nightRotateSpeed*Time.deltaTime*skySpeed);
            }
        }
    }
}