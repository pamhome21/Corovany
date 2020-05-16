import {Action} from "../actions";
import {ActionType} from "../ActionType";

interface LogState {
    commands: string[]
    spellLog: string[]
}

const initialState: LogState = {
    commands: [],
    spellLog: [],
}

export default function(state = initialState, action: Action): LogState{
    switch (action.type) {
        case ActionType.ExecuteCommand:
            return {
                ...state,
                commands: [...state.commands, `To server: ${JSON.stringify(action.payload)}`]
            };
        case ActionType.AddCommand:
            return {
                ...state,
                commands: [...state.commands,
                    `From server(${action.payload.commandName}): ${JSON.stringify(JSON.parse(action.payload.newCommand), null, 4)}`],
                spellLog: action.payload.commandName === 'FrontendSpellLog' ?
                    [...state.spellLog, JSON.stringify(JSON.parse(action.payload.newCommand), null, 4)] : state.spellLog,
            }
        default:
            return state
    }
} 