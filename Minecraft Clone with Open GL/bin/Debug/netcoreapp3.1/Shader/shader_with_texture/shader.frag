#version 330

out vec4 outputColor;

in vec2 aTexCoords;

uniform sampler2D texture0;

void main()
{
    outputColor = texture(texture0, aTexCoords);
    //outputColor = vec4(0.3,0.4,0.1,1.0);
}