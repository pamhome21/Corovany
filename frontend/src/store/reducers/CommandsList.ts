import {ActionType} from "../ActionType";
import {Action} from "../actions";
import {SendMessage} from "../../sockets/sockets";

export interface ApplicationState {
    commands: string[]
}

const initialState: ApplicationState = {
    commands: []
}

export default function (state = initialState, action: Action): ApplicationState {
    switch (action.type) {
        case ActionType.ExecuteCommand:
            SendMessage(JSON.stringify(action.payload));
            return {
                ...state,
                commands: [...state.commands, `To server: ${JSON.stringify(action.payload)}`]
            };
        case ActionType.AddCommand:
            return {
                ...state,
                commands: [...state.commands, `From server: ${JSON.stringify(JSON.parse(action.payload.newCommand), null, 4)}`]
            }
        default:
            return state;
    }
}