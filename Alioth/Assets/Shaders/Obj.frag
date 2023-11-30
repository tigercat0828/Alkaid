#version 460 core

in vec2 fTexCoord;

uniform sampler2D texture0;
uniform sampler2D texture1;


out vec4 FragColor;

void main(){
	//FragColor = texture(texture0, fTexCoord);
	FragColor = mix(
		texture(texture0, fTexCoord),
		texture(texture1, fTexCoord),
		0.2f
	);
}