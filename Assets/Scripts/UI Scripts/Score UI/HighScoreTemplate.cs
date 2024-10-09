using Assets.Script;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HighScoreTemplate : MonoBehaviour
{
    private Transform entryContainer;
    private Transform entryTemplate;

    private void Awake()
    {
        entryContainer = transform.Find("Score Container");
        entryTemplate = entryContainer.Find("Entry Template");
        entryTemplate.gameObject.SetActive(false);

        float templateHeight = 50f;
        List<StatSave> recordList = JsonSave.LoadListFromJson();

        for (int i = 0; i < 10; i++)
        {
            Transform entryTransform = Instantiate(entryTemplate, entryContainer);
            RectTransform entryRectTransform = entryTransform.GetComponent<RectTransform>();
            entryRectTransform.anchoredPosition = new Vector2(0, -templateHeight * (recordList.Count - 1 - i));
            entryTransform.gameObject.SetActive(true);


            entryTransform.Find("Score Entry").GetComponent<TextMeshProUGUI>().text = recordList[i].Score.ToString();
            entryTransform.Find("Timer Entry").GetComponent<TextMeshProUGUI>().text = recordList[i].Time.ToString();
        }
    }
}
