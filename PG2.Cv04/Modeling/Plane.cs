using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PG2.Mathematics;
using PG2.Rendering;
using PG2.Shading;

namespace PG2.Modeling
{
    public class Plane : Model
    {
        #region Properties

        // TODO: Define object properties: Origin, Normal
        public Vector3 Origin = new Vector3();
        public Vector3 Normal = new Vector3();

        #endregion


        #region Init

        public Plane()
        {
        }

        public Plane(Shader shader, Vector3 origin, Vector3 normal)
        {
            // TODO: Initialize class members Shader (inherited from base Model object), Origin, Normal;
            Origin = origin;
            Normal = normal;
            Shader = shader;
        }

        #endregion


        #region Raytracing

        public override void Collide(Ray ray)
        {
            Collide(ray, this);
        }

        // Collide ray with object and return:
        //   intersection ray.HitParameter, surface normal at intersection point ray.HitNormal and intersected object ray.HitModel
        public static void Collide(Ray ray, Plane plane)
        {
            // TODO: Compute ray-plane intersection
            double t = -plane.Normal * (ray.Origin - plane.Origin) / (ray.Direction * plane.Normal);
            if (t >= 5*Eps && t - ray.HitParameter <= 0)
            {
                ray.HitParameter = t;
                ray.HitModel = plane;
                ray.HitNormal = plane.Normal.Normalized;
            }
        }

        #endregion
    }
}
