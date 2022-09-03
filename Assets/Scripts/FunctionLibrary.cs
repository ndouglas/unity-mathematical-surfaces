using UnityEngine;
using static UnityEngine.Mathf;

public static class FunctionLibrary {

    public delegate Vector3 Function (float u, float v, float t);

    public enum FunctionName { 
        Wave, 
        MultiWave, 
        Ripple,
        Sphere,
    }

	static Function[] functions = {
        Wave,
        MultiWave,
        Ripple,
        Sphere,
    };

    public static Function GetFunction (FunctionName name) {
		return functions[(int)name];
	}

	public static Vector3 Wave (float u, float v, float t) {
		Vector3 position;
		position.x = u;
		position.y = Sin(PI * (u + v + t));
		position.z = v;
		return position;
	}

	public static Vector3 MultiWave (float u, float v, float t) {
		Vector3 position;
		position.x = u;
		position.y = Sin(PI * (u + 0.5f * t));
		position.y += 0.5f * Sin(2f * PI * (v + t));
		position.y += Sin(PI * (u + v + 0.25f * t));
		position.y *= 1f / 2.5f;
		position.z = v;
		return position;
	}

	public static Vector3 Ripple (float u, float v, float t) {
		float d = Sqrt(u * u + v * v);
		Vector3 position;
		position.x = u;
		position.y = Sin(PI * (4f * d - t));
		position.y /= 1f + 10f * d;
		position.z = v;
		return position;
	}

	public static Vector3 Sphere (float u, float v, float t) {
		Vector3 position;
        float r = 0.9f + 0.1f * Sin(PI * (6f * u + 4f * v + t));
        float s = r * Cos(0.5f * PI * v);
		position.x = s * Sin(PI * u);
		position.y = r * Sin(0.5f * PI * v);
		position.z = s * Cos(PI * u);
		return position;
	}

}
