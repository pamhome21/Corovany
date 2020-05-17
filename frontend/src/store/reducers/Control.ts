import {Action} from "../actions";
import {ActionType} from "../ActionType";
import {Perk} from "../../interfaces/GameInterfaces";

interface ControlState {
    selectedPerk: Perk | null
}

const initialState: ControlState = {
    selectedPerk: null,
}

export default function (state = initialState, action: Action): ControlState {
    switch (action.type) {
        case ActionType.SelectPerk:
            return {
                ...state,
                selectedPerk: action.payload as Perk,
            }
        case ActionType.ExecuteCommand:
            if (action.payload.Type === 'NextTurnCommand')
                return {
                    ...state,
                    selectedPerk: null,
                }
            return state;
        default:
            return state
    }
}