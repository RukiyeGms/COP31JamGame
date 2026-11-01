using UnityEngine;

public class CodexUI : MonoBehaviour
{
    [SerializeField] GameObject codexPanel;

    public bool IsOpen =>
        codexPanel != null && codexPanel.activeSelf;

    private void Awake()
    {
        codexPanel.SetActive(false);
    }

    public void OpenCodex()
    {
        codexPanel.SetActive(true);
        codexPanel.transform.SetAsLastSibling();
    }

    public void CloseCodex()
    {
        FishListManager fishList =
            codexPanel.GetComponent<FishListManager>();

        if (fishList != null)
        {
            fishList.CloseDetailPanel();
        }

        codexPanel.SetActive(false);
    }
}

