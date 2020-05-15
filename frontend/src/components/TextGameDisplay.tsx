import React from 'react';
import {useSelector} from "react-redux";
import {getCharacters, getCurrentUnit, getGameState, getPlayer, getQueue, getUnits} from "../store/selectors";

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

export function TextGameDisplay(props: any) {
    const player = useSelector(getPlayer);
    const gameState = useSelector(getGameState);
    const characters = useSelector(getCharacters);
    const units = useSelector(getUnits);
    const queue = useSelector(getQueue);
    const currentUnit = useSelector(getCurrentUnit);

    return <div style={{
        backgroundColor: 'rgb(245, 245, 245)',
        overflowY: 'scroll',
        height: '400px'
    }}>
        <p>Состояние:</p>
        <div style={{marginLeft: '15px'}}>
            <p>Игра находится в состоянии "{gameStateNames[gameState.state]}".
                {gameState.state === "finished" && `Результат боя: ${gameState.won ? 'Победа' : 'Билли Бонс умер...'}`}</p>
            {player && <p>Игрок {player.Name} с ID {player.Id}</p>}
        </div>
        {(gameState.state === 'stateReady' || gameState.state === 'combatReady' || gameState.state === 'finished') && <>
            <p>Персонажи:</p>
            <div style={{marginLeft: '15px'}}>
                {characters.filter(c => c !== null).map((c, i) => <div key={i}>
                    <p>Имя: {c.Name}</p>
                    <div style={{marginLeft: '15px'}}>
                        <p>Класс: {c.CharClass.Name}</p>
                        <p>Здоровье: {c.HealthPoints}</p>
                        <p>Инициатива: {c.Initiative}</p>
                        <p>Мораль: {c.MoralePoints}</p>
                        <p>Очки способностей: {c.SpecialPoints}</p>
                    </div>
                </div>)}
            </div>
        </>}
        {(gameState.state === 'combatReady' || gameState.state === 'finished') && currentUnit && <>
            <p>Текущий юнит: {currentUnit.Character.Name}</p>
            <p>Спосбности:</p>
            <div style={{marginLeft: '15px'}}>
                {currentUnit.Character.CharClass.Perks.map((p, i) => <div key={i}>
                    <p>Название: {p.Name}</p>
                    <div style={{marginLeft: '15px'}}>
                        <p>Стоимость: {p.Cost}</p>
                    </div>
                </div>)}
            </div>
        </>}
        {(gameState.state === 'combatReady' || gameState.state === 'finished') && <>
            <p>Юниты:</p>
            <div style={{marginLeft: '15px'}}>
                {units.map((u, i) => <div key={i}>
                    <p>Имя: {u.Character.Name}</p>
                    <div style={{marginLeft: '15px'}}>
                        <p>Класс: {u.Character.CharClass.Name}</p>
                        <p>Текущее здоровье: {u.HealthPoints}</p>
                        <p>Текущая инициатива: {u.Initiative}</p>
                        <p>Текущая мораль: {u.MoralePoints}</p>
                        <p>Текущие очки способностей: {u.SpecialPoints}</p>
                        <p>Состояние: {unitStates[u.State]}</p>
                    </div>
                </div>)}
            </div>
        </>}
        {(gameState.state === 'combatReady' || gameState.state === 'finished') && <>
            <p>Очередь:</p>
            <div style={{marginLeft: '15px'}}>
                {queue.map((u, i) => <span key={i}>{u.Character.Name}, </span>)}
            </div>
        </>}

    </div>
}