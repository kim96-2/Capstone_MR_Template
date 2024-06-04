using TMPro;
using UnityEngine;

public class KeywordButton : MonoBehaviour
{
    public TMP_Text text;
    public void OnClick()
    {
        KeywordAPI.Instance.GetKeyword((result =>
        {
            string data = KeywordAPI.Instance.ParseKeyword(result);
            text.text = data;
        }));
    }
}
