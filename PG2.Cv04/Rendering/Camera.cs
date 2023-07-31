using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PG2.Mathematics;
using System.Drawing;
using PG2.Modeling;
using PG2.Shading;
using PG2.Lighting;

namespace PG2.Rendering
{
    public class Camera
    {
        public struct HitPoint
        {
            // TODO: Define HitPoint structure variables: Position, Color, Normal
            public Vector3 Position;
            public Vector3 Color;
            public Vector3 Normal;

            public HitPoint(Vector3 position, Vector3 color, Vector3 normal)
            {
                this.Position = position;
                this.Color = color;
                this.Normal = normal;
            }
        }

        #region Properties

        // TODO: Define camera properties Position, Target
        // TODO: Declare Up vector to (0, 0, 1), FovY value to 45

        // TODO: Define U, V, W vectors camera to world space

        // TODO: Define frame(picture) properties Bitmap, Width, Height, Pixels buffer
        // TODO: Declare BgColor to (0, 0, 0)

        public Vector3 Position;
        public Vector3 Target;
        public Vector3 Up = new Vector3(0, 0, 1);
        public double FovY = 45.0;
        public Vector3 W, U, V;
        public int Width, Height;
        public Bitmap Bitmap;
        public Vector3[] Pixels;
        public Vector3 BgColor = new Vector3(0, 0, 0);

        // Define models in World
        public World World;

        // TODO: Define clipping planes zNear, zFar
        public Double zNear, zFar;

        // TODO: Declare UseShadows to control shadows rendering to true
        // TODO: Declare UseAttenuation to decrease light intensity by attenuation to true
        public bool UseShadows = true;
        public bool UseLightAttenuation = true;

        // TODO: Define MaxReflectionLevel, MaxRefractionLevel limit =
        // the maximal number of reflection and refraction iterations
        public int MaxReflectionLevel = 2;
        public int MaxRefractionLevel = 2;

        // TODO: Declare MinIntensity being the minimal intensity to 0.01.
        // Rays with intensity less than min intensity are not traced
        public Double MinIntensity = 0.01;

        #endregion

        #region Init

        public Camera(Int32 width, Int32 height)
        {
            // TODO: Initialize class members Width, Height, Bitmap, Pixels buffer
            Width = width;
            Height = height;
            Bitmap = new Bitmap(Width, Height);
            Pixels = new Vector3[Width * Height];
        }

        #endregion

        #region Buffer Acess

        public Vector3 GetPixel(Int32 i, Int32 j)
        {
            //return Vector3.Zero; // Please remove me after code completion !
            //Console.WriteLine(Pixels[i + j * Width]);
            return Pixels[i + j * Width];
        }

        public void SetPixel(Int32 i, Int32 j, Vector3 color)
        {
            Pixels[i + j * Width] = color;
        }

        #endregion

        #region Rendering

        public void Render()
        {
            RayTrace();
            //Console.WriteLine(GetPixel())
            PresentFrame();
        }

        /// <summary>Derived from Computer Graphics - David Mount.
        /// Implementations can differ - make your own from scratch. 
        /// See http://goo.gl/q6Sz0 (page 84) and http://goo.gl/rB8J6 (page 9-10)
        /// </summary>
        public void RayTrace()
        {
            // TODO: Initialize camera (U, V, W)
            W = (Position - Target).Normalized;
            U = (Up % W).Normalized;
            V = W % U;
            // TODO: Compute perspective projection with FovY as a field of view
            double h = 2 * Math.Tan(Math.PI * FovY / 360);
            double w = h * Width / Height;

            // Ray trace the scene. One ray is enough for one pixel
            for (int r = 0; r < Height; r++)
            {
                for (int c = 0; c < Width; c++)
                {
                    Model.initializeTracedModelsStack();
                    double ur = h * r / Height - h / 2;
                    double uc = w * c / Width - w / 2;

                    //Vector3 P = Position + uc * U + ur * V - W;

                    Vector3 direction = uc * U + ur * V - W;// (P - Position);

                    // TODO: Create ray and calculate color with RayTrace()
                    //       Store color to Pixels buffer with SetPixel()

                    Ray ray = new Ray(Position, direction, 0, 0);
                    
                    if(r == 125 && c == 320)
                    {
                        ;
                    }
                    Console.Write(r + " " + c + ": ");
                    Vector3 Color = RayTrace(ray, 1.0);

                    
                    SetPixel(r, c, Color);
                }
            }
        }

        // Ray trace the generated ray, compute the lighting, shadows, reflections and refractions
        // "ray" parameter is traced ray
        // "intensity" parameter is the light intensity after the iterations
        public Vector3 RayTrace(Ray ray, Double intensity)
        {
            // Exit if actual light intensity is less than MinIntensity
            if (intensity < MinIntensity) return Vector3.Zero;

            // TODO: Calculate ray intersection with all models (primitives) in World, use World.Collide()
            //       Return background color if intersection does not exists else calculate hitpoint color
            
            World.Collide(ray);

            HitPoint hitPoint = new HitPoint();
            hitPoint.Color = BgColor;

            // TODO: Calculate hitPoint.Position, use ray.GetHitPoint()
            if (ray.HitModel == null)
            {
                return BgColor;
            }

            // TODO: Set hitPoint.Color to ambient color for ray.HitModel object
            hitPoint.Color = ray.HitModel.Shader.GetAmbientColor(hitPoint.Position);
            //hitPoint.Position = ray.Origin + ray.HitParameter * ray.Direction;
            hitPoint.Position = ray.GetHitPoint();
            hitPoint.Normal = ray.HitNormal;

            // TODO: Create light ray
            // TODO: Create and set view direction

            // For each light in the world do
            foreach (Light light in World.Lights)
            {
                double attenuation = UseLightAttenuation ? light.GetAttenuationFactor(hitPoint.Position) : 1.0;

                // TODO: Setup light ray to the current light, use light.SetLightRayAt()
                Vector3 lighDir = (light.Origin - hitPoint.Position).Normalized;

                Vector3 color = ray.HitModel.Shader.GetAmbientColor(hitPoint.Position) + ray.HitModel.Shader.GetColor(hitPoint.Position, hitPoint.Normal.Normalized, ray.Direction.Normalized, lighDir.Normalized, attenuation, light);

                Ray lightRay = new Ray();
                light.SetLightRayAt(hitPoint.Position, lightRay);

                // TODO: Collide light ray with scene to check for shadows, use World.Collide()
                double ambientIntensity = 1.0;
                if (UseShadows && hitPoint.Normal * lighDir > 0)
                {
                    World.Collide(lightRay);
                    HitPoint hit = new HitPoint();
                    hit.Position = new Vector3(0, 0, 0);

                    // TODO: Check if the nearest occlusion object is between light and hit point
                    if (lightRay.HitModel != null)
                    {
                        hit.Position = lightRay.GetHitPoint();
                        if ((hit.Position - hitPoint.Position).Length - (hitPoint.Position - light.Origin).Length <= 10e-5 && (hit.Position - hitPoint.Position).Length > 10e-5)
                        {
                            ambientIntensity = ray.HitModel.Shader.GetAmbientColor(hitPoint.Position).Length*1.65;
                        }
                    }
                }
                hitPoint.Color += ambientIntensity * color;

                // Set n1n2 to the default refraction index ratio n1 / n2 = 1.0 / 1.0       
                Double n1n2 = 1.0;

                // TODO: Do you have the guts to uncomment me? Go on, try
                Boolean TotalReflection = false;
                if (Model.is3DTransparentObject(ray.HitModel))
                {
                    Int32 state;
                    n1n2 = Model.getRefractionIndexRatio(ray.HitModel, out state);
                    if (state == Model.Exiting)
                        hitPoint.Normal = -hitPoint.Normal;

                    if (ray.HitModel.Shader.RefractionFactor > 0 && ray.RefractionLevel < MaxRefractionLevel)
                    {
                        TotalReflection = Vector3.Refract(hitPoint.Normal, -ray.Direction.Normalized, n1n2, out Vector3 refractionDir);
                        if (!TotalReflection)
                        {                            
                            Ray refractionRay = new Ray(hitPoint.Position, refractionDir.Normalized, ray.ReflectionLevel, ray.RefractionLevel + 1);
                            Vector3 refractedColor = RayTrace(refractionRay, ray.HitModel.Shader.RefractionFactor * intensity);
                            hitPoint.Color += refractedColor;
                        }
                        // If ReflectionLevel of ray is less than MaxReflectionLevel trace recursively reflected ray
                        //    If total internal reflection happened during refraction you need call reflection too
                        //    You need to construct the reflection ray, use Vector3.Reflect() function
                        //    After the ray is created - we can raytrace this ray and add the result to hitPoint.Color
                        // TODO: Calculate reflection color
                    }
                }
                
                if((ray.HitModel.Shader.ReflectionFactor > 0.0 || TotalReflection) && ray.ReflectionLevel < MaxReflectionLevel)
                {
                    Vector3.Reflect(hitPoint.Normal, -ray.Direction, out Vector3 reflectionDir);
                    Ray reflectionRay = new Ray(hitPoint.Position, reflectionDir.Normalized, ray.ReflectionLevel + 1, ray.RefractionLevel);
                    double c = ray.HitModel.Shader.ReflectionFactor;//refracted ? 0.3 : 0.3;
                    Vector3 reflectedColor = RayTrace(reflectionRay, c * intensity);
                    hitPoint.Color += reflectedColor;
                }
                
                // If RefractionLevel of ray is less than MaxRefractionLevel trace recursively refracted ray
                //    You need to construct the refraction ray, use Vector3.Refract() function
                //    After the ray is created - we can raytrace this ray and add the result to hitPoint.Color
                // TODO: Calculate refraction color

                
                // TODO: Calculate attenuation factor
                // TODO: Evaluate local shading (e.g. phong-model) and accumulate color, use ray.HitModel.Shader.GetColor()
                //       Don't forget set color to shadow color if hit point is inside a shadow
            }
            
            return intensity * hitPoint.Color;
        }

        // Create picture. Copy all the pixels from pixel buffer to the Bitmap
        // Color is clamped in post process
        public void PresentFrame()
        {
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    // DONE
                    // TODO: Retrieve color from Pixels buffer, use GetPixel()
                    //       Don't forget clamp color to max 1.0
                    //       Store pixel color to the Bitmap, use appropriate procedure
                    Vector3 color = Vector3.Clamp(GetPixel(x, y), 0.0, 1.0);
                    Bitmap.SetPixel(y, Width - 1 - x, Color.FromArgb(255, (int)Math.Round(color.X * 255), (int)Math.Round(color.Y * 255), (int)Math.Round(color.Z * 255)));
                }
            }
        }

        #endregion
    }
}
