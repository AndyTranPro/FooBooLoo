import { createSlice, createAsyncThunk } from '@reduxjs/toolkit';
import { getGames, createGame } from '../api/api';

export interface RuleSet {
    divisor: number;
    replacement: string;
}

export interface Game {
    gameId: number;
    name: string;
    author: string;
    min: number;
    max: number;
    rules: { divisor: number; replacement: string }[][];
}

// The games state contains the list of games and the status of the request
export interface GamesState {
    games: Game[];
    status: 'idle' | 'loading' | 'failed';
}

// The initial state of the games slice
const initialState: GamesState = {
    games: [],
    status: 'idle'
};

// fetches the list of games from the server
export const fetchGames = createAsyncThunk('games/fetchGames', async () => {
    const response = await getGames();
    return response;
});

// adds a new game to the server
export const addGame = createAsyncThunk('games/addGame', async (newGame: { name: string; author: string; min: number, max: number, rules: { divisor: number; replacement: string }[] }) => {
    const response = await createGame(newGame.name, newGame.author, newGame.min, newGame.max, newGame.rules);
    return response;
});

// The games slice contains the list of games and the status of the request
const gamesSlice = createSlice({
    name: 'games',
    initialState,
    reducers: {},
    extraReducers: (builder) => {
        builder
            .addCase(fetchGames.pending, (state) => {
                state.status = 'loading';
            })
            .addCase(fetchGames.fulfilled, (state, action) => {
                state.status = 'idle';
                state.games = action.payload;
            })
            .addCase(fetchGames.rejected, (state) => {
                state.status = 'failed';
            })
            .addCase(addGame.fulfilled, (state, action) => {
                state.games.push(action.payload);
            });
    },
});

export default gamesSlice.reducer;
