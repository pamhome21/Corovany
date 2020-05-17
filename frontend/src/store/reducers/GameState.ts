import {ActionType} from "../ActionType";
import {Action, ExecuteCommand} from "../actions";
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
    turnCounter: number
}

const initialState: ApplicationState = {
    state: 'uninitialized',
    players: [],
    characters: [],
    currentUnit: null,
    units: [],
    queue: [],
    won: false,
    turnCounter: 0,
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
                    if (commandValue.length > 0 && state.state === 'uninitialized'){
                        SendMessage(JSON.stringify({
                            Type: 'InitializeGameCommand',
                            Args: [],
                        }));
                    }
                    return {
                        ...state,
                        players: commandValue,
                    }
                case('GameInitialized'):
                    if (state.state === 'uninitialized'){
                        SendMessage(JSON.stringify({
                            Type: 'InitializeCombatSystemCommand',
                            Args: [],
                        }))
                    }
                    return {
                        ...state,
                        state: 'stateReady',
                        characters: commandValue
                    }
                case('BattleFieldUpdated'):
                    return {
                        ...state,
                        state: state.state !== 'finished' ? 'combatReady' : "finished",
                        currentUnit: commandValue.CurrentUnit,
                        units: commandValue.Units,
                        queue: commandValue.Queue,
                        turnCounter: commandValue.TurnCounter,
                    }
                case('BattleEnd'):
                    return {
                        ...state,
                        state: 'finished',
                        won: commandValue,
                    }
                case('FrontendError'):
                    // alert(action.payload.newCommand);
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