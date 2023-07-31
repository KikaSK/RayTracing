using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PG2.Mathematics;
using PG2.Rendering;
using PG2.Shading;

namespace PG2.Modeling
{
    public class Block : Model
    {
        #region Properties

        // TODO: Define AABB block object properties Min, Max (top-left and bottom-right)
        Vector3 Min = new Vector3();
        Vector3 Max = new Vector3();

        #endregion


        #region Init

        public Block()
        {
        }

        public Block(Shader shader, Vector3 min, Vector3 max)
        {
            // TODO: Initialize class members Shader (inherited from base Model object), Min, Max
            Shader = shader;
            Min = min;
            Max = max;
        }

        #endregion


        #region Raytracing

        public override void Collide(Ray ray)
        {
            Collide(ray, this);
        }
        private static Double CollidePlane(Plane plane, Ray ray)
        {
            Double t = -plane.Normal * (ray.Origin - plane.Origin) / (ray.Direction * plane.Normal);
            if (t - 1 >= Eps)
            {
                return t;
            }
            return Double.MaxValue;
        }

        // Collide ray with object and return:
        //   intersection ray.HitParameter, surface normal at intersection point ray.HitNormal and intersected object ray.HitModel
        public static void Collide(Ray ray, Block box)
        {
            // TODO: Compute ray-block intersection            
            //          z
            //         |
            //         |
            //         |________ y
            //        /       
            //       /
            //      x

            // Back 
            Plane Back = new Plane(box.Shader, box.Min, new Vector3(-1, 0, 0));
            Plane Front = new Plane(box.Shader, box.Max, new Vector3(1, 0, 0));
            Plane Left = new Plane(box.Shader, box.Min, new Vector3(0, -1, 0));
            Plane Right = new Plane(box.Shader, box.Max, new Vector3(0, 1, 0));
            Plane Bottom = new Plane(box.Shader, box.Min, new Vector3(0, 0, -1));
            Plane Top = new Plane(box.Shader, box.Max, new Vector3(0, 0, 1));

            Double t = Double.MaxValue;

            Vector3 hit;
            Vector3 normal = new Vector3();

            Double tBack = CollidePlane(Back, ray);
            hit = ray.Origin + tBack * ray.Direction;
            if (hit.Y >= box.Min.Y && hit.Z >= box.Min.Z && hit.Y <= box.Max.Y && hit.Z <= box.Max.Z)
            {
                if (tBack < t)
                {
                    t = tBack;
                    normal = new Vector3(-1, 0, 0);
                }
            }

            Double tFront = CollidePlane(Front, ray);
            hit = ray.Origin + tFront * ray.Direction;
            if (hit.Y >= box.Min.Y && hit.Z >= box.Min.Z && hit.Y <= box.Max.Y && hit.Z <= box.Max.Z)
            {
                if (tFront < t)
                {
                    t = tFront;
                    normal = new Vector3(1, 0, 0);
                }
            }

            Double tLeft = CollidePlane(Left, ray);
            hit = ray.Origin + tLeft * ray.Direction;
            if (hit.X >= box.Min.X && hit.Z >= box.Min.Z && hit.X <= box.Max.X && hit.Z <= box.Max.Z)
            {
                if (tLeft < t)
                {
                    t = tLeft;
                    normal = new Vector3(0, -1, 0);
                }
            }

            Double tRight = CollidePlane(Right, ray);
            hit = ray.Origin + tRight * ray.Direction;
            if (hit.X >= box.Min.X && hit.Z >= box.Min.Z && hit.X <= box.Max.X && hit.Z <= box.Max.Z)
            {
                if (tRight < t)
                {
                    t = tRight;
                    normal = new Vector3(0, 1, 0);
                }
            }

            Double tBottom = CollidePlane(Bottom, ray);
            hit = ray.Origin + tBottom * ray.Direction;
            if (hit.X >= box.Min.X && hit.Y >= box.Min.Y && hit.X <= box.Max.X && hit.Y <= box.Max.Y)
            {
                if (tBottom < t)
                {
                    t = tBottom;
                    normal = new Vector3(0, 0, -1);
                }
            }

            Double tTop = CollidePlane(Top, ray);
            hit = ray.Origin + tTop * ray.Direction;
            if (hit.X >= box.Min.X && hit.Y >= box.Min.Y && hit.X <= box.Max.X && hit.Y <= box.Max.Y)
            {
                if (tTop < t)
                {
                    t = tTop;
                    normal = new Vector3(0, 0, 1);
                }
            }

            if (t < Double.MaxValue && t - 1 >= Eps && t - ray.HitParameter <= Eps)
            {
                ray.HitParameter = t;
                ray.HitModel = box;
                ray.HitNormal = normal;
            }
        }

        #endregion
    }
}
