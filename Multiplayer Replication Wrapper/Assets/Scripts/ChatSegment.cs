using TMPro;
using UnityEngine;

public class ChatSegment : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI PlayerIDText;
    [SerializeField] TextMeshProUGUI ChatMessageText;

    public void Initialize(string PlayerID,string Message)
    {
        PlayerIDText.SetText(PlayerID);
        ChatMessageText.SetText(Message);
    }
}
