import {ActionType} from "./ActionType";
import {Perk, Unit} from "../interfaces/GameInterfaces";

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

export const SelectPerk: (perk: Perk, unit: Unit) => Action = (perk: Perk, unit: Unit) => ({
    type: ActionType.SelectPerk,
    payload: {perk, unit}
})