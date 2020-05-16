import React, {useEffect} from 'react';
import {useDispatch, useSelector} from "react-redux";
import {getCharacters, getCurrentUnit, getGameState, getPlayer, getQueue, getUnits} from "../store/selectors";
import {ExecuteCommand} from "../store/actions";

const gameStateNames = {
    uninitialized: 'Не инициализированно',
    stateReady: 'Подготовка к бою',
    combatReady: 'Бой идет',
    finished: 'Бой окончен'
}

const unitStates = [
    'Активен',
    'Сбежал',
    'Мертв',
]

export function GraphicalGameDisplay(props: any) {
    const player = useSelector(getPlayer);
    const gameState = useSelector(getGameState);
    const characters = useSelector(getCharacters);
    const units = useSelector(getUnits);
    const queue = useSelector(getQueue);
    const currentUnit = useSelector(getCurrentUnit);
    const dispatch = useDispatch();
    useEffect(() => {
        dispatch(ExecuteCommand({
            Type: 'ReceiveFullDataStateCommand',
            Args: [],
        }))
    }, [])
    return <svg height={850} width={800}>
        <image xlinkHref={'Resources/Panels/top_panel.png'}/>
        <image y={50} xlinkHref={'Resources/Stages/desert_map.png'}/>
        <image transform={'scale(1, 0.6)'} y={600 * (1/0.6)} xlinkHref={'Resources/Panels/bottom_panel.png'}/>
        
        {player && <>
            <text textAnchor={'end'} x={800 - 15} y={20} fill={'white'}>Player name: {player.Name}</text>
            <text textAnchor={'end'} x={800 - 15} y={40} fill={'lightgray'}>ID: {player.Id}</text>
        </>}
        
        {(gameState.state === 'combatReady' || gameState.state === 'finished') && currentUnit && <>
            <svg y={600}>
                <image width={128} height={128} xlinkHref={`Resources/Characters/${currentUnit.Character.CharClass.Name}/prev.png`}/>
                <text fill={'white'} x={115} y={25}>{currentUnit.Character.Name}</text>
                <text fill={'white'} x={115} y={45}>{currentUnit.Character.CharClass.Name}</text>
                <text fill={'pink'} x={115} y={65}>{currentUnit.HealthPoints}/{currentUnit.Character.HealthPoints}</text>
                <text fill={'yellow'} x={115} y={85}>{currentUnit.MoralePoints}/{currentUnit.Character.MoralePoints}</text>
                <text fill={'cyan'} x={115} y={105}>{currentUnit.SpecialPoints}/{currentUnit.Character.SpecialPoints}</text>
            </svg>
            {currentUnit.Character.CharClass.Perks.map((perk, index) => <svg y={590} x={200 + index * 150}>
                <image width={120} height={120} xlinkHref={`Resources/Skills/${perk.SkillFile}`}/>
                <text y={120} fill={'white'}>{perk.Name}</text>
                <text y={100} x={80} fill={'cyan'}>{perk.Cost}</text>
            </svg>)}
        </>}
    </svg>
}