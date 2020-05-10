import {RootState} from "./reducers";

export const getCommands = (store: RootState) => {
    return store.CommandsList.commands;
}