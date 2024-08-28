import gamesReducer, { fetchGames, addGame, Game, GamesState } from '../features/gamesSlice';
import { getGames, createGame } from '../api/api';

jest.mock('../api/api');

describe('gamesSlice', () => {
    let initialState: GamesState;

    beforeEach(() => {
        initialState = {
            games: [],
            status: 'idle',
        };
    });

    describe('reducers and extraReducers', () => {
        it('should handle initial state', () => {
            expect(gamesReducer(undefined, { type: 'unknown' })).toEqual(initialState);
        });

        it('should handle fetchGames.pending', () => {
            const nextState = gamesReducer(initialState, fetchGames.pending('', undefined));
            expect(nextState.status).toEqual('loading');
        });

        it('should handle fetchGames.fulfilled', () => {
            const mockGames: Game[] = [
                { gameId: 1, name: 'Game 1', author: 'Author 1', min: 1, max: 10, rules: [[{ divisor: 3, replacement: 'Fizz' }]] },
                { gameId: 2, name: 'Game 2', author: 'Author 2', min: 1, max: 10, rules: [[{ divisor: 5, replacement: 'Buzz' }]] },
            ];

            const nextState = gamesReducer(
                initialState,
                fetchGames.fulfilled(mockGames, '', undefined)
            );
            expect(nextState.status).toEqual('idle');
            expect(nextState.games).toEqual(mockGames);
        });

        it('should handle fetchGames.rejected', () => {
            const nextState = gamesReducer(initialState, fetchGames.rejected(null, '', undefined));
            expect(nextState.status).toEqual('failed');
        });

        it('should handle addGame.fulfilled', () => {
            const newGame = { gameId: 3, name: 'Game 3', author: 'Author 3', min: 1, max: 100, rules: [{ divisor: 7, replacement: 'Bizz' }] };

            const nextState = gamesReducer(initialState, addGame.fulfilled(newGame, '', newGame));
            expect(nextState.games).toContainEqual(newGame);
        });
    });

    describe('async actions', () => {
        it('fetchGames should dispatch correct actions on success', async () => {
            const mockGames = { $values: [{ gameId: 1, name: 'Game 1', author: 'Author 1', min: 1, max: 10, rules: [[{ divisor: 3, replacement: 'Fizz' }]] }] };
            (getGames as jest.Mock).mockResolvedValueOnce(mockGames);

            const dispatch = jest.fn();
            const thunk = fetchGames();

            await thunk(dispatch, () => ({ games: initialState }), null);

            const [pending, fulfilled] = dispatch.mock.calls;

            expect(pending[0].type).toEqual(fetchGames.pending.type);
            expect(fulfilled[0].type).toEqual(fetchGames.fulfilled.type);
            expect(fulfilled[0].payload).toEqual(mockGames);
        });

        it('fetchGames should dispatch correct actions on failure', async () => {
            (getGames as jest.Mock).mockRejectedValueOnce(new Error('Network Error'));

            const dispatch = jest.fn();
            const thunk = fetchGames();

            await thunk(dispatch, () => ({ games: initialState }), null);

            const [pending, rejected] = dispatch.mock.calls;

            expect(pending[0].type).toEqual(fetchGames.pending.type);
            expect(rejected[0].type).toEqual(fetchGames.rejected.type);
        });

        it('addGame should dispatch correct actions on success', async () => {
            const newGame = { gameId: 3, name: 'Game 3', author: 'Author 3', min: 1, max: 100, rules: [{ divisor: 7, replacement: 'Bizz' }] };
            (createGame as jest.Mock).mockResolvedValueOnce(newGame);

            const dispatch = jest.fn();
            const thunk = addGame(newGame);

            await thunk(dispatch, () => ({ games: initialState }), null);

            const [pending, fulfilled] = dispatch.mock.calls;

            expect(pending[0].type).toEqual(addGame.pending.type);
            expect(fulfilled[0].type).toEqual(addGame.fulfilled.type);
            expect(fulfilled[0].payload).toEqual(newGame);
        });
    });
});