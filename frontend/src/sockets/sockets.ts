import * as signalR from '@microsoft/signalr';

const connection = new signalR.HubConnectionBuilder().withUrl('/hub').build();

connection.on('newCommand', (command: string) => {
    console.log();
});

const connectionPromise = connection.start().catch(err => console.log(err));

export async function SendMessage(message: string) {
    await connectionPromise;
    console.log(message);
    await connection.send('NewCommand', message);
}