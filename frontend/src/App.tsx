import React from 'react';
import { BrowserRouter as Router, Route, Routes } from 'react-router-dom';
import './global.css';
import Home from './pages/Home';
import Game from './pages/Game';
import SessionPage from './pages/SessionPage';
import SessionResultsPage from './pages/SessionResultsPage';

const App: React.FC = () => {
    return (
        <Router>
            <Routes>
                <Route path="/" element={<Home />} />
                <Route path="/games/:gameId" element={<Game />} />
                <Route path="/sessions/:sessionId" element={<SessionPage />} />
                <Route path="/sessions/:sessionId/results" element={<SessionResultsPage />} />
            </Routes>
        </Router>
    );
};

export default App;
