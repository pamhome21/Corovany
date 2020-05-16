import {RootState} from "./reducers";

export const getCommands = (store: RootState) => store.Logs.commands;

export const getSpells = (store: RootState) => store.Logs.spellLog;

export const getQueue = (store: RootState) => store.CommandsList.queue;

export const getCurrentUnit = (store: RootState) => store.CommandsList.currentUnit;

export const getUnits = (store: RootState) => store.CommandsList.units;

export const getPlayer = (store: RootState) => store.CommandsList.players[0];

export const getCharacters = (store: RootState) => store.CommandsList.characters;

export const getGameState = (store: RootState) => ({
    state: store.CommandsList.state,
    won: store.CommandsList.won
});