#version 330
in vec2 TexCoords;//debug purpose
uniform sampler2D depthMap;//debug purpose
out vec4 FragColor;

void main()
{
	float depthValue = texture(depthMap, TexCoords).r; //debug purpose
	FragColor = vec4(vec3(depthValue),1.0);
}