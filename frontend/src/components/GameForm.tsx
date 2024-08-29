import React, { useState } from 'react';
import { useAppDispatch, useAppSelector } from '../features/store';
import { addGame } from '../features/gamesSlice';
import { TextField, Button, Box, Grid, Typography, Alert } from '@mui/material';

const GameForm: React.FC = () => {
    const [name, setName] = useState('');
    const games = useAppSelector((state) => state.games.games);
    const [author, setAuthor] = useState('');
    const [min, setMin] = useState<number | string>(0);
    const [max, setMax] = useState<number | string>(100);
    const [rules, setRules] = useState<{ divisor: string; replacement: string }[]>([{ divisor: '', replacement: '' }]);
    const [errors, setErrors] = useState({
        name: false,
        author: false,
        min: false,
        max: false,
        rules: [] as boolean[]
    });
    const [submitError, setSubmitError] = useState<string | null>(null);
    const dispatch = useAppDispatch();

    const handleAddRule = () => {
        setRules([...rules, { divisor: '', replacement: '' }]);
        setErrors({ ...errors, rules: [...errors.rules, false] });
    };

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();
        // check if the name already exists
        const existingGame = games.find(game => game.name === name);
        if (existingGame) {
            setSubmitError('Game already exists. Please choose a different name.');
            return;
        } else {
            setSubmitError(null);
        }

        e.preventDefault();
        const newErrors = {
            name: !name,
            author: !author,
            min: min === '',
            max: max === '' || max === 0,
            rules: rules.map(rule => !rule.divisor || !rule.replacement)
        };
        setErrors(newErrors);
    
        if (Object.values(newErrors).some(error => error === true || (Array.isArray(error) && error.some(e => e)))) {
            return;
        }
    
        const formattedRules = rules.map(rule => ({
            divisor: parseInt(rule.divisor),
            replacement: rule.replacement
        }));
    
        await dispatch(addGame({ name, author, min: Number(min), max: Number(max), rules: formattedRules }));
    };

    return (
        <Box component="form" onSubmit={handleSubmit} sx={{ mt: 1 }}>
            {submitError && (
                <Alert severity="error" sx={{ mb: 2 }}>
                    {submitError}
                </Alert>
            )}
            <Typography
                variant="h4"
                gutterBottom
                sx={{
                    textAlign: 'center',
                    background: 'linear-gradient(45deg, #FE6B8B 30%, #FF8E53 90%)',
                    WebkitBackgroundClip: 'text',
                    WebkitTextFillColor: 'transparent',
                    fontFamily: 'Roboto, sans-serif',
                    fontWeight: 'bold'
                }}
            >
                Create a New Game
            </Typography>
            <Grid container spacing={2}>
                <Grid item xs={12}>
                    <TextField
                        label="Game Name"
                        value={name}
                        onChange={(e) => setName(e.target.value)}
                        fullWidth
                        variant="outlined"
                        error={errors.name}
                        helperText={errors.name ? 'Game Name is required' : ''}
                    />
                </Grid>
                <Grid item xs={12}>
                    <TextField
                        label="Author"
                        value={author}
                        onChange={(e) => setAuthor(e.target.value)}
                        fullWidth
                        variant="outlined"
                        error={errors.author}
                        helperText={errors.author ? 'Author is required' : ''}
                    />
                </Grid>
                <Grid item xs={6}>
                    <TextField
                        label="Min"
                        type="number"
                        value={min}
                        onChange={(e) => setMin(e.target.value === '' ? '' : parseInt(e.target.value))}
                        fullWidth
                        variant="outlined"
                        inputProps={{ min: 0 }}
                        error={errors.min}
                        helperText={errors.min ? 'Min is required' : ''}
                    />
                </Grid>
                <Grid item xs={6}>
                    <TextField
                        label="Max"
                        type="number"
                        value={max}
                        onChange={(e) => setMax(e.target.value === '' ? '' : parseInt(e.target.value))}
                        fullWidth
                        variant="outlined"
                        inputProps={{ min: min }}
                        error={errors.max}
                        helperText={errors.max ? 'Max is required' : ''}
                    />
                </Grid>
                <Grid item xs={12}>
                    <Typography variant="h6" gutterBottom sx={{ fontWeight: 'bold' }}>
                        Rules
                    </Typography>
                    {rules.map((rule, index) => (
                        <Grid container spacing={2} key={index} marginBottom={2}>
                            <Grid item xs={6}>
                                <TextField
                                    label="Divisor"
                                    value={rule.divisor}
                                    onChange={(e) => {
                                        const newRules = [...rules];
                                        newRules[index].divisor = e.target.value;
                                        setRules(newRules);
                                    }}
                                    fullWidth
                                    variant="outlined"
                                    error={errors.rules[index]}
                                    helperText={errors.rules[index] ? 'Number is required' : ''}
                                />
                            </Grid>
                            <Grid item xs={6}>
                                <TextField
                                    label="Replacement"
                                    value={rule.replacement}
                                    onChange={(e) => {
                                        const newRules = [...rules];
                                        newRules[index].replacement = e.target.value;
                                        setRules(newRules);
                                    }}
                                    fullWidth
                                    variant="outlined"
                                    error={errors.rules[index]}
                                    helperText={errors.rules[index] ? 'Word is required' : ''}
                                />
                            </Grid>
                        </Grid>
                    ))}
                    <Button
                        variant="contained"
                        onClick={handleAddRule}
                        sx={{
                            fontWeight: 'bold',
                            background: 'linear-gradient(45deg, #FE6B8B 30%, #FF8E53 90%)',
                            color: 'white',
                            '&:hover': {
                                background: 'linear-gradient(45deg, #FF8E53 30%, #FE6B8B 90%)',
                            },
                        }}
                    >
                        Add Rule
                    </Button>
                </Grid>
                <Grid item xs={12}>
                    <Button
                        type="submit"
                        variant="contained"
                        color="secondary"
                        fullWidth
                        sx={{ mt: 2, fontWeight: 'bold' }}
                    >
                        Create Game
                    </Button>
                </Grid>
            </Grid>
        </Box>
    );
};

export default GameForm;