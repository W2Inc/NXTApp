attribute vec3 size;
attribute vec3 color;

varying float vSize;
varying vec3 vColor;

void main() {
	vColor = color;
	vSize = size.x;
	gl_PointSize = vSize;
	gl_Position = projectionMatrix * modelViewMatrix * vec4(position, 1.0);
}
