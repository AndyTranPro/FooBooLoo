import React from 'react';
import { render, screen, fireEvent } from '@testing-library/react';
import '@testing-library/jest-dom';
import { Provider } from 'react-redux';
import configureStore from 'redux-mock-store';
import { MemoryRouter } from 'react-router-dom';
import SessionResults from '../components/SessionResults';

jest.mock('react-lottie', () => () => <div>Lottie Animation</div>);

const mockStore = configureStore([]);

const mockNavigate = jest.fn();
jest.mock('react-router-dom', () => ({
    ...jest.requireActual('react-router-dom'),
    useNavigate: () => mockNavigate,
}));

const renderComponent = (store: any) => {
    return render(
        <Provider store={store}>
            <MemoryRouter>
                <SessionResults />
            </MemoryRouter>
        </Provider>
    );
};

describe('SessionResults Component', () => {
    let store: any;

    beforeEach(() => {
        store = mockStore({
            session: {
                score: 8,
                numQuestions: 10,
            },
        });
    });

    test('renders correctly and matches snapshot', () => {
        const { asFragment } = renderComponent(store);
        expect(asFragment()).toMatchSnapshot();
    });

    test('displays congratulatory message when score is 80% or higher', () => {
        renderComponent(store);

        expect(screen.getByText(/Congratulations! You did a great job!/i)).toBeInTheDocument();
        expect(screen.getByText(/Your Score: 8\/10/i)).toBeInTheDocument();
        expect(screen.getByText(/Lottie Animation/i)).toBeInTheDocument();
        expect(screen.getByTestId('CheckCircleOutlineIcon')).toBeInTheDocument();
    });

    test('displays condolence message when score is less than 50%', () => {
        store = mockStore({
            session: {
                score: 4,
                numQuestions: 10,
            },
        });
        renderComponent(store);

        expect(screen.getByText(/Better luck next time!/i)).toBeInTheDocument();
        expect(screen.getByText(/Your Score: 4\/10/i)).toBeInTheDocument();
        expect(screen.getByTestId('SentimentDissatisfiedIcon')).toBeInTheDocument();
    });

    test('back to home page', () => {

        renderComponent(store);

        const button = screen.getByRole('button', { name: /Back to Home/i });
        fireEvent.click(button);

        expect(mockNavigate).toHaveBeenCalledWith('/');
    });
});
