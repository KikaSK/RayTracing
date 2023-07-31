using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PG2.Mathematics;
using PG2.Rendering;
using PG2.Shading;

namespace PG2.Modeling
{
    public class Triangle : Model
    {
        #region Properties

        // TODO: Define object properties Vertex1, Vertex2, Vertex3
        Vector3 Vertex1 = new Vector3();
        Vector3 Vertex2 = new Vector3();
        Vector3 Vertex3 = new Vector3();

        #endregion


        #region Init

        public Triangle()
        {
        }

        public Triangle(Shader shader, Vector3 v1, Vector3 v2, Vector3 v3)
        {
            // TODO: Initialize class members Shader (inherited from base Model object), Vertex1, Vertex2, Vertex3
            Vertex1 = v1;
            Vertex2 = v2;
            Vertex3 = v3;
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
        public static void Collide(Ray ray, Triangle triangle)
        {
            // Möller–Trumbore intersection algorithm (http://en.wikipedia.org/wiki/Möller–Trumbore_intersection_algorithm)
            // TODO: Compute ray-triangle intersection 
            Vector3 normal = Vector3.Normalize((triangle.Vertex3 - triangle.Vertex1) % (triangle.Vertex2 - triangle.Vertex1));
            Vector3 origin = triangle.Vertex1;

            double t = -normal * (ray.Origin - origin) / (ray.Direction * normal);

            if (t - 1 < Eps || t - ray.HitParameter > Eps) return;

            Vector3 P = ray.Origin + t * ray.Direction;

            Vector3 A = triangle.Vertex1;
            Vector3 B = triangle.Vertex2;
            Vector3 C = triangle.Vertex3;

            Vector3 AB = B - A;
            Vector3 AC = C - A;
            Vector3 PA = A - P;
            Vector3 PB = B - P;
            Vector3 PC = C - P;

            double areaABC = normal * (AB % AC);
            double areaPBC = normal * (PB % PC);
            double areaPCA = normal * (PC % PA);

            double a = areaPBC / areaABC;
            double b = areaPCA / areaABC;

            if (a > Eps && a - 1 <= Eps && b > Eps && b - 1 <= Eps && a + b > Eps && a + b - 1 <= Eps)
            {
                ray.HitParameter = t;
                ray.HitModel = triangle;
                ray.HitNormal = Vector3.Normalize(AB % AC);
            }
            return;
        }

        #endregion
    }
}
