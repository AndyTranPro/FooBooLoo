import React from 'react';
import { render, screen } from '@testing-library/react';
import '@testing-library/jest-dom';
import { useParams } from 'react-router-dom';
import SessionPage from '../pages/SessionPage';
import Session from '../components/Session';

jest.mock('react-router-dom', () => ({
    ...jest.requireActual('react-router-dom'),
    useParams: jest.fn(),
}));

jest.mock('../components/Session', () => jest.fn(() => <div>Mocked Session</div>));

describe('SessionPage Component', () => {
    test('renders correctly and matches snapshot', () => {
        (useParams as jest.Mock).mockReturnValue({ sessionId: '123' });
        const { asFragment } = render(<SessionPage />);
        expect(asFragment()).toMatchSnapshot();
    });

    test('renders session component with correct sessionId', () => {
        (useParams as jest.Mock).mockReturnValue({ sessionId: '123' });
        render(<SessionPage />);
        expect(screen.getByText('Mocked Session')).toBeInTheDocument();
        expect(Session).toHaveBeenCalledWith({ sessionId: 123 }, {});
    });
});