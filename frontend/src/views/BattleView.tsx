import React, {useState} from 'react';
import {useDispatch, useSelector} from "react-redux";
import {getCommands} from "../store/selectors";
import {ExecuteCommand} from "../store/actions";
import {TextGameDisplay} from "../components/TextGameDisplay";

// Displays battle actions
export function BattleView(props: any) {
    const commands = useSelector(getCommands);
    const dispatch = useDispatch();
    const executeInitState = () => {
        dispatch(ExecuteCommand({
            Type: 'InitializeGameCommand',
            Args: [],
        }))
    }
    const [playerId, updatePlayerId] = useState('');
    const [playerName, updatePlayerName] = useState('');
    const executeAddPlayer = () => {
        dispatch(ExecuteCommand({
            Type: 'AddPlayerCommand',
            Args: [playerId, playerName]
        }))
    }
    const executeInitializeCombatSystem = () => {
        dispatch(ExecuteCommand({
            Type: 'InitializeCombatSystemCommand',
            Args: [],
        }))
    }
    const [perkKey, updatePerkKey] = useState('');
    const [targetKey, updateTargetKey] = useState('');
    const executeNextTurnCommand = () => {
        dispatch(ExecuteCommand({
            Type: 'NextTurnCommand',
            Args: [perkKey, targetKey]
        }))
    }
    const executeResetCommand = () => {
        dispatch(ExecuteCommand({
            Type: 'InitializeGameStateResetCommand',
            Args: [],
        }))
    }
    return <>
        <div style={{
            backgroundColor: 'rgb(250, 250, 250)'
        }}>
            <div>
                <button onClick={executeAddPlayer}>Execute add player</button>
                <input placeholder={'id'} onChange={(e) => updatePlayerId(e.target.value)}/>
                <input placeholder={'name'} onChange={(e) => updatePlayerName(e.target.value)}/>
            </div>
            <div>
                <button onClick={executeInitState}>Execute init state</button>
            </div>
            <div>
                <button onClick={executeInitializeCombatSystem}>Execute initialize combat system</button>
            </div>
            <div>
                <button onClick={executeNextTurnCommand}>Execute next turn command</button>
                <input placeholder={'perkKey'} onChange={(e) => updatePerkKey(e.target.value)}/>
                <input placeholder={'targetKey'} onChange={(e) => updateTargetKey(e.target.value)}/>
            </div>
            <div>
                <button onClick={executeResetCommand}>Execute reset command</button>
            </div>
            <div>
                <TextGameDisplay/>
            </div>
        </div>
        <div style={{
            height: '400px',
            overflowY: 'scroll',
            backgroundColor: 'rgb(235, 235, 235)'
        }}>
            <p>Команды:</p>
            <div style={{marginLeft: '15px'}}>
                {commands.map((command, i) => <pre key={i}>{command}</pre>)}
            </div>
        </div>
    </>
}   