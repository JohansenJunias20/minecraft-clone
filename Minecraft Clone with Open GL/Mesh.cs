using LearnOpenTK.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using OpenTK.Graphics.OpenGL4;
using System.Globalization;

namespace Minecraft_Clone_with_Open_GL
{
    //parentnya
    public class Mesh
    {

        //Vector 3 pastikan menggunakan OpenTK.Mathematics
        //tanpa protected otomatis komputer menganggap sebagai private
        List<float> vertices = new List<float>();
        List<Vector2> rawTextureVertices = new List<Vector2>(); // texture coordinate dari load from obj
        List<Vector3> normals = new List<Vector3>();// titik normals (bisa dari obj ataupun dari create box vertices)
        List<Vector3> rawPositonVertices = new List<Vector3>();// position coordinate dari load from obj
        List<uint> vertexIndices = new List<uint>();
        int _vertexBufferObject;
        int _elementBufferObject;
        int _vertexArrayObject;
        Shader _shader;
        Texture texture;
        public Matrix4 transform = Matrix4.Identity;
        public bool useIBO = false; // jarang dipakai si, selalu false karena load from obj pun tidak bisa pake ibo karena ada texture dan normal
        public Vector3 size;
        Vector3 color = new Vector3(0, 0, 0);
        struct Vertex
        {
            public Vector3 position;
            public Vector2 texture;
            public Vector3 normal;
        }
        List<float> Vertices = new List<float>(); // hasil akhir vertices dari load from obj
        //menambahkan hirarki kedalam parent
        public List<Mesh> child = new List<Mesh>();
        private bool _texture, _lighting; // memberi tau apakah mesh ini dipakai dengan texture lighting atau tidak
        private bool loadFromOBJ = false;
        private bool stickWithCamera = false;
        public Vector3 rotationByGlobalAxis = new Vector3(0f);
        public Vector3 rotationByLocalAxis = new Vector3(0f);
        public Vector3 scale = new Vector3(1f);
        public float shininess = 32f;
        public Mesh(bool texture = false, bool lighting = false, bool stickWithCamera = false, float shininess=32f)
        {
            this.shininess = shininess;
            _texture = texture;
            _lighting = lighting;
            this.stickWithCamera = stickWithCamera;
        }
        public void setupObject()
        {
            //_shader = new Shader(@$"{Program.__DIR__}\Shader\shader_normal\shader.vert", @$"{Program.__DIR__}\Shader\shader_normal\shader.frag");

            //inisialisasi Transformasi
            //transform = Matrix4.Identity;
            float maxZ, minZ, maxX, minX;
            maxZ = float.MinValue;
            minZ = float.MaxValue;
            maxX = float.MinValue;
            minX = float.MaxValue;
            if (_texture && loadFromOBJ)
            {
                for (int i = 0; i < vertexIndices.Count; i++)
                {
                    Vertex temp = new Vertex();
                    int indexVertexPos = (int)vertexIndices[i];
                    temp.position = rawPositonVertices[indexVertexPos];
                    //pencarian titik tengah dari sebuah obj file
                    if (maxZ <= temp.position.Z)
                        maxZ = temp.position.Z;
                    if (maxX <= temp.position.X)
                        maxX = temp.position.X;
                    if (minZ >= temp.position.Z)
                        minZ = temp.position.Z;
                    if (minX >= temp.position.X)
                        minX = temp.position.X;

                    if (_texture)
                    {
                        int indexTexture = (int)textureIndices[i];
                        temp.texture = rawTextureVertices[indexTexture];

                    }
                    if (_lighting)
                    {
                        int indexNormal = (int)normalIndices[i];
                        temp.normal = normals[indexNormal];
                    }
                    if (_texture && _lighting)
                    {
                        float[] result = { temp.position.X, temp.position.Y, temp.position.Z, temp.texture.X, temp.texture.Y, temp.normal.X, temp.normal.Y, temp.normal.Z };
                        Vertices.AddRange(result);

                    }
                    else if (_texture)
                    {
                        float[] result = { temp.position.X, temp.position.Y, temp.position.Z, temp.texture.X, temp.texture.Y };
                        Vertices.AddRange(result);

                    }
                    else if (_lighting)
                    {
                        float[] result = { temp.position.X, temp.position.Y, temp.position.Z, temp.normal.X, temp.normal.Y, temp.normal.Z };
                        Vertices.AddRange(result);

                    }
                    else
                    {
                        float[] result = { temp.position.X, temp.position.Y, temp.position.Z };
                        Vertices.AddRange(result);

                    }
                    //float[] result = { temp.position.X,temp.position.Y, temp.position.Z };
                }
                position = new Vector3((minX + maxX) / 2f, 0f, (minZ + maxZ) / 2f);
                Console.WriteLine(position);

            }
            else if (_texture)
            {

            }

            //float[] tmp = { -0.5f
            _vertexArrayObject = GL.GenVertexArray();
            GL.BindVertexArray(_vertexArrayObject);

            Console.WriteLine($"Vertices count: {vertices.Count}");
            //inisialisasi buffer
            _vertexBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);
            if (_texture && loadFromOBJ)
                GL.BufferData(BufferTarget.ArrayBuffer,
                           Vertices.Count * sizeof(float),
                           Vertices.ToArray(),
                           BufferUsageHint.StaticDraw);
            else if (_texture)
                GL.BufferData(BufferTarget.ArrayBuffer,
                           vertices.ToArray().Length * sizeof(float),
                           vertices.ToArray(),
                           BufferUsageHint.StaticDraw);
            else
                GL.BufferData(BufferTarget.ArrayBuffer,
                            vertices.ToArray().Length * sizeof(float),
                            vertices.ToArray(),
                            BufferUsageHint.StaticDraw);
            Console.WriteLine($"vertices count: {vertices.ToArray().Length}");
            //Console.WriteLine($"verticesIndices count: {vertexIndices.Count}");

            //Console.WriteLine($"normal count: {normals.Count}");
            //Console.WriteLine($"normalIndices count: {normalIndices.Count}");

            //Console.WriteLine($"texture count: {rawTextureVertices.Count}");
            //Console.WriteLine($"textureIndices count: {textureIndices.Count}");
            //for (int i = 0; i < vertices.Count; i++)
            //{
            //    Console.WriteLine(vertices.ToArray()[i].X);
            //}
            //inisialisasi array
            init_shader();


            init_LayoutPosition();

            if (_lighting)
                init_LayoutNormal();


            if (useIBO)
                init_IBO();


            ////inisialisasi index vertex
            //_elementBufferObject = GL.GenBuffer();
            //GL.BindBuffer(BufferTarget.ElementArrayBuffer, _elementBufferObject);
            ////parameter 2 dan 3 perlu dirubah
            //GL.BufferData(BufferTarget.ElementArrayBuffer,
            //    vertexIndices.Count * sizeof(uint),
            //    vertexIndices.ToArray(), BufferUsageHint.StaticDraw);
            //Console.WriteLine($"indices count: {vertexIndices.Count}");

            //inisialisasi shader





            #region unused
            //for (int i = 0; i < textureIndices.Count; i++)
            //{
            //    textureVertices.Add(rawTextureVertices[(int)textureIndices[i]]);
            //}
            //Texture coordinate
            //_textureBufferObject = GL.GenBuffer();
            //GL.BindBuffer(BufferTarget.TextureBuffer, _textureBufferObject);
            //GL.BufferData(BufferTarget.TextureBuffer, textureVertices.ToArray().Length * sizeof(float), textureVertices.ToArray(), BufferUsageHint.StaticDraw);
            //GL.BufferData<Vector2>(BufferTarget.TextureBuffer,
            //       textureVertices.Count * Vector2.SizeInBytes,
            //       textureVertices.ToArray(),
            //       BufferUsageHint.StaticDraw);
            #endregion

            transform *= Matrix4.CreateScale(scale);
            transform *= Matrix4.CreateTranslation(position);
            //var texCoordLocation = _shader.GetAttribLocation("aTexCoord");
            //GL.EnableVertexAttribArray(texCoordLocation);
            //GL.VertexAttribPointer(texCoordLocation, 2, VertexAttribPointerType.Float, false, 5 * sizeof(float), 3 * sizeof(float));
            //texture = Texture.LoadFromFile(@"C:\Users\c1419\Downloads\Compressed\New folder (34)\textures\Grass_Block_TEX.png");
            //texture.Use(TextureUnit.Texture0);

        }
      
        public void unbindVAO()
        {
            GL.BindVertexArray(0);
        }
        private void init_IBO()
        {
            //inisialisasi index vertex
            _elementBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, _elementBufferObject);
            //parameter 2 dan 3 perlu dirubah
            GL.BufferData(BufferTarget.ElementArrayBuffer,
                vertexIndices.Count * sizeof(uint),
                vertexIndices.ToArray(), BufferUsageHint.StaticDraw);
            Console.WriteLine($"indices count: {vertexIndices.Count}");
        }
        private void init_shader()
        {
            if (!stickWithCamera)
            {

                if (_texture && _lighting)
                {
                    // 3 position + 2 texture + 3 normals
                    _shader = new Shader(@$"{Program.__DIR__}\Shader\shader_with_lighting_texture\shader.vert", @$"{Program.__DIR__}\Shader\shader_with_lighting_texture\lighting.frag");
                    _shader.SetMatrix4("transform", transform);
                    _shader.Use();

                }
                else if (_texture)
                {
                    // 3 position + 2 texture
                    _shader = new Shader(@$"{Program.__DIR__}\Shader\shader_with_texture\shader.vert", @$"{Program.__DIR__}\Shader\shader_with_texture\shader.frag");
                    _shader.SetMatrix4("transform", transform);
                    _shader.Use();
                }
                else if (_lighting)
                {
                    _shader = new Shader(@$"{Program.__DIR__}\Shader\shader_with_lighting\shader.vert", @$"{Program.__DIR__}\Shader\shader_with_lighting\lighting.frag");
                    _shader.SetMatrix4("transform", transform);
                    _shader.Use();
                }
                else
                {
                    _shader = new Shader(@$"{Program.__DIR__}\Shader\shader_normal\shader.vert", @$"{Program.__DIR__}\Shader\shader_normal\shader.frag");

                    _shader.Use();
                    Console.WriteLine("doesnt use lighting and texture");
                }
            }
            else
            {

                if (_texture && _lighting)
                {
                    // 3 position + 2 texture + 3 normals
                    _shader = new Shader(@$"{Program.__DIR__}\Shader\shader_with_lighting_texture\shader.vert", @$"{Program.__DIR__}\Shader\shader_with_lighting_texture\lighting.frag");
                    _shader.SetMatrix4("transform", transform);
                    _shader.Use();

                }
                else if (_texture)
                {
                    // 3 position + 2 texture
                    _shader = new Shader(@$"{Program.__DIR__}\Shader\shader_with_texture\shader.vert", @$"{Program.__DIR__}\Shader\shader_with_texture\shader.frag");
                    _shader.SetMatrix4("transform", transform);
                    Console.WriteLine("stick with camera and textured");
                    _shader.Use();
                }
                else if (_lighting)
                {
                    _shader = new Shader(@$"{Program.__DIR__}\Shader\shader_with_lighting\shader.vert", @$"{Program.__DIR__}\Shader\shader_with_lighting\lighting.frag");
                    _shader.SetMatrix4("transform", transform);
                    _shader.Use();
                }
                else
                {
                    _shader = new Shader(@$"{Program.__DIR__}\Shader\shader_normal\shader.vert", @$"{Program.__DIR__}\Shader\shader_normal\shader.frag");

                    _shader.Use();
                    Console.WriteLine("doesnt use lighting and texture");

                }
            }


        }
        private void init_LayoutPosition()
        {

            GL.EnableVertexAttribArray(0);
            if (_texture && _lighting)
            {
                // 3 position + 2 texture + 3 normals
                GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 8 * sizeof(float), 0);

            }
            else if (_texture)
            {
                // 3 position + 2 texture
                GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 5 * sizeof(float), 0);
            }
            else if (_lighting)
            {
                GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 5 * sizeof(float), 0);

            }
            else
            {
                GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);

            }
        }
        private void init_LayoutNormal()
        {

            GL.EnableVertexAttribArray(1);
            GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 8 * sizeof(float), 5 * sizeof(float));
        }
        public void init_LayoutTexture()
        {
            var texCoordLocation = _shader.GetAttribLocation("aTexCoord");

            GL.EnableVertexAttribArray(texCoordLocation);

            //GL.BindVertexArray(_vertexArrayObject);
            //dia tetap posisi ke 3 jadi tidak perlu di if

            if (_texture && _lighting)
            {
                // 3 position + 2 texture + 3 normals
                GL.VertexAttribPointer(texCoordLocation, 2, VertexAttribPointerType.Float, false, 8 * sizeof(float), 3 * sizeof(float));

            }
            else if (_texture)
            {
                // 3 position + 2 texture
                GL.VertexAttribPointer(texCoordLocation, 2, VertexAttribPointerType.Float, false, 5 * sizeof(float), 3 * sizeof(float));
            }



        }
        //<summary>
        //hanya boleh dipanggil bila texture set to true
        //</summary>
        public void setTexture(string path)

        {
            init_LayoutTexture();
            texture = Texture.LoadFromFile(path);
            texture.Use(TextureUnit.Texture0);
        }
        //<summary>
        //hanya boleh dipanggil bila texture set to false
        //</summary>
        public void setColor(Vector3 color)
        {
            this.color = color;
        }
        public void render(Camera _camera, string type = null)
        {
            GL.BindVertexArray(_vertexArrayObject);

            if (type == null)
            {
                if (_texture)
                    texture.Use(TextureUnit.Texture0);
                int indexTexture = 1;
                if (!_texture) // karena tidak pakai texture
                    _shader.SetVector3("color", new Vector3(1));
                transform = Matrix4.Identity * Matrix4.CreateTranslation(position);
                _shader.SetMatrix4("transform", transform);
                if (!stickWithCamera)
                {
                    if (_lighting)
                    {

                        //Global.Light.directionalLights[0].depthMapTexture.Use(TextureUnit.Texture1);
                        _shader.SetInt("dirLightCount", Global.Light.directionalLights.Count);
                        _shader.SetInt("pointLightCount", Global.Light.pointLights.Count);

                        _shader.SetMatrix4("projection", _camera.GetProjectionMatrix());
                        _shader.SetMatrix4("view", _camera.GetViewMatrix());
                        //_shader.SetInt($"depthMap", 1);

                        for (int i = 0; i < Global.Light.directionalLights.Count; i++)
                        {
                            _shader.SetMatrix4($"dirLight[{i}].lightSpaceMatrix", Global.Light.directionalLights[i].light.getSpaceMatrix()); //arah light.
                            _shader.SetInt($"dirLight[{i}].shadowMap", indexTexture);
                            indexTexture += 1;

                        }
                        //_shader.SetInt($"depthMapPointLight[0]", 1);
                        for (int i = 0; i < Global.Light.pointLights.Count; i++)
                        {
                            _shader.SetInt($"depthMapPointLight{i}", indexTexture);
                            //_shader.SetMatrix4($"dirLight[{i}].lightSpaceMatrix", Global.Light.directionalLights[i].light.getSpaceMatrix());
                            indexTexture += 1;

                        }
                    }


                }
                else
                {
                    //_shader.SetMatrix4("projection", _camera.GetProjectionMatrix()); // TOLONG SETELAH DEPTH DEBUG SELESAI DI UNCOMMENT

                }
                //texture harus di aktifkan terlebih dahulu sebelum mengaktifkan shader.
                //karena saat mengaktifkan shader, shader sudah bisa mengakses texture by index dari sampler2d.


                if (_lighting)
                {
                    _shader.SetInt("material.ambient", 0); //memberi tahu open gl bahwa texture(0,texCoord) pada ambient -> menggunakan texture0
                    _shader.SetInt("material.diffuse", 0);//memberi tahu open gl bahwa texture(0,texCoord) pada diffuse -> menggunakan texture0
                    _shader.SetInt("material.specular", 0);//memberi tahu open gl bahwa texture(0,texCoord) pada specular -> menggunakan texture0

                    _shader.SetFloat("material.shininess", shininess);//tingkat mengkilat sebuah objek, (akan dikalkulasikan dengan specular)

                    //memasukking semua direction light ke fragment.
                    for (int i = 0; i < Global.Light.directionalLights.Count; i++)
                    {
                        _shader.SetVector3($"dirLight[{i}].ambient", Global.Light.directionalLights[i].ambient); //warna ambient dari l ight
                        _shader.SetVector3($"dirLight[{i}].diffuse", Global.Light.directionalLights[i].diffuse); //warna diffuse dari light
                        _shader.SetVector3($"dirLight[{i}].specular", Global.Light.directionalLights[i].specular); //warna specular dari light
                        _shader.SetVector3($"dirLight[{i}].direction", Global.Light.directionalLights[i].light.GetDirection()); //arah light.


                    }
                    //_shader.SetInt("pointLightCount", 0);
                    for (int i = 0; i < Global.Light.pointLights.Count; i++)
                    {
                        //sebelumnya harus dihitung jarak antara point light dengan posisi frag position, bila jauh maka tidak perlu
                        //untuk mengetahui jauh atau tidak bisa menggunakan rumus:
                        _shader.SetVector3($"pointLight[{i}].position", Global.Light.pointLights[i].light.Position); //warna ambient dari l ight
                        //_shader.SetVector3($"pointLight[{i}].position", new Vector3(0.0f, 3.5f, 0.0f)); //warna ambient dari l ight
                        //_shader.SetFloat($"pointLight[{i}].constant", Global.Light.pointLights[i].light.constant); //warna specular dari light
                        _shader.SetFloat($"pointLight[{i}].linear", Global.Light.pointLights[i].light.linear); //warna specular dari light
                        _shader.SetFloat($"pointLight[{i}].quadric", Global.Light.pointLights[i].light.quadric); //warna specular dari light
                        _shader.SetVector3($"pointLight[{i}].ambient", Global.Light.pointLights[i].light.Ambient); //warna ambient dari l ight
                        _shader.SetVector3($"pointLight[{i}].specular", Global.Light.pointLights[i].light.Specular); //warna specular dari light
                        _shader.SetVector3($"pointLight[{i}].diffuse", Global.Light.pointLights[i].light.Diffuse); //warna diffuse dari light
                        //_shader.SetFloat($"pointLight[{i}].MaxDistance", Global.Light.pointLights[i].light.farVal);
                        //_shader.SetFloat($"far_plane", 8f);
                        _shader.SetFloat($"far_plane", Global.Light.pointLights[i].light.farVal);
                        //_shader.set($"pointLight[{i}].shadows", Global.Light.pointLights[i].light.GetDirection()); //arah light.
                        //_shader.SetMatrix4($"pointLight[{i}].lightSpaceMatrix", Global.Light.pointLights[i].light.getSpaceMatrixs()); //arah light.

                    }


                    //untuk menentukan posisi specular, kita harus tau posisi mata(camera) kita apakah tegak lurus terhadap light source.
                    //sehingga kita perlu mepassingkan posisi kamera kedalam fragment shader untuk dikalkulasikan posisi specularnya.
                    _shader.SetVector3("viewPos", _camera.Position);
                }
                //using depth shadow texture
                //Global.Light._depthTexture.Use(TextureUnit.Texture1);
                //_shader.SetInt("shadowMap", 1);

                //else
                //    _shader.SetVector3("color", color);
                _shader.Use();
            }

            else if (type == "depthDirLight")
            {
                for (int i = 0; i < Global.Light.directionalLights.Count; i++)
                {
                    Global.Light.directionalLights[i].depthMapShader.Use();
                    Global.Light.directionalLights[i].depthMapShader.SetMatrix4("lightSpaceMatrix", Global.Light.directionalLights[i].light.getSpaceMatrix());
                    Global.Light.directionalLights[i].depthMapShader.SetMatrix4("model", transform);

                }


            }
            else if (type == "depthPointLight")
            {

                //Global.Light.pointLights[0].depthMapShader.Use();
                for (int j = 0; j < 6; j++)
                {
                    Global.Light.pointLights[0].depthMapShader.SetMatrix4($"shadowMatrices{j.ToString()}", Global.Light.pointLights[0].light.getSpaceMatrixs()[j]);
                }

                Global.Light.pointLights[0].depthMapShader.SetFloat("far_plane", 8f);
                Global.Light.pointLights[0].depthMapShader.SetMatrix4("model", Matrix4.Identity);
                Global.Light.pointLights[0].depthMapShader.SetVector3("lightPos", new Vector3(0.0f, 3.0f, 0.0f));
                //Global.Light.pointLights[0].depthMapShader.SetFloat("far_plane", Global.Light.pointLights[0].light.farVal);
                //Global.Light.pointLights[0].depthMapShader.SetVector3("lightPos", Global.Light.pointLights[0].light.Position);
                //Global.Light.pointLights[0].depthMapShader.SetMatrix4("model", transform);
            }
            //perlu diganti di parameter 2
            //_shader.SetMatrix4("transform", _transform);
            if (useIBO)
                GL.DrawElements(PrimitiveType.Triangles,
                vertexIndices.Count,
                DrawElementsType.UnsignedInt, 0);
            else if (_texture && loadFromOBJ)
                GL.DrawArrays(PrimitiveType.Triangles, 0, Vertices.Count / 8);
            else if (_texture)
                GL.DrawArrays(PrimitiveType.Triangles, 0, _texture && _lighting ? vertices.Count / 8 : _texture ? vertices.Count / 5 : vertices.Count / 3);
            else
            {

            }
            //GL.DrawElements(PrimitiveType.Triangles,
            //    vertexIndices.Count,
            //    DrawElementsType.UnsignedInt, 0);
            ////ada disini
            //foreach (var meshobj in child)
            //{
            //    meshobj.render(_camera, _color);
            //}
        }

        public void renderDepth(Camera _camera, ref ShaderGeom shaderDepth, List<Matrix4> spaceMatrixs, int indexLight)
        {
            GL.BindVertexArray(_vertexArrayObject); // 3

            for (int j = 0; j < 6; j++)
            {
                shaderDepth.SetMatrix4($"shadowMatrices{j.ToString()}", spaceMatrixs[j]);
            }
            shaderDepth.SetFloat("far_plane", Global.Light.pointLights[indexLight].light.farVal);
            //shaderDepth.SetFloat("far_plane", Global.Light.pointLights[0].light.farVal);
            shaderDepth.SetMatrix4("model", transform);
            //shaderDepth.SetVector3("lightPos", Global.Light.pointLights[0].light.Position);
            shaderDepth.SetVector3("lightPos", Global.Light.pointLights[indexLight].light.Position);
            shaderDepth.Use();

            if (useIBO)
                GL.DrawElements(PrimitiveType.Triangles,
                vertexIndices.Count,
                DrawElementsType.UnsignedInt, 0);
            else if (_texture && loadFromOBJ)
                GL.DrawArrays(PrimitiveType.Triangles, 0, Vertices.Count / 8);
            else if (_texture)
                GL.DrawArrays(PrimitiveType.Triangles, 0, _texture && _lighting ? vertices.Count / 8 : _texture ? vertices.Count / 5 : vertices.Count / 3);
            else
            {

            }

            GL.BindVertexArray(0); // 3

        }
        public void update()
        {
            transform = Matrix4.Identity;
            transform *= Matrix4.CreateRotationX(MathHelper.DegreesToRadians(rotationByLocalAxis.X));
            transform *= Matrix4.CreateRotationY(MathHelper.DegreesToRadians(rotationByLocalAxis.Y));
            transform *= Matrix4.CreateRotationZ(MathHelper.DegreesToRadians(rotationByLocalAxis.Z));
            transform *= Matrix4.CreateTranslation(position);
            transform *= Matrix4.CreateRotationX(MathHelper.DegreesToRadians(rotationByGlobalAxis.X));
            transform *= Matrix4.CreateRotationY(MathHelper.DegreesToRadians(rotationByGlobalAxis.Y));
            transform *= Matrix4.CreateRotationZ(MathHelper.DegreesToRadians(rotationByGlobalAxis.Z));
        }
        public void renderWithoutTexture()
        {
            //Global.Light._depthTexture.Use(TextureUnit.Texture0);
            //texture.Use(TextureUnit.Texture0);
            Global.Shader.shaderWithoutTexture.Use();
            Global.Shader.shaderWithoutTexture.SetMatrix4("transform", Matrix4.Identity);
            Global.Shader.shaderWithoutTexture.SetVector3("color", new Vector3(0.3f, 0.2f, 0.4f));
            //Global.Light.debugDepthShader.SetMatrix4("transform", Matrix4.Identity);
            //Global.Light.debugDepthShader.SetMatrix4("transform",transform);
            //Global.Light.debugDepthShader.SetInt("depthMap",0);
            GL.DrawArrays(PrimitiveType.Triangles, 0, 6);
        }
        public void createSquareVerticesNoTexture(Vector3 position, Vector3 size)
        {
            this.position = position;
            float _positionX = 0f;
            float _positionY = 0f;
            float _positionZ = 0f;
            this.size = size;
            vertices.Clear();

            Vector3 _boxLength = size;

            //Buat temporary vector
            Vector3 temp_vector;
            vertices.Clear();
            //1. Inisialisasi vertex

            #region segitiga 1 (depan kiri)
            // Titik 1 LTF
            temp_vector.X = _positionX - _boxLength.X / 2.0f; // x 
            temp_vector.Y = _positionY + _boxLength.Y / 2.0f; // y
            temp_vector.Z = _positionZ + _boxLength.Z / 2.0f; // z
            vertices.Add(temp_vector.X);
            vertices.Add(temp_vector.Y);
            vertices.Add(temp_vector.Z);

            // Titik 2 RBF
            temp_vector.X = _positionX + _boxLength.X / 2.0f; // x
            temp_vector.Y = _positionY - _boxLength.Y / 2.0f; // y
            temp_vector.Z = _positionZ + _boxLength.Z / 2.0f; // z

            vertices.Add(temp_vector.X);
            vertices.Add(temp_vector.Y);
            vertices.Add(temp_vector.Z);
            // Titik 3 LBF
            temp_vector.X = _positionX - _boxLength.X / 2.0f; // x
            temp_vector.Y = _positionY - _boxLength.Y / 2.0f; // y
            temp_vector.Z = _positionZ + _boxLength.Z / 2.0f; // z
            vertices.Add(temp_vector.X);
            vertices.Add(temp_vector.Y);
            vertices.Add(temp_vector.Z);
            #endregion

            #region segitiga  (depan kanan)
            // Titik 1 LTF
            temp_vector.X = _positionX - _boxLength.X / 2.0f; // x 
            temp_vector.Y = _positionY + _boxLength.Y / 2.0f; // y
            temp_vector.Z = _positionZ + _boxLength.Z / 2.0f; // z

            vertices.Add(temp_vector.X);
            vertices.Add(temp_vector.Y);
            vertices.Add(temp_vector.Z);

            // Titik 2 RTF
            temp_vector.X = _positionX + _boxLength.X / 2.0f; // x
            temp_vector.Y = _positionY + _boxLength.Y / 2.0f; // y
            temp_vector.Z = _positionZ + _boxLength.Z / 2.0f; // z

            vertices.Add(temp_vector.X);
            vertices.Add(temp_vector.Y);
            vertices.Add(temp_vector.Z);
            // Titik 3 RBF
            temp_vector.X = _positionX + _boxLength.X / 2.0f; // x
            temp_vector.Y = _positionY - _boxLength.Y / 2.0f; // y
            temp_vector.Z = _positionZ + _boxLength.Z / 2.0f; // z
            vertices.Add(temp_vector.X);
            vertices.Add(temp_vector.Y);
            vertices.Add(temp_vector.Z);
            transform = Matrix4.Identity;
            #endregion
        }
        public void createSquareVertices(Vector3 position, Vector3 size)
        {
            this.position = position;
            float _positionX = 0f;
            float _positionY = 0f;
            float _positionZ = 0f;
            this.size = size;
            vertices.Clear();

            Vector3 _boxLength = size;

            //Buat temporary vector
            Vector3 temp_vector;
            vertices.Clear();
            //1. Inisialisasi vertex

            #region segitiga 1 (depan kiri)
            // Titik 1 LTF
            temp_vector.X = _positionX - _boxLength.X / 2.0f; // x 
            temp_vector.Y = _positionY + _boxLength.Y / 2.0f; // y
            temp_vector.Z = _positionZ + _boxLength.Z / 2.0f; // z
            vertices.Add(temp_vector.X);
            vertices.Add(temp_vector.Y);
            vertices.Add(temp_vector.Z);
            vertices.Add(0f);
            vertices.Add(0f);

            // Titik 2 RBF
            temp_vector.X = _positionX + _boxLength.X / 2.0f; // x
            temp_vector.Y = _positionY - _boxLength.Y / 2.0f; // y
            temp_vector.Z = _positionZ + _boxLength.Z / 2.0f; // z

            vertices.Add(temp_vector.X);
            vertices.Add(temp_vector.Y);
            vertices.Add(temp_vector.Z);
            vertices.Add(1f);
            vertices.Add(1f);
            // Titik 3 LBF
            temp_vector.X = _positionX - _boxLength.X / 2.0f; // x
            temp_vector.Y = _positionY - _boxLength.Y / 2.0f; // y
            temp_vector.Z = _positionZ + _boxLength.Z / 2.0f; // z
            vertices.Add(temp_vector.X);
            vertices.Add(temp_vector.Y);
            vertices.Add(temp_vector.Z);
            vertices.Add(0f);
            vertices.Add(1f);
            #endregion

            #region segitiga  (depan kanan)
            // Titik 1 LTF
            temp_vector.X = _positionX - _boxLength.X / 2.0f; // x 
            temp_vector.Y = _positionY + _boxLength.Y / 2.0f; // y
            temp_vector.Z = _positionZ + _boxLength.Z / 2.0f; // z

            vertices.Add(temp_vector.X);
            vertices.Add(temp_vector.Y);
            vertices.Add(temp_vector.Z);
            vertices.Add(0f);
            vertices.Add(0f);

            // Titik 2 RTF
            temp_vector.X = _positionX + _boxLength.X / 2.0f; // x
            temp_vector.Y = _positionY + _boxLength.Y / 2.0f; // y
            temp_vector.Z = _positionZ + _boxLength.Z / 2.0f; // z

            vertices.Add(temp_vector.X);
            vertices.Add(temp_vector.Y);
            vertices.Add(temp_vector.Z);
            vertices.Add(1f);
            vertices.Add(0f);
            // Titik 3 RBF
            temp_vector.X = _positionX + _boxLength.X / 2.0f; // x
            temp_vector.Y = _positionY - _boxLength.Y / 2.0f; // y
            temp_vector.Z = _positionZ + _boxLength.Z / 2.0f; // z
            vertices.Add(temp_vector.X);
            vertices.Add(temp_vector.Y);
            vertices.Add(temp_vector.Z);
            vertices.Add(1f);
            vertices.Add(1f);
            transform = Matrix4.Identity;
            vertices = new List<float> {
            -1.0f,  1.0f, 0.0f, 0.0f, 1.0f, //KIRI ATAS
            -1.0f, -1.0f, 0.0f, 0.0f, 0.0f, //KIRI BAWAH
             1.0f,  1.0f, 0.0f, 1.0f, 1.0f, //KANAN ATAS
             1.0f,  1.0f, 0.0f, 1.0f, 1.0f, //KANAN ATAS
            -1.0f, -1.0f, 0.0f, 0.0f, 0.0f, //KIRI BAWAH
             1.0f, -1.0f, 0.0f, 1.0f, 0.0f, // KANAN BAWAH
            };
            #endregion
        }
        public List<float> getVertices()
        {
            return vertices;
        }
        public List<uint> getVertexIndices()
        {
            return vertexIndices;
        }
        public void setVertexIndices(List<uint> temp)
        {
            vertexIndices = temp;
        }
        public int getVertexBufferObject()
        {
            return _vertexBufferObject;
        }
        public int getElementBufferObject()
        {
            return _elementBufferObject;
        }
        public int getVertexArrayObject()
        {
            return _vertexArrayObject;
        }
        public Shader getShader()
        {
            return _shader;
        }
        public Matrix4 getTransform()
        {
            return transform;
        }
        public void rotate()
        {
            //rotate parentnya
            //sumbu Y
            //transform = transform * Matrix4.CreateRotationY(MathHelper.DegreesToRadians(20f));
            ////rotate childnya
            //foreach (var meshobj in child)
            //{
            //    meshobj.rotate();
            //}
        }
        //public void scale()
        //{
        //    transform = transform * Matrix4.CreateScale(1.9f);
        //}
        //translasi perpindahan suatu objek ke posisi lain
        public void translate()
        {
            //perpindahan sebanyak 0.1 ke x, 0.1 ke y, dan 0 ke z
            transform = transform * Matrix4.CreateTranslation(0.1f, 0.1f, 0.0f);
        }
        List<uint> textureIndices = new List<uint>();
        List<uint> normalIndices = new List<uint>();
        public void LoadObjFile(string path)
        {
            //komputer ngecek, apakah file bisa diopen atau tidak
            if (!File.Exists(path))
            {
                //mengakhiri program dan kita kasih peringatan
                throw new FileNotFoundException("Unable to open \"" + path + "\", does not exist.");
            }
            //lanjut ke sini
            using (StreamReader streamReader = new StreamReader(path))
            {
                useIBO = false;
                loadFromOBJ = true;
                while (!streamReader.EndOfStream)
                {
                    //aku ngambil 1 baris tersebut -> dimasukkan ke dalam List string -> dengan di split pakai spasi
                    List<string> words = new List<string>(streamReader.ReadLine().ToLower().Split(' '));
                    //removeAll(kondisi dimana penghapusan terjadi)
                    words.RemoveAll(s => s == string.Empty);
                    //Melakukan pengecekkan apakah dalam satu list -> ada isinya atau tidak list nya tersebut
                    //kalau ada continue, perintah-perintah yang ada dibawahnya tidak akan dijalankan 
                    //dan dia bakal kembali keatas lagi / melanjutkannya whilenya
                    if (words.Count == 0)
                        continue;

                    //System.Console.WriteLine("New While");
                    //foreach (string x in words)
                    //               {
                    //	System.Console.WriteLine("tes");
                    //	System.Console.WriteLine(x);
                    //               }

                    string type = words[0];
                    //remove at -> menghapus data dalam suatu indexs dan otomatis data pada indeks
                    //berikutnya itu otomatis mundur kebelakang 1
                    words.RemoveAt(0);


                    switch (type)
                    {
                        // vertex
                        //parse merubah dari string ke tipe variabel yang diinginkan
                        //ada /10 karena saaat ini belum masuk materi camera
                        case "v":
                            rawPositonVertices.Add(new Vector3(float.Parse(words[0], CultureInfo.InvariantCulture.NumberFormat), float.Parse(words[1], CultureInfo.InvariantCulture.NumberFormat), float.Parse(words[2], CultureInfo.InvariantCulture.NumberFormat)));
                            //Console.WriteLine(words[0]);
                            //Console.WriteLine(float.Parse(words[0], CultureInfo.InvariantCulture.NumberFormat));
                            break;

                        case "vt":
                            rawTextureVertices.Add(new Vector2(float.Parse(words[0], CultureInfo.InvariantCulture.NumberFormat), float.Parse(words[1], CultureInfo.InvariantCulture.NumberFormat)));
                            break;

                        case "vn":
                            normals.Add(new Vector3(float.Parse(words[0], CultureInfo.InvariantCulture.NumberFormat), float.Parse(words[1], CultureInfo.InvariantCulture.NumberFormat), float.Parse(words[2], CultureInfo.InvariantCulture.NumberFormat)));
                            break;
                        // face
                        case "f":
                            foreach (string w in words)
                            {
                                if (w.Length == 0)
                                    continue;

                                string[] comps = w.Split('/');
                                try
                                {

                                    if (comps[0].ToString() != "")
                                        vertexIndices.Add(uint.Parse(comps[0]) - 1);
                                    if (comps[1].ToString() != "")
                                        textureIndices.Add(uint.Parse(comps[1]) - 1);
                                    if (comps[2].ToString() != "")
                                        normalIndices.Add(uint.Parse(comps[2]) - 1);
                                }
                                catch (System.FormatException ex)
                                {
                                    Console.WriteLine(ex.Message);
                                    Console.WriteLine($" '{comps[0]}' ");
                                    Console.WriteLine($" '{comps[1]}' ");
                                    Console.WriteLine($" '{comps[2]}' ");
                                }

                            }
                            break;

                        default:
                            break;
                    }
                }
            }
        }

        public Vector3 position = new Vector3(0);
        //<summary>
        //<param name="textureCoordinate">urutan: texture coord bisa dilihat di body function</param>
        //</sumary>
        public void createBoxVertices(Vector3 position, Vector3 size, List<Vector2> textureCoordinate = null)
        {
            if (textureCoordinate == null)
            {
                createBoxVerticesNoTexture(position, size);
                transform = Matrix4.Identity;
                transform *= Matrix4.CreateTranslation(position.X, position.Y, position.Z);

                return;
            }
            //biar lebih fleksibel jangan inisialiasi posisi dan 
            //panjang kotak didalam tapi ditaruh ke parameter
            this.position = position;
            float _positionX = 0f;
            float _positionY = 0f;
            float _positionZ = 0f;
            this.size = size;
            vertices.Clear();

            Vector3 _boxLength = size;

            //Buat temporary vector
            Vector3 temp_vector;
            vertices.Clear();
            //1. Inisialisasi vertex

            #region segitiga 1 (depan kiri)
            // Titik 1 LTF
            temp_vector.X = _positionX - _boxLength.X / 2.0f; // x 
            temp_vector.Y = _positionY + _boxLength.Y / 2.0f; // y
            temp_vector.Z = _positionZ + _boxLength.Z / 2.0f; // z
            vertices.Add(temp_vector.X);
            vertices.Add(temp_vector.Y);
            vertices.Add(temp_vector.Z);
            vertices.Add(textureCoordinate[0].X);
            vertices.Add(textureCoordinate[0].Y);
            vertices.Add(0f);
            vertices.Add(0f);
            vertices.Add(1f);

            // Titik 2 RBF
            temp_vector.X = _positionX + _boxLength.X / 2.0f; // x
            temp_vector.Y = _positionY - _boxLength.Y / 2.0f; // y
            temp_vector.Z = _positionZ + _boxLength.Z / 2.0f; // z

            vertices.Add(temp_vector.X);
            vertices.Add(temp_vector.Y);
            vertices.Add(temp_vector.Z);
            vertices.Add(textureCoordinate[1].X);
            vertices.Add(textureCoordinate[1].Y);
            vertices.Add(0f);
            vertices.Add(0f);
            vertices.Add(1f);
            // Titik 3 LBF
            temp_vector.X = _positionX - _boxLength.X / 2.0f; // x
            temp_vector.Y = _positionY - _boxLength.Y / 2.0f; // y
            temp_vector.Z = _positionZ + _boxLength.Z / 2.0f; // z
            vertices.Add(temp_vector.X);
            vertices.Add(temp_vector.Y);
            vertices.Add(temp_vector.Z);
            vertices.Add(textureCoordinate[2].X);
            vertices.Add(textureCoordinate[2].Y);
            vertices.Add(0f);
            vertices.Add(0f);
            vertices.Add(1f);
            #endregion

            #region segitiga  (depan kanan)
            // Titik 1 LTF
            temp_vector.X = _positionX - _boxLength.X / 2.0f; // x 
            temp_vector.Y = _positionY + _boxLength.Y / 2.0f; // y
            temp_vector.Z = _positionZ + _boxLength.Z / 2.0f; // z

            vertices.Add(temp_vector.X);
            vertices.Add(temp_vector.Y);
            vertices.Add(temp_vector.Z);
            vertices.Add(textureCoordinate[3].X);
            vertices.Add(textureCoordinate[3].Y);
            vertices.Add(0f);
            vertices.Add(0f);
            vertices.Add(1f);

            // Titik 2 RTF
            temp_vector.X = _positionX + _boxLength.X / 2.0f; // x
            temp_vector.Y = _positionY + _boxLength.Y / 2.0f; // y
            temp_vector.Z = _positionZ + _boxLength.Z / 2.0f; // z

            vertices.Add(temp_vector.X);
            vertices.Add(temp_vector.Y);
            vertices.Add(temp_vector.Z);
            vertices.Add(textureCoordinate[4].X);
            vertices.Add(textureCoordinate[4].Y);
            vertices.Add(0f);
            vertices.Add(0f);
            vertices.Add(1f);
            // Titik 3 RBF
            temp_vector.X = _positionX + _boxLength.X / 2.0f; // x
            temp_vector.Y = _positionY - _boxLength.Y / 2.0f; // y
            temp_vector.Z = _positionZ + _boxLength.Z / 2.0f; // z
            vertices.Add(temp_vector.X);
            vertices.Add(temp_vector.Y);
            vertices.Add(temp_vector.Z);
            vertices.Add(textureCoordinate[5].X);
            vertices.Add(textureCoordinate[5].Y);
            vertices.Add(0f);
            vertices.Add(0f);
            vertices.Add(1f);
            #endregion


            #region segitiga (kiri kiri)
            // Titik 1 LTB
            temp_vector.X = _positionX - _boxLength.X / 2.0f; // x 
            temp_vector.Y = _positionY + _boxLength.Y / 2.0f; // y
            temp_vector.Z = _positionZ - _boxLength.Z / 2.0f; // z

            vertices.Add(temp_vector.X);
            vertices.Add(temp_vector.Y);
            vertices.Add(temp_vector.Z);
            vertices.Add(textureCoordinate[6].X);
            vertices.Add(textureCoordinate[6].Y);
            vertices.Add(-1f);
            vertices.Add(0f);
            vertices.Add(0f);

            // Titik 2 LTF
            temp_vector.X = _positionX - _boxLength.X / 2.0f; // x
            temp_vector.Y = _positionY + _boxLength.Y / 2.0f; // y
            temp_vector.Z = _positionZ + _boxLength.Z / 2.0f; // z

            vertices.Add(temp_vector.X);
            vertices.Add(temp_vector.Y);
            vertices.Add(temp_vector.Z);
            vertices.Add(textureCoordinate[7].X);
            vertices.Add(textureCoordinate[7].Y);
            vertices.Add(-1f);
            vertices.Add(0f);
            vertices.Add(0f);
            // Titik 3 LBB
            temp_vector.X = _positionX - _boxLength.X / 2.0f; // x
            temp_vector.Y = _positionY - _boxLength.Y / 2.0f; // y
            temp_vector.Z = _positionZ - _boxLength.Z / 2.0f; // z
            vertices.Add(temp_vector.X);
            vertices.Add(temp_vector.Y);
            vertices.Add(temp_vector.Z);
            vertices.Add(textureCoordinate[8].X);
            vertices.Add(textureCoordinate[8].Y);
            vertices.Add(-1f);
            vertices.Add(0f);
            vertices.Add(0f);
            #endregion

            #region segitiga (kiri kanan)
            // Titik 1 LTF
            temp_vector.X = _positionX - _boxLength.X / 2.0f; // x 
            temp_vector.Y = _positionY + _boxLength.Y / 2.0f; // y
            temp_vector.Z = _positionZ + _boxLength.Z / 2.0f; // z

            vertices.Add(temp_vector.X);
            vertices.Add(temp_vector.Y);
            vertices.Add(temp_vector.Z);
            vertices.Add(textureCoordinate[9].X);
            vertices.Add(textureCoordinate[9].Y);
            vertices.Add(-1f);
            vertices.Add(0f);
            vertices.Add(0f);

            // Titik 2 LBF
            temp_vector.X = _positionX - _boxLength.X / 2.0f; // x
            temp_vector.Y = _positionY - _boxLength.Y / 2.0f; // y
            temp_vector.Z = _positionZ + _boxLength.Z / 2.0f; // z

            vertices.Add(temp_vector.X);
            vertices.Add(temp_vector.Y);
            vertices.Add(temp_vector.Z);
            vertices.Add(textureCoordinate[10].X);
            vertices.Add(textureCoordinate[10].Y);
            vertices.Add(-1f);
            vertices.Add(0f);
            vertices.Add(0f);
            // Titik 3 LBB
            temp_vector.X = _positionX - _boxLength.X / 2.0f; // x
            temp_vector.Y = _positionY - _boxLength.Y / 2.0f; // y
            temp_vector.Z = _positionZ - _boxLength.Z / 2.0f; // z
            vertices.Add(temp_vector.X);
            vertices.Add(temp_vector.Y);
            vertices.Add(temp_vector.Z);
            vertices.Add(textureCoordinate[11].X);
            vertices.Add(textureCoordinate[11].Y);
            vertices.Add(-1f);
            vertices.Add(0f);
            vertices.Add(0f);
            #endregion


            #region segitiga (kanan kiri)
            // Titik 1 RTF
            temp_vector.X = _positionX + _boxLength.X / 2.0f; // x 
            temp_vector.Y = _positionY + _boxLength.Y / 2.0f; // y
            temp_vector.Z = _positionZ + _boxLength.Z / 2.0f; // z

            vertices.Add(temp_vector.X);
            vertices.Add(temp_vector.Y);
            vertices.Add(temp_vector.Z);
            vertices.Add(textureCoordinate[12].X);
            vertices.Add(textureCoordinate[12].Y);
            vertices.Add(1f);
            vertices.Add(0f);
            vertices.Add(0f);

            // Titik 2 RBF
            temp_vector.X = _positionX + _boxLength.X / 2.0f; // x
            temp_vector.Y = _positionY - _boxLength.Y / 2.0f; // y
            temp_vector.Z = _positionZ + _boxLength.Z / 2.0f; // z
            vertices.Add(temp_vector.X);
            vertices.Add(temp_vector.Y);
            vertices.Add(temp_vector.Z);
            vertices.Add(textureCoordinate[13].X);
            vertices.Add(textureCoordinate[13].Y);
            vertices.Add(1f);
            vertices.Add(0f);
            vertices.Add(0f);

            // Titik 3 RBB
            temp_vector.X = _positionX + _boxLength.X / 2.0f; // x
            temp_vector.Y = _positionY - _boxLength.Y / 2.0f; // y
            temp_vector.Z = _positionZ - _boxLength.Z / 2.0f; // z
            vertices.Add(temp_vector.X);
            vertices.Add(temp_vector.Y);
            vertices.Add(temp_vector.Z);
            vertices.Add(textureCoordinate[14].X);
            vertices.Add(textureCoordinate[14].Y);
            vertices.Add(1f);
            vertices.Add(0f);
            vertices.Add(0f);
            #endregion

            #region segitiga (kanan kanan)
            // Titik 1 RTB
            temp_vector.X = _positionX + _boxLength.X / 2.0f; // x 
            temp_vector.Y = _positionY + _boxLength.Y / 2.0f; // y
            temp_vector.Z = _positionZ - _boxLength.Z / 2.0f; // z

            vertices.Add(temp_vector.X);
            vertices.Add(temp_vector.Y);
            vertices.Add(temp_vector.Z);
            vertices.Add(textureCoordinate[15].X);
            vertices.Add(textureCoordinate[15].Y);
            vertices.Add(1f);
            vertices.Add(0f);
            vertices.Add(0f);
            // Titik 2 RTF
            temp_vector.X = _positionX + _boxLength.X / 2.0f; // x
            temp_vector.Y = _positionY + _boxLength.Y / 2.0f; // y
            temp_vector.Z = _positionZ + _boxLength.Z / 2.0f; // z

            vertices.Add(temp_vector.X);
            vertices.Add(temp_vector.Y);
            vertices.Add(temp_vector.Z);
            vertices.Add(textureCoordinate[16].X);
            vertices.Add(textureCoordinate[16].Y);
            vertices.Add(1f);
            vertices.Add(0f);
            vertices.Add(0f);
            // Titik 3 RBB
            temp_vector.X = _positionX + _boxLength.X / 2.0f; // x
            temp_vector.Y = _positionY - _boxLength.Y / 2.0f; // y
            temp_vector.Z = _positionZ - _boxLength.Z / 2.0f; // z
            vertices.Add(temp_vector.X);
            vertices.Add(temp_vector.Y);
            vertices.Add(temp_vector.Z);
            vertices.Add(textureCoordinate[17].X);
            vertices.Add(textureCoordinate[17].Y);
            vertices.Add(1f);
            vertices.Add(0f);
            vertices.Add(0f);
            #endregion


            #region segitiga 1 (belakang kanan)
            // Titik 1 LTB
            temp_vector.X = _positionX - _boxLength.X / 2.0f; // x 
            temp_vector.Y = _positionY + _boxLength.Y / 2.0f; // y
            temp_vector.Z = _positionZ - _boxLength.Z / 2.0f; // z

            vertices.Add(temp_vector.X);
            vertices.Add(temp_vector.Y);
            vertices.Add(temp_vector.Z);
            vertices.Add(textureCoordinate[18].X);
            vertices.Add(textureCoordinate[18].Y);
            vertices.Add(0f);
            vertices.Add(0f);
            vertices.Add(-1f);

            // Titik 2 RTB
            temp_vector.X = _positionX + _boxLength.X / 2.0f; // x
            temp_vector.Y = _positionY + _boxLength.Y / 2.0f; // y
            temp_vector.Z = _positionZ - _boxLength.Z / 2.0f; // z

            vertices.Add(temp_vector.X);
            vertices.Add(temp_vector.Y);
            vertices.Add(temp_vector.Z);
            vertices.Add(textureCoordinate[19].X);
            vertices.Add(textureCoordinate[19].Y);
            vertices.Add(0f);
            vertices.Add(0f);
            vertices.Add(-1f);
            // Titik 3 RBB
            temp_vector.X = _positionX + _boxLength.X / 2.0f; // x
            temp_vector.Y = _positionY - _boxLength.Y / 2.0f; // y
            temp_vector.Z = _positionZ - _boxLength.Z / 2.0f; // z
            vertices.Add(temp_vector.X);
            vertices.Add(temp_vector.Y);
            vertices.Add(temp_vector.Z);
            vertices.Add(textureCoordinate[20].X);
            vertices.Add(textureCoordinate[20].Y);
            vertices.Add(0f);
            vertices.Add(0f);
            vertices.Add(-1f);
            #endregion


            #region segitiga  (belakang kanan)
            // Titik 1 LBB
            temp_vector.X = _positionX - _boxLength.X / 2.0f; // x 
            temp_vector.Y = _positionY - _boxLength.Y / 2.0f; // y
            temp_vector.Z = _positionZ - _boxLength.Z / 2.0f; // z

            vertices.Add(temp_vector.X);
            vertices.Add(temp_vector.Y);
            vertices.Add(temp_vector.Z);
            vertices.Add(textureCoordinate[21].X);
            vertices.Add(textureCoordinate[21].Y);
            vertices.Add(0f);
            vertices.Add(0f);
            vertices.Add(-1f);

            // Titik 2 LTB
            temp_vector.X = _positionX - _boxLength.X / 2.0f; // x
            temp_vector.Y = _positionY + _boxLength.Y / 2.0f; // y
            temp_vector.Z = _positionZ - _boxLength.Z / 2.0f; // z

            vertices.Add(temp_vector.X);
            vertices.Add(temp_vector.Y);
            vertices.Add(temp_vector.Z);
            vertices.Add(textureCoordinate[22].X);
            vertices.Add(textureCoordinate[22].Y);
            vertices.Add(0f);
            vertices.Add(0f);
            vertices.Add(-1f);
            // Titik 3 RBB
            temp_vector.X = _positionX + _boxLength.X / 2.0f; // x
            temp_vector.Y = _positionY - _boxLength.Y / 2.0f; // y
            temp_vector.Z = _positionZ - _boxLength.Z / 2.0f; // z
            vertices.Add(temp_vector.X);
            vertices.Add(temp_vector.Y);
            vertices.Add(temp_vector.Z);
            vertices.Add(textureCoordinate[23].X);
            vertices.Add(textureCoordinate[23].Y);
            vertices.Add(0f);
            vertices.Add(0f);
            vertices.Add(-1f);
            #endregion



            #region segitiga 1 (atas kiri)
            // Titik 1 LTB
            temp_vector.X = _positionX - _boxLength.X / 2.0f; // x 
            temp_vector.Y = _positionY + _boxLength.Y / 2.0f; // y
            temp_vector.Z = _positionZ - _boxLength.Z / 2.0f; // z

            vertices.Add(temp_vector.X);
            vertices.Add(temp_vector.Y);
            vertices.Add(temp_vector.Z);
            vertices.Add(textureCoordinate[24].X);
            vertices.Add(textureCoordinate[24].Y);
            vertices.Add(0f);
            vertices.Add(1f);
            vertices.Add(0f);

            // Titik 2 RTB
            temp_vector.X = _positionX + _boxLength.X / 2.0f; // x
            temp_vector.Y = _positionY + _boxLength.Y / 2.0f; // y
            temp_vector.Z = _positionZ - _boxLength.Z / 2.0f; // z

            vertices.Add(temp_vector.X);
            vertices.Add(temp_vector.Y);
            vertices.Add(temp_vector.Z);
            vertices.Add(textureCoordinate[25].X);
            vertices.Add(textureCoordinate[25].Y);
            vertices.Add(0f);
            vertices.Add(1f);
            vertices.Add(0f);
            // Titik 3 LTF
            temp_vector.X = _positionX - _boxLength.X / 2.0f; // x
            temp_vector.Y = _positionY + _boxLength.Y / 2.0f; // y
            temp_vector.Z = _positionZ + _boxLength.Z / 2.0f; // z
            vertices.Add(temp_vector.X);
            vertices.Add(temp_vector.Y);
            vertices.Add(temp_vector.Z);
            vertices.Add(textureCoordinate[26].X);
            vertices.Add(textureCoordinate[26].Y);
            vertices.Add(0f);
            vertices.Add(1f);
            vertices.Add(0f);
            #endregion


            #region segitiga  (atas kanan)
            // Titik 1 LTF
            temp_vector.X = _positionX - _boxLength.X / 2.0f; // x 
            temp_vector.Y = _positionY + _boxLength.Y / 2.0f; // y
            temp_vector.Z = _positionZ + _boxLength.Z / 2.0f; // z

            vertices.Add(temp_vector.X);
            vertices.Add(temp_vector.Y);
            vertices.Add(temp_vector.Z);
            vertices.Add(textureCoordinate[27].X);
            vertices.Add(textureCoordinate[27].Y);
            vertices.Add(0f);
            vertices.Add(1f);
            vertices.Add(0f);

            // Titik 2 RTB
            temp_vector.X = _positionX + _boxLength.X / 2.0f; // x
            temp_vector.Y = _positionY + _boxLength.Y / 2.0f; // y
            temp_vector.Z = _positionZ - _boxLength.Z / 2.0f; // z

            vertices.Add(temp_vector.X);
            vertices.Add(temp_vector.Y);
            vertices.Add(temp_vector.Z);
            vertices.Add(textureCoordinate[28].X);
            vertices.Add(textureCoordinate[28].Y);
            vertices.Add(0f);
            vertices.Add(1f);
            vertices.Add(0f);
            // Titik 3 RTF
            temp_vector.X = _positionX + _boxLength.X / 2.0f; // x
            temp_vector.Y = _positionY + _boxLength.Y / 2.0f; // y
            temp_vector.Z = _positionZ + _boxLength.Z / 2.0f; // z
            vertices.Add(temp_vector.X);
            vertices.Add(temp_vector.Y);
            vertices.Add(temp_vector.Z);
            vertices.Add(textureCoordinate[29].X);
            vertices.Add(textureCoordinate[29].Y);
            vertices.Add(0f);
            vertices.Add(1f);
            vertices.Add(0f);
            #endregion

            #region segitiga 1 (bawah kiri)
            // Titik 1 LBB
            temp_vector.X = _positionX - _boxLength.X / 2.0f; // x 
            temp_vector.Y = _positionY - _boxLength.Y / 2.0f; // y
            temp_vector.Z = _positionZ - _boxLength.Z / 2.0f; // z

            vertices.Add(temp_vector.X);
            vertices.Add(temp_vector.Y);
            vertices.Add(temp_vector.Z);
            vertices.Add(textureCoordinate[30].X);
            vertices.Add(0f);
            vertices.Add(-1f);
            vertices.Add(0f);
            vertices.Add(textureCoordinate[30].Y);

            // Titik 2 RBB
            temp_vector.X = _positionX + _boxLength.X / 2.0f; // x
            temp_vector.Y = _positionY - _boxLength.Y / 2.0f; // y
            temp_vector.Z = _positionZ - _boxLength.Z / 2.0f; // z

            vertices.Add(temp_vector.X);
            vertices.Add(temp_vector.Y);
            vertices.Add(temp_vector.Z);
            vertices.Add(textureCoordinate[31].X);
            vertices.Add(textureCoordinate[31].Y);
            vertices.Add(0f);
            vertices.Add(-1f);
            vertices.Add(0f);
            // Titik 3 LBF
            temp_vector.X = _positionX - _boxLength.X / 2.0f; // x
            temp_vector.Y = _positionY - _boxLength.Y / 2.0f; // y
            temp_vector.Z = _positionZ + _boxLength.Z / 2.0f; // z
            vertices.Add(temp_vector.X);
            vertices.Add(temp_vector.Y);
            vertices.Add(temp_vector.Z);
            vertices.Add(textureCoordinate[32].X);
            vertices.Add(textureCoordinate[32].Y);
            vertices.Add(0f);
            vertices.Add(-1f);
            vertices.Add(0f);
            #endregion


            #region segitiga  (bawah kanan)
            // Titik 1 LBF
            temp_vector.X = _positionX - _boxLength.X / 2.0f; // x 
            temp_vector.Y = _positionY - _boxLength.Y / 2.0f; // y
            temp_vector.Z = _positionZ + _boxLength.Z / 2.0f; // z

            vertices.Add(temp_vector.X);
            vertices.Add(temp_vector.Y);
            vertices.Add(temp_vector.Z);
            vertices.Add(textureCoordinate[33].X);
            vertices.Add(textureCoordinate[33].Y);
            vertices.Add(0f);
            vertices.Add(-1f);
            vertices.Add(0f);

            // Titik 2 RBB
            temp_vector.X = _positionX + _boxLength.X / 2.0f; // x
            temp_vector.Y = _positionY - _boxLength.Y / 2.0f; // y
            temp_vector.Z = _positionZ - _boxLength.Z / 2.0f; // z

            vertices.Add(temp_vector.X);
            vertices.Add(temp_vector.Y);
            vertices.Add(temp_vector.Z);
            vertices.Add(textureCoordinate[34].X);
            vertices.Add(textureCoordinate[34].Y);
            vertices.Add(0f);
            vertices.Add(-1f);
            vertices.Add(0f);
            // Titik 3 RBF
            temp_vector.X = _positionX + _boxLength.X / 2.0f; // x
            temp_vector.Y = _positionY - _boxLength.Y / 2.0f; // y
            temp_vector.Z = _positionZ + _boxLength.Z / 2.0f; // z
            vertices.Add(temp_vector.X);
            vertices.Add(temp_vector.Y);
            vertices.Add(temp_vector.Z);
            vertices.Add(textureCoordinate[35].X);
            vertices.Add(textureCoordinate[35].Y);
            vertices.Add(0f);
            vertices.Add(-1f);
            vertices.Add(0f);
            #endregion

            transform = Matrix4.Identity;
            transform *= Matrix4.CreateTranslation(position.X, position.Y, position.Z);

            return;

            //2. Inisialisasi index vertex
            //vertexIndices = new List<uint> {
            //    // Segitiga Depan 1
            //    0, 1, 2,
            //    // Segitiga Depan 2
            //    1, 2, 3,
            //    // Segitiga Atas 1
            //    0, 4, 5,
            //    // Segitiga Atas 2
            //    0, 1, 5,
            //    // Segitiga Kanan 1
            //    1, 3, 5,
            //    // Segitiga Kanan 2
            //    3, 5, 7,
            //    // Segitiga Kiri 1
            //    0, 2, 4,
            //    // Segitiga Kiri 2
            //    2, 4, 6,
            //    // Segitiga Belakang 1
            //    4, 5, 6,
            //    // Segitiga Belakang 2
            //    5, 6, 7,
            //    // Segitiga Bawah 1
            //    2, 3, 6,
            //    // Segitiga Bawah 2
            //    3, 6, 7
            //};

        }

        private void createBoxVerticesNoTexture(Vector3 position, Vector3 size)
        {
            //Buat temporary vector
            Vector3 temp_vector;
            float _positionX = 0f;
            float _positionY = 0f;
            float _positionZ = 0f;

            Vector3 _boxLength = size;
            //1. Inisialisasi vertex
            // Titik 1
            temp_vector.X = _positionX - _boxLength.X / 2.0f; // x 
            temp_vector.Y = _positionY + _boxLength.Y / 2.0f; // y
            temp_vector.Z = _positionZ - _boxLength.Z / 2.0f; // z





            vertices.Add(temp_vector.X);
            vertices.Add(temp_vector.Y);
            vertices.Add(temp_vector.Z);


            // Titik 2
            temp_vector.X = _positionX + _boxLength.X / 2.0f; // x
            temp_vector.Y = _positionY + _boxLength.Y / 2.0f; // y
            temp_vector.Z = _positionZ - _boxLength.Z / 2.0f; // z






            vertices.Add(temp_vector.X);
            vertices.Add(temp_vector.Y);
            vertices.Add(temp_vector.Z);

            // Titik 3
            temp_vector.X = _positionX - _boxLength.X / 2.0f; // x
            temp_vector.Y = _positionY - _boxLength.Y / 2.0f; // y
            temp_vector.Z = _positionZ - _boxLength.Z / 2.0f; // z





            vertices.Add(temp_vector.X);
            vertices.Add(temp_vector.Y);
            vertices.Add(temp_vector.Z);


            // Titik 4
            temp_vector.X = _positionX + _boxLength.X / 2.0f; // x
            temp_vector.Y = _positionY - _boxLength.Y / 2.0f; // y
            temp_vector.Z = _positionZ - _boxLength.Z / 2.0f; // z






            vertices.Add(temp_vector.X);
            vertices.Add(temp_vector.Y);
            vertices.Add(temp_vector.Z);


            // Titik 5
            temp_vector.X = _positionX - _boxLength.X / 2.0f; // x
            temp_vector.Y = _positionY + _boxLength.Y / 2.0f; // y
            temp_vector.Z = _positionZ + _boxLength.Z / 2.0f; // z






            vertices.Add(temp_vector.X);
            vertices.Add(temp_vector.Y);
            vertices.Add(temp_vector.Z);


            // Titik 6
            temp_vector.X = _positionX + _boxLength.X / 2.0f; // x
            temp_vector.Y = _positionY + _boxLength.Y / 2.0f; // y
            temp_vector.Z = _positionZ + _boxLength.Z / 2.0f; // z






            vertices.Add(temp_vector.X);
            vertices.Add(temp_vector.Y);
            vertices.Add(temp_vector.Z);


            // Titik 7
            temp_vector.X = _positionX - _boxLength.X / 2.0f; // x
            temp_vector.Y = _positionY - _boxLength.Y / 2.0f; // y
            temp_vector.Z = _positionZ + _boxLength.Z / 2.0f; // z






            vertices.Add(temp_vector.X);
            vertices.Add(temp_vector.Y);
            vertices.Add(temp_vector.Z);


            // Titik 8
            temp_vector.X = _positionX + _boxLength.X / 2.0f; // x
            temp_vector.Y = _positionY - _boxLength.Y / 2.0f; // y
            temp_vector.Z = _positionZ + _boxLength.Z / 2.0f; // z




            vertices.Add(temp_vector.X);
            vertices.Add(temp_vector.Y);
            vertices.Add(temp_vector.Z);

            //2. Inisialisasi index vertex
            vertexIndices = new List<uint> {
                // Segitiga Depan 1
                0, 1, 2,
                // Segitiga Depan 2
                1, 2, 3,
                // Segitiga Atas 1
                0, 4, 5,
                // Segitiga Atas 2
                0, 1, 5,
                // Segitiga Kanan 1
                1, 3, 5,
                // Segitiga Kanan 2
                3, 5, 7,
                // Segitiga Kiri 1
                0, 2, 4,
                // Segitiga Kiri 2
                2, 4, 6,
                // Segitiga Belakang 1
                4, 5, 6,
                // Segitiga Belakang 2
                5, 6, 7,
                // Segitiga Bawah 1
                2, 3, 6,
                // Segitiga Bawah 2
                3, 6, 7
            };
            useIBO = true;
        }
    }
}
