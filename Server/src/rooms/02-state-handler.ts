import { Room, Client } from "colyseus";
import { Schema, type, MapSchema } from "@colyseus/schema";

export class Player extends Schema {
    @type("number") x = Math.floor(Math.random() * 256) -128;
    @type("number") z = Math.floor(Math.random() * 256) -128;
    @type("uint8") d = 2;
    @type("uint8") skin = 2;
}

export class State extends Schema {
    @type({ map: Player })
    players = new MapSchema<Player>();

    something = "This attribute won't be sent to the client-side";

    createPlayer(sessionId: string, skin: number) {
        const player = new Player();
        player.skin = skin;
        this.players.set(sessionId, player);
    }

    removePlayer(sessionId: string) {
        this.players.delete(sessionId);
    }

    movePlayer (sessionId: string, movement: any) {
        this.players.get(sessionId).x = movement.x;
        this.players.get(sessionId).z = movement.z;
    }
}

export class StateHandlerRoom extends Room<State> {
    maxClients = 4;
    lastGivenSkin = 0;
    skins: number[] = [0];

    onCreate (options) {
        for(var i = 1; i < options.skins; i++){
            this.skins.push(i);
        }
        console.log("StateHandlerRoom created!", options);

        this.setState(new State());

        this.onMessage("move", (client, data) => {        
            this.state.movePlayer(client.sessionId, data);
        });
    }

    onAuth(client, options, req) {
        return true;
    }

    onJoin (client: Client) {
        this.lastGivenSkin++;
        if(this.lastGivenSkin >= this.skins.length){
            this.lastGivenSkin = 0; 
        }
        this.state.createPlayer(client.sessionId, this.lastGivenSkin);
    }

    onLeave (client) {
        this.state.removePlayer(client.sessionId);
    }

    onDispose () {
        console.log("Dispose StateHandlerRoom");
    }

}
