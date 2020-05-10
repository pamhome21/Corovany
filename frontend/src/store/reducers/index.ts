import {combineReducers} from "redux";
import CommandsList from "./CommandsList";

export const rootReducer = combineReducers({CommandsList});

export type RootState = ReturnType<typeof rootReducer>;