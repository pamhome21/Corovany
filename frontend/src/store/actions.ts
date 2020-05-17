import {ActionType} from "./ActionType";
import {Perk} from "../interfaces/GameInterfaces";

export interface Action {
    type: ActionType
    payload?: any
}

interface Command {
    Type: string,
    Args: string[]
}

export const ExecuteCommand: (payload: Command) => Action = (payload: Command) => ({
    type: ActionType.ExecuteCommand,
    payload
})

export const AddCommand: (command: string, commandName: string) => Action = (command: string, commandName: string) => ({
    type: ActionType.AddCommand,
    payload: {
        commandName,
        newCommand: command
    }
})

export const SelectPerk: (payload: Perk) => Action = (payload: Perk) => ({
    type: ActionType.SelectPerk,
    payload
})