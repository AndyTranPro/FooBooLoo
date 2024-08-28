import React from 'react';
import { render } from '@testing-library/react';
import '@testing-library/jest-dom';
import SessionResultsPage from '../pages/SessionResultsPage';

jest.mock('../components/SessionResults', () => () => <div>Mocked SessionResults</div>);

describe('SessionResultsPage', () => {
    test('renders without crashing', () => {
        const { container } = render(<SessionResultsPage />);
        expect(container).toBeInTheDocument();
    });

    test('renders SessionResults component', () => {
        const { getByText } = render(<SessionResultsPage />);
        expect(getByText('Mocked SessionResults')).toBeInTheDocument();
    });
});