#version 330 core
layout (triangles) in;
layout (triangle_strip, max_vertices=18) out;


uniform mat4 shadowMatrices0;
uniform mat4 shadowMatrices1;
uniform mat4 shadowMatrices2;
uniform mat4 shadowMatrices3;
uniform mat4 shadowMatrices4;
uniform mat4 shadowMatrices5;

out vec4 FragPos; // FragPos from GS (output per emitvertex)

void main()
{
    mat4 shadowMatrices[6];
    shadowMatrices[0] = shadowMatrices0;
    shadowMatrices[1] = shadowMatrices1;
    shadowMatrices[2] = shadowMatrices2;
    shadowMatrices[3] = shadowMatrices3;
    shadowMatrices[4] = shadowMatrices4;
    shadowMatrices[5] = shadowMatrices5;
    for(int face = 0; face < 6; ++face)
    {
        gl_Layer = face; // built-in variable that specifies to which face we render.
        for(int i = 0; i < 3; ++i) // for each triangle's vertices
        {
            FragPos = gl_in[i].gl_Position;
            gl_Position =  FragPos *shadowMatrices[face];
            EmitVertex();
        }    
        EndPrimitive();
    }
} 