


#ifndef refraction
#define refraction

	void refract_float(in float IOR, in float3 viewDir, in float3 normalDir, out float3 output) {
		output = refract(normalize(viewDir), normalize(normalDir), IOR);
		return;
	}



#endif

