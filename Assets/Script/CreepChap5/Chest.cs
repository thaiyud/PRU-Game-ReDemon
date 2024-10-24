using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    //[SerializeField] private int startingHealth = 1;
    public Animator chestAnimator; // Animator điều khiển animation của rương
    public GameObject heart; // Mô hình trái tim
    public bool isOpen = false; // Để theo dõi rương có mở hay không

    void Start()
    {
        heart.SetActive(false); // Ẩn trái tim khi bắt đầu
    }

    void Update()
    {
        if (isOpen) { heart.SetActive(isOpen); } }
    // Phương thức này được gọi khi vũ khí của chiến binh va chạm với rương
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Weapon") && !isOpen) // Kiểm tra có phải vũ khí của chiến binh không
        {
            OpenChest();
        }
    }

    // Phương thức mở rương
    void OpenChest()
    {
        chestAnimator.SetTrigger("OpenChest"); // Kích hoạt animation mở rương
        isOpen = true; // Đánh dấu rương đã mở
        StartCoroutine(ChestOpenSequence()); // Bắt đầu chuỗi sự kiện khi mở rương
    }

    // Chuỗi sự kiện sau khi rương mở
    IEnumerator ChestOpenSequence()
    {
        // Chờ cho animation mở rương hoàn tất (giả sử thời gian animation là 1 giây)
        yield return new WaitForSeconds(1.0f);

        // Làm rương biến mất
        gameObject.SetActive(false);

        // Hiển thị trái tim
        heart.SetActive(true);
    }
}
