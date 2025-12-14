const dgram = require('dgram')
const server = dgram.createSocket('udp4')
 
const players = {};

const Position = (0,0,0)

server.on('error',(error) => {
console.log('error in server\n' + error.message)
server.close()
})

server.on('listening', () =>
{
    const address = server.address()
    console.log(`server is listening on ${address.address}:${address.port}`)
})

server.on('message', (message , senderinfo) =>
{
    const jsonStr = message.toString()
    let msg ;
    try
    {
        msg = JSON.parse(jsonStr)
    }catch(err)
    {
        console.log("Invalid JSON Received")
        return
    }

    const {type , payload} = msg

    switch(type)
    {
        case "PlayerJoin":
            const join_data = JSON.parse(payload)
            const playerID = join_data.PlayerID

            console.log(`[Server] Player Joined ${playerID} from ${senderinfo.address}:${senderinfo.port}`)

            players[playerID] = {
                id:playerID,
                address:senderinfo.address,
                port:senderinfo.port
            }
            
            
            broadcastPlayerList();
            broadcast("New Player Joined",playerID);
            break

        case "PlayerLeave":
            const leave_data = JSON.parse(payload)
            const leave_playerID = leave_data.PlayerID

            console.log(`[Server] Player ${leave_playerID} Left`)

            if (players[leave_playerID])
            {
                delete players[leave_playerID]
            }

            broadcastPlayerList();
            broadcast("A Player Left",leave_playerID);

            break
        case "Chat":
            const data = JSON.parse(payload)
            const chat_playerID = data.PlayerID
            const chat_message = data.text

            broadcast(chat_message,chat_playerID)
            break
        case "NetPing":
            const ping_data = JSON.parse(payload)
            const ping_message = JSON.stringify({type:"NetPing",payload:JSON.stringify({TimeStamp:ping_data.TimeStamp,Sequence:ping_data.Sequence})})
            server.send(ping_message,senderinfo.port,senderinfo.address)
            break
    }


    
})
server.bind(5500)


function broadcast(messageObj,ID) {
    const buffer = JSON.stringify({PlayerID:ID,text: messageObj})
    const data = JSON.stringify({type:"Chat",payload:buffer})
    Object.values(players).forEach(p => {
        server.send(data, p.port, p.address);
    });
}


function broadcastPlayerList() {
    const playerdata = JSON.stringify({playerdatas:Object.values(players)})
    const data = JSON.stringify({type:"PlayerList",payload:playerdata})
    Object.values(players).forEach(p =>
    {
        server.send(data,p.port,p.address);
    }
    )

}