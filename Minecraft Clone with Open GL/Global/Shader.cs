using System;
using System.Collections.Generic;
using System.Text;

namespace Minecraft_Clone_with_Open_GL.Global
{
    class Shader
    {
        public static LearnOpenTK.Common.Shader shaderWithoutTexture = new LearnOpenTK.Common.Shader(@$"{Program.__DIR__}\Shader\shader_normal\shader.vert",
            @$"{Program.__DIR__}\Shader\shader_normal\shader.frag");
    }
}
