import React, { useEffect, useState } from 'react';
import { useAppSelector, useAppDispatch } from '../features/store';
import { sendAnswer, retrieveResults } from '../features/sessionSlice';
import { useNavigate } from 'react-router-dom';
import { Typography, TextField, Button, Box, Paper } from '@mui/material';
import { keyframes } from '@emotion/react';
import HelpOutlineIcon from '@mui/icons-material/HelpOutline';

import TimerIcon from '@mui/icons-material/Timer';

// Keyframes for animations
const fadeIn = keyframes`
  from {
    opacity: 0;
  }
  to {
    opacity: 1;
  }
`;

// A component that is responsible for handling the session

const Session: React.FC<{ sessionId: number }> = ({ sessionId }) => {
    const dispatch = useAppDispatch();
    const navigate = useNavigate();
    const currentNumber = useAppSelector((state) => state.session.currentNumber);
    const duration = useAppSelector((state) => state.session.duration);
    const [answer, setAnswer] = useState('');
    const [timeLeft, setTimeLeft] = useState(duration);

    // Timer logic
    useEffect(() => {
        if (timeLeft > 0) {
            const timer = setTimeout(() => setTimeLeft(timeLeft - 1), 1000);
            return () => clearTimeout(timer);
        } else {
            handleEndSession();
        }
    }, [timeLeft]);

    // Submit the answer to the server
    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();
        await dispatch(sendAnswer({ sessionId, number: currentNumber!, answer }));
        setAnswer('');
    };

    const handleEndSession = async () => {
        await dispatch(retrieveResults(sessionId));
        navigate(`/sessions/${sessionId}/results`);
    };

    return (
        <Box
            sx={{
                display: 'flex',
                flexDirection: 'column',
                alignItems: 'center',
                justifyContent: 'center',
                height: '80vh',
                textAlign: 'center',
                animation: `${fadeIn} 1s ease-in-out`,
            }}
        >
            <Paper elevation={3} sx={{ p: 4, maxWidth: 600, width: '100%' }}>
                <HelpOutlineIcon sx={{ fontSize: 80, color: '#FE6B8B', marginBottom: '20px' }} />
                <Typography
                    variant="h4"
                    gutterBottom
                    sx={{
                        background: 'linear-gradient(45deg, #FE6B8B 30%, #FF8E53 90%)',
                        WebkitBackgroundClip: 'text',
                        WebkitTextFillColor: 'transparent',
                        fontFamily: 'Roboto, sans-serif',
                        fontWeight: 'bold',
                    }}
                >
                    Session in Progress
                </Typography>
                <Box sx={{ display: 'flex', alignItems: 'center', justifyContent: 'center', mt: 2 }}>
                    <TimerIcon sx={{ mr: 1, color: timeLeft < 10 ? 'red' : 'inherit' }} />
                    <Typography
                        variant="h6"
                        sx={{
                            color: timeLeft < 10 ? 'red' : 'inherit',
                            fontWeight: 'bold',
                        }}
                    >
                        Time Left: {timeLeft}s
                    </Typography>
                </Box>
                {currentNumber && (
                    <Box sx={{ mt: 2 }}>
                        <Typography variant="h6" gutterBottom fontFamily={'cursive'}>
                            Current Number: {currentNumber}
                        </Typography>
                        <form onSubmit={handleSubmit}>
                            <TextField
                                label="Your Answer"
                                value={answer}
                                onChange={(e) => setAnswer(e.target.value)}
                                fullWidth
                                variant="outlined"
                                margin="normal"
                            />
                            <Button
                                type="submit"
                                variant="contained"
                                color="primary"
                                fullWidth
                                sx={{
                                    mt: 2,
                                    background: 'linear-gradient(45deg, #FE6B8B 30%, #FF8E53 90%)',
                                    color: 'white',
                                    fontWeight: 'bold',
                                    '&:hover': {
                                        background: 'linear-gradient(45deg, #FF8E53 30%, #FE6B8B 90%)',
                                    },
                                }}
                            >
                                Submit
                            </Button>
                        </form>
                    </Box>
                )}
                <Button
                    onClick={handleEndSession}
                    variant="contained"
                    color="secondary"
                    fullWidth
                    sx={{
                        mt: 2,
                        background: 'linear-gradient(45deg, #2196F3 30%, #21CBF3 90%)',
                        color: 'white',
                        fontWeight: 'bold',
                        '&:hover': {
                            background: 'linear-gradient(45deg, #21CBF3 30%, #2196F3 90%)',
                        },
                    }}
                >
                    End Session
                </Button>
            </Paper>
        </Box>
    );
};

export default Session;