using UnityEngine;
using UnityEngine.UI;

public class Removal : MonoBehaviour
{
    public Button button;

    Transform Content;

    // Start is called before the first frame update
    void Start()
    {
        Content = button.transform.parent.parent;
        button.onClick.AddListener(Delete);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Delete()
    {
        Destroy(this.transform.parent.gameObject);
        this.gameObject.transform.parent.parent.BroadcastMessage("OnDelete");
    }

    private void OnDelete()
    {
        /*for (int i = 0; i < Content.childCount; i++)
            Content.GetChild(i).GetComponentInChildren<Text>().text = $"{i + 1}:";*/
        //Debug.Log(Content.GetChild(i).GetChild(0).GetComponent<Text>().text); // 
        this.transform.parent.GetChild(0).GetComponent<Text>().text = (this.transform.parent.GetSiblingIndex() + 1) + ":";
    }
}
