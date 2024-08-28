import React, { useState } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import { useAppDispatch } from '../features/store';
import { beginSession } from '../features/sessionSlice';
import { Typography, TextField, Button, Box, Grid } from '@mui/material';
import SportsEsportsIcon from '@mui/icons-material/SportsEsports';
import PersonIcon from '@mui/icons-material/Person';
import TimerIcon from '@mui/icons-material/Timer';
import { keyframes } from '@emotion/react';

// Keyframes for animations
const bounce = keyframes`
  0%, 20%, 50%, 80%, 100% {
    transform: translateY(0);
  }
  40% {
    transform: translateY(-30px);
  }
  60% {
    transform: translateY(-15px);
  }
`;

// A page to start a new session for a game

const Game: React.FC = () => {
    const { gameId } = useParams<{ gameId: string }>();
    const dispatch = useAppDispatch();
    const navigate = useNavigate();
    const [playerName, setPlayerName] = useState('');
    const [duration, setDuration] = useState(60);

    const handleStartSession = async () => {
        const response = await dispatch(beginSession({ gameId: Number(gameId), playerName, duration }));
        const sessionId = response.payload.sessionId;
        navigate(`/sessions/${sessionId}`);
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
            }}
        >
            <SportsEsportsIcon sx={{ fontSize: 80, color: '#FE6B8B', animation: `${bounce} 2s infinite` }} />
            <Typography
                variant="h4"
                gutterBottom
                sx={{
                    mt: 2,
                    background: 'linear-gradient(45deg, #FE6B8B 30%, #FF8E53 90%)',
                    WebkitBackgroundClip: 'text',
                    WebkitTextFillColor: 'transparent',
                    fontFamily: 'Roboto, sans-serif',
                    fontWeight: 'bold',
                }}
            >
                Start a Session
            </Typography>
            <Grid container spacing={1} alignItems="center" justifyContent="center" sx={{ maxWidth: 600 }}>
                <Grid item xs={12}>
                    <Box sx={{ display: 'flex', alignItems: 'center' }}>
                        <PersonIcon sx={{ mr: 1, color: '#FF8E53' }} />
                        <TextField
                            label="Player Name"
                            value={playerName}
                            onChange={(e) => setPlayerName(e.target.value)}
                            fullWidth
                            variant="outlined"
                            margin="normal"
                        />
                    </Box>
                </Grid>
                <Grid item xs={12}>
                    <Box sx={{ display: 'flex', alignItems: 'center' }}>
                        <TimerIcon sx={{ mr: 1, color: '#FF8E53' }} />
                        <TextField
                            label="Duration (seconds)"
                            type="number"
                            value={duration}
                            onChange={(e) => setDuration(Number(e.target.value))}
                            fullWidth
                            variant="outlined"
                            margin="normal"
                            inputProps={{ min: 0 }}
                        />
                    </Box>
                </Grid>
                <Grid item xs={12}>
                    <Button
                        onClick={handleStartSession}
                        fullWidth
                        sx={{
                            mt: 2,
                            position: 'relative',
                            overflow: 'hidden',
                            color: 'red',
                            border: '2px solid transparent',
                            borderRadius: '8px',
                            fontWeight: 'bold',
                            background: '#F5F5F5',
                            cursor: 'pointer',
                            '&::before': {
                                content: '""',
                                position: 'absolute',
                                top: 0,
                                left: '-100%',
                                width: '100%',
                                height: '100%',
                                background: 'linear-gradient(45deg, #FF8E53 30%, #FE6B8B 90%)',
                                transition: 'left 0.8s ease-in-out',
                                zIndex: 0,
                            },
                            '&:hover::before': {
                                left: 0,
                            },
                            '&:hover': {
                                color: 'transparent',
                                border: '2px solid transparent',
                            },
                            '& > *': {
                                position: 'relative',
                                zIndex: 1,
                            },
                        }}
                    >
                        Start Session
                    </Button>
                </Grid>
            </Grid>
        </Box>
    );
};

export default Game;