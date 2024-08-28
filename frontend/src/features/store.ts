import { configureStore } from '@reduxjs/toolkit';
import gamesReducer from './gamesSlice';
import sessionReducer from './sessionSlice';
import { TypedUseSelectorHook, useDispatch, useSelector } from 'react-redux';

// Configure the store with the games and session reducers to manage the state

export const store = configureStore({
    reducer: {
        games: gamesReducer,
        session: sessionReducer,
    },
});

export type RootState = ReturnType<typeof store.getState>;
export type AppDispatch = typeof store.dispatch;

// Custom hook to use AppDispatch
export const useAppDispatch = () => useDispatch<AppDispatch>();

// Custom hook to use AppSelector
export const useAppSelector: TypedUseSelectorHook<RootState> = useSelector;