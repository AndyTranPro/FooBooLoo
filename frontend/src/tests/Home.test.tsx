import React from 'react';
import { render, screen } from '@testing-library/react';
import '@testing-library/jest-dom';
import Home from '../pages/Home';

jest.mock('../components/GameForm', () => () => <div>Mocked GameForm</div>);
jest.mock('../components/GameList', () => () => <div>Mocked GameList</div>);

describe('Home Component', () => {
    test('renders correctly and matches snapshot', () => {
        const { asFragment } = render(<Home />);
        expect(asFragment()).toMatchSnapshot();
    });

    test('renders the title "FizzBuzz Game"', () => {
        render(<Home />);
        expect(screen.getByText('FizzBuzz Game')).toBeInTheDocument();
    });

    test('renders GameForm and GameList components', () => {
        render(<Home />);
        expect(screen.getByText('Mocked GameForm')).toBeInTheDocument();
        expect(screen.getByText('Mocked GameList')).toBeInTheDocument();
    });
});