import { createSlice, createAsyncThunk } from '@reduxjs/toolkit';
import { 
    startSession,
    submitAnswer,
    getResults
} from '../api/api';

// A feature to manage the session state

export interface SessionState {
    sessionId: number | null;
    currentNumber: number | null;
    numQuestions: number;
    score: number;
    status: 'idle' | 'in-progress' | 'ended';
    duration: number;
    error: string | null;
}

// The initial state of the session slice
const initialState: SessionState = {
    sessionId: null,
    currentNumber: null,
    numQuestions: 0,
    score: 0,
    status: 'idle',
    duration: 0,
    error: null,
};

// begins a new session
export const beginSession = createAsyncThunk('session/beginSession', async (sessionData: { gameId: number; playerName: string; duration: number }) => {
    const response = await startSession(sessionData.gameId, sessionData.playerName, sessionData.duration);
    return response;
});

// sends the answer to the server
export const sendAnswer = createAsyncThunk('session/sendAnswer', async (answerData: { sessionId: number; number: number; answer: string }) => {
    const response = await submitAnswer(answerData.sessionId, answerData.number, answerData.answer);
    return response;
});

// get the results of the session
export const retrieveResults = createAsyncThunk('session/retrieveResults', async (sessionId: number) => {
    const results = await getResults(sessionId);
    return results;
});

// The session slice contains the session data and the status of the request
const sessionSlice = createSlice({
    name: 'session',
    initialState,
    reducers: {
        endSession(state) {
            state.status = 'ended';
        },
    },
    extraReducers: (builder) => {
        builder
            .addCase(beginSession.fulfilled, (state, action) => {
                state.sessionId = action.payload.sessionId;
                state.status = 'in-progress';
                state.score = 0;
                state.numQuestions = 0;
                state.duration = action.payload.duration;
                state.currentNumber = action.payload.sessionNumbers[0].numberServed;
                state.error = null;
            })
            .addCase(sendAnswer.fulfilled, (state, action) => {
                state.currentNumber = action.payload;
            })
            .addCase(retrieveResults.fulfilled, (state, action) => {
                state.status = 'ended';
                state.score = action.payload.finalScore;
                state.numQuestions = action.payload.totalQuestions - 1;
            });
    },
});

export const { endSession } = sessionSlice.actions;
export default sessionSlice.reducer;
