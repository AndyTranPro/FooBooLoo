import React from 'react';
import { Container, Typography, Box, Grid } from '@mui/material';
import GameList from '../components/GameList';
import GameForm from '../components/GameForm';
import VideogameAssetOutlinedIcon from '@mui/icons-material/VideogameAssetOutlined';

// The Home page, which displays the game form and the list of games

const Home: React.FC = () => {
    return (
        <Container>
            <Box my={4}>
                <Box
                    sx={{
                        display: 'flex',
                        background: 'linear-gradient(45deg, #FE6B8B 30%, #FF8E53 90%)',
                        padding: '15px',
                        borderRadius: '8px',
                        textAlign: 'center',
                        color: 'white',
                        justifyContent: 'space-around',
                        alignItems: 'center',
                    }}
                >
                    <VideogameAssetOutlinedIcon sx={{ fontSize: 80, marginBottom: 1 }} />
                    <Typography
                        variant="h3"
                        component="h1"
                        gutterBottom
                        sx={{
                            justifyContent: 'center',
                            WebkitBackgroundClip: 'text',
                            fontFamily: 'cursive',
                            fontWeight: 'bold'
                        }}
                    >
                        FizzBuzz Game
                    </Typography>
                    <VideogameAssetOutlinedIcon sx={{ fontSize: 80, marginBottom: 1 }} />
                </Box>
                <Grid container spacing={4} mt={4}>
                    <Grid item xs={12} md={6}>
                        <GameForm />
                    </Grid>
                    <Grid item xs={12} md={6}>
                        <GameList />
                    </Grid>
                </Grid>
            </Box>
        </Container>
    );
};

export default Home;