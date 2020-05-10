import React, {} from 'react';
import {useDispatch, useSelector} from "react-redux";
import {getCommands} from "../store/selectors";
import {ExecuteCommand} from "../store/actions";

// Displays battle actions
export function BattleView(props: any){
    const commands = useSelector(getCommands);
    const dispatch = useDispatch();
    return <div>
        <button onClick={() => dispatch(ExecuteCommand())}>Execute init state</button>
        {commands.map(command => <p>{command}</p>)}
    </div>
}