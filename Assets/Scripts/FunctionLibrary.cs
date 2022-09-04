using UnityEngine;
using static UnityEngine.Mathf;

public static class FunctionLibrary {

    public delegate Vector3 Function (float u, float v, float t);

    public enum FunctionName { 
        Wave, 
        MultiWave, 
        Ripple,
        Sphere,
        Torus,
    }

	static Function[] functions = {
        Wave,
        MultiWave,
        Ripple,
        Sphere,
        Torus,
    };

    public static Function GetFunction (FunctionName name) {
		return functions[(int)name];
	}

    public static FunctionName GetNextFunctionName (FunctionName name) {
		return (int)name < functions.Length - 1 ? name + 1 : 0;
	}

	public static FunctionName GetRandomFunctionName () {
		var choice = (FunctionName)Random.Range(0, functions.Length);
		return choice;
	}

	public static FunctionName GetRandomFunctionNameOtherThan (FunctionName name) {
		var choice = (FunctionName)Random.Range(1, functions.Length);
		return choice == name ? 0 : choice;
	}

	public static Vector3 Morph (float u, float v, float t, Function from, Function to, float progress) {
        return Vector3.LerpUnclamped(from(u, v, t), to(u, v, t), SmoothStep(0f, 1f, progress));
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

	public static Vector3 Torus (float u, float v, float t) {
		Vector3 position;
		float r1 = 0.7f + 0.1f * Sin(PI * (6f * u + 0.5f * t));
		float r2 = 0.15f + 0.05f * Sin(PI * (8f * u + 4f * v + 2f * t));
		float s = r1 + r2 * Cos(PI * v);
		position.x = s * Sin(PI * u);
		position.y = r2 * Sin(PI * v);
		position.z = s * Cos(PI * u);
		return position;
	}

}
