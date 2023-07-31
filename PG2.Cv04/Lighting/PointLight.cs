using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PG2.Mathematics;
using PG2.Rendering;

namespace PG2.Lighting
{
    public class PointLight : Light
    {
        #region Properties

        // Declare light Linear attenuation factor coefficient to 0.02
        // Declare light Quadratic attenuation factor coefficient to 0.00
        public float LinearAttenuation = 0.02f;
        public float QuadraticAttenuation = 0.00f;

        #endregion


        #region Lighting

        public override Double GetAttenuationFactor(Vector3 point)
        {
            double r = (Origin - point).Length;
            return 1.0 / (1 + LinearAttenuation * r + QuadraticAttenuation * r * r);
            // TODO: Calculate light attenuation factor for point, use .Length method for the length of a vector
            //return 1.0; // Please remove me after code completion !
        }


        public override void SetLightRayAt(Vector3 point, Ray ray)
        {
            // TODO: Set normalized light vector from point to Origin of the light, use ray.Set() method
            Vector3 LightVector = (Origin - point).Normalized;
            ray.Set(point, LightVector);
        }

        #endregion
    }
}
