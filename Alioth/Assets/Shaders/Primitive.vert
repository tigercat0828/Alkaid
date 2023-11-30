#version 460 core
layout (location = 0) in vec3 aPosition;

uniform mat4 uModelMat;
uniform mat4 uViewMat;
uniform mat4 uProjectionMat;

void main(){

	gl_Position =  uProjectionMat * uViewMat * uModelMat * vec4(aPosition, 1.0);
}