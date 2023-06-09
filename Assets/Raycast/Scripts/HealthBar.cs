using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Image foreground;

	private void Update() {
		transform.rotation = Quaternion.LookRotation(transform.position - Camera.main.transform.position);
	}

	public void SetCurrentLife(float currentHealth, float maxHealth) {
        foreground.fillAmount = currentHealth / maxHealth;
    }
}
