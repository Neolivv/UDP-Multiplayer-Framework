using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class ChatManager : MonoBehaviour
{
    public static ChatManager Instance;

    [SerializeField] ChatSegment ChatSegmentPrefab;
    [SerializeField] Transform ChatPanel;
    [SerializeField] TMP_InputField ChatInputField;

    const int MaxChatSegments = 8;
    List<ChatSegment> chatSegments = new List<ChatSegment>();

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }
    private void OnEnable() => ChatInputField.onSubmit.AddListener(SendChat);
    private void OnDisable() => ChatInputField.onSubmit.RemoveListener(SendChat);
    public void DisplayChat(ChatMessage data)
    {
        if(ChatSegmentPrefab == null)
        {
            Debug.LogError("Please Assign ChatSegment Prefab");
            return;
        }
        ChatSegment chatSegment = Instantiate(ChatSegmentPrefab,ChatPanel);
        chatSegment.Initialize(data.PlayerID, data.text);
        chatSegments.Add(chatSegment);

        //Updating ChatPanel
        if (chatSegments.Count > MaxChatSegments)
        {
            ChatSegment toRemove = chatSegments[0];
            chatSegments.RemoveAt(0);
            Destroy(toRemove.gameObject);
        } 
            
    }

    public void SendChat(string Message)
    {
        //Generating Chat Message
        ChatMessage chat = new ChatMessage{ PlayerID = NetworkManager.Instance.PlayerID, text = Message };
        byte[] data = MessageFactory.CreateMessage<ChatMessage>("Chat", chat);

        //Sending Chat Message
        NetworkManager.Instance.Send(data);

        //Emptying the InputField
        ChatInputField.text = string.Empty;
    }
    

}
