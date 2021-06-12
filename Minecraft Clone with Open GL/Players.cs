using LearnOpenTK.Common;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Minecraft_Clone_with_Open_GL
{
    struct Network
    {
        public string ID; //unique ID agar server mengetahui identitas client
    }
    class Players
    {
        public Network network;
        public bool isMove = false;
        public Vector3 position; //posisi ini akan di set oleh incoming UDP message.
        public float yawDirection = -90f;
        public bool isUsed { get; private set; } //artinya apakah objec-t ini sudah dipakai/diisi oleh sebuah player
        Steve.Steve steve;
        public Players()
        {
            isUsed = false;
            steve = new Steve.Steve(new Vector3(0,1.4f, 0));
        }
        public void render(Camera camera, string type = null)
        {
            steve.render(camera, type);
        }
        public void renderDepth(Camera camera, ref ShaderGeom shader, List<Matrix4> LightSpaceMatrix, int i)
        {
            steve.renderDepth(camera,ref shader, LightSpaceMatrix,i);

        }
        public void Update()
        {
            // di + 180f karena inilah direction steve yang cocok dengan direction camera musuh (coba coba)
            //Console.WriteLine($"yaw direction  {yawDirection - 90f}");
            Console.WriteLine($"position  {position }");
            steve.moveTo(position, -(yawDirection - 90f)); //posisi ini diupdate dari udp.
            steve.AnimateRun();
        }
    }
}
