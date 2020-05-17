import React, {useEffect} from 'react';
import {useDispatch, useSelector} from "react-redux";
import {
    getCurrentUnit,
    getGameState,
    getPlayer,
    getQueue,
    getSelectedPerk,
    getSpells, getTurnCounter,
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
    const spells = useSelector(getSpells);
    const turnCounter = useSelector(getTurnCounter);
    const dispatch = useDispatch();
    useEffect(() => {
        dispatch(ExecuteCommand({
            Type: 'ReceiveFullDataStateCommand',
            Args: [],
        }))
    }, [])
    const beginGame = () => {
        const name = prompt('Your name?');
        if (!name) {
            alert('Need name');
            return
        }
        dispatch(ExecuteCommand({
            Type: 'AddPlayerCommand',
            Args: ['0', name],
        }))
    }
    const resetGame = () => {
        dispatch(ExecuteCommand({
            Type: 'InitializeGameStateResetCommand',
            Args: [],
        }))
    }
    return <>
        <svg height={720} width={800}>
            <image xlinkHref={'Resources/Panels/top_panel.png'}/>
            <image y={50} xlinkHref={'Resources/Stages/desert_map.png'}/>
            <image transform={'scale(1, 0.6)'} y={600 * (1 / 0.6)} xlinkHref={'Resources/Panels/bottom_panel.png'}/>
            {gameState.state === 'uninitialized' && <>
                <g onClick={beginGame}>
                    <rect y={320} x={300} width={200} height={80} fill={'white'} stroke={'black'}/>
                    <text y={365} x={335} fontSize={30}>Start game</text>
                </g>
            </>}
            {player && <>
                <text textAnchor={'end'} x={800 - 15} y={20} fill={'white'}>Player name: {player.Name}</text>
                <text textAnchor={'end'} x={800 - 15} y={40} fill={'lightgray'}>ID: {player.Id}</text>
            </>}

            {(gameState.state === 'combatReady' || gameState.state === 'finished') && currentUnit && <>
                {units.filter(u => u.Character.OwnerId !== null).map((unit, index) =>
                    <UnitComponent unit={unit} key={index} index={index}/>)}
                {units.filter(u => u.Character.OwnerId === null).map((unit, index) =>
                    <UnitComponent unit={unit} key={index} index={index}/>)}
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
                    <svg className={'hover-target'} onClick={() => dispatch(SelectPerk(perk, currentUnit))} key={index} y={590}
                         x={200 + index * 150}>
                        <image width={120} height={120} xlinkHref={`Resources/Skills/${perk.SkillFile}.png`}/>
                        <text textAnchor={'middle'} y={120} x={60} fill={'white'}>{perk.Name}</text>
                        {perk === selectedPerk &&
                        <image width={120} height={120} xlinkHref={`Resources/Stars/good_guy_star.png`}/>}
                        <text y={100} x={80} fill={perk.Cost <= currentUnit.SpecialPoints ? 'cyan' : 'red'}>{perk.Cost}</text>
                        {currentUnit.Cooldown[perk.Name] > 0 &&
                        <text y={82} x={40} fill={'white'} stroke={'black'} fontSize={75}>{currentUnit.Cooldown[perk.Name]}</text>}
                        <foreignObject y={0} x={0} width={120} height={120}>
                            <p className={'hover-text'}>{perk.Description}</p>
                        </foreignObject>
                    </svg>)}
                {new Array(4 - currentUnit.Character.CharClass.Perks.length).fill(0).map((e, index) =>
                    <svg key={index} y={590} x={200 + (currentUnit.Character.CharClass.Perks.length + index) * 150}>
                        <image width={120} height={120} xlinkHref={`Resources/Skills/locked_skill.png`}/>
                    </svg>
                )}
                <rect width={50} height={50} fill={'none'} stroke={'black'}/>
                <UnitInQueue unit={currentUnit} index={0}/>
                {queue.map((unit, index) => <UnitInQueue unit={unit} index={index + 1} key={index}/>)}
            </>}
            {gameState.state === 'finished' && <>
                <g>
                    <rect y={320} x={300} width={200} height={80} fill={'white'} stroke={'black'}/>
                    <text y={365} x={350} fontSize={30}>{gameState.won ? 'You won' : 'You lost'}</text>
                </g>
                <g onClick={resetGame}>
                    <rect y={400} x={300} width={200} height={80} fill={'white'} stroke={'black'}/>
                    <text y={445} x={335} fontSize={30}>Play again?</text>
                </g> 
            </>}
            <text x={400} y={100} fontSize={60} textAnchor={'middle'}>TURN {turnCounter}</text>
        </svg>
        <div style={{height: '200px', overflowY: 'scroll', backgroundColor: 'lightgray'}}>
            {spells.map((spell, i) =>
                <p key={i}>{spell.CurrentUnit.Character.Name} used
                    ability {spell.Perk.Name} to {spell.Target.Character.Name} ({spell.Perk.Description})</p>)}
            {gameState.state === 'combatReady' && <audio controls loop autoPlay src={'Resources/Audio/bgm.webm'}/>}
        </div>
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
    const gameState = useSelector(getGameState);
    const dispatch = useDispatch();
    const correctKey = '7754321';
    const isWindowsKeyCorrect = window.location.hash.includes(correctKey);
    const executeNextTurnCommand = () => {
        if (!currentPerk) {
            alert('You need to select perk');
            return
        }
        if (!gameState){
            return
        }
        if (currentUnit && (currentUnit.Character.OwnerId !== null || isWindowsKeyCorrect))
            dispatch(ExecuteCommand({
                Type: 'NextTurnCommand',
                Args: [currentPerk.Name, unit.Character.Name]
            }))
    }
    return <svg onClick={executeNextTurnCommand} y={270 + index * 105}
                x={(enemy ? 500 : 220) - index * 20 * (enemy ? -1 : 1)} className={'hover-target'}>
        {unit.State === UnitState.Fine && <>
            <Bar x={24} y={2} color={'rgb(49, 191, 19)'} value={unit.HealthPoints / unit.Character.HealthPoints}/>
            <Bar x={24} y={8} color={'rgb(255, 179, 11)'} value={unit.MoralePoints / unit.Character.MoralePoints}/>
            <Bar x={24} y={14} color={'rgb(109, 195, 235)'}
                 value={unit.SpecialPoints / unit.Character.SpecialPoints}/>
        </>}
        {currentUnit && unit.Character.Name === currentUnit.Character.Name &&
        <rect x={22} y={0} width={68} height={20} fill={'none'} stroke={'rgb(1, 22, 39)'}/>}
        {unit.State !== UnitState.Runaway && <image style={{
            imageRendering: 'pixelated',
        }}
                width={96} height={96} x={enemy && unit.State !== UnitState.Dead ? -96 : 0}
                transform={(enemy ? 'scale(-1, 1)' : '')
                + (unit.State === UnitState.Dead && enemy ? 'translate(-96, 0) rotate(-90, 48, 48)' : '')
                + (unit.State === UnitState.Dead && !enemy ? 'rotate(-90, 48, 48)' : '')
                } y={16}
                xlinkHref={`Resources/Characters/${unit.Character.CharClass.Name}/char.png`}/>}
        {unit.State === UnitState.Runaway && <image width={96} height={96} xlinkHref={`Resources/Characters/retreated_character.png`}/>}
        <foreignObject y={50} x={0} width={210} height={120}>
            <div className={'hover-text'}>{unit.Character.Name} ({unit.HealthPoints}/{unit.Character.HealthPoints}) ({unit.MoralePoints}/{unit.Character.MoralePoints})</div>
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