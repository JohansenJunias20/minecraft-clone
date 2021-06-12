using LearnOpenTK.Common;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Text;
using OpenTK.Graphics.OpenGL;

namespace Minecraft_Clone_with_Open_GL.Steve
{
    public class Steve
    {
        Mesh head;
        Mesh body;
        Mesh handLeft;
        Mesh handRight;
        Mesh legLeft;
        Mesh legRight;
        private float degreeHand = 0f;
        public Vector3 rotation;
        public Steve(Vector3 position)
        {
            body = new Mesh(true, true);
            List<Vector2> result = generateTextureCoordBody();
            body.createBoxVertices(new Vector3(position), new Vector3(0.55f, 0.8f, 0.25f), result);
            //body.createBoxVertices(new Vector3(position), new Vector3(2f, 2f, 2f), result);
            body.setupObject();
            body.setTexture(@$"{Program.__DIR__}\assets\Steve\body.png");
            body.unbindVAO();

            head = new Mesh(true, true);
            result.Clear();
            result = generateTextureCoordHead();
            head.createBoxVertices(new Vector3(position.X, position.Y + body.size.Y / 2f + 0.56f / 2f, position.Z), new Vector3(0.55f, 0.56f, 0.5f), result);
            head.setupObject();
            head.setTexture(@$"{Program.__DIR__}\assets\Steve\head\template-texture_0004_Layer-5.png");
            head.unbindVAO();


            handLeft = new Mesh(true, true);
            result.Clear();
            result = generateTextureCoordLeftHand();
            handLeft.createBoxVertices(new Vector3(position.X - body.size.X / 2f - 0.25f / 2f, position.Y, position.Z), new Vector3(0.25f, 0.8f, 0.25f), result);
            handLeft.setupObject();
            handLeft.setTexture(@$"{Program.__DIR__}\assets\Steve\hand.png");
            handLeft.unbindVAO();

            handRight = new Mesh(true, true);
            result.Clear();
            result = generateTextureCoordRightHand();
            handRight.createBoxVertices(new Vector3(position.X + body.size.X / 2f + 0.25f / 2f, position.Y, position.Z), new Vector3(0.25f, 0.8f, 0.25f), result);
            handRight.setupObject();
            handRight.setTexture(@$"{Program.__DIR__}\assets\Steve\hand.png");
            handRight.unbindVAO();



            legLeft = new Mesh(true, true);
            result.Clear();
            result = generateTextureCoordLeftLeg();
            legLeft.createBoxVertices(new Vector3(position.X - body.size.X / 2f + (0.55f / 2f) / 2f, position.Y - body.size.Y / 2f - 0.8f / 2f, position.Z), new Vector3(0.55f / 2f, 0.8f, 0.25f), result);
            legLeft.setupObject();
            legLeft.setTexture(@$"{Program.__DIR__}\assets\Steve\leg.png");
            legLeft.unbindVAO();


            legRight = new Mesh(true, true);
            result.Clear();
            result = generateTextureCoordRightLeg();
            legRight.createBoxVertices(new Vector3(position.X + body.size.X / 2f - (0.55f / 2f) / 2f, position.Y - body.size.Y / 2f - 0.8f / 2f, position.Z), new Vector3(0.55f / 2f, 0.8f, 0.25f), result);
            legRight.setupObject();
            legRight.setTexture(@$"{Program.__DIR__}\assets\Steve\leg.png");
            legRight.unbindVAO();


        }
        private List<Vector2> generateTextureCoordHead()
        {
            List<Vector2> temp = new List<Vector2>();
            temp.Add(new Vector2(212f / 321f, 0f)); // persegi depan, kiri atas
            temp.Add(new Vector2(266f / 321f, 1f)); // persegi depan, kanan bawah
            temp.Add(new Vector2(212f / 321f, 1f)); // persegi depan, kiri bawah

            temp.Add(new Vector2(212f / 321f, 0f)); // persegi depan, kiri atas
            temp.Add(new Vector2(266f / 321f, 0f)); // persegi depan, kanan atas
            temp.Add(new Vector2(266f / 321f, 1f)); // persegi depan, kanan bawah

            temp.Add(new Vector2(157f / 321f, 0f)); // persegi kiri, kiri atas
            temp.Add(new Vector2(212f / 321f, 0f)); // persegi kiri, kanan atas
            temp.Add(new Vector2(157f / 321f, 1f)); // persegi kiri, kiri bawah

            temp.Add(new Vector2(212f / 321f, 0f)); // persegi kiri, kanan atas
            temp.Add(new Vector2(212f / 321f, 1f)); // persegi kiri, kanan bawah
            temp.Add(new Vector2(157f / 321f, 1f)); // persegi kiri, kiri bawah

            temp.Add(new Vector2(266f / 321f, 0f)); // persegi kanan, kiri atas
            temp.Add(new Vector2(266f / 321f, 1f)); // persegi kanan, kiri bawah
            temp.Add(new Vector2(321f / 321f, 1f)); // persegi kanan, kanan bawah



            temp.Add(new Vector2(321f / 321f, 0f)); // persegi kanan, kanan atas
            temp.Add(new Vector2(266f / 321f, 0f)); // persegi kanan, kiri atas
            temp.Add(new Vector2(321f / 321f, 1f)); // persegi kanan, kanan bawah

            temp.Add(new Vector2(109f / 321f, 0f)); // persegi belakang, kiri atas
            temp.Add(new Vector2(157f / 321f, 0f)); // persegi belakang, kanan atas
            temp.Add(new Vector2(157f / 321f, 1f)); // persegi belakang, kanan bawah

            temp.Add(new Vector2(109f / 321f, 1f)); // persegi belakang, kiri bawah
            temp.Add(new Vector2(109f / 321f, 0f)); // persegi belakang, kiri atas
            temp.Add(new Vector2(157f / 321f, 1f)); // persegi belakang, kanan bawah

            temp.Add(new Vector2(55f / 321f, 0f)); // persegi atas, kiri atas
            temp.Add(new Vector2(109f / 321f, 0f)); // persegi atas, kanan atas
            temp.Add(new Vector2(55f / 321f, 1f)); // persegi atas, kiri bawah

            temp.Add(new Vector2(109f / 321f, 0f)); // persegi atas, kanan atas
            temp.Add(new Vector2(109f / 321f, 1f)); // persegi atas, kanan bawah
            temp.Add(new Vector2(55f / 321f, 1f)); // persegi atas, kiri bawah

            temp.Add(new Vector2(0f / 321f, 0f)); // persegi bawah, kiri atas
            temp.Add(new Vector2(55f / 321f, 0f)); // persegi bawah, kanan atas
            temp.Add(new Vector2(0f / 321f, 1f)); // persegi bawah, kiri bawah

            //ini terpaksai pakai texture yg lain. belum diEDIT (SEGITIA 1 BAWAH KIRI)
            temp.Add(new Vector2(0f / 321f, 1f)); // persegi bawah, kiri bawah
            temp.Add(new Vector2(55f / 321f, 0f / 321f)); // persegi bawah, kanan bawah
            temp.Add(new Vector2(55f / 321f, 1f)); // persegi bawah, kiri atas
            return temp;



        }
        public void moveTo(Vector3 position, float yawDirection)
        {
            //legLeft.position = new Vector3(position.X - body.size.X / 2f + (0.55f / 2f) / 2f, position.Y - body.size.Y / 2f - 0.8f / 2f, position.Z);
            //legRight.position = new Vector3(position.X + body.size.X / 2f - (0.55f / 2f) / 2f, position.Y - body.size.Y / 2f - 0.8f / 2f, position.Z);
            //body.position = position;
            //head.position = new Vector3(position.X, position.Y + body.size.Y / 2f + 0.56f / 2f, position.Z);
            //handLeft.position = new Vector3(position.X - body.size.X / 2f - 0.25f / 2f, position.Y, position.Z);
            //handRight.position = new Vector3(position.X + body.size.X / 2f + 0.25f / 2f, position.Y, position.Z);


            legLeft.position = position;
            legRight.position = position;
            body.position = position;
            head.position = position;
            handLeft.position = position;
            handRight.position = position;

            rotation = new Vector3(0, yawDirection, 0);
            //legLeft.rotationByGlobalAxis = new Vector3(0f,yawDirection,0);
            //legRight.rotationByGlobalAxis = new Vector3(0f,yawDirection,0);
            //body.rotationByGlobalAxis = new Vector3(0f,yawDirection,0);
            //head.rotationByGlobalAxis =new Vector3(0f,yawDirection,0);
            //handLeft.rotationByGlobalAxis = new Vector3(0f,yawDirection,0);
            //handRight.rotationByGlobalAxis = new Vector3(0f,yawDirection,0);


        }
        public void AnimateRun()
        {
            animateHands();
            animateLegs();
            body.transform = Matrix4.Identity;
            body.transform *= Matrix4.CreateRotationX(MathHelper.DegreesToRadians(rotation.X));
            body.transform *= Matrix4.CreateRotationY(MathHelper.DegreesToRadians(rotation.Y));
            body.transform *= Matrix4.CreateRotationZ(MathHelper.DegreesToRadians(rotation.Z));
            body.transform *= Matrix4.CreateTranslation(body.position);

            head.transform = Matrix4.Identity;
            head.transform *= Matrix4.CreateTranslation(new Vector3(0f, 0f + body.size.Y / 2f + 0.56f / 2f, 0f));
            head.transform *= Matrix4.CreateRotationX(MathHelper.DegreesToRadians(rotation.X));
            head.transform *= Matrix4.CreateRotationY(MathHelper.DegreesToRadians(rotation.Y));
            head.transform *= Matrix4.CreateRotationZ(MathHelper.DegreesToRadians(rotation.Z));
            head.transform *= Matrix4.CreateTranslation(head.position);
        }
        private void animateLegs()
        {
            animateLeftLeg();
            animateRightLeg();
        }
        private void animateLeftLeg()
        {
            //translate ke 0 0 0
            legLeft.transform = Matrix4.Identity;
            //translate agar di titik 0 0 0 adalah poros dari tangan
            legLeft.transform *= Matrix4.CreateTranslation(new Vector3(0f, -legLeft.size.Y / 2f, 0));
            //rotasi
            legLeft.transform *= Matrix4.CreateRotationX(MathHelper.DegreesToRadians(-degreeHand));
            //translate ke 0 0 0
            legLeft.transform *= Matrix4.CreateTranslation(new Vector3(0f, +legLeft.size.Y / 2f, 0));

            //trans
            legLeft.transform *= Matrix4.CreateTranslation(new Vector3(-body.size.X / 2f + (0.55f / 2f) / 2f, -body.size.Y / 2f - 0.8f / 2f, 0f));
            legLeft.transform *= Matrix4.CreateRotationX(MathHelper.DegreesToRadians(0));
            legLeft.transform *= Matrix4.CreateRotationY(MathHelper.DegreesToRadians(rotation.Y));
            legLeft.transform *= Matrix4.CreateRotationZ(MathHelper.DegreesToRadians(0));

            //translate ke balik ke posisi awal
            legLeft.transform *= Matrix4.CreateTranslation(new Vector3(legLeft.position.X, legLeft.position.Y, legLeft.position.Z));
        }

        private void animateRightLeg()
        {
            //translate ke 0 0 0
            legRight.transform = Matrix4.Identity;
            ////translate agar di titik 0 0 0 adalah poros dari tangan
            legRight.transform *= Matrix4.CreateTranslation(new Vector3(0f, -legRight.size.Y / 2f, 0));
            ////rotasi
            legRight.transform *= Matrix4.CreateRotationX(MathHelper.DegreesToRadians(degreeHand));
            ////translate ke 0 0 0
            legRight.transform *= Matrix4.CreateTranslation(new Vector3(0f, +legRight.size.Y / 2f, 0));

            legRight.transform *= Matrix4.CreateTranslation(new Vector3(body.size.X / 2f - (0.55f / 2f) / 2f, -body.size.Y / 2f - 0.8f / 2f, 0f));
            legRight.transform *= Matrix4.CreateRotationX(MathHelper.DegreesToRadians(0));
            legRight.transform *= Matrix4.CreateRotationY(MathHelper.DegreesToRadians(rotation.Y));
            legRight.transform *= Matrix4.CreateRotationZ(MathHelper.DegreesToRadians(0));

            //translate ke balik ke posisi awal
            legRight.transform *= Matrix4.CreateTranslation(new Vector3(legRight.position.X, legRight.position.Y, legRight.position.Z));
        }
        private void animateHands()
        {
            animateLeftHand();
            animateRightHand();
        }
        bool decreaseDegreeHand;
        float speedHandAnimation = 0.03f;
        float maxOpenDegreeHand = 40f;
        private void animateLeftHand()
        {
            if (degreeHand > maxOpenDegreeHand)
            {
                decreaseDegreeHand = true;
            }
            if (degreeHand < -maxOpenDegreeHand)
            {
                decreaseDegreeHand = false;

            }

            if (decreaseDegreeHand)
                degreeHand -= speedHandAnimation;
            else
                degreeHand += speedHandAnimation;


            //translate ke 0 0 0
            handLeft.transform = Matrix4.Identity;
            //translate agar di titik 0 0 0 adalah poros dari tangan
            handLeft.transform *= Matrix4.CreateTranslation(new Vector3(0f, -handLeft.size.Y / 2f + handLeft.size.Z / 2f, 0));
            //rotasi
            handLeft.transform *= Matrix4.CreateRotationX(MathHelper.DegreesToRadians(degreeHand));
            //translate ke 0 0 0
            handLeft.transform *= Matrix4.CreateTranslation(new Vector3(0f, +handLeft.size.Y / 2f - handLeft.size.Z / 2f, 0));

            handLeft.transform *= Matrix4.CreateTranslation(new Vector3(0f - body.size.X / 2f - 0.25f / 2f, 0f, 0f));
            handLeft.transform *= Matrix4.CreateRotationX(MathHelper.DegreesToRadians(0));
            handLeft.transform *= Matrix4.CreateRotationY(MathHelper.DegreesToRadians(rotation.Y));
            handLeft.transform *= Matrix4.CreateRotationZ(MathHelper.DegreesToRadians(0));

            //translate ke balik ke posisi awal
            handLeft.transform *= Matrix4.CreateTranslation(new Vector3(handLeft.position.X, handLeft.position.Y, handLeft.position.Z));

        }

        private void animateRightHand()
        {

            //translate ke 0 0 0
            handRight.transform = Matrix4.Identity;
            //handRight.transform *= Matrix4.CreateTranslation(new Vector3(-handRight.position.X, -handRight.position.Y, -handRight.position.Z));
            //translate agar di titik 0 0 0 adalah poros dari tangan
            handRight.transform *= Matrix4.CreateTranslation(new Vector3(0f, -handRight.size.Y / 2f + handRight.size.Z / 2f, 0));
            //rotasi
            handRight.transform *= Matrix4.CreateRotationX(MathHelper.DegreesToRadians(-degreeHand));
            //translate ke 0 0 0
            handRight.transform *= Matrix4.CreateTranslation(new Vector3(0f, +handRight.size.Y / 2f - handRight.size.Z / 2f, 0));

            handRight.transform *= Matrix4.CreateTranslation(new Vector3(body.size.X / 2f + 0.25f / 2f, 0, 0));
            handRight.transform *= Matrix4.CreateRotationX(MathHelper.DegreesToRadians(0));
            handRight.transform *= Matrix4.CreateRotationY(MathHelper.DegreesToRadians(rotation.Y));
            handRight.transform *= Matrix4.CreateRotationZ(MathHelper.DegreesToRadians(0));

            //translate ke balik ke posisi awal
            handRight.transform *= Matrix4.CreateTranslation(new Vector3(handRight.position.X, handRight.position.Y, handRight.position.Z));
        }
        private List<Vector2> generateTextureCoordBody()
        {
            List<Vector2> temp = new List<Vector2>();
            temp.Add(new Vector2(82f / 164f, 0f)); // persegi depan, kiri atas
            temp.Add(new Vector2(136f / 164f, 1f)); // persegi depan, kanan bawah
            temp.Add(new Vector2(82f / 164f, 1f)); // persegi depan, kiri bawah

            temp.Add(new Vector2(82f / 164f, 0f)); // persegi depan, kiri atas
            temp.Add(new Vector2(136f / 164f, 0f)); // persegi depan, kanan atas
            temp.Add(new Vector2(136f / 164f, 1f)); // persegi depan, kanan bawah

            temp.Add(new Vector2(54f / 164f, 0f)); // persegi kiri, kiri atas
            temp.Add(new Vector2(82f / 164f, 0f)); // persegi kiri, kanan atas
            temp.Add(new Vector2(54f / 164f, 1f)); // persegi kiri, kiri bawah

            temp.Add(new Vector2(82f / 164f, 0f)); // persegi kiri, kanan atas
            temp.Add(new Vector2(82f / 164f, 1f)); // persegi kiri, kanan bawah
            temp.Add(new Vector2(64f / 164f, 1f)); // persegi kiri, kiri bawah

            temp.Add(new Vector2(136f / 164f, 0f)); // persegi kanan, kiri atas
            temp.Add(new Vector2(136f / 164f, 1f)); // persegi kanan, kiri bawah
            temp.Add(new Vector2(164f / 164f, 1f)); // persegi kanan, kanan bawah

            temp.Add(new Vector2(164f / 164f, 0f)); // persegi kanan, kanan atas
            temp.Add(new Vector2(156f / 164f, 0f)); // persegi kanan, kiri atas
            temp.Add(new Vector2(164f / 164f, 1f)); // persegi kanan, kanan bawah

            temp.Add(new Vector2(0f / 164f, 0f)); // persegi belakang, kiri atas
            temp.Add(new Vector2(54f / 164f, 0f)); // persegi belakang, kanan atas
            temp.Add(new Vector2(54f / 164f, 1f)); // persegi belakang, kiri bawah

            temp.Add(new Vector2(0f / 164f, 1f)); // persegi belakang, kiri bawah
            temp.Add(new Vector2(0f / 164f, 0f)); // persegi belakang, kanan atas
            temp.Add(new Vector2(54f / 164f, 1f)); // persegi belakang, kanan bawah

            //ini terpaksai pakai texture yg lain.
            temp.Add(new Vector2(0f / 164f, 0f)); // persegi atas, kiri atas
            temp.Add(new Vector2(64f / 164f, 0f)); // persegi atas, kanan atas
            temp.Add(new Vector2(0 / 164f, 28f / 164f)); // persegi atas, kiri bawah

            //ini terpaksai pakai texture yg lain.
            temp.Add(new Vector2(55f / 164f, 0f)); // persegi atas, kanan atas
            temp.Add(new Vector2(55f / 164f, 28f / 164f)); // persegi atas, kanan bawah
            temp.Add(new Vector2(0 / 164f, 28f / 164f)); // persegi atas, kiri bawah

            //ini terpaksai pakai texture yg lain.
            temp.Add(new Vector2(55f / 164f, 0f)); // persegi atas, kanan atas
            temp.Add(new Vector2(55f / 164f, 28f / 164f)); // persegi atas, kanan bawah
            temp.Add(new Vector2(0 / 164f, 28f / 164f)); // persegi atas, kiri bawah

            //ini terpaksai pakai texture yg lain. belum diEDIT (SEGITIA 1 BAWAH KIRI)
            temp.Add(new Vector2(0f / 164f, 0f)); // persegi bawah, kiri bawah
            temp.Add(new Vector2(54f / 164f, 0f / 164f)); // persegi bawah, kanan bawah
            temp.Add(new Vector2(0 / 164f, 28f / 164f)); // persegi bawah, kiri atas
            return temp;

            temp.Add(new Vector2(0f / 164f, 28f / 165f)); // persegi bawah, kiri bawah
            temp.Add(new Vector2(64f / 164f, 0f / 164f)); // persegi bawah, kanan bawah
            temp.Add(new Vector2(64f / 164f, 28f / 164f)); // persegi bawah, kiri atas



        }

        private List<Vector2> generateTextureCoordLeftHand()
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

            temp.Add(new Vector2(28f / 137f, 54f / 82f)); // persegi atas, kiri atas
            temp.Add(new Vector2(0f / 137f, 54f / 82f)); // persegi atas, kanan atas
            temp.Add(new Vector2(28f / 137f, 1f)); // persegi atas, kiri bawah

            temp.Add(new Vector2(0f / 137f, 54f / 82f)); // persegi atas, kanan atas
            temp.Add(new Vector2(28f / 137f, 1f)); // persegi atas, kanan bawah
            temp.Add(new Vector2(28f / 137f, 1f)); // persegi atas, kiri bawah

            temp.Add(new Vector2(0f / 137f, 26f / 82f)); // persegi bawah, kiri atas
            temp.Add(new Vector2(28f / 137f, 26f / 82f)); // persegi bawah, kanan atas
            temp.Add(new Vector2(0f / 137f, 54f / 82f)); // persegi bawah, kiri bawah

            //ini terpaksai pakai texture yg lain. belum diEDIT (SEGITIA 1 BAWAH KIRI)
            temp.Add(new Vector2(0f / 137f, 54f / 82f)); // persegi bawah, kiri bawah
            temp.Add(new Vector2(28f / 137f, 54f / 82f)); // persegi bawah, kanan bawah
            temp.Add(new Vector2(28f / 137f, 26f / 82f)); // persegi bawah, kiri atas
            return temp;



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

        private List<Vector2> generateTextureCoordLeftLeg()
        {
            List<Vector2> temp = new List<Vector2>();
            temp.Add(new Vector2(82f / 137f, 0f)); // persegi depan, kiri atas
            temp.Add(new Vector2(109f / 137f, 1f)); // persegi depan, kanan bawah
            temp.Add(new Vector2(82f / 137f, 1f)); // persegi depan, kiri bawah

            temp.Add(new Vector2(82f / 137f, 0f)); // persegi depan, kiri atas
            temp.Add(new Vector2(109f / 137f, 0f)); // persegi depan, kanan atas
            temp.Add(new Vector2(109f / 137f, 1f)); // persegi depan, kanan bawah

            temp.Add(new Vector2(55f / 137f, 0f)); // persegi kiri, kiri atas
            temp.Add(new Vector2(82f / 137f, 0f)); // persegi kiri, kanan atas
            temp.Add(new Vector2(55f / 137f, 1f)); // persegi kiri, kiri bawah

            temp.Add(new Vector2(82f / 137f, 0f)); // persegi kiri, kanan atas
            temp.Add(new Vector2(82f / 137f, 1f)); // persegi kiri, kanan bawah
            temp.Add(new Vector2(55f / 137f, 1f)); // persegi kiri, kiri bawah

            temp.Add(new Vector2(82f / 137f, 0f)); // persegi kanan, kiri atas
            temp.Add(new Vector2(82f / 137f, 1f)); // persegi kanan, kiri bawah
            temp.Add(new Vector2(55f / 137f, 1f)); // persegi kanan, kanan bawah

            temp.Add(new Vector2(55f / 137f, 0f)); // persegi kanan, kanan atas
            temp.Add(new Vector2(82f / 137f, 0f)); // persegi kanan, kiri atas
            temp.Add(new Vector2(55f / 137f, 1f)); // persegi kanan, kanan bawah

            temp.Add(new Vector2(55f / 137f, 0f)); // persegi belakang, kiri atas
            temp.Add(new Vector2(28f / 137f, 0f)); // persegi belakang, kanan atas
            temp.Add(new Vector2(28f / 137f, 1f)); // persegi belakang, kanan bawah

            temp.Add(new Vector2(55f / 137f, 1f)); // persegi belakang, kiri bawah
            temp.Add(new Vector2(55f / 137f, 0f)); // persegi belakang, kiri atas
            temp.Add(new Vector2(28f / 137f, 1f)); // persegi belakang, kanan bawah

            temp.Add(new Vector2(28f / 137f, 54f / 137f)); // persegi atas, kiri atas
            temp.Add(new Vector2(0f / 137f, 54f / 137f)); // persegi atas, kanan atas
            temp.Add(new Vector2(29f / 137f, 1f)); // persegi atas, kiri bawah

            temp.Add(new Vector2(28f / 137f, 54f / 81f)); // persegi atas, kanan atas
            temp.Add(new Vector2(28f / 137f, 1f)); // persegi atas, kanan bawah
            temp.Add(new Vector2(0f / 137f, 1f)); // persegi atas, kiri bawah

            temp.Add(new Vector2(0f / 137f, 27f / 81f)); // persegi bawah, kiri atas
            temp.Add(new Vector2(28f / 137f, 27f / 81f)); // persegi bawah, kanan atas
            temp.Add(new Vector2(0f / 137f, 54f / 81f)); // persegi bawah, kiri bawah


            temp.Add(new Vector2(0f / 137f, 54f / 81f)); // persegi bawah, kiri bawah
            temp.Add(new Vector2(28f / 137f, 54f / 81f)); // persegi bawah, kanan bawah
            temp.Add(new Vector2(0f / 137f, 27f / 81f)); // persegi bawah, kiri atas
            return temp;

        }

        private List<Vector2> generateTextureCoordRightLeg()
        {
            List<Vector2> temp = new List<Vector2>();
            temp.Add(new Vector2(82f / 137f, 0f)); // persegi depan, kiri atas
            temp.Add(new Vector2(109f / 137f, 1f)); // persegi depan, kanan bawah
            temp.Add(new Vector2(82f / 137f, 1f)); // persegi depan, kiri bawah

            temp.Add(new Vector2(82f / 137f, 0f)); // persegi depan, kiri atas
            temp.Add(new Vector2(109f / 137f, 0f)); // persegi depan, kanan atas
            temp.Add(new Vector2(109f / 137f, 1f)); // persegi depan, kanan bawah

            temp.Add(new Vector2(137f / 137f, 0f)); // persegi kiri, kiri atas
            temp.Add(new Vector2(109f / 137f, 0f)); // persegi kiri, kanan atas
            temp.Add(new Vector2(137f / 137f, 1f)); // persegi kiri, kiri bawah

            temp.Add(new Vector2(109f / 137f, 0f)); // persegi kiri, kanan atas
            temp.Add(new Vector2(109f / 137f, 1f)); // persegi kiri, kanan bawah
            temp.Add(new Vector2(137f / 137f, 1f)); // persegi kiri, kiri bawah

            temp.Add(new Vector2(109f / 137f, 0f)); // persegi kanan, kiri atas
            temp.Add(new Vector2(109f / 137f, 1f)); // persegi kanan, kiri bawah
            temp.Add(new Vector2(137f / 137f, 1f)); // persegi kanan, kanan bawah

            temp.Add(new Vector2(137f / 137f, 0f)); // persegi kanan, kanan atas
            temp.Add(new Vector2(109f / 137f, 0f)); // persegi kanan, kiri atas
            temp.Add(new Vector2(137f / 137f, 1f)); // persegi kanan, kanan bawah

            temp.Add(new Vector2(55f / 137f, 0f)); // persegi belakang, kiri atas
            temp.Add(new Vector2(28f / 137f, 0f)); // persegi belakang, kanan atas
            temp.Add(new Vector2(28f / 137f, 1f)); // persegi belakang, kanan bawah

            temp.Add(new Vector2(55f / 137f, 1f)); // persegi belakang, kiri bawah
            temp.Add(new Vector2(55f / 137f, 0f)); // persegi belakang, kiri atas
            temp.Add(new Vector2(28f / 137f, 1f)); // persegi belakang, kanan bawah

            temp.Add(new Vector2(28f / 137f, 54f / 137f)); // persegi atas, kiri atas
            temp.Add(new Vector2(0f / 137f, 54f / 137f)); // persegi atas, kanan atas
            temp.Add(new Vector2(29f / 137f, 1f)); // persegi atas, kiri bawah

            temp.Add(new Vector2(28f / 137f, 54f / 81f)); // persegi atas, kanan atas
            temp.Add(new Vector2(28f / 137f, 1f)); // persegi atas, kanan bawah
            temp.Add(new Vector2(0f / 137f, 1f)); // persegi atas, kiri bawah

            temp.Add(new Vector2(0f / 137f, 27f / 81f)); // persegi bawah, kiri atas
            temp.Add(new Vector2(28f / 137f, 27f / 81f)); // persegi bawah, kanan atas
            temp.Add(new Vector2(0f / 137f, 54f / 81f)); // persegi bawah, kiri bawah


            temp.Add(new Vector2(0f / 137f, 54f / 81f)); // persegi bawah, kiri bawah
            temp.Add(new Vector2(28f / 137f, 54f / 81f)); // persegi bawah, kanan bawah
            temp.Add(new Vector2(0f / 137f, 27f / 81f)); // persegi bawah, kiri atas
            return temp;

        }
        public void renderDepth(Camera camera, ref ShaderGeom shader, List<Matrix4> LightSpaceMatrix, int i)
        {


            body.renderDepth(camera, ref shader, LightSpaceMatrix, i);
            head.renderDepth(camera, ref shader,LightSpaceMatrix, i);
            handLeft.renderDepth(camera, ref shader,LightSpaceMatrix, i);
            handRight.renderDepth(camera, ref shader,LightSpaceMatrix, i);
            legLeft.renderDepth(camera, ref shader,LightSpaceMatrix, i);
            legRight.renderDepth(camera, ref shader,LightSpaceMatrix, i);
        }
        public void render(Camera camera, string type = null)
        {

            body.render(camera, type);
            head.render(camera, type);
            handLeft.render(camera, type);
            handRight.render(camera, type);
            legLeft.render(camera, type);
            legRight.render(camera, type);
        }
        public void renderDepthMap()
        {

            //body.renderDepthMap();
            //head.renderDepthMap();
            //handLeft.renderDepthMap();
            //handRight.renderDepthMap();
            //legLeft.renderDepthMap();
            //legRight.renderDepthMap();
        }
    }
}
