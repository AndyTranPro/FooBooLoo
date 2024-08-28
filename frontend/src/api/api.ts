import axios from 'axios';

const API_BASE_URL = 'http://localhost:5054/api';

export const api = axios.create({
    baseURL: API_BASE_URL,
});

export const createGame = async (name: string, author: string, min: number, max: number, rules: { divisor: number, replacement: string }[]) => {
    const formData = new FormData();
    formData.append('name', name);
    formData.append('author', author);
    formData.append('min', min.toString());
    formData.append('max', max.toString());

    rules.forEach((rule) => {
        formData.append(`RuleSet`, JSON.stringify(rule));
    });
    const response = await api.post('/games', formData, {
        headers: {
            'Content-Type': 'multipart/form-data',
        },
    });
    return response.data.data;
};

export const getGames = async () => {
    const response = await api.get('/games');
    return response.data.data;
};

export const startSession = async (gameId: number, playerName: string, duration: number) => {
    const response = await api.post('/sessions', { gameId, playerName, duration });
    return response.data.data;
};

export const getSession = async (sessionId: number) => {
    const response = await api.get(`/sessions/${sessionId}`);
    return response.data.data;
};

export const submitAnswer = async (sessionId: number, number: number, answer: string) => {
    const response = await api.post(`/sessions/${sessionId}/submit-answer`, { number, answer });
    return response.data.data;
};

export const getResults = async (sessionId: number) => {
    const response = await api.get(`/sessions/${sessionId}/results`);
    return response.data.data;
}
