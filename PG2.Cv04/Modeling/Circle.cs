using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PG2.Mathematics;
using PG2.Rendering;
using PG2.Shading;

namespace PG2.Modeling
{
    public class Circle : Model
    {
        #region Properties

        // TODO: Define object properties: Origin, Normal, Radius
        Vector3 Origin = new Vector3();
        Vector3 Normal = new Vector3();
        double Radius;

        #endregion


        #region Init

        public Circle()
        {
        }

        public Circle(Shader shader, Vector3 origin, Vector3 normal, Double radius)
        {
            // TODO: Initialize class members Shader (inherited from base Model object), Origin, Normal, Radius;
            Origin = origin;
            Normal = normal;
            Radius = radius;
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
        public static void Collide(Ray ray, Circle circle)
        {
            // TODO: Compute ray-circle intersection
            double t = -circle.Normal * (ray.Origin - circle.Origin) / (ray.Direction * circle.Normal);
            if (t - 1 >= Eps && t - ray.HitParameter <= Eps)
            {
                Vector3 hitPoint = ray.Origin + t * ray.Direction;
                double dist = Vector3.GetLengthSquared(hitPoint - circle.Origin);
                if (dist < circle.Radius * circle.Radius)
                {
                    ray.HitParameter = t;
                    ray.HitModel = circle;
                    ray.HitNormal = circle.Normal;
                }
            }
        }

        #endregion
    }
}
