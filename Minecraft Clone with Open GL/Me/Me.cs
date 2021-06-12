using LearnOpenTK.Common;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Minecraft_Clone_with_Open_GL.Me
{
    class Me
    {
        Mesh handRight;
        public bool isMove = false;
        public Me()
        {
            //handRight = new Mesh(true, true,true);
            //List<Vector2> result = new List<Vector2>();

            //result = generateTextureCoordRightHand();
            //handRight.createBoxVertices(new Vector3(0f, 0f, 0f), new Vector3(0.25f, 0.8f, 0.25f), result);
            //handRight.setupObject();
            //handRight.setTexture(@$"{Program.__DIR__}\assets\Steve\hand.png");
            //handRight.unbindVAO();
        }
        private List<Vector2> generateTextureCoordRightHand()
        {
            List<Vector2> temp = new List<Vector2>();
            temp.Add(new Vector2(28f / 137f, 0f)); // persegi depan, kiri atas
            temp.Add(new Vector2(55f / 137f, 1f)); // persegi depan, kanan bawah
            temp.Add(new Vector2(28f / 137f, 1f)); // persegi depan, kiri bawah

            temp.Add(new Vector2(28f / 137f, 0f)); // persegi depan, kiri atas
            temp.Add(new Vector2(55f / 137f, 0f)); // persegi depan, kanan atas
            temp.Add(new Vector2(55f / 137f, 1f)); // persegi depan, kanan bawah

            temp.Add(new Vector2(55f / 137f, 0f)); // persegi kiri, kiri atas
            temp.Add(new Vector2(83f / 137f, 0f)); // persegi kiri, kanan atas
            temp.Add(new Vector2(55f / 137f, 1f)); // persegi kiri, kiri bawah

            temp.Add(new Vector2(83f / 137f, 0f)); // persegi kiri, kanan atas
            temp.Add(new Vector2(83f / 137f, 1f)); // persegi kiri, kanan bawah
            temp.Add(new Vector2(55f / 137f, 1f)); // persegi kiri, kiri bawah

            temp.Add(new Vector2(137f / 137f, 0f)); // persegi kanan, kiri atas
            temp.Add(new Vector2(137f / 137f, 1f)); // persegi kanan, kiri bawah
            temp.Add(new Vector2(110f / 137f, 1f)); // persegi kanan, kanan bawah



            temp.Add(new Vector2(110f / 137f, 0f)); // persegi kanan, kanan atas
            temp.Add(new Vector2(137f / 137f, 0f)); // persegi kanan, kiri atas
            temp.Add(new Vector2(110f / 137f, 1f)); // persegi kanan, kanan bawah

            temp.Add(new Vector2(110f / 137f, 0f)); // persegi belakang, kiri atas
            temp.Add(new Vector2(83f / 137f, 0f)); // persegi belakang, kanan atas
            temp.Add(new Vector2(83f / 137f, 1f)); // persegi belakang, kanan bawah

            temp.Add(new Vector2(110f / 137f, 1f)); // persegi belakang, kiri bawah
            temp.Add(new Vector2(110f / 137f, 0f)); // persegi belakang, kiri atas
            temp.Add(new Vector2(83f / 137f, 1f)); // persegi belakang, kanan bawah

            temp.Add(new Vector2(0f / 137f, 54f / 82f)); // persegi atas, kiri atas
            temp.Add(new Vector2(28f / 137f, 54f / 82f)); // persegi atas, kanan atas
            temp.Add(new Vector2(0f / 137f, 1f)); // persegi atas, kiri bawah

            temp.Add(new Vector2(28f / 137f, 54f / 82f)); // persegi atas, kanan atas
            temp.Add(new Vector2(28f / 137f, 1f)); // persegi atas, kanan bawah
            temp.Add(new Vector2(0f / 137f, 1f)); // persegi atas, kiri bawah

            temp.Add(new Vector2(0f / 137f, 26f / 82f)); // persegi bawah, kiri atas
            temp.Add(new Vector2(28f / 137f, 26f / 82f)); // persegi bawah, kanan atas
            temp.Add(new Vector2(0f / 137f, 54f / 82f)); // persegi bawah, kiri bawah

            //ini terpaksai pakai texture yg lain. belum diEDIT (SEGITIA 1 BAWAH KIRI)
            temp.Add(new Vector2(0f / 137f, 54f / 82f)); // persegi bawah, kiri bawah
            temp.Add(new Vector2(28f / 137f, 54f / 82f)); // persegi bawah, kanan bawah
            temp.Add(new Vector2(28f / 137f, 26f / 82f)); // persegi bawah, kiri atas
            return temp;



        }
        public void render(Camera camera)
        {
            //if (isUpdated)
            //    isUpdated = false;
            //else
            //    throw new Exception("you must call update on onUpdateFrame first!");
            //handRight.render(camera);
        }
        bool isUpdated = false;
        public void Update(Vector3 cameraposition, float cameraYaw, float cameraPitch)
        {
            //isUpdated = true;
            //handRight.position = new Vector3(0f, 0f, 0f);
            //handRight.transform = Matrix4.Identity;
            //handRight.transform *= Matrix4.CreateRotationX(MathHelper.DegreesToRadians(120f));
            //handRight.transform *= Matrix4.CreateRotationY(MathHelper.DegreesToRadians(30f));
            //handRight.transform *= Matrix4.CreateTranslation(new Vector3(0.7f, -0.7f, -0.8f));

        }
        float dummy = 0;
    }
}
