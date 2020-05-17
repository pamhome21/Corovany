import {ActionType} from "../ActionType";
import {Action} from "../actions";
import {SendMessage} from "../../sockets/sockets";
import {Character, Player, Unit} from "../../interfaces/GameInterfaces";

export interface ApplicationState {
    state: 'uninitialized' | 'stateReady' | 'combatReady' | 'finished'
    currentUnit: Unit | null
    units: Unit[]
    queue: Unit[]
    players: Player[]
    characters: Character[]
    won: boolean
}

const initialState: ApplicationState = {
    state: 'uninitialized',
    players: [],
    characters: [],
    currentUnit: null,
    units: [],
    queue: [],
    won: false,
}

export default function (state = initialState, action: Action): ApplicationState {
    switch (action.type) {
        case ActionType.ExecuteCommand:
            SendMessage(JSON.stringify(action.payload));
            return state
        case ActionType.AddCommand:
            const commandValue = JSON.parse(action.payload.newCommand);
            switch (action.payload.commandName) {
                case('PlayerAdded'):
                    return {
                        ...state,
                        players: commandValue,
                    }
                case('GameInitialized'):
                    return {
                        ...state,
                        state: 'stateReady',
                        characters: commandValue
                    }
                case('BattleFieldUpdated'):
                    return {
                        ...state,
                        state: 'combatReady',
                        currentUnit: commandValue.CurrentUnit,
                        units: commandValue.Units,
                        queue: commandValue.Queue,
                    }
                case('BattleEnd'):
                    return {
                        ...state,
                        state: 'finished',
                        won: commandValue,
                    }
                case('FrontendError'):
                    alert(action.payload.newCommand);
                    return state
                case('Reset'):
                    return initialState
                default:
                    return state
            }
        default:
            return state;
    }
}