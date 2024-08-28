import React from 'react';
import { render, screen, fireEvent } from '@testing-library/react';
import '@testing-library/jest-dom';
import { Provider } from 'react-redux';
import configureStore from 'redux-mock-store';
import GameForm from '../components/GameForm';
import { addGame } from '../features/gamesSlice';
import { useAppDispatch } from '../features/store';

// Mock the useAppDispatch hook and addGame action
jest.mock('../features/store', () => ({
    useAppDispatch: jest.fn(),
}));

jest.mock('../features/gamesSlice', () => ({
    addGame: jest.fn(),
}));

const mockStore = configureStore([]);

const renderComponent = () => {
    const store = mockStore({});
    return render(
        <Provider store={store}>
            <GameForm />
        </Provider>
    );
};

describe('GameForm Component', () => {
    test('renders correctly with all form fields', () => {
        renderComponent();

        expect(screen.getByLabelText(/Game Name/i)).toBeInTheDocument();
        expect(screen.getByLabelText(/Author/i)).toBeInTheDocument();
        expect(screen.getByLabelText(/Min/i)).toBeInTheDocument();
        expect(screen.getByLabelText(/Max/i)).toBeInTheDocument();
        expect(screen.getByText(/Rules/i)).toBeInTheDocument();
        expect(screen.getByText(/Add Rule/i)).toBeInTheDocument();
        expect(screen.getByText(/Create Game/i)).toBeInTheDocument();
    });

    test('can update form fields', () => {
        renderComponent();

        fireEvent.change(screen.getByLabelText(/Game Name/i), { target: { value: 'Test Game' } });
        fireEvent.change(screen.getByLabelText(/Author/i), { target: { value: 'John Doe' } });
        fireEvent.change(screen.getByLabelText(/Min/i), { target: { value: '10' } });
        fireEvent.change(screen.getByLabelText(/Max/i), { target: { value: '100' } });

        expect(screen.getByLabelText(/Game Name/i)).toHaveValue('Test Game');
        expect(screen.getByLabelText(/Author/i)).toHaveValue('John Doe');
        expect(screen.getByLabelText(/Min/i)).toHaveValue(10);
        expect(screen.getByLabelText(/Max/i)).toHaveValue(100);
    });

    test('can add a new rule', () => {
        renderComponent();

        fireEvent.click(screen.getByText(/Add Rule/i));

        const ruleDivisorInputs = screen.getAllByLabelText(/Divisor/i);
        const ruleWordInputs = screen.getAllByLabelText(/Replacement/i);

        expect(ruleDivisorInputs.length).toBe(2);
        expect(ruleWordInputs.length).toBe(2);
    });

    test('submits the form and dispatches addGame', () => {
        const mockDispatch = jest.fn();
        (useAppDispatch as jest.Mock).mockReturnValue(mockDispatch);

        renderComponent();

        fireEvent.change(screen.getByLabelText(/Game Name/i), { target: { value: 'Test Game' } });
        fireEvent.change(screen.getByLabelText(/Author/i), { target: { value: 'John Doe' } });
        fireEvent.change(screen.getByLabelText(/Min/i), { target: { value: '10' } });
        fireEvent.change(screen.getByLabelText(/Max/i), { target: { value: '100' } });

        fireEvent.change(screen.getAllByLabelText(/Divisor/i)[0], { target: { value: '3' } });
        fireEvent.change(screen.getAllByLabelText(/Replacement/i)[0], { target: { value: 'Fizz' } });

        fireEvent.submit(screen.getByRole('button', { name: /Create Game/i }));

        expect(mockDispatch).toHaveBeenCalled();
        expect(addGame).toHaveBeenCalledWith({
            name: 'Test Game',
            author: 'John Doe',
            min: 10,
            max: 100,
            rules: [{ divisor: 3, replacement: 'Fizz' }],
        });
    });

    test('does not submit the form with invalid data', () => {
        const mockDispatch = jest.fn();
        (useAppDispatch as jest.Mock).mockReturnValue(mockDispatch);

        renderComponent();

        fireEvent.change(screen.getByLabelText(/Game Name/i), { target: { value: '' } });
        fireEvent.change(screen.getByLabelText(/Author/i), { target: { value: '' } });

        fireEvent.submit(screen.getByRole('button', { name: /Create Game/i }));

        expect(mockDispatch).not.toHaveBeenCalled();
    });
    
    test('shows error messages for invalid form fields', () => {
      renderComponent();

      fireEvent.change(screen.getByLabelText(/Game Name/i), { target: { value: '' } });
      fireEvent.change(screen.getByLabelText(/Author/i), { target: { value: '' } });
      fireEvent.change(screen.getByLabelText(/Min/i), { target: { value: '' } });
      fireEvent.change(screen.getByLabelText(/Max/i), { target: { value: '' } });

      fireEvent.submit(screen.getByRole('button', { name: /Create Game/i }));

      expect(screen.getByText(/Game Name is required/i)).toBeInTheDocument();
      expect(screen.getByText(/Author is required/i)).toBeInTheDocument();
      expect(screen.getByText(/Min is required/i)).toBeInTheDocument();
      expect(screen.getByText(/Max is required/i)).toBeInTheDocument();
  });
});