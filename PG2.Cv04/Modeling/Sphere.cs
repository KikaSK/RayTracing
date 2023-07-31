using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PG2.Mathematics;
using PG2.Rendering;
using PG2.Shading;

namespace PG2.Modeling
{
    public class Sphere : Model
    {
        #region Properties

        // TODO: Define object properties: Origin, Radius
        Vector3 Origin = new Vector3();
        double Radius;


        #endregion


        #region Init

        public Sphere()
        {
        }

        public Sphere(Shader shader, Vector3 origin, Double radius)
        {
            // TODO: Initialize class members Shader (inherited from base Model object), Origin, Radius
            Origin = origin;
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
        public static void Collide(Ray ray, Sphere sphere)
        {
            // TODO: Compute ray-sphere intersection
            Vector3 v = sphere.Origin - ray.Origin;
            double t0 = ray.Direction.Normalized * v;
            double d_sq = v * v - t0 * t0;
            double t_d_sq = sphere.Radius * sphere.Radius - d_sq;
            if (t_d_sq < 0) return;
            else
            {
                double t;
                
                /*Int32 state;
                double n1n2 = Model.getRefractionIndexRatio(sphere, out state);
                if (state == Model.Exiting)
                    t = t0 - Math.Sqrt(t_d_sq);
                else
                    t = t0 + Math.Sqrt(t_d_sq);
                */
                //hitPoint.Normal = -hitPoint.Normal;

                // TODO ked ray vchadza do gule tak nastavit na + inak na -, zistit kedy ray vychadza a vchadza
                
                Vector3 new_origin = ray.Origin + Eps * ray.Direction.Normalized;
                Vector3 dist = sphere.Origin - new_origin;
                if(Math.Abs(dist.Length - sphere.Radius) < Eps)
                    t = t0 + Math.Sqrt(t_d_sq);
                else t = t0 - Math.Sqrt(t_d_sq);
                
                t = t / ray.Direction.Length;

                if (t > Eps && t - ray.HitParameter <= Eps)
                {
                    ray.HitParameter = t;
                    ray.HitModel = sphere;
                    Vector3 hitPoint = ray.Origin + t * ray.Direction;
                    ray.HitNormal = Vector3.Normalize(hitPoint - sphere.Origin);
                }
            }
        }

        #endregion
    }
}
