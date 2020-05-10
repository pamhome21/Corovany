import {ActionType} from "./ActionType";

export interface Action {
    type: ActionType
    payload?: any
}

export const ExecuteCommand: () => Action = () => ({
    type: ActionType.ExecuteCommand,
})

export const AddCommand: (command: string) => Action = (command: string) => ({
    type: ActionType.AddCommand,
    payload: {
        newCommand: command
    }
})