#version 330 core
out vec4 FragColor;
#define MAX_DIRECTIONAL_LIGHT 3
#define MAX_POINT_LIGHT 6

struct Material{
    sampler2D ambient;
    sampler2D diffuse;
    sampler2D specular;
    float shininess;
};
struct DirLight{//sumber cahaya
    vec3 direction;
    sampler2D shadowMap;
    mat4 lightSpaceMatrix;
    vec3 ambient;
    vec3 diffuse;
    vec3 specular;
};
struct PointLight{//sumber cahaya
    vec3 position;
    
    float constant;
    float linear;
    float quadratic;
    
    vec3 ambient;
    vec3 diffuse;
    vec3 specular;
    
    //shadow purposes
    samplerCube depthMap;
    float far_plane;
};

// uniform float shininess;
uniform DirLight dirLight[MAX_DIRECTIONAL_LIGHT];
uniform int dirLightCount;
uniform PointLight pointLight[MAX_POINT_LIGHT];
uniform int pointLightCount;

uniform Material material;
uniform vec3 viewPos;

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
float ShadowPointCalc(PointLight _pointLight){
    // get vector between fragment position and light position
    vec3 fragToLight=FragPos-_pointLight.position;
    // ise the fragment to light vector to sample from the depth map
    float closestDepth=texture(_pointLight.depthMap,fragToLight).r;
    // it is currently in linear range between [0,1], let's re-transform it back to original depth value
    closestDepth*=_pointLight.far_plane;
    // now get current linear depth as the length between the fragment and light position
    float currentDepth=length(fragToLight);
    // test for shadows
    float bias=.05;// we use a much larger bias since depth is now in [near_plane, far_plane] range
    float shadow=currentDepth-bias>closestDepth?1.:0.;
    // display closestDepth as debug (to visualize depth cubemap)
    // FragColor = vec4(vec3(closestDepth / far_plane), 1.0);
    
    return shadow;
    
}
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

vec3 calculatePointLight(PointLight _pointLight){
    vec3 normal=normalize(Normal);
    vec3 viewDir=normalize(viewPos-FragPos);
    vec3 lightDir=normalize(_pointLight.position-FragPos);
    // diffuse shading
    float diff=max(dot(normal,lightDir),0.);
    // specular shading
    vec3 reflectDir=reflect(-lightDir,normal);
    vec3 halfwayDir=normalize(lightDir+viewDir);
    float spec=pow(max(dot(normal,halfwayDir),0.),material.shininess);
    // attenuation
    float distance=length(_pointLight.position-FragPos);
    float attenuation=1./(_pointLight.constant+_pointLight.linear*distance+_pointLight.quadratic*(distance*distance));
    // combine results
    vec3 ambient=_pointLight.ambient*vec3(texture(material.diffuse,TexCoords));
    vec3 diffuse=_pointLight.diffuse*diff*vec3(texture(material.diffuse,TexCoords));
    vec3 specular=_pointLight.specular*spec*vec3(texture(material.specular,TexCoords));
    // float shadow=ShadowCalculation(fs_in.FragPos);
    ambient*=attenuation;
    diffuse*=attenuation;
    specular*=attenuation;
    
    float shadow=ShadowPointCalc(_pointLight);
    return ambient+(shadow*(diffuse+specular));
    
}

void main()
{
    vec3 result;
    for(int i=0;i<dirLightCount;i++){
    // result=calculateDirLight(0);
    }
    
    // for(int i=0;i<dirLightCount;i++){
        //     if(i==0){
            //         result=calculateDirLight(dirLight[i]);
        //     }
        //     else{
            //         result+=calculateDirLight(dirLight[i]);
            
        //     }
    // }
     
    int index = dirLightCount - 1;
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
    
    for(int i=0;i<pointLightCount;i++){
        
        result+=calculatePointLight(pointLight[i]);
    }
    result = ambient+(shadow*(diffuse+specular));
    FragColor=vec4(result,1.);
}

