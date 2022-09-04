using UnityEngine;

public class Graph : MonoBehaviour {

	[SerializeField]
	Transform pointPrefab = default;

	[SerializeField, Range(10, 200)]
	int resolution = 10;

    Transform[] points;

	[SerializeField]
	FunctionLibrary.FunctionName function = default;

	[SerializeField, Min(0f)]
	float functionDuration = 1f, transitionDuration = 1f;

    float duration;

	bool transitioning;

	FunctionLibrary.FunctionName transitionFunction;

	public enum TransitionMode { Cycle, Random }

	[SerializeField]
	TransitionMode transitionMode;

	void Awake () {
		float step = 2f / resolution;
		var scale = Vector3.one * step;
        points = new Transform[resolution * resolution];
		for (int i = 0; i < points.Length; i++) {
			Transform point = points[i] = Instantiate(pointPrefab);
			point.localScale = scale;
            point.SetParent(transform, false);
		}
	}

	void Update () {
        duration += Time.deltaTime;
        if (transitioning) {
			if (duration >= transitionDuration) {
				duration -= transitionDuration;
				transitioning = false;
			}            
        }
		else if (duration >= functionDuration) {
			duration -= functionDuration;
			transitioning = true;
			transitionFunction = function;
            PickNextFunction();
		}
		if (transitioning) {
			UpdateFunctionTransition();
		}
		else {
			UpdateFunction();
		}
	}

	void UpdateFunction () {
        float time = Time.time;
        FunctionLibrary.Function func = FunctionLibrary.GetFunction(function);
        float step = 2f / resolution;
        float v = 0.5f * step - 1f;
		for (int i = 0, x = 0, z = 0; i < points.Length; i++, x++) {
			if (x == resolution) {
				x = 0;
				z += 1;
			}
			float u = (x + 0.5f) * step - 1f;
			v = (z + 0.5f) * step - 1f;
			points[i].localPosition = func(u, v, time);
		}
	}

	void UpdateFunctionTransition () {
		FunctionLibrary.Function
			from = FunctionLibrary.GetFunction(transitionFunction),
			to = FunctionLibrary.GetFunction(function);
		float progress = duration / transitionDuration;
		float time = Time.time;
		float step = 2f / resolution;
		float v = 0.5f * step - 1f;
		for (int i = 0, x = 0, z = 0; i < points.Length; i++, x++) {
			if (x == resolution) {
				x = 0;
				z += 1;
			}
			float u = (x + 0.5f) * step - 1f;
			v = (z + 0.5f) * step - 1f;
			points[i].localPosition = FunctionLibrary.Morph(
				u, v, time, from, to, progress
			);
		}
    }

	void PickNextFunction () {
		function = transitionMode == TransitionMode.Cycle ?
			FunctionLibrary.GetNextFunctionName(function) :
			FunctionLibrary.GetRandomFunctionNameOtherThan(function);
	}

}
