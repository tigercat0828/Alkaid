#version 460 core
layout (location = 0) in vec3 aPosition;
layout (location = 1) in vec3 aColor;

out vec3 fColor;

uniform mat4 uModelMat;
uniform mat4 uViewMat;
uniform mat4 uProjectionMat;

void main(){

	fColor = aColor;
	gl_Position =  uProjectionMat * uViewMat * uModelMat * vec4(aPosition, 1.0);
}