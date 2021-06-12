#version 330 core

layout(location = 0) in vec3 aPosition;
layout (location = 1) in vec3 aNormal;
layout(location = 2) in vec2 aTexCoord;


uniform mat4 transform;
uniform mat4 projection;

out vec3 Normal;
out vec3 FragPos;
out vec2 objectColor;
void main(void)
{
    objectColor = aTexCoord;
    gl_Position = vec4(aPosition, 1.0) * transform * projection;
    FragPos = vec3(vec4(aPosition, 1.0) * transform);
    Normal = aNormal * mat3(transpose(inverse(transform)));
}