# CSE-3541-Custom-Raytrace

Inputs:
[Space] => Rotate camera around the scene (hit play and enjoy the view :>)

The programs implemented are two different Unity3D ray trace renderers:

(each scene processes a different style of rendering, described below)

CPU Raytracer:
 - The first renders moving objects and shading in real time using methods
	similar to those described in the lecture notes. This renders using C# scripts
	and no shader programing, so it runs entirely on the CPU. It can either render
	in real-time or a single instance, but when running in real-time, only renders
	at around 3 frames-per-second. Because of this discovery, I decided to pivot
	partway through my implementation to instead program to the GPU through shader
	programming (as described below). This is why this portion may not seem very 
	fleshed out and complete. 
	
 - Includes dynamic shading utilizing point, spot, and directional lights.
 
![CPU_Raytrace_before](https://user-images.githubusercontent.com/49098697/189493245-4061fadd-1850-4455-bee0-6f6c33b71051.PNG)![CPU_Raytrace_after](https://user-images.githubusercontent.com/49098697/189493323-e343e7b9-841a-4ba8-af51-aaff0bd52d75.PNG)



GPU Raytracer:
 - The second renders a random selection of spheres of varying sizes on a plane to show
	how ray reflections work with spheres of different albedos, specular values,
	shape, size, etc. using compute shaders in Unity. The languages I used were 
	C# for scripts and HLSL for the shader programming. 

 - Includes full reflections (based on an energy system so they don't reflect forever),
	anti-aliasing (only while camera is still - anti-aliasing only properly renders 
	while the objects are immobile), directional lighting effects, and hard shadows. 
	
	![GPU_Raytrace](https://user-images.githubusercontent.com/49098697/189493261-261f973b-caa7-4a47-919e-b543c3b2a935.PNG)


While neither included as many features as I'd anticipated, it was fun developing two
different styles of renderer. I hope to continue this project in my spare time to
implement further features and clean up the messy shader code. 

The GPU raytracer was developed based on a paper written by Turner Whitted in 1980:
https://dl.acm.org/citation.cfm?id=358882
