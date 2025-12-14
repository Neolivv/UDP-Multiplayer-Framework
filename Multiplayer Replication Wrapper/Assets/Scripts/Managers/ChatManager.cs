using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class ChatManager : MonoBehaviour
{
    public static ChatManager Instance;

    public NetworkManager NetworkManager;
    public ChatSegment ChatSegmentPrefab;
    [SerializeField] Transform ChatPanel;
    [SerializeField] TMP_InputField ChatInputField;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }
    private void OnEnable()
    {
        ChatInputField.onSubmit.AddListener(SendChat);
    }
    public void DisplayChat(ChatMessage data)
    {
        if(ChatSegmentPrefab == null)
        {
            Debug.LogError("Please Assign ChatSegment Prefab");
            return;
        }
        ChatSegment chatSegment = Instantiate(ChatSegmentPrefab,ChatPanel);
        chatSegment.Initialize(data.PlayerID, data.text);
    }

    public void SendChat(string Message)
    {
        //Generating Chat Message
        ChatMessage chat = new ChatMessage{ PlayerID = NetworkManager.PlayerID, text = Message };
        byte[] data = MessageFactory.CreateMessage<ChatMessage>("Chat", chat);

        //Sending Chat Message
        NetworkManager.Send(data);

        //Emptying the InputField
        ChatInputField.text = string.Empty;
    }
    

}
