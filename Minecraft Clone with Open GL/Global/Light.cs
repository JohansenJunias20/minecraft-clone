using System;
using System.Collections.Generic;
using System.Text;
using OpenTK.Graphics.OpenGL;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.Common;
using System.Drawing.Imaging;
using System.Drawing;
using OpenTK.Mathematics;
using LearnOpenTK.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;


namespace Minecraft_Clone_with_Open_GL.Global
{
    public class CDirectionLight // contain directionallight object with its shadow map shader
    {
        public DirectionalLight light;
        public LearnOpenTK.Common.Shader depthMapShader;
        public depthTexture depthMapTexture;
        public int frameBufferObject;
        public Vector3 ambient = new Vector3(0.5f);
        public Vector3 diffuse = new Vector3(0.5f);
        public Vector3 specular = new Vector3(1f);
        public Vector4 orthoSize
        {
            get => light.orthoSize;
            set
            {
                light.orthoSize = value;
            }
        }
        public float farValue
        {
            get => light.farVal;
            set
            {
                light.farVal = value;
            }
        }
        public float nearValue
        {
            get => light.nearVal;
            set
            {
                light.nearVal = value;
            }
        }
        public CDirectionLight(Vector4 orthoSize, DirectionalLight light, float near, float far, Vector3 diffuse, Vector3 ambient, Vector3 specular)
        {
            this.diffuse = diffuse;
            this.ambient = ambient;
            this.specular = specular;
            this.light = light;
            this.light.orthoSize = orthoSize;
            this.orthoSize = orthoSize;
            this.nearValue = near;
            this.farValue = far;

        }
        public void init(string depthShaderVertPath, string depthShaderFragPath)
        {
            this.frameBufferObject = GL.GenFramebuffer();
            depthMapTexture = new depthTexture();

            GL.BindFramebuffer(FramebufferTarget.Framebuffer, frameBufferObject);
            GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.DepthAttachment, TextureTarget.Texture2D, depthMapTexture.Handle, 0);
            GL.DrawBuffer(DrawBufferMode.None);
            GL.ReadBuffer(ReadBufferMode.None);
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
            depthMapShader = new LearnOpenTK.Common.Shader(depthShaderVertPath, depthShaderFragPath);
        }
        public void drawShadowToTexture(ref Dictionary<string, Mesh> Meshes)
        {
            GL.Viewport(0, 0, Global.SHADOW.WIDTH, Global.SHADOW.HEIGHT);
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, frameBufferObject);
            GL.Clear(ClearBufferMask.DepthBufferBit);
            foreach (var key in Meshes.Keys)
            {
                Meshes[key].render(Global.Camera.camera, "depthDirLight");
            }
            //plane.render(camera, true);
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
        }
        public void UNSAFE_endShadowToTexture()
        {
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
        }
        public void UNSAFE_startShadowToTexture()
        {
            GL.Viewport(0, 0, Global.SHADOW.WIDTH, Global.SHADOW.HEIGHT);
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, frameBufferObject);
        }
    }

    public class CPointLight // contain pointlight object with its shadow map shaderGeom and 6 Texture in 1 Handle.
    {
        public PointLight light;
        public LearnOpenTK.Common.ShaderGeom depthMapShader; // point light perlu shader geom
        public depthTexturePointLight depthMapTexture;
        public Mesh mesh;
        public int frameBufferObject;
        //public Vector3 ambient = new Vector3(0.5f);
        //public Vector3 diffuse = new Vector3(0.5f);
        //public Vector3 specular = new Vector3(1f);
        //public List<Matrix4> shadowTransform = new List<Matrix4>();

        public CPointLight(Vector3 position, float aspectRatio, float near, float far, Vector3 ambient, Vector3 diffuse, Vector3 specular
            , float constant, float linear, float quadric)
        {
            // 1 projection untuk 6 sisi shadow.
            // view nya saja yang berbeda-beda dari 6 sisi.
            this.light = new PointLight(position, aspectRatio, near, far, ambient, diffuse, specular, constant, linear, quadric);

            mesh = new Mesh(false, false);
            mesh.createBoxVertices(position, new Vector3(0.001f));
            mesh.setupObject();
        }
        public void init(string depthShaderVertPath, string depthShaderFragPath, string depthShaderGeomPath)
        {
            this.frameBufferObject = GL.GenFramebuffer();
            depthMapTexture = new depthTexturePointLight();

            GL.BindFramebuffer(FramebufferTarget.Framebuffer, frameBufferObject);
            GL.FramebufferTexture(FramebufferTarget.Framebuffer, FramebufferAttachment.DepthAttachment, depthMapTexture.Handle, 0);
            GL.DrawBuffer(DrawBufferMode.None);
            GL.ReadBuffer(ReadBufferMode.None);
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
            depthMapShader = new LearnOpenTK.Common.ShaderGeom(depthShaderVertPath, depthShaderFragPath, depthShaderGeomPath);
        }

        public void drawShadowToTexture(ref Dictionary<string, Mesh> Meshes, ref ShaderGeom shaderGeom,List<Matrix4> spaceMatrixs )
        {
            //GL.Viewport(0, 0, Global.SHADOW.WIDTH, Global.SHADOW.HEIGHT);
            //GL.BindFramebuffer(FramebufferTarget.Framebuffer, frameBufferObject);
            //GL.Clear(ClearBufferMask.DepthBufferBit);
            //depthMapShader.Use();
            //for (int i = 0; i < 6; i++)
            //{
            //    depthMapShader.SetMatrix4($"shadowMatrices{i.ToString()}", light.getSpaceMatrixs()[i]);
            //}
            //foreach (var key in Meshes.Keys)
            //{
            //    Meshes[key].renderDepth(Global.Camera.camera,ref shaderGeom, spaceMatrixs,i);
            //}
            ////plane.render(camera, true);
            //GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
        }
        public void UNSAFE_endShadowToTexture()
        {

            GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
        }
        public void UNSAFE_startShadowToTexture()
        {
            GL.Viewport(0, 0,1024, 1024);
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, frameBufferObject);
            GL.Clear(ClearBufferMask.DepthBufferBit);
        }
    }


    class Light
    {
        public static List<CDirectionLight> directionalLights = new List<CDirectionLight>();
        public static List<CPointLight> pointLights = new List<CPointLight>();
        public static LearnOpenTK.Common.Shader depthShader;
        public static LearnOpenTK.Common.Shader debugDepthShaderQuad;
        public static Matrix4 lightSpaceMatrix;
        public static depthTexture _depthTexture;

    }
}
