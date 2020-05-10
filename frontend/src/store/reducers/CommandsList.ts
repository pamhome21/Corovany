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
            return state;
        case ActionType.AddCommand:
            return {
                ...state,
                commands: [...state.commands, action.payload.newCommand]
            }
        default:
            return state;
    }
}