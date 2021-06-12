using OpenTK.Graphics.ES30;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;

namespace LearnOpenTK.Common
{
    public class PointLight
    {
        private float _fov = MathHelper.PiOver2;
        public float AspectRatio { private get; set; }

        public Matrix4 shadowProjection;
        public PointLight(Vector3 position, float aspectRatio, float near, float far, Vector3 ambient, Vector3 diffuse, Vector3 specular
           , float Constant, float Linear, float Quadric

            )
        {
            //position,aspectRatio,near,far,ambient,diffuse,specular

            Position = position;
            lightSpaceMatrixs.Clear();
            shadowProjection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(90.0f), aspectRatio, near, far);
            lightSpaceMatrixs.Add(Matrix4.LookAt(Position, Position + new Vector3(1.0f, 0.0f, 0.0f), new Vector3(0.0f, -1.0f, 0.0f)) * shadowProjection);// 2 perkalian terbalik
            lightSpaceMatrixs.Add(Matrix4.LookAt(Position, Position + new Vector3(-1.0f, 0.0f, 0.0f), new Vector3(0.0f, -1.0f, 0.0f)) * shadowProjection);
            lightSpaceMatrixs.Add(Matrix4.LookAt(Position, Position + new Vector3(0.0f, 1.0f, 0.0f), new Vector3(0.0f, 0.0f, 1.0f)) * shadowProjection);
            lightSpaceMatrixs.Add(Matrix4.LookAt(Position, Position + new Vector3(0.0f, -1.0f, 0.0f), new Vector3(0.0f, 0.0f, -1.0f)) * shadowProjection);
            lightSpaceMatrixs.Add(Matrix4.LookAt(Position, Position + new Vector3(0.0f, 0.0f, 1.0f), new Vector3(0.0f, -1.0f, 0.0f)) * shadowProjection);
            lightSpaceMatrixs.Add(Matrix4.LookAt(Position, Position + new Vector3(0.0f, 0.0f, -1.0f), new Vector3(0.0f, -1.0f, 0.0f)) * shadowProjection);

            this.AspectRatio = aspectRatio;
            this.farVal = far;
            this.nearVal = near;
            this.Ambient = ambient;
            this.Diffuse = diffuse;
            this.Specular = specular;

            this.constant = Constant;
            this.linear = Linear;
            this.quadric = Quadric;
        }

        // The position of the camera
        public Vector3 Position { get; set; }
        public Vector3 Ambient { get; set; }
        public Vector3 Diffuse { get; set; }
        public Vector3 Specular { get; set; }

        public float constant = 1.0f;
        public float linear = 0.09f;
        public float quadric = 0.032f;
        private int depthCubeMap;
        public float nearVal = 1.0f;
        public float farVal = 25.0f;
        private List<Matrix4> lightSpaceMatrixs = new List<Matrix4>(); // penglihatan camera dari 6 sisi.
        public List<Matrix4> getSpaceMatrixs()
        {
            lightSpaceMatrixs.Clear();
            shadowProjection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(90.0f), AspectRatio, nearVal, farVal);
            lightSpaceMatrixs.Add(Matrix4.LookAt(Position, Position + new Vector3(1.0f, 0.0f, 0.0f), new Vector3(0.0f, -1.0f, 0.0f)) * shadowProjection);// 2 perkalian terbalik
            lightSpaceMatrixs.Add(Matrix4.LookAt(Position, Position + new Vector3(-1.0f, 0.0f, 0.0f), new Vector3(0.0f, -1.0f, 0.0f)) * shadowProjection);
            lightSpaceMatrixs.Add(Matrix4.LookAt(Position, Position + new Vector3(0.0f, 1.0f, 0.0f), new Vector3(0.0f, 0.0f, 1.0f)) * shadowProjection);
            lightSpaceMatrixs.Add(Matrix4.LookAt(Position, Position + new Vector3(0.0f, -1.0f, 0.0f), new Vector3(0.0f, 0.0f, -1.0f)) * shadowProjection);
            lightSpaceMatrixs.Add(Matrix4.LookAt(Position, Position + new Vector3(0.0f, 0.0f, 1.0f), new Vector3(0.0f, -1.0f, 0.0f)) * shadowProjection);
            lightSpaceMatrixs.Add(Matrix4.LookAt(Position, Position + new Vector3(0.0f, 0.0f, -1.0f), new Vector3(0.0f, -1.0f, 0.0f)) * shadowProjection);

            return lightSpaceMatrixs;
        }
        //public Matrix4 GetProjectionMatrix()
        //{
        //    //return Matrix4.CreatePerspectiveFieldOfView(_fov, AspectRatio, 0.01f, 100f);
        //}

    }
}
