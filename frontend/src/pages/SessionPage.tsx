import React from 'react';
import { useParams } from 'react-router-dom';
import Session from '../components/Session';

// A page to manage the current session

const SessionPage: React.FC = () => {
    const { sessionId } = useParams<{ sessionId: string }>();

    return (
        <div>
            <Session sessionId={Number(sessionId)} />
        </div>
    );
};

export default SessionPage;
