using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class MessageDispatcher
{
    public static void Handle(NetMessage message)
    {
        switch (message.type)
        {
            case MessageTypes.PlayerList:
                PlayerList playerlist = MessageFactory.DecodePayLoad<PlayerList>(message);
                Debug.Log("[Server] Player List Updated\n");
                MainThreadDispatcher.Enqueue(() =>
                {
                    if (playerlist.playerdatas == null) return;
                    foreach (var playerData in playerlist.playerdatas)
                    {
                        if (!GameManager.Instance.PlayerExists(playerData.id))
                        {
                            GameManager.Instance.SpawnPlayer(playerData.id);
                        }
                    }

                    var currentIDs = new HashSet<string>(playerlist.playerdatas.Select(p => p.id));
                    foreach (var data in playerlist.playerdatas)
                    {
                        var id = data.id;
                        if (!currentIDs.Contains(id))
                        {
                            GameManager.Instance.RemovePlayer(id);
                        }
                    }
                });
                break;
            case MessageTypes.PlayerJoin:
                PlayerJoin join = MessageFactory.DecodePayLoad<PlayerJoin>(message);
                Debug.Log($"[Server] Player {join.PlayerID} Joined");
                break;
            case MessageTypes.PlayerLeave:
                PlayerLeave leave = MessageFactory.DecodePayLoad<PlayerLeave>(message);
                Debug.Log($"[Server] Player {leave.PlayerID} Left");
                break;
            case MessageTypes.PlayerMove:
                PlayerMove move = MessageFactory.DecodePayLoad<PlayerMove>(message);
                Debug.Log($"[Server] Player Moved To : ({move.x},{move.y},{move.z})");
                break;
            case MessageTypes.Chat:
                ChatMessage chat = MessageFactory.DecodePayLoad<ChatMessage>(message);
                Debug.Log($"[Server] Chat : {chat.text}");
                MainThreadDispatcher.Enqueue(() =>
                {
                    ChatManager.Instance.DisplayChat(chat);
                });
                
                break;
            case MessageTypes.NetPing:
                NetPing ping = MessageFactory.DecodePayLoad<NetPing>(message);
                MainThreadDispatcher.Enqueue(() => { NetworkManager.Instance.OnPong(ping); });
                break;
        }
    }
}
