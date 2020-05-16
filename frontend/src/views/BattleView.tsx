import React, {useState} from 'react';
import {useDispatch, useSelector} from "react-redux";
import {getCommands, getSpells} from "../store/selectors";
import {ExecuteCommand} from "../store/actions";
import {TextGameDisplay} from "../components/TextGameDisplay";
import {GraphicalGameDisplay} from "../components/GraphicalGameDisplay";

// Displays battle actions
export function BattleView(props: any) {
    const spells = useSelector(getSpells);
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

    const executeReceiveFullDataStateCommand = () => {
        dispatch(ExecuteCommand({
            Type: 'ReceiveFullDataStateCommand',
            Args: [],
        }))
    }
    return <>
        <div style={{float: 'left'}}>
            <div style={{
                backgroundColor: 'rgb(250, 250, 250)',
                width: '40vw',
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
                    <button
                        onClick={executeReceiveFullDataStateCommand}>Execute receive full data state command</button>
                </div>
            </div>
            <div>
                <TextGameDisplay/>
            </div>
            <div style={{
                height: '200px',
                width: '100%',
                overflowY: 'scroll',
                backgroundColor: 'rgb(235, 235, 235)'
            }}>
            <p>Способности:</p>
                <div style={{marginLeft: '15px'}}>
                    {spells.map((spell, i) => <pre key={i}>{spell}</pre>)}
                </div>
            </div>
        </div>

        <div style={{float: 'left'}}>
            <GraphicalGameDisplay/>
        </div>
    </>
}   