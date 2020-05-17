import React, {useEffect} from 'react';
import {useDispatch, useSelector} from "react-redux";
import {
    // getCharacters,
    getCurrentUnit,
    getGameState,
    getPlayer,
    getQueue,
    getSelectedPerk,
    getUnits
} from "../store/selectors";
import {ExecuteCommand, SelectPerk} from "../store/actions";
import {Unit, UnitState} from "../interfaces/GameInterfaces";

// const gameStateNames = {
//     uninitialized: 'Не инициализированно',
//     stateReady: 'Подготовка к бою',
//     combatReady: 'Бой идет',
//     finished: 'Бой окончен'
// }
//
// const unitStates = [
//     'Активен',
//     'Сбежал',
//     'Мертв',
// ]

export function GraphicalGameDisplay(props: any) {
    const player = useSelector(getPlayer);
    const gameState = useSelector(getGameState);
    // const characters = useSelector(getCharacters);
    const units = useSelector(getUnits);
    const queue = useSelector(getQueue);
    const currentUnit = useSelector(getCurrentUnit);
    const selectedPerk = useSelector(getSelectedPerk);
    const dispatch = useDispatch();
    useEffect(() => {
        dispatch(ExecuteCommand({
            Type: 'ReceiveFullDataStateCommand',
            Args: [],
        }))
    }, [])

    return <>
        <svg height={850} width={800}>
            <image xlinkHref={'Resources/Panels/top_panel.png'}/>
            <image y={50} xlinkHref={'Resources/Stages/desert_map.png'}/>
            <image transform={'scale(1, 0.6)'} y={600 * (1 / 0.6)} xlinkHref={'Resources/Panels/bottom_panel.png'}/>

            {player && <>
                <text textAnchor={'end'} x={800 - 15} y={20} fill={'white'}>Player name: {player.Name}</text>
                <text textAnchor={'end'} x={800 - 15} y={40} fill={'lightgray'}>ID: {player.Id}</text>
            </>}

            {(gameState.state === 'combatReady' || gameState.state === 'finished') && currentUnit && <>
                <svg y={600}>
                    <image width={128} height={128}
                           xlinkHref={`Resources/Characters/${currentUnit.Character.CharClass.Name}/prev.png`}/>
                    <text fill={'white'} x={115} y={25}>{currentUnit.Character.Name}</text>
                    <text fill={'white'} x={115} y={45}>{currentUnit.Character.CharClass.Name}</text>
                    <text fill={'pink'} x={115}
                          y={65}>{currentUnit.HealthPoints}/{currentUnit.Character.HealthPoints}</text>
                    <text fill={'yellow'} x={115}
                          y={85}>{currentUnit.MoralePoints}/{currentUnit.Character.MoralePoints}</text>
                    <text fill={'cyan'} x={115}
                          y={105}>{currentUnit.SpecialPoints}/{currentUnit.Character.SpecialPoints}</text>
                </svg>
                {currentUnit.Character.CharClass.Perks.map((perk, index) =>
                    <svg className={'hover-target'} onClick={() => dispatch(SelectPerk(perk))} key={index} y={590}
                         x={200 + index * 150}>
                        <image width={120} height={120} xlinkHref={`Resources/Skills/${perk.SkillFile}.png`}/>
                        <text textAnchor={'middle'} y={120} x={60} fill={'white'}>{perk.Name}</text>
                        {perk === selectedPerk &&
                        <image width={120} height={120} xlinkHref={`Resources/Stars/good_guy_star.png`}/>}
                        <text y={100} x={80} fill={'cyan'}>{perk.Cost}</text>
                        <foreignObject y={0} x={0} width={120} height={120}>
                            <p className={'hover-text'}>{perk.Description}</p>
                        </foreignObject>
                    </svg>)}
                {new Array(4 - currentUnit.Character.CharClass.Perks.length).fill(0).map((e, index) =>
                    <svg key={index} y={590} x={200 + (currentUnit.Character.CharClass.Perks.length + index) * 150}>
                        <image width={120} height={120} xlinkHref={`Resources/Skills/locked_skill.png`}/>
                    </svg>)}
                {units.filter(u => u.Character.OwnerId !== null).map((unit, index) =>
                    <UnitComponent unit={unit} key={index} index={index}/>)}
                {units.filter(u => u.Character.OwnerId === null).map((unit, index) =>
                    <UnitComponent unit={unit} key={index} index={index}/>)}
                <rect width={50} height={50} fill={'none'} stroke={'black'}/>
                <UnitInQueue unit={currentUnit} index={0}/>
                {queue.map((unit, index) => <UnitInQueue unit={unit} index={index + 1} key={index}/>)}
            </>}
        </svg>
    </>
}

interface UnitProps {
    unit: Unit,
    index: number,
}

function UnitComponent({unit, index}: UnitProps) {
    const enemy = unit.Character.OwnerId === null;
    const currentPerk = useSelector(getSelectedPerk);
    const currentUnit = useSelector(getCurrentUnit);
    const dispatch = useDispatch();
    const executeNextTurnCommand = () => {
        if (!currentPerk) {
            alert('You need to select perk');
            return
        }
        dispatch(ExecuteCommand({
            Type: 'NextTurnCommand',
            Args: [currentPerk.Name, unit.Character.Name]
        }))
    }
    return <svg onClick={executeNextTurnCommand} y={320 + index * 90}
                x={(enemy ? 500 : 220) - index * 20 * (enemy ? -1 : 1)} className={'hover-target'}>
        <Bar x={16} y={2} color={'rgb(49, 191, 19)'} value={unit.HealthPoints / unit.Character.HealthPoints}/>
        <Bar x={16} y={8} color={'rgb(255, 179, 11)'} value={unit.MoralePoints / unit.Character.MoralePoints}/>
        <Bar x={16} y={14} color={'rgb(109, 195, 235)'} value={unit.SpecialPoints / unit.Character.SpecialPoints}/>
        {currentUnit && unit.Character.Name === currentUnit.Character.Name &&
        <rect x={14} y={0} width={68} height={20} fill={'none'} stroke={'rgb(1, 22, 39)'}/>}
        <image className={'unit'} style={{
            imageRendering: 'pixelated',
            opacity: unit.State === UnitState.Runaway ? 0 : 1,
        }}
               width={96} height={96} x={enemy && unit.State !== UnitState.Dead ? -96 : 0}
               transform={(enemy ? 'scale(-1, 1)' : '')
               + (unit.State === UnitState.Dead && enemy ? 'translate(-96, 0) rotate(-90, 48, 48)' : '')
               + (unit.State === UnitState.Dead && !enemy ? 'rotate(-90, 48, 48)' : '')
               }
               xlinkHref={`Resources/Characters/${unit.Character.CharClass.Name}/char.png`}/>
        <foreignObject y={50} x={0} width={120} height={120}>
            <p className={'hover-text'}>{unit.Character.Name}</p>
        </foreignObject>
    </svg>
}

interface BarProps {
    x: number,
    y: number,
    color: string,
    value: number,
}

function Bar({x, y, color, value}: BarProps) {
    return <>
        <rect stroke={'black'} height={5} width={64} x={x} y={y} fill={'rgb(102, 102, 102)'}/>
        <rect height={5} width={64 * (value >= 0 ? value : 0)} x={x} y={y} fill={color}/>
    </>
}

function UnitInQueue({unit, index}: UnitProps) {
    const enemy = unit.Character.OwnerId === null;
    return <svg className={'hover-target'} x={-4 + index * 54}>
        <image y={-4} width={60} height={60} transform={enemy ? 'scale(-1, 1)' : ''} x={enemy ? -60 : 0}
               xlinkHref={`Resources/Characters/${unit.Character.CharClass.Name}/prev.png`}/>
        <image y={-4} width={60} height={60} transform={enemy ? 'scale(-1, 1)' : ''} x={enemy ? -60 : 0}
               xlinkHref={enemy ? 'Resources/Stars/bad_guy_star.png' : 'Resources/Stars/good_guy_star.png'}/>
        <foreignObject y={50} x={0} width={120} height={120}>
            <p className={'hover-text'}>{unit.Character.Name}</p>
        </foreignObject>
    </svg>
}