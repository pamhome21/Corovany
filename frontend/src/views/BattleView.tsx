import React, {} from 'react';
import {useDispatch, useSelector} from "react-redux";
import {getCommands} from "../store/selectors";
import {ExecuteCommand} from "../store/actions";

// Displays battle actions
export function BattleView(props: any){
    const commands = useSelector(getCommands);
    const dispatch = useDispatch();
    const executeInitState = () => {
        dispatch(ExecuteCommand({
            Type: 'InitializeGameCommand',
            Args: [],
        }))
    }
    return <div>
        <button onClick={executeInitState}>Execute init state</button>
        {commands.map(command => <p>{command}</p>)}
    </div>
}