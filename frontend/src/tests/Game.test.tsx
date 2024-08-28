import React from 'react';
import { render, screen, fireEvent } from '@testing-library/react';
import '@testing-library/jest-dom';
import { Provider } from 'react-redux';
import configureStore from 'redux-mock-store';
import { MemoryRouter, useNavigate, useParams } from 'react-router-dom';
import Game from '../pages/Game';

jest.mock('react-router-dom', () => ({
    ...jest.requireActual('react-router-dom'),
    useNavigate: jest.fn(),
    useParams: jest.fn(),
}));

jest.mock('../features/sessionSlice', () => ({
    beginSession: jest.fn(),
}));

const mockStore = configureStore([]);
const mockNavigate = useNavigate as jest.Mock;
const mockUseParams = useParams as jest.Mock;

const renderComponent = (store: any, route: string) => {
    return render(
        <Provider store={store}>
            <MemoryRouter initialEntries={[route]}>
                <Game />
            </MemoryRouter>
        </Provider>
    );
};

describe('Game Component', () => {
    let store: any;

    beforeEach(() => {
        store = mockStore({});
        store.dispatch = jest.fn();
        mockNavigate.mockReset();
        mockUseParams.mockReturnValue({ gameId: '1' });
    });

    test('renders correctly and matches snapshot', () => {
        const { asFragment } = renderComponent(store, '/games/1');
        expect(asFragment()).toMatchSnapshot();
    });

    test('renders player name and duration input fields', () => {
        renderComponent(store, '/games/1');

        expect(screen.getByLabelText(/Player Name/i)).toBeInTheDocument();
        expect(screen.getByLabelText(/Duration \(seconds\)/i)).toBeInTheDocument();
    });

    test('updates player name and duration when inputs change', () => {
        renderComponent(store, '/games/1');

        const playerNameInput = screen.getByLabelText(/Player Name/i);
        const durationInput = screen.getByLabelText(/Duration \(seconds\)/i);

        fireEvent.change(playerNameInput, { target: { value: 'John Doe' } });
        fireEvent.change(durationInput, { target: { value: 120 } });

        expect(playerNameInput).toHaveValue('John Doe');
        expect(durationInput).toHaveValue(120);
    });
});
