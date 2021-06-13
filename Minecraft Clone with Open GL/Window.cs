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
using System.Threading.Tasks;
using System.IO;
using PixelFormat = OpenTK.Graphics.OpenGL.PixelFormat;

namespace Minecraft_Clone_with_Open_GL
{

    class Window : GameWindow
    {
        //Mesh grass;
        //Mesh Steve;
        //Mesh ender;
        bool NIGHT_MODE = true;
        Queue<string> JoinPlayers = new Queue<string>();
        Me.Me Me;
        Dictionary<string, Steve.Steve> Players = new System.Collections.Generic.Dictionary<string, Steve.Steve>();
        Mesh plane;
        Socket socket = new Socket();
        public Window(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings) : base(gameWindowSettings, nativeWindowSettings)
        {
            socket.connect();
        }




        List<Players> players = new List<Players>();
        public void setPosition(Vector3 position, float yaw, string ID)
        {
            for (int i = 0; i < players.Count; i++)
            {
                if (players[i].network.ID == ID)
                {
                    players[i].position = position;
                    players[i].yawDirection = yaw;
                    break;
                }
            }

        }
        public void playerJoin(string ID)
        {
            Console.WriteLine("SOME ONE JOIN, GENERATING NEW STEVE!");
            JoinPlayers.Enqueue(ID);
            //Players.Add(ID,);
            //tes1.Add("hello", steve);
            //players.Add(temp);
        }
        Camera camera;
        Mesh ender;
        Mesh village;
        Mesh sapi;
        Mesh mom;
        Mesh zombie;
        Mesh city;
        List<Vector2> generateTextureCoordPlane()
        {
            List<Vector2> temp = new List<Vector2>();
            temp.Add(new Vector2(0f)); // persegi depan, kiri atas
            temp.Add(new Vector2(0f)); // persegi depan, kanan bawah
            temp.Add(new Vector2(0f)); // persegi depan, kiri bawah

            temp.Add(new Vector2(0f)); // persegi depan, kiri atas
            temp.Add(new Vector2(0f)); // persegi depan, kanan atas
            temp.Add(new Vector2(0f)); // persegi depan, kanan bawah

            temp.Add(new Vector2(0f)); // persegi kiri, kiri atas
            temp.Add(new Vector2(0f)); // persegi kiri, kanan atas
            temp.Add(new Vector2(0f)); // persegi kiri, kiri bawah

            temp.Add(new Vector2(0f)); // persegi kiri, kanan atas
            temp.Add(new Vector2(0f)); // persegi kiri, kanan bawah
            temp.Add(new Vector2(0f)); // persegi kiri, kiri bawah

            temp.Add(new Vector2(0f)); // persegi kanan, kiri atas
            temp.Add(new Vector2(0f)); // persegi kanan, kiri bawah
            temp.Add(new Vector2(0f)); // persegi kanan, kanan bawah

            temp.Add(new Vector2(0f)); // persegi kanan, kanan atas
            temp.Add(new Vector2(0f)); // persegi kanan, kiri atas
            temp.Add(new Vector2(0f)); // persegi kanan, kanan bawah

            temp.Add(new Vector2(0f)); // persegi belakang, kiri atas
            temp.Add(new Vector2(0f)); // persegi belakang, kanan atas
            temp.Add(new Vector2(0f)); // persegi belakang, kanan bawah

            temp.Add(new Vector2(0f)); // persegi belakang, kiri bawah
            temp.Add(new Vector2(0f)); // persegi belakang, kiri atas
            temp.Add(new Vector2(0f)); // persegi belakang, kanan bawah

            temp.Add(new Vector2(0f, 1f)); // persegi atas, kiri atas
            temp.Add(new Vector2(1f, 1f)); // persegi atas, kanan atas
            temp.Add(new Vector2(0f, 0f)); // persegi atas, kiri bawah

            temp.Add(new Vector2(0f, 0f)); // persegi atas, kiri bawah
            temp.Add(new Vector2(1f, 1f)); // persegi atas, kanan atas
            temp.Add(new Vector2(1f, 0f)); // persegi atas, kanan bawah

            temp.Add(new Vector2(0f)); // persegi bawah, kiri atas
            temp.Add(new Vector2(0f)); // persegi bawah, kanan atas
            temp.Add(new Vector2(0f)); // persegi bawah, kiri bawah


            temp.Add(new Vector2(0f)); // persegi bawah, kiri bawah
            temp.Add(new Vector2(0f)); // persegi bawah, kanan bawah
            temp.Add(new Vector2(0f)); // persegi bawah, kiri atas
            return temp;
        }
        protected override void OnLoad()
        {

            //ender = new Mesh(true, true);
            //ender.LoadObjFile(@"C:\Users\c1419\Downloads\Compressed\New folder (38)\source\ender_dragon.obj");
            //ender.position = new Vector3(0f);

            //ender.setupObject();
            //ender.setTexture(@"C:\Users\c1419\Downloads\Compressed\New folder (38)\textures\ender_dragon.png");

            city = new Mesh(true, true, shininess: 0f);
            city.LoadObjFile($@"{Program.__DIR1__}assets\environment\city.obj");
            city.setupObject();
            city.setTexture($@"{Program.__DIR1__}assets\environment\city.png");

            //city = new Mesh(true, true, shininess: 0f);
            //city.LoadObjFile($@"C:\Users\c1419\Downloads\New folder (63)\landscape.obj");
            //city.setupObject();
            //city.setTexture($@"C:\Users\c1419\Downloads\New folder (63)\ground_Base_Color.png");


            //sapi = new Mesh(true, true, shininess: 4f);
            //sapi.LoadObjFile(@"C:\Users\c1419\OneDrive\Documents\New folder (5)\sapi1.obj");
            //sapi.setupObject();
            //sapi.setTexture(@"C:\Users\c1419\OneDrive\Documents\New folder (5)\cow_texture.png");

            //zombie = new Mesh(true, true, shininess: 4f);
            //zombie.LoadObjFile($@"{Program.__DIR1__}assets\mobs\zombie.obj");
            ////zombie.transform = Matrix4.Identity  *  Matrix4.CreateTranslation();
            //zombie.setupObject();
            //zombie.setTexture($@"{Program.__DIR1__}assets\mobs\zombie1.png");

            //Meshes.Add("zombie", zombie);
            //Meshes.Add("sapi", sapi);
            ////Meshes.Add("ender", ender);
            Meshes.Add("city", city);

            #region generate 1 directional light
            Global.Light.directionalLights = new List<Global.CDirectionLight>()
            {
                new Global.CDirectionLight(
                    new Vector4(-2f,2f,-2f,2f),
                    new DirectionalLight(new Vector3(0f,2f,-0.6f)),
                    0.01f,  // near value
                    3f, // far value
                    diffuse:new Vector3(0.15f),
                    ambient:new Vector3(0.15f),
                    specular:new Vector3(0.15f)
                    )

            };
            Global.Light.directionalLights[0].light.Pitch = -60f;
            #endregion

            #region generate 1 point light
            Global.Light.pointLights = new List<Global.CPointLight>()
            {

                  new Global.CPointLight(
                    new Vector3(-0.054f, 0.323f, 0.446f),
                   Global.SHADOW.WIDTH/Global.SHADOW.HEIGHT, // aspectRatio
                    0.03f, // near
                    0.9f, // far
                    new Vector3(0.4f), // warna ambient
                    new Vector3(0.4f), // warna diffuse
                    new Vector3(0.4f), //warna specular
                    1.0f, //constant
                    	7f,// linear
                    9f //quadric
                )
                //,
                //    new Global.CPointLight(
                //    new Vector3(0.0f, 3.5f, 0.0f),
                //    Global.SHADOW.WIDTH/Global.SHADOW.HEIGHT, // aspectRatio
                //    0.1f, // near
                //    4f, // far
                //    new Vector3(0.9f), // warna ambient
                //    new Vector3(0.9f), // warna diffuse
                //    new Vector3(0.9f), //warna specular
                //    1.0f, //constant
                //    0.14f,// linear
                //    //0.07f,// linear
                //    0.07f //quadric
                //    //0.017f //quadric
                //)
            };



            #endregion

            #region init_shadow dummy 
            //_frameBufferObject = GL.GenFramebuffer();

            //depthMapTexture = GL.GenTexture();
            //GL.BindTexture(TextureTarget.TextureCubeMap, depthMapTexture);
            //for (int i = 0; i < 6; ++i)
            //    GL.TexImage2D(TextureTarget.TextureCubeMapPositiveX + i, 0, PixelInternalFormat.DepthComponent, 1024, 1024, 0, PixelFormat.DepthComponent, PixelType.Float, IntPtr.Zero);
            //GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureMinFilter, (float)TextureMinFilter.Nearest);
            //GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureMagFilter, (float)TextureMagFilter.Nearest);
            //GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureWrapS, (float)TextureWrapMode.ClampToEdge);
            //GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureWrapT, (float)TextureWrapMode.ClampToEdge);
            //GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureWrapR, (float)TextureWrapMode.ClampToEdge);
            //// attach depth texture as FBO's depth buffer
            //GL.BindFramebuffer(FramebufferTarget.Framebuffer, _frameBufferObject); //2
            //GL.FramebufferTexture(FramebufferTarget.Framebuffer, FramebufferAttachment.DepthAttachment, depthMapTexture, 0);
            //GL.DrawBuffer(DrawBufferMode.None);
            //GL.ReadBuffer(ReadBufferMode.None);
            //GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
            //depthPointLightShader = new ShaderGeom(@$"{ Program.__DIR__}\Shader\shader_depthmap\PointLight\shader.vert",
            //    @$"{Program.__DIR__}\Shader\shader_depthmap\PointLight\shader.frag",
            //    @$"{Program.__DIR__}\Shader\shader_depthmap\PointLight\shader.gs");
            #endregion

            #region init_shadow ASLI 
            for (int i = 0; i < Global.Light.directionalLights.Count; i++)
            {
                Global.Light.directionalLights[i].init(@$"{Program.__DIR__}\Shader\shader_depthmap\3.1.1.shadow_mapping_depth.vs",
                        @$"{Program.__DIR__}\Shader\shader_depthmap\3.1.1.shadow_mapping_depth.fs");
            }

            for (int i = 0; i < Global.Light.pointLights.Count; i++)
            {
                Global.Light.pointLights[i].init(@$"{Program.__DIR__}\Shader\shader_depthmap\PointLight\shader.vert",
                @$"{Program.__DIR__}\Shader\shader_depthmap\PointLight\shader.frag",
                @$"{Program.__DIR__}\Shader\shader_depthmap\PointLight\shader.gs");
            }
            #endregion


            Global.Camera.camera = new LearnOpenTK.Common.Camera(new Vector3(0f, 0f, 1.0f), Size.X / (float)Size.Y);

            GL.ClearColor(0.529f, 0.808f, 0.922f, 1.0f);
            Me = new Me.Me();
            plane = new Mesh(true, true);
            plane.createBoxVertices(new Vector3(0f), new Vector3(5f, 0f, 5f), generateTextureCoordPlane());
            plane.setupObject();
            plane.setTexture(@$"{ Program.__DIR__}\assets\environment\floor.jpg");


            //Meshes.Add("plane", plane);


            debugDepthShaderQuad = new Shader(@$"{Program.__DIR__}\Shader\shader_debug_depth_quad\3.1.1.debug_quad.vs",
                @$"{Program.__DIR__}\Shader\shader_debug_depth_quad\3.1.1.debug_quad_depth.fs");
            init_quad();


            CursorGrabbed = true;
            Task.Run(async () =>
            {
                for (; ; )
                {

                    await Task.Delay(5);
                    if (Me.isMove)
                        socket.sendPosition(camera.Position, camera.Yaw);
                    Me.isMove = false;
                }
            });

            //var temp = new Players();
            //temp.network.ID = "2";
            //players.Add(temp);





            base.OnLoad();
        }

        Shader debugDepthShaderQuad;
        public Dictionary<string, Mesh> Meshes = new Dictionary<string, Mesh>();
        void init_quad()
        {
            quadVertices = new float[]{
            // positions        // texture Coords
            -1.0f,  1.0f, 0.0f, 0.0f, 1.0f,
            -1.0f, -1.0f, 0.0f, 0.0f, 0.0f,
             1.0f,  1.0f, 0.0f, 1.0f, 1.0f,
             1.0f, -1.0f, 0.0f, 1.0f, 0.0f,
                };
            vaoQuad = GL.GenVertexArray();

            GL.BindVertexArray(vaoQuad);
            vboQuad = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, vboQuad);
            GL.BufferData(BufferTarget.ArrayBuffer, quadVertices.Length * sizeof(float), quadVertices, BufferUsageHint.StaticDraw);

            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 5 * sizeof(float), 0);
            int location = debugDepthShaderQuad.GetAttribLocation("aTexCoords");
            GL.EnableVertexAttribArray(location);
            GL.VertexAttribPointer(location, 2, VertexAttribPointerType.Float, false, 5 * sizeof(float), 3 * sizeof(float));
            GL.BindVertexArray(0);
        }


        int vaoQuad;
        int vboQuad;
        int counterDummy = 0;
        bool onceTime = false;
        float[] quadVertices;
        Vector3 skyColor = new Vector3(0f);
        Vector3 nightSkyColor = new Vector3(0.1f, 0.2f, 0.3f);
        Vector3 daySkyColor = new Vector3(153f / 255f, 204f / 255f, 255f / 255f);
        float dayTransitionSpeed = 0.0003f;
        protected override void OnRenderFrame(FrameEventArgs args)
        {


            GL.Enable(EnableCap.DepthTest);

            GL.ClearColor(0.6f, 0.2f, 0.5f, 1.0f);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            #region draw shadow from directional lights to texture

            for (int i = 0; i < Global.Light.directionalLights.Count; i++)
            {
                Global.Light.directionalLights[i].drawShadowToTexture(ref Meshes); //passing semua mesh agar didraw pada shadow map.
                Global.Light.directionalLights[i].UNSAFE_startShadowToTexture(); // inisialisasi fbo 
                for (int j = 0; j < players.Count; j++)
                {
                    players[i].render(camera, "depthDirLight");
                }
                //ender.render(camera, "depthDirLight");
                //village.render(camera, "depthDirLight");
                Global.Light.directionalLights[i].UNSAFE_endShadowToTexture(); // unbind fbo
            }
            #endregion
            #region draw shadow from point lights to texture ASLI
            if (!onceTime && counterDummy == 1)
            {

                for (int i = 0; i < Global.Light.pointLights.Count; i++)
                {
                    Console.WriteLine("masuk!");
                    onceTime = true;
                    //GL.Viewport(0, 0, 1024, 1024);
                    //GL.BindFramebuffer(FramebufferTarget.Framebuffer, _frameBufferObject);
                    //GL.Clear(ClearBufferMask.DepthBufferBit);

                    //UPDATE DULU LIGHTSPACE MATRIX

                    Global.Light.pointLights[i].UNSAFE_startShadowToTexture(); // bind fbo 
                                                                               //Global.Light.pointLights[i].drawShadowToTexture(ref Meshes, ref Global.Light.pointLights[i].depthMapShader, Global.Light.pointLights[i].light.getSpaceMatrixs());
                    foreach (var key in Meshes.Keys)
                    {
                        Meshes[key].renderDepth(camera, ref Global.Light.pointLights[i].depthMapShader,
                          Global.Light.pointLights[i].light.getSpaceMatrixs(), i);
                    }
                    //ender.renderDepth(camera, ref Global.Light.pointLights[i].depthMapShader,
                    //Global.Light.pointLights[i].light.getSpaceMatrixs(), i);

                    for (int j = 0; j < players.Count; j++)
                    {
                        players[j].renderDepth(camera, ref Global.Light.pointLights[i].depthMapShader,
                            Global.Light.pointLights[i].light.getSpaceMatrixs(), i);
                    }


                    GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
                    //Global.Light.pointLights[i].UNSAFE_endShadowToTexture(); // unbind fbo

                }
            }
            else
            {
                if (!onceTime)
                    counterDummy++;
            }
            //GL.Viewport(0, 0, Size.X, Size.Y);



            #endregion

            #region draw shadow from point lights to texture dummy
            //LightSpaceMatrix = new List<Matrix4>();
            //Matrix4 LightProjectionMatrix = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(90f), (float)Size.X / (float)Size.Y, near_val, far_val);
            //LightSpaceMatrix.Add(Matrix4.LookAt(LightPosition, LightPosition + new Vector3(1, 0, 0), new Vector3(0, -1, 0)) * LightProjectionMatrix);
            //LightSpaceMatrix.Add(Matrix4.LookAt(LightPosition, LightPosition + new Vector3(-1, 0, 0), new Vector3(0, -1, 0)) * LightProjectionMatrix);
            //LightSpaceMatrix.Add(Matrix4.LookAt(LightPosition, LightPosition + new Vector3(0, 1, 0), new Vector3(0, 0, 1)) * LightProjectionMatrix);
            //LightSpaceMatrix.Add(Matrix4.LookAt(LightPosition, LightPosition + new Vector3(0, -1, 0), new Vector3(0, 0, -1)) * LightProjectionMatrix);
            //LightSpaceMatrix.Add(Matrix4.LookAt(LightPosition, LightPosition + new Vector3(0, 0, 1), new Vector3(0, -1, 0)) * LightProjectionMatrix);
            //LightSpaceMatrix.Add(Matrix4.LookAt(LightPosition, LightPosition + new Vector3(0, 0, -1), new Vector3(0, -1, 0)) * LightProjectionMatrix);

            //#region rendering depth

            //GL.Viewport(0, 0, 1024, 1024);
            //GL.BindFramebuffer(FramebufferTarget.Framebuffer, _frameBufferObject); // 1
            //GL.Clear(ClearBufferMask.DepthBufferBit);
            //foreach (var item in Meshes.Keys)
            //{
            //    Meshes[item].renderDepth(Global.Camera.camera, ref depthPointLightShader, LightSpaceMatrix);
            //}

            //for (int i = 0; i < players.Count; i++)
            //{
            //    players[i].renderDepth(Global.Camera.camera, ref depthPointLightShader, LightSpaceMatrix);
            //}
            //GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0); // 1
            //#endregion

            //GL.Viewport(0, 0, Size.X, Size.Y);


            #endregion


            //GL.Viewport(0, 0, Global.SCREEN.WIDTH, Global.SCREEN.HEIGHT);
            #region transition day night logic
            if (NIGHT_MODE &&
                (
                !(skyColor.X >= nightSkyColor.X - 0.05f && skyColor.X <= nightSkyColor.X + 0.05f) ||
                !(skyColor.Y >= nightSkyColor.Y - 0.05f && skyColor.Y <= nightSkyColor.Y + 0.05f) ||
                !(skyColor.Z >= nightSkyColor.Z - 0.05f && skyColor.Z <= nightSkyColor.Z + 0.05f)
                )
               )
            {
                if (skyColor.X > nightSkyColor.X)
                {
                    skyColor.X -= dayTransitionSpeed;
                }
                else
                {

                    skyColor.X += dayTransitionSpeed;
                }

                if (skyColor.Y > nightSkyColor.Y)
                {
                    skyColor.Y -= dayTransitionSpeed;
                }
                else
                {

                    skyColor.Y += dayTransitionSpeed;
                }

                if (skyColor.Z > nightSkyColor.Z)
                {
                    skyColor.Z -= dayTransitionSpeed;
                }
                else
                {

                    skyColor.Z += dayTransitionSpeed;
                }

                GL.ClearColor(skyColor.X, skyColor.Y, skyColor.Z, 1.0f);


            }

            if (!NIGHT_MODE &&
                (
                !(skyColor.X >= daySkyColor.X - 0.05f && skyColor.X <= daySkyColor.X + 0.05f) ||
                !(skyColor.Y >= daySkyColor.Y - 0.05f && skyColor.Y <= daySkyColor.Y + 0.05f) ||
                !(skyColor.Z >= daySkyColor.Z - 0.05f && skyColor.Z <= daySkyColor.Z + 0.05f)
                )
               )
            {
                if (skyColor.X > daySkyColor.X)
                {
                    skyColor.X -= dayTransitionSpeed;
                }
                else
                {

                    skyColor.X += dayTransitionSpeed;
                }

                if (skyColor.Y > daySkyColor.Y)
                {
                    skyColor.Y -= dayTransitionSpeed;
                }
                else
                {

                    skyColor.Y += dayTransitionSpeed;
                }

                if (skyColor.Z > daySkyColor.Z)
                {
                    skyColor.Z -= dayTransitionSpeed;
                }
                else
                {

                    skyColor.Z += dayTransitionSpeed;
                }

                GL.ClearColor(skyColor.X, skyColor.Y, skyColor.Z, 1.0f);

            }
            GL.ClearColor(skyColor.X, skyColor.Y, skyColor.Z, 1.0f);
            #endregion
            //GL.ClearColor(0.3f, 0.5f, 0.9f, 1.0f);

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            #region activate depth textures ASLI
            int textureIndexDirection = 0;
            int textureIndexPoint = 0;
            for (int i = 0; i < Global.Light.directionalLights.Count + Global.Light.pointLights.Count; i++)
            {
                if (i >= Global.Light.directionalLights.Count)
                {
                    Global.Light.pointLights[textureIndexPoint].depthMapTexture.Use(Global.TextureUnit.unit[i + 1]); // walaupun directional light ada banyak, sementara cuma pakai texture shadow index 0
                    //GL.ActiveTexture(TextureUnit.Texture1);
                    //GL.BindTexture(TextureTarget.TextureCubeMap, depthMapTexture); 
                    textureIndexPoint++;
                }
                else
                {
                    Global.Light.directionalLights[textureIndexDirection].depthMapTexture.Use(Global.TextureUnit.unit[i + 1]); // walaupun directional light ada banyak, sementara cuma pakai texture shadow index 0
                    textureIndexDirection++;
                }

            }
            #endregion

            #region activate depth textures dummy
            //GL.ActiveTexture(TextureUnit.Texture1);
            //GL.BindTexture(TextureTarget.TextureCubeMap, depthMapTexture);
            #endregion
            foreach (var item in Meshes.Keys)
            {
                Meshes[item].render(Global.Camera.camera);
            }
            //ender.render(camera);
            //village.render(camera);

            #region activate depth textures ASLI
            textureIndexDirection = 0;
            textureIndexPoint = 0;
            for (int i = 0; i < Global.Light.directionalLights.Count + Global.Light.pointLights.Count; i++)
            {
                if (i >= Global.Light.directionalLights.Count)
                {
                    //Global.Light.pointLights[textureIndexPoint].depthMapTexture.Use(Global.TextureUnit.unit[i + 1]); // walaupun directional light ada banyak, sementara cuma pakai texture shadow index 0
                    Global.Light.pointLights[textureIndexPoint].depthMapTexture.Use(Global.TextureUnit.unit[i + 1]); // walaupun directional light ada banyak, sementara cuma pakai texture shadow index 0
                    //GL.ActiveTexture(TextureUnit.Texture1);
                    //GL.BindTexture(TextureTarget.TextureCubeMap, depthMapTexture);
                    textureIndexPoint++;
                }
                else
                {
                    Global.Light.directionalLights[textureIndexDirection].depthMapTexture.Use(Global.TextureUnit.unit[i + 1]); // walaupun directional light ada banyak, sementara cuma pakai texture shadow index 0
                    textureIndexDirection++;
                }

            }
            #endregion

            #region activate depth textures dummy
            //GL.ActiveTexture(TextureUnit.Texture1);
            //GL.BindTexture(TextureTarget.TextureCubeMap, depthMapTexture);
            #endregion
            //steve.render(camera);
            for (int i = 0; i < players.Count; i++)
            {
                players[i].render(Global.Camera.camera);
            }
            #region debugging shadow map directional light
            //GL.BindVertexArray(vaoQuad);
            //Global.Light.directionalLights[0].depthMapTexture.Use(OpenTK.Graphics.OpenGL4.TextureUnit.Texture0);
            //debugDepthShaderQuad.Use();
            //GL.DrawArrays(BeginMode.TriangleStrip, 0, 4);
            #endregion

            //render point light box.
            for (int i = 0; i < Global.Light.pointLights.Count; i++)
            {
                Global.Light.pointLights[i].mesh.render(camera);
            }
            SwapBuffers();
            base.OnRenderFrame(args);
            return;

        }
        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);
        }
        Vector3 dummyPos = new Vector3(0);
        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            //plane.update();
            //test.update();
            //steve.AnimateRun();

            //steve.moveTo(dummyPos, 0);
            //Global.Light.light.Pitch += 0.05f;
            //Global.Light.light.Yaw += 0.02f;
            //mendequeue antrian player yang join untuk dibuatkan mesh nya
            while (JoinPlayers.Count != 0)
            {
                var temp = new Players();
                string ID = JoinPlayers.Dequeue();
                temp.network.ID = ID;
                players.Add(temp);
            }
            //for (int i = 0; i < players.Count; i++)
            //{
            //    players[i].Update();
            //}
            //if (dummyCounter % 30000 == 0)
            //{
            //    socket.sendPosition(new Vector3(0,1,2));
            //    dummyCounter = 0;
            //}
            //steve.AnimateRun();
            const float cameraSpeed = 0.9f;
            camera = Global.Camera.camera;

            // Escape keyboard
            if (KeyboardState.IsKeyDown(Keys.Escape))
            {
                Close();
            }
            // Zoom in
            if (KeyboardState.IsKeyDown(Keys.I))
            {
                camera.Fov -= 0.05f;
            }
            // Zoom out
            if (KeyboardState.IsKeyDown(Keys.O))
            {
                camera.Fov += 0.05f;
            }

            // Rotasi X di pivot Camera
            // Lihat ke atas (T)
            if (KeyboardState.IsKeyDown(Keys.T))
            {
                camera.Pitch += 0.05f;
            }
            // Lihat ke bawah (G)
            if (KeyboardState.IsKeyDown(Keys.G))
            {
                camera.Pitch -= 0.05f;
            }
            // Rotasi Y di pivot Camera
            // Lihat ke kiri (F)
            if (KeyboardState.IsKeyDown(Keys.F))
            {
                camera.Yaw -= 0.05f;
                Me.isMove = true;

            }
            // Lihat ke kanan (H)
            if (KeyboardState.IsKeyDown(Keys.H))
            {
                camera.Yaw += 0.05f;
                Me.isMove = true;

            }

            // Maju (W)
            if (KeyboardState.IsKeyDown(Keys.W))
            {
                camera.Position += camera.Front * cameraSpeed * (float)args.Time;
                Me.isMove = true;
            }
            // Mundur (S)
            if (KeyboardState.IsKeyDown(Keys.S))
            {
                camera.Position -= camera.Front * cameraSpeed * (float)args.Time;
                Me.isMove = true;

            }
            // Kiri (A)
            if (KeyboardState.IsKeyDown(Keys.A))
            {
                Me.isMove = true;
                camera.Position -= camera.Right * cameraSpeed * (float)args.Time;
            }
            // Kanan (D)
            if (KeyboardState.IsKeyDown(Keys.D))
            {
                Me.isMove = true;
                camera.Position += camera.Right * cameraSpeed * (float)args.Time;
            }
            // Naik (Spasi)
            if (KeyboardState.IsKeyDown(Keys.Space))
            {
                Me.isMove = true;
                camera.Position += camera.Up * cameraSpeed * (float)args.Time;
                Console.WriteLine(camera.Position);
                //Global.Light.light.Position = new Vector3(Global.Light.light.Position.X, Global.Light.light.Position.Y + 0.001f, Global.Light.light.Position.Z);

            }
            // Turun (Ctrl)
            if (KeyboardState.IsKeyDown(Keys.LeftShift))
            {
                Me.isMove = true;
                camera.Position -= camera.Up * cameraSpeed * (float)args.Time;
                //Global.Light.light.Position = new Vector3(Global.Light.light.Position.X, Global.Light.light.Position.Y - 0.001f, Global.Light.light.Position.Z);
            }
            //Console.WriteLine($"yaw: {camera.Yaw}, pitch: {camera.Pitch}");
            //Me.Update(camera.Position, camera.Yaw, camera.Pitch);


            if (!IsFocused)
            {
                return;
            }

            if (KeyboardState.IsKeyDown(Keys.KeyPad7))
            {
                //Global.Light.directionalLights[0].light.Pitch += 0.05f;
                Global.Light.pointLights[0].mesh.transform = Matrix4.Identity;
                Global.Light.pointLights[0].light.Position = new Vector3(Global.Light.pointLights[0].light.Position.X, Global.Light.pointLights[0].light.Position.Y + 0.01f
                    ,
                     Global.Light.pointLights[0].light.Position.Z);
                Global.Light.pointLights[0].mesh.transform *= Matrix4.CreateTranslation(Global.Light.pointLights[0].light.Position);
            }
            if (KeyboardState.IsKeyDown(Keys.KeyPad9))
            {
                //Global.Light.directionalLights[0].light.Pitch += 0.05f;
                Global.Light.pointLights[0].mesh.transform = Matrix4.Identity;

                Global.Light.pointLights[0].light.Position = new Vector3(Global.Light.pointLights[0].light.Position.X, Global.Light.pointLights[0].light.Position.Y - 0.01f
                    ,
                     Global.Light.pointLights[0].light.Position.Z);
                Global.Light.pointLights[0].mesh.transform *= Matrix4.CreateTranslation(Global.Light.pointLights[0].light.Position);
            }
            // Lih

            // lighting rotation
            // Rotasi X di pivot Light
            // Lihat ke atas (T)
            if (KeyboardState.IsKeyDown(Keys.KeyPad8))
            {
                //Global.Light.directionalLights[0].light.Pitch += 0.05f;
                Global.Light.pointLights[0].mesh.transform = Matrix4.Identity;

                Global.Light.pointLights[0].light.Position = new Vector3(Global.Light.pointLights[0].light.Position.X, Global.Light.pointLights[0].light.Position.Y
                ,
                Global.Light.pointLights[0].light.Position.Z + 0.01f);
                //Global.Light.pointLights[0].mesh.transform *= Matrix4.CreateTranslation(Global.Light.pointLights[0].light.Position);
            }
            // Lihat ke bawah (G)
            if (KeyboardState.IsKeyDown(Keys.KeyPad5))
            {
                //Global.Light.directionalLights[0].light.Pitch -= 0.05f;
                Global.Light.pointLights[0].mesh.transform = Matrix4.Identity;

                Global.Light.pointLights[0].light.Position = new Vector3(Global.Light.pointLights[0].light.Position.X, Global.Light.pointLights[0].light.Position.Y
                ,
                Global.Light.pointLights[0].light.Position.Z - 0.01f);
                //Global.Light.pointLights[0].mesh.transform *= Matrix4.CreateTranslation(Global.Light.pointLights[0].light.Position);
            }
            // Rotasi Y di pivot Light
            // Lihat ke kiri (F)
            if (KeyboardState.IsKeyDown(Keys.KeyPad4))
            {
                //Global.Light.light.Yaw -= 0.05f;
                //Global.Light.light.Position = new Vector3(Global.Light.light.Position.X - 0.01f, Global.Light.light.Position.Y, Global.Light.light.Position.Z);
                Global.Light.pointLights[0].mesh.transform = Matrix4.Identity;
                Global.Light.pointLights[0].light.Position = new Vector3(Global.Light.pointLights[0].light.Position.X + 0.01f, Global.Light.pointLights[0].light.Position.Y
                ,
                 Global.Light.pointLights[0].light.Position.Z);
                Me.isMove = true;
                Global.Light.pointLights[0].mesh.transform *= Matrix4.CreateTranslation(Global.Light.pointLights[0].light.Position);

            }
            // Lihat ke kanan (H)
            if (KeyboardState.IsKeyDown(Keys.KeyPad6))
            {
                Global.Light.pointLights[0].mesh.transform = Matrix4.Identity;

                Global.Light.pointLights[0].light.Position = new Vector3(Global.Light.pointLights[0].light.Position.X - 0.01f, Global.Light.pointLights[0].light.Position.Y
                ,
                 Global.Light.pointLights[0].light.Position.Z);
                Me.isMove = true;
                Global.Light.pointLights[0].mesh.transform *= Matrix4.CreateTranslation(Global.Light.pointLights[0].light.Position);

            }

            const float sensitivity = 0.2f;
            if (_firstMove)
            {
                _lastMousePosition = new Vector2(MouseState.X, MouseState.Y);
                _firstMove = false;
            }
            else
            {
                // Hitung selisih mouse position
                var deltaX = MouseState.X - _lastMousePosition.X;
                var deltaY = MouseState.Y - _lastMousePosition.Y;
                _lastMousePosition = new Vector2(MouseState.X, MouseState.Y);

                Global.Camera.camera.Yaw += deltaX * sensitivity;
                Global.Camera.camera.Pitch -= deltaY * sensitivity;
                if (deltaX != 0 || deltaY != 0)
                    Me.isMove = true;
            }
            if (DateTimeOffset.Now.ToUnixTimeSeconds() - lastTime >= 1)
            {
                lastTime = DateTimeOffset.Now.ToUnixTimeSeconds();
                Console.WriteLine(fps);
                fps = 0;

            }
            fps++;
            //grass.transform *= Matrix4.CreateRotationY(MathHelper.DegreesToRadians(0.01f));
            //dummyCounter++;

            base.OnUpdateFrame(args);
        }
        int fps = 0;
        long lastTime = DateTimeOffset.Now.ToUnixTimeSeconds();
        protected override void OnKeyDown(KeyboardKeyEventArgs e)
        {
            //Console.WriteLine(e.Key.ToString());
            base.OnKeyDown(e);
        }

        protected override void OnKeyUp(KeyboardKeyEventArgs e)
        {
            if (e.Key == Keys.Z)
            {
                NIGHT_MODE = !NIGHT_MODE;
            }
            base.OnKeyUp(e);
        }

        private bool _firstMove;
        private Vector2 _lastMousePosition;

    }
}
