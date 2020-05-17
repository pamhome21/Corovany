import {RootState} from "./reducers";

export const getCommands = (store: RootState) => store.Logs.commands;

export const getSpells = (store: RootState) => store.Logs.spellLog;

export const getQueue = (store: RootState) => store.GameState.queue;

export const getCurrentUnit = (store: RootState) => store.GameState.currentUnit;

export const getUnits = (store: RootState) => store.GameState.units;

export const getPlayer = (store: RootState) => store.GameState.players[0];

export const getCharacters = (store: RootState) => store.GameState.characters;

export const getGameState = (store: RootState) => ({
    state: store.GameState.state,
    won: store.GameState.won
});

export const getSelectedPerk = (store: RootState) => store.Control.selectedPerk;