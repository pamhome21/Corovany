import {Action} from "../actions";
import {ActionType} from "../ActionType";
import {Perk, Unit} from "../../interfaces/GameInterfaces";

interface ControlState {
    selectedPerk: Perk | null
}

const initialState: ControlState = {
    selectedPerk: null,
}

export default function (state = initialState, action: Action): ControlState {
    switch (action.type) {
        case ActionType.SelectPerk:
            const selectedPerk = action.payload.perk as Perk;
            const currentUnit = action.payload.unit as Unit;
            if (selectedPerk.Cost > currentUnit.SpecialPoints || currentUnit.Cooldown[selectedPerk.Name] !== 0)
                return state
            return {
                ...state,
                selectedPerk,
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