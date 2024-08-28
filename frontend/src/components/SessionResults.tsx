import React, { useState, useEffect } from 'react';
import { useAppSelector } from '../features/store';
import { useNavigate } from 'react-router-dom';
import { Typography, Button, Box, Paper } from '@mui/material';
import CheckCircleOutlineIcon from '@mui/icons-material/CheckCircleOutline';
import SentimentDissatisfiedIcon from '@mui/icons-material/SentimentDissatisfied';
import Lottie from 'react-lottie';
import animationData from '../animations/congrats.json';


// A component to display the results of the session

const SessionResults: React.FC = () => {
    const score = useAppSelector((state) => state.session.score);
    const navigate = useNavigate();
    const totalQuestions = useAppSelector((state) => state.session.numQuestions);
    const [message, setMessage] = useState('');
    const [isCongrats, setIsCongrats] = useState(false);

    useEffect(() => {
        const percentage = (score / totalQuestions) * 100;
        if (percentage >= 80) {
            setMessage('Congratulations! You did a great job!');
            setIsCongrats(true);
        } else if (percentage < 50) {
            setMessage('Better luck next time!');
            setIsCongrats(false);
        }
    }, [score, totalQuestions]);

    const BackToHomePage = () => {
        navigate("/");
    };

    const defaultOptions = {
        loop: true,
        autoplay: true,
        animationData: animationData,
        rendererSettings: {
            preserveAspectRatio: 'xMidYMid slice'
        }
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
            <Paper elevation={5} sx={{ p: 4, maxWidth: 600, width: '100%' }}>
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
                    Game Over!
                </Typography>
                {isCongrats ? (
                    <>
                        <Lottie options={defaultOptions} height={150} width={150} />
                        <Typography variant="h6" component="h2" sx={{ mt: 2 }}>
                            {message}
                        </Typography>
                        <CheckCircleOutlineIcon sx={{ fontSize: 50, color: 'green', mt: 2 }} />
                    </>
                ) : (
                    <>
                        <SentimentDissatisfiedIcon sx={{ fontSize: 50, color: 'red', mt: 2 }} />
                        <Typography variant="h6" component="h2" sx={{ mt: 2 }}>
                            {message}
                        </Typography>
                    </>
                )}
                <Typography variant="h6" gutterBottom>
                    Your Score: {score}/{totalQuestions}
                </Typography>
                <Button
                    onClick={BackToHomePage}
                    variant="contained"
                    color="primary"
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
                    Back to Home
                </Button>
            </Paper>
        </Box>
    );
};

export default SessionResults;