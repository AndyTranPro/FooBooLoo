import sessionReducer, {
    beginSession,
    sendAnswer,
    SessionState,
    endSession,
    retrieveResults,
} from '../features/sessionSlice';
import {
    startSession,
    submitAnswer,
    getResults,
} from '../api/api';

jest.mock('../api/api');

describe('sessionSlice', () => {
    let initialState: SessionState;

    beforeEach(() => {
        initialState = {
            sessionId: null,
            currentNumber: null,
            numQuestions: 0,
            score: 0,
            status: 'idle',
            duration: 0,
            error: null,
        };
    });

    describe('reducers and extraReducers', () => {
        it('should handle initial state', () => {
            expect(sessionReducer(undefined, { type: 'unknown' })).toEqual(initialState);
        });

        it('should handle endSession action', () => {
            const nextState = sessionReducer(initialState, endSession());
            expect(nextState.status).toEqual('ended');
        });

        it('should handle beginSession.fulfilled', () => {
            const payload = {
                sessionId: 1,
                duration: 60,
                sessionNumbers: [{ numberServed: 42 }],
            };
            const nextState = sessionReducer(initialState, beginSession.fulfilled(payload, '', { gameId: 1, playerName: 'John Doe', duration: 60 }));
            expect(nextState.sessionId).toEqual(payload.sessionId);
            expect(nextState.status).toEqual('in-progress');
            expect(nextState.duration).toEqual(payload.duration);
            expect(nextState.score).toEqual(0);
            expect(nextState.numQuestions).toEqual(0);
            expect(nextState.currentNumber).toEqual(payload.sessionNumbers[0].numberServed);
            expect(nextState.error).toBeNull();
        });

        it('should handle sendAnswer.fulfilled', () => {
            const payload = 42;
            const nextState = sessionReducer(initialState, sendAnswer.fulfilled(payload, '', { sessionId: 1, number: 42, answer: 'Fizz' }));
            expect(nextState.currentNumber).toEqual(payload);
        });

        it('should handle retrieveResults.fulfilled', () => {
            const payload = {
                finalScore: 10,
                totalQuestions: 15,
            };
            const nextState = sessionReducer(initialState, retrieveResults.fulfilled(payload, '', 1));
            expect(nextState.status).toEqual('ended');
            expect(nextState.score).toEqual(payload.finalScore);
            expect(nextState.numQuestions).toEqual(payload.totalQuestions - 1);
        });
    });

    describe('async actions', () => {
        it('beginSession should dispatch correct actions on success', async () => {
            const mockResponse = { sessionId: 1, duration: 60, sessionNumbers: [{ numberServed: 42 }] };
            (startSession as jest.Mock).mockResolvedValueOnce(mockResponse);

            const dispatch = jest.fn();
            const thunk = beginSession({ gameId: 1, playerName: 'John Doe', duration: 60 });

            await thunk(dispatch, () => ({ session: initialState }), null);

            const [pending, fulfilled] = dispatch.mock.calls;

            expect(pending[0].type).toEqual(beginSession.pending.type);
            expect(fulfilled[0].type).toEqual(beginSession.fulfilled.type);
            expect(fulfilled[0].payload).toEqual(mockResponse);
        });

        it('sendAnswer should dispatch correct actions on success', async () => {
            const mockResponse = 42;
            (submitAnswer as jest.Mock).mockResolvedValueOnce(mockResponse);

            const dispatch = jest.fn();
            const thunk = sendAnswer({ sessionId: 1, number: 42, answer: 'Fizz' });

            await thunk(dispatch, () => ({ session: initialState }), null);

            const [pending, fulfilled] = dispatch.mock.calls;

            expect(pending[0].type).toEqual(sendAnswer.pending.type);
            expect(fulfilled[0].type).toEqual(sendAnswer.fulfilled.type);
            expect(fulfilled[0].payload).toEqual(mockResponse);
        });

        it('retrieveResults should dispatch correct actions on success', async () => {
            const mockResults = { finalScore: 10, totalQuestions: 15 };
            (getResults as jest.Mock).mockResolvedValueOnce(mockResults);

            const dispatch = jest.fn();
            const thunk = retrieveResults(1);

            await thunk(dispatch, () => ({ session: initialState }), null);

            const [pending, fulfilled] = dispatch.mock.calls;

            expect(pending[0].type).toEqual(retrieveResults.pending.type);
            expect(fulfilled[0].type).toEqual(retrieveResults.fulfilled.type);
            expect(fulfilled[0].payload).toEqual(mockResults);
        });
    });
});