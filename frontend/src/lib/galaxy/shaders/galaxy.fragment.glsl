precision highp float;
precision highp int;

in vec2 vUv;
in vec3 fragPosition;

varying vec3 vColor;

void main() {
	float distance = length(gl_PointCoord - vec2(0.5, 0.5));
	if(distance > 0.4)
		discard;//quick fix to avoid ugly edges
	float intensity = 1.0 - smoothstep(0.0, 0.5, distance);
	gl_FragColor = vec4(vColor, 1.0) * intensity;
}
