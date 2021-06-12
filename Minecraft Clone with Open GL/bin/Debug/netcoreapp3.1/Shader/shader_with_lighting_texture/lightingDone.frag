#version 330 core
out vec4 FragColor;

in vec3 Normal;
in vec3 FragPos;
in vec2 TexCoords;

struct Material{
    sampler2D ambient;//======
    sampler2D diffuse;//======
    sampler2D specular;//======
    float shininess;//======
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
    // float far_plane;
};

uniform samplerCube depthMapPointLight[1];
uniform PointLight pointLight[1];
uniform vec3 viewPos;
uniform Material material;

uniform float far_plane = 10;

float ShadowCalculation(vec3 fragPos)
{
    vec3 fragToLight=fragPos-pointLight[0].position;
    float closestDepth=texture(depthMapPointLight[0],fragToLight).r;
    closestDepth*=far_plane;
    float currentDepth=length(fragToLight);
    float bias=.5;// we use a much larger bias since depth is now in [near_plane, far_plane] range
    float shadow=currentDepth-bias>closestDepth?1.:0.;
    
    return shadow;
}

void main()
{
    vec3 normal=normalize(Normal);
    
    vec3 ambient=pointLight[0].ambient*vec3(texture(material.ambient,TexCoords));
    
    // diffuse
    vec3 lightDir=normalize(pointLight[0].position-FragPos);
    float diff=max(dot(lightDir,normal),0.);
    vec3 diffuse=pointLight[0].diffuse*diff*vec3(texture(material.diffuse,TexCoords));//dibalik sama saja
    
    // specular
    vec3 viewDir=normalize(viewPos-FragPos);
    vec3 reflectDir=reflect(-lightDir,normal);
    float spec=0.;
    vec3 halfwayDir=normalize(lightDir+viewDir);
    spec=pow(max(dot(normal,halfwayDir),0.),material.shininess);
    vec3 specular=pointLight[0].specular*spec*vec3(texture(material.specular,TexCoords));
    
    // calculate shadow
    float shadow=ShadowCalculation(FragPos);
    
    float distance=length(pointLight[0].position-FragPos);
    float attenuation=1./(1.+.14*distance+.07*(distance*distance));
    ambient*=attenuation;
    diffuse*=attenuation;
    specular*=attenuation;
    
    vec3 lighting=(ambient+(1.-shadow)*(diffuse+specular));
    
    FragColor=vec4(lighting,1.);
}