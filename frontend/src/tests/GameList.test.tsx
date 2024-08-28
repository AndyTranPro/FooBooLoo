import React from 'react';
import { render, screen, waitFor } from '@testing-library/react';
import '@testing-library/jest-dom';
import { Provider } from 'react-redux';
import configureStore from 'redux-mock-store';
import GameList from '../components/GameList';
import { fetchGames } from '../features/gamesSlice';
import { MemoryRouter } from 'react-router-dom';

jest.mock('../features/gamesSlice', () => ({
    fetchGames: jest.fn(() => ({ type: 'FETCH_GAMES' })),
}));

const mockStore = configureStore([]);

const renderComponent = (store: any) => {
    return render(
        <Provider store={store}>
            <MemoryRouter>
                <GameList />
            </MemoryRouter>
        </Provider>
    );
};

describe('GameList Component', () => {
    let store: any;

    beforeEach(() => {
        store = mockStore({
            games: {
                games: [
                    { gameId: '1', name: 'Game 1' },
                    { gameId: '2', name: 'Game 2' },
                ],
            },
        });
        store.dispatch = jest.fn();
    });

    test('renders correctly and matches snapshot', () => {
        const { asFragment } = renderComponent(store);
        expect(asFragment()).toMatchSnapshot();
    });

    test('dispatches fetchGames action on component mount', () => {
        renderComponent(store);
        expect(store.dispatch).toHaveBeenCalledWith(fetchGames());
    });

    test('displays a list of games', () => {
        renderComponent(store);

        expect(screen.getByText(/Available Games/i)).toBeInTheDocument();
        expect(screen.getByText('Game 1')).toBeInTheDocument();
        expect(screen.getByText('Game 2')).toBeInTheDocument();
    });

    test('renders game buttons with correct links', () => {
        renderComponent(store);

        const game1Link = screen.getByRole('link', { name: /Game 1/i });
        const game2Link = screen.getByRole('link', { name: /Game 2/i });

        expect(game1Link).toHaveAttribute('href', '/games/1');
        expect(game2Link).toHaveAttribute('href', '/games/2');
    });

    test('handles empty game list correctly', async () => {
        store = mockStore({
            games: {
                games: [],
            },
        });

        renderComponent(store);

        await waitFor(() => {
            expect(screen.getByText(/Available Games/i)).toBeInTheDocument();
            expect(screen.queryByRole('link')).not.toBeInTheDocument();
        });
    });
});
