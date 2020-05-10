import {ActionType} from "../ActionType";
import {Action} from "../actions";

export interface ApplicationState {
    commands: string[]
}

const initialState: ApplicationState = {
    commands: []
}

export default function (state = initialState, action: Action): ApplicationState {
    switch (action.type) {
        case ActionType.ExecuteCommand:
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