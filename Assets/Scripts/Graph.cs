using UnityEngine;

public class Graph : MonoBehaviour {

	[SerializeField]
	Transform pointPrefab = default;

	[SerializeField, Range(10, 100)]
	int resolution = 10;

    Transform[] points;

	[SerializeField]
	FunctionLibrary.FunctionName function = default;

	void Awake () {
		float step = 2f / resolution;
		var position = Vector3.zero;
		var scale = Vector3.one * step;
        points = new Transform[resolution * resolution];
		for (int i = 0, x = 0, z = 0; i < points.Length; i++, x++) {
			if (x == resolution) {
				x = 0;
                z += 1;
			}
			Transform point = points[i] = Instantiate(pointPrefab);
			position.x = (x + 0.5f) * step - 1f;
            position.z = (z + 0.5f) * step - 1f;
			position.y = position.x;
			point.localPosition = position;
			point.localScale = scale;
            point.SetParent(transform, false);
		}
	}

	void Update () {
        float time = Time.time;
        FunctionLibrary.Function func = FunctionLibrary.GetFunction(function);
		for (int i = 0; i < points.Length; i++) {
            Transform point = points[i];
            Vector3 position = point.localPosition;
			position.y = func(position.x, position.z, time);
            point.localPosition = position;
        }
	}

}
