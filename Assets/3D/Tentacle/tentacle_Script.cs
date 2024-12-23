
[ExecuteInEditMode]
public class TentacleInteract : MonoBehaviour {

	public Material material;
	public Transform obj;
	public Vector3 offset;

	public void Update() {
		if (material == null || obj == null) return;
		float radius = obj.localScale.x - 0.5f;
		material.SetFloat("_Radius", radius);
		material.SetVector("_InteractPos", transform.InverseTransformPoint(obj.position + offset * radius));
	}
}