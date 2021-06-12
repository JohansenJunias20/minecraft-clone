#version 330 core
out vec4 FragColor;
#define MAX_DIRECTIONAL_LIGHT 3
float ShadowCalc(float dotLightNormal, mat4 lightSpaceMatrix)
{
    vec4 FragPosLightSpace =  vec4(FragPos, 1.0) * lightSpaceMatrix;
    vec3 pos=FragPosLightSpace.xyz*.5+.5;
    if(pos.z>1.){
        pos.z=1.;
    }
    float depth=texture(shadowMap,pos.xy).r;
    float bias=max(.05*(1.-dotLightNormal),.005);
    return(depth+bias)<pos.z?0.:1.;
    //return depth < pos.z ? 0.0 : 1.0;
}

struct Material{
    sampler2D ambient;
    sampler2D diffuse;
    sampler2D specular;
    float shininess;//Shininess is the power the specular light is raised to
};
struct DirLight{//sumber cahaya
    vec3 direction;
    sampler2D shadowMap;
    mat4 lightSpaceMatrix;
    vec3 ambient;
    vec3 diffuse;
    vec3 specular;
};

uniform DirLight dirLight[MAX_DIRECTIONAL_LIGHT];
uniform int dirLightCount;
uniform Material material;

uniform vec3 viewPos;

//from vert file.
in vec3 Normal; asd
in vec3 FragPos;
in vec2 TexCoords;
in vec4 FragPosLightSpace; //position fragment terhadap directional light
void main()
{
    vec3 result;
    for(int i=0;i<dirLightCount;i++){
        if(i==0)
        result=calculateDirLight(dirLight[i]);
        else
        result+=calculateDirLight(dirLight[i]);
    }
    
    FragColor=vec4(result,1.);
}

vec3 calculateDirLight(DirLight dirLight){
    //ambient
    vec3 ambient=dirLight.ambient*vec3(texture(material.ambient,TexCoords));
    
    //diffuse
    vec3 norm=normalize(Normal);
    vec3 lightDir=normalize(-dirLight.direction);
    float diff=max(dot(norm,lightDir),0.);//We make sure the value is non negative with the max function.
    vec3 diffuse=dirLight.diffuse*diff*vec3(texture(material.diffuse,TexCoords));
    
    //specular
    float specularStrength=.5;
    vec3 viewDir=normalize(viewPos-FragPos);
    vec3 reflectDir=reflect(-lightDir,norm);
    float spec=pow(max(dot(viewDir,reflectDir),0.),material.shininess);
    vec3 specular=dirLight.specular*spec*vec3(texture(material.specular,TexCoords));
    
    //shadow
    float dotLightNormal=dot(dirLight.direction,Normal);
    float shadow=ShadowCalc(dotLightNormal,dirLight.lightSpaceMatrix);
    
    return ambient+(shadow*(diffuse+specular));
    
}