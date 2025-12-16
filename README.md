# UDP-Multiplayer-Framework

Multiplayer Replication Wrapper

**Description**:
A Unity multiplayer framework demonstrating the basics of networked gameplay over UDP. This project focuses on client-side prediction, ping measurement, player management, and chat, providing a simple but educational example of multiplayer networking from scratch.

**Features**

 - Client-side prediction for smooth local movement

 - Ping / latency measurement between client and server

 - Player join/leave management with automatic spawning

 - Basic chat system between players

 - Tick-based updates for deterministic simulation

 - Custom UDP transport layer (Node.js server)

**Architecture Overview**

 - Server (Node.js UDP server)

    - Tracks connected players and broadcasts player list

    - Handles chat and ping messages

 - Client (Unity)

    - NetworkManager: handles UDP transport, tick updates, ping

    - Movement: local movement simulation (prediction)

    - MessageDispatcher: deserializes and processes incoming messages

    - PlayerController: reads input and triggers movement

**Networking**

 - Messages are JSON-serialized

 - Message types: PlayerJoin, PlayerLeave, PlayerMove, Chat, Ping

 - Clients send their input; server broadcasts player list and chat

**Future Improvements**

 - Add server reconciliation for authoritative movement

 - Add Entity interpolation

 - Implement authoritative physics and collisions

 - Optimize serialization (binary instead of JSON)



https://github.com/user-attachments/assets/b4a5b8a0-4b28-4182-82a7-70a16357c852

