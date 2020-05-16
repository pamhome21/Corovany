import {combineReducers} from "redux";
import CommandsList from "./CommandsList";
import Logs from "./Logs"

export const rootReducer = combineReducers({CommandsList, Logs});

export type RootState = ReturnType<typeof rootReducer>;