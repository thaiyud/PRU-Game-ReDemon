using UnityEngine;

public class SlowZone : MonoBehaviour
{
	public float slowSpeed = 2f; // Tốc độ chậm trong vùng
	private float originalSpeed; // Lưu tốc độ ban đầu của nhân vật

	void OnTriggerEnter2D(Collider2D other)
	{
		// Kiểm tra xem đối tượng vào vùng có phải là nhân vật hay không
		if (other.CompareTag("Player"))
		{
			// Lấy tốc độ di chuyển hiện tại của nhân vật (giả sử nó được điều khiển bởi một script khác có tốc độ là một thuộc tính public)
			PlayerController3 playerMovement = other.GetComponent<PlayerController3>();

			if (playerMovement != null)
			{
				originalSpeed = playerMovement.moveSpeed; // Lưu tốc độ gốc
				playerMovement.moveSpeed = slowSpeed; // Thiết lập tốc độ chậm khi vào vùng
			}
		}
	}

	void OnTriggerExit2D(Collider2D other)
	{
		// Kiểm tra khi nhân vật ra khỏi vùng
		if (other.CompareTag("Player"))
		{
			PlayerController3 playerMovement = other.GetComponent<PlayerController3>();

			if (playerMovement != null)
			{
				playerMovement.moveSpeed = originalSpeed; // Khôi phục tốc độ gốc khi ra khỏi vùng
			}
		}
	}
}