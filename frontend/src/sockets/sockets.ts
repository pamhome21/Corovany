import * as signalR from '@microsoft/signalr';
import {store} from '../store/store';
import {AddCommand} from "../store/actions";

const connection = new signalR.HubConnectionBuilder().withUrl('/hub').build();

connection.on('newCommand', (command: string) => {
    store.dispatch(AddCommand(command));
});

const connectionPromise = connection.start().catch(err => console.log(err));

export async function SendMessage(message: string) {
    await connectionPromise;
    console.log(message);
    await connection.send('NewCommand', message);
}