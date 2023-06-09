using UnityEngine;
using UnityEngine.UI;

public class Shoot : MonoBehaviour
{
    public float range;
    public LayerMask layersToHit;
    public LineRenderer laser;
    public Transform laserOrigin;
	public float laserDPS;

    private RaycastHit hit;

	void Update()
    {
		if (Input.GetKey(KeyCode.Mouse0)) {
			laser.enabled = true;
			if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, range)) { // This return true if it hits something that has a layer
				laser.SetPosition(0, laserOrigin.position);
				laser.SetPosition(1, hit.point);

                // Check if the hitted object's layer is in a specific layer mask
				if ((layersToHit & (1 << hit.collider.gameObject.layer)) != 0) {
					hit.collider.gameObject.GetComponent<EnemyController>().GetHit(laserDPS);
				}
			}
            else {
				laser.SetPosition(0, laserOrigin.position);
				laser.SetPosition(1, Camera.main.transform.position + Camera.main.transform.forward * range);
			}
		}
        if (Input.GetKeyUp(KeyCode.Mouse0))
            laser.enabled = false;
    }
}
