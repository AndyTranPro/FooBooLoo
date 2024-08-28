import MockAdapter from 'axios-mock-adapter';
import {
    createGame,
    getGames,
    startSession,
    getSession,
    submitAnswer,
    getResults,
    api
} from '../api/api';

const mock = new MockAdapter(api);

describe('API utility functions', () => {
    afterEach(() => {
        mock.reset();
    });

    test('createGame should post game data and return response data', async () => {
        const mockResponse = { data: { id: 1, name: 'Test Game' } };
        mock.onPost('/games').reply(200, mockResponse);

        const result = await createGame('Test Game', 'John Doe', 1, 100, [{ divisor: 3, replacement: 'Fizz' }, { divisor: 5, replacement: 'Buzz' }]);

        expect(result).toEqual(mockResponse.data);
        expect(mock.history.post.length).toBe(1);
        const formData = mock.history.post[0].data;
        expect(formData.get('name')).toEqual('Test Game');
        expect(formData.get('author')).toEqual('John Doe');
        expect(formData.get('min')).toEqual('1');
        expect(formData.get('max')).toEqual('100');
        expect(formData.getAll('RuleSet')).toEqual([
            JSON.stringify({ divisor: 3, replacement: 'Fizz' }),
            JSON.stringify({ divisor: 5, replacement: 'Buzz' }),
        ]);
    });

    test('getGames should fetch games and return response data', async () => {
        const mockResponse = { data: [{ id: 1, name: 'Test Game' }] };
        mock.onGet('/games').reply(200, mockResponse);

        const result = await getGames();

        expect(result).toEqual(mockResponse.data);
        expect(mock.history.get.length).toBe(1);
    });

    test('startSession should post session data and return response data', async () => {
        const mockResponse = { data: { sessionId: 1, gameId: 1 } };
        mock.onPost('/sessions').reply(200, mockResponse);

        const result = await startSession(1, 'John Doe', 60);

        expect(result).toEqual(mockResponse.data);
        expect(mock.history.post.length).toBe(1);
        expect(JSON.parse(mock.history.post[0].data)).toEqual({
            gameId: 1,
            playerName: 'John Doe',
            duration: 60,
        });
    });

    test('getSession should fetch session data by sessionId and return response data', async () => {
        const mockResponse = { data: { sessionId: 1, gameId: 1 } };
        mock.onGet('/sessions/1').reply(200, mockResponse);

        const result = await getSession(1);

        expect(result).toEqual(mockResponse.data);
        expect(mock.history.get.length).toBe(1);
    });

    test('submitAnswer should post answer and return response data', async () => {
        const mockResponse = { data: { correct: true } };
        mock.onPost('/sessions/1/submit-answer').reply(200, mockResponse);

        const result = await submitAnswer(1, 15, 'FizzBuzz');

        expect(result).toEqual(mockResponse.data);
        expect(mock.history.post.length).toBe(1);
        expect(JSON.parse(mock.history.post[0].data)).toEqual({
            number: 15,
            answer: 'FizzBuzz',
        });
    });

    test('getResults should fetch results by sessionId and return response data', async () => {
        const mockResponse = { data: { score: 10, total: 15 } };
        mock.onGet('/sessions/1/results').reply(200, mockResponse);

        const result = await getResults(1);

        expect(result).toEqual(mockResponse.data);
        expect(mock.history.get.length).toBe(1);
    });
});