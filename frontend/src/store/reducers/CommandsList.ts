import {ActionType} from "../ActionType";
import {Action} from "../actions";
import {SendMessage} from "../../sockets/sockets";

export interface ApplicationState {
    state: 'uninitialized' | 'stateReady' | 'combatReady' | 'finished'
    currentUnit: Unit | null
    units: Unit[]
    queue: Unit[]
    players: Player[]
    characters: Character[]
    commands: string[]
    won: boolean
}

const initialState: ApplicationState = {
    state: 'uninitialized',
    commands: [],
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
            return {
                ...state,
                commands: [...state.commands, `To server: ${JSON.stringify(action.payload)}`]
            };
        case ActionType.AddCommand:
            const commandValue = JSON.parse(action.payload.newCommand);
            switch (action.payload.commandName) {
                case('PlayerAdded'):
                    return {
                        ...state,
                        commands: [...state.commands,
                            `From server(${action.payload.commandName}): ${JSON.stringify(JSON.parse(action.payload.newCommand), null, 4)}`],
                        players: commandValue,
                    }
                case('GameInitialized'):
                    return {
                        ...state,
                        state: 'stateReady',
                        commands: [...state.commands,
                            `From server(${action.payload.commandName}): ${JSON.stringify(JSON.parse(action.payload.newCommand), null, 4)}`],
                        characters: commandValue
                    }
                case('BattleFieldUpdated'):
                    return {
                        ...state,
                        state: 'combatReady',
                        commands: [...state.commands,
                            `From server(${action.payload.commandName}): ${JSON.stringify(JSON.parse(action.payload.newCommand), null, 4)}`],
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
                case('Reset'):
                    return initialState
                default:
                    return {
                        ...state,
                        commands: [...state.commands,
                            `From server(${action.payload.commandName}): ${JSON.stringify(JSON.parse(action.payload.newCommand), null, 4)}`]
                    }
            }
        default:
            return state;
    }
}

export interface Player {
    Name: string
    Id: string
}

export interface Character {
    Name: string
    CharClass: CharacterClass
    Level: number
    Initiative: number
    HealthPoints: number
    MoralePoints: number
    SpecialPoints: number
    OwnerId: string
}

export interface CharacterClass {
    Name: string
    HealthPoints: number
    MoralePoints: number
    SpecialPoints: number
    Initiative: number
    Perks: Perk[]
}

export interface Perk {
    Name: string
    Cost: number
    Cooldown: number
    LevelToUnlock: number
}

export interface Unit {
    Character: Character
    HealthPoints: number
    MoralePoints: number
    SpecialPoints: number
    Initiative: number
    State: UnitState
}

export enum UnitState {
    Fine = 0,
    Alive = 1,
    Dead = 2,
}