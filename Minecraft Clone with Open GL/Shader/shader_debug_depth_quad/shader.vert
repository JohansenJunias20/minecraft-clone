#version 330 core

layout(location = 0) in vec3 aPosition;
layout(location = 2) in vec2 aTexCoord;
//uniform vec4 transform;
out vec2 TexCoords;
void main(void)
{
    TexCoords = aTexCoord;
    //gl_Position = vec4(aPosition, 1.0) * transform;
    gl_Position = vec4(aPosition, 1.0) ;
}