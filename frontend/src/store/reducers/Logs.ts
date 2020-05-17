import {Action} from "../actions";
import {ActionType} from "../ActionType";
import {Perk, Unit} from "../../interfaces/GameInterfaces";

interface LogState {
    commands: string[]
    spellLog: combatAction[]
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
            if (action.payload.commandName === 'Reset')
                return initialState
            return {
                ...state,
                commands: [...state.commands,
                    `From server(${action.payload.commandName})`],
                spellLog: action.payload.commandName === 'FrontendSpellLog' ?
                    [...state.spellLog, JSON.parse(action.payload.newCommand) as combatAction] : state.spellLog,
            }
        default:
            return state
    }
} 

interface combatAction{
    CurrentUnit: Unit
    Perk: Perk
    Target: Unit
}