export interface Player {
    Name: string
    Id: string
}

export interface Character {
    Name: string
    CharClass: CharacterClass
    Level: number
    Initiative: number
    HealthPoints: number
    MoralePoints: number
    SpecialPoints: number
    OwnerId: string
}

export interface CharacterClass {
    Name: string
    HealthPoints: number
    MoralePoints: number
    SpecialPoints: number
    Initiative: number
    Perks: Perk[]
}

export interface Perk {
    Name: string
    SkillFile: string
    Description: string
    Cost: number
    Cooldown: number
    LevelToUnlock: number
}

export interface Unit {
    Character: Character
    HealthPoints: number
    MoralePoints: number
    SpecialPoints: number
    Initiative: number
    State: UnitState
    Cooldown: {
        [key: string]: number
    }
}

export enum UnitState {
    Fine = 0,
    Runaway = 1,
    Dead = 2,
}