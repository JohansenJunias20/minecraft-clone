#version 330 core
out vec4 FragColor;
#define MAX_DIRECTIONAL_LIGHT 3
#define MAX_POINT_LIGHT 21
#extension GL_ARB_bindless_texture:require

struct Material{
    sampler2D ambient;//======
    sampler2D diffuse;//======
    sampler2D specular;//======
    float shininess;//======
};
struct DirLight{//sumber cahaya
    vec3 direction;//======
    sampler2D shadowMap;//======
    mat4 lightSpaceMatrix;//======
    vec3 ambient;//======
    vec3 diffuse;//======
    vec3 specular;//======
};
struct PointLight{//sumber cahaya
    vec3 position;
    
    float constant;
    float linear;
    float quadric;
    
    vec3 ambient;
    vec3 diffuse;
    vec3 specular;
    
    //shadow purposes
};

// uniform float shininess;
uniform DirLight dirLight[MAX_DIRECTIONAL_LIGHT];
uniform int dirLightCount;//======
uniform int pointLightCount;//======

// uniform float far_plane;
// uniform samplerCube depthMapPointLight[MAX_POINT_LIGHT];
uniform samplerCube depthMapPointLight0;
uniform samplerCube depthMapPointLight1;
uniform samplerCube depthMapPointLight2;
uniform samplerCube depthMapPointLight3;
uniform samplerCube depthMapPointLight4;
uniform samplerCube depthMapPointLight5;
uniform samplerCube depthMapPointLight6;
uniform samplerCube depthMapPointLight7;
uniform samplerCube depthMapPointLight8;
uniform samplerCube depthMapPointLight9;
uniform samplerCube depthMapPointLight10;
uniform samplerCube depthMapPointLight11;
uniform samplerCube depthMapPointLight12;
uniform samplerCube depthMapPointLight13;
uniform samplerCube depthMapPointLight14;
uniform samplerCube depthMapPointLight15;
uniform samplerCube depthMapPointLight16;
uniform samplerCube depthMapPointLight17;
uniform samplerCube depthMapPointLight18;
uniform samplerCube depthMapPointLight19;
uniform samplerCube depthMapPointLight20;

uniform PointLight pointLight[MAX_POINT_LIGHT];
uniform float far_plane=10;

uniform Material material;
uniform vec3 viewPos;//======

//from vert file.
in vec3 Normal;
in vec3 FragPos;
in vec2 TexCoords;
//in vec4 FragPosLightSpace; //position fragment terhadap directional light

float ShadowCalc(float dotLightNormal,mat4 lightSpaceMatrix,sampler2D shadowMap)
{
    vec4 FragPosLightSpace=vec4(FragPos,1.)*lightSpaceMatrix;
    vec3 pos=FragPosLightSpace.xyz*.5+.5;
    if(pos.z>1.){
        pos.z=1.;
    }
    float depth=texture(shadowMap,pos.xy).r;
    float bias=max(.05*(1.-dotLightNormal),.005);
    return(depth+bias)<pos.z?0.:1.;
    //return depth < pos.z ? 0.0 : 1.0;
}
// float ShadowCalcPointLight(vec3 fragPos,inout samplerCube depthMapPointLight)
// {
    //     vec3 fragToLight=fragPos-pointLight[0].position;
    //     float closestDepth=texture(depthMapPointLight,fragToLight).r;
    //     closestDepth*=far_plane;
    //     float currentDepth=length(fragToLight);
    //     float bias=.5;// we use a much larger bias since depth is now in [near_plane, far_plane] range
    //     float shadow=currentDepth-bias>closestDepth?1.:0.;
    
    //     return shadow;
// }
vec3 calculateDirLight(int index){
    //ambient
    vec3 ambient=dirLight[index].ambient*vec3(texture(material.ambient,TexCoords));
    
    //diffuse
    vec3 norm=normalize(Normal);
    vec3 lightDir=normalize(-dirLight[index].direction);
    float diff=max(dot(norm,lightDir),0.);//We make sure the value is non negative with the max function.
    vec3 diffuse=dirLight[index].diffuse*diff*vec3(texture(material.diffuse,TexCoords));
    
    //specular
    vec3 viewDir=normalize(viewPos-FragPos);
    vec3 reflectDir=reflect(-lightDir,norm);
    float spec=pow(max(dot(viewDir,reflectDir),0.),material.shininess);
    vec3 specular=dirLight[index].specular*spec*vec3(texture(material.specular,TexCoords));
    
    //shadow
    float dotLightNormal=dot(dirLight[index].direction,Normal);
    float shadow=ShadowCalc(dotLightNormal,dirLight[index].lightSpaceMatrix,dirLight[index].shadowMap);
    
    return ambient+(shadow*(diffuse+specular));
    
}

// vec3 calculatePointLight(int index,inout samplerCube depthMapPointLight){
    
    //     vec3 normal=normalize(Normal);
    
    //     vec3 ambient=pointLight[index].ambient*vec3(texture(material.ambient,TexCoords));
    
    //     // diffuse
    //     vec3 lightDir=normalize(pointLight[index].position-FragPos);
    //     float diff=max(dot(lightDir,normal),0.);
    //     vec3 diffuse=pointLight[index].diffuse*diff*vec3(texture(material.diffuse,TexCoords));//dibalik sama saja
    
    //     // specular
    //     vec3 viewDir=normalize(viewPos-FragPos);
    //     vec3 reflectDir=reflect(-lightDir,normal);
    //     float spec=0.;
    //     vec3 halfwayDir=normalize(lightDir+viewDir);
    //     spec=pow(max(dot(normal,halfwayDir),0.),material.shininess);
    //     vec3 specular=pointLight[index].specular*spec*vec3(texture(material.specular,TexCoords));
    
    //     // calculate shadow
    //     float shadow=ShadowCalcPointLight(FragPos,depthMapPointLight);
    
    //     float distance=length(pointLight[index].position-FragPos);
    //     float attenuation=1./(1.+.14*distance+.07*(distance*distance));
    //     ambient*=attenuation;
    //     diffuse*=attenuation;
    //     specular*=attenuation;
    
    //     vec3 lighting=(ambient+(1.-shadow)*(diffuse+specular));
    //     return lighting;
// }

void main()
{
    samplerCube depthMapPointLights[MAX_POINT_LIGHT];
    depthMapPointLights[0]=depthMapPointLight0;
    depthMapPointLights[1]=depthMapPointLight1;
    // depthMapPointLights[2]=depthMapPointLight2;
    // depthMapPointLights[3]=depthMapPointLight3;
    // depthMapPointLights[4]=depthMapPointLight4;
    // depthMapPointLights[5]=depthMapPointLight5;
    // depthMapPointLights[6]=depthMapPointLight6;
    // depthMapPointLights[7]=depthMapPointLight7;
    // depthMapPointLights[8]=depthMapPointLight8;
    // depthMapPointLights[9]=depthMapPointLight9;
    // depthMapPointLights[10]=depthMapPointLight10;
    // depthMapPointLights[11]=depthMapPointLight11;
    // depthMapPointLights[12]=depthMapPointLight12;
    // depthMapPointLights[13]=depthMapPointLight13;
    // depthMapPointLights[14]=depthMapPointLight14;
    // depthMapPointLights[15]=depthMapPointLight15;
    // depthMapPointLights[16]=depthMapPointLight16;
    // depthMapPointLights[17]=depthMapPointLight17;
    // depthMapPointLights[18]=depthMapPointLight18;
    // depthMapPointLights[19]=depthMapPointLight19;
    // depthMapPointLights[20]=depthMapPointLight20;
    // for(int i=0;i<MAX_POINT_LIGHT;i++){
        //     depthMapPointLight[i]=0;
    // }
    vec3 result;
    
    // for(int i=0;i<dirLightCount;i++){
        //     if(i==0){
            //         result=calculateDirLight(i);
        //     }
        //     else{
            //         result+=calculateDirLight(i);
            
        //     }
    // }
    
    for(int i=0;i<pointLightCount;i++){
        // if(i==0){}
        // result=calculatePointLight(i,depthMapPointLights[i]);
        // else
        
        vec3 normal=normalize(Normal);
        
        vec3 ambient=pointLight[i].ambient*vec3(texture(material.ambient,TexCoords));
        
        // diffuse
        vec3 lightDir=normalize(pointLight[i].position-FragPos);
        float diff=max(dot(lightDir,normal),0.);
        vec3 diffuse=pointLight[i].diffuse*diff*vec3(texture(material.diffuse,TexCoords));//dibalik sama saja
        
        // specular
        vec3 viewDir=normalize(viewPos-FragPos);
        vec3 reflectDir=reflect(-lightDir,normal);
        float spec=0.;
        vec3 halfwayDir=normalize(lightDir+viewDir);
        spec=pow(max(dot(normal,halfwayDir),0.),material.shininess);
        vec3 specular=pointLight[i].specular*spec*vec3(texture(material.specular,TexCoords));
        
        // calculate shadow
        // float shadow=ShadowCalcPointLight(FragPos,depthMapPointLight);
        vec3 fragToLight=FragPos-pointLight[i].position;
        float closestDepth=texture(depthMapPointLights[i],fragToLight).r;
        closestDepth*=far_plane;
        float currentDepth=length(fragToLight);
        float bias=.1;// we use a much larger bias since depth is now in [near_plane, far_plane] range
        float shadow=currentDepth-bias>closestDepth?1.:0.;
        
        float distance=length(pointLight[i].position-FragPos);
        float attenuation=
        1./(1.+pointLight[i].linear*distance+
            pointLight[i].quadric*(distance*distance));
            
            // float attenuation=1./(pointLight[i].linear+pointLight[i].quadric*distance+.07*(distance*distance));
            ambient*=attenuation;
            diffuse*=attenuation;
            specular*=attenuation;
            if(i==0){
                result=(ambient+(1.-shadow*0)*(diffuse+specular));
                
            }
            else{
                result+=(ambient+(1.-shadow*0)*(diffuse+specular));
                
            }
            
            // result+=calculatePointLight(i,depthMapPointLights[i]);
        }
        // result=calculatePointLight(pointLight[0],0);
        
        // result=calculateDirLight(0);
        // result+=vec3(0.4,0.5,0.6);
        FragColor=vec4(result,1.);
        // FragColor=vec4(0.4,0.5,0.6,1.);
    }
    