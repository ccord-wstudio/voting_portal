import React, { useState, useEffect } from 'react';
import Cookies from 'js-cookie';

function Vote() {
    const [selectedVote, setSelectedVote] = useState('yes');
    const [userId, setUserId] = useState(null);

    useEffect(() => {
        // Retrieve user ID from the cookie when the component mounts
        const storedUserId = Cookies.get('votingportal1234');
        if (storedUserId) {
            setUserId(parseInt(storedUserId));
        }
    }, []);

    const handleVoteSubmit = async () => {
        try {
            if (!userId) {
                alert('User ID not found. Please log in.');
                return;
            }

            // Fetch API to send the vote to the server
            const response = await fetch('http://localhost:5213/api/vote', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify({
                    userId: userId, // Include the user ID in the vote submission
                    questionId: 1,
                    response: selectedVote,
                }),
            });

            if (response.ok) {
                const result = await response.json();
                alert(result.message); // Display success message
            } else {
                const error = await response.json();
                alert(error.message); // Display error message
            }
        } catch (error) {
            console.error('Error during vote submission:', error);
        }
    };

    return (
        <div className="Vote">
            <header className="App-header">
                <h2>Should Chris get the job?</h2>
                <select
                    name="vote"
                    id="vote"
                    value={selectedVote}
                    onChange={(e) => setSelectedVote(e.target.value)}
                >
                    <option value="yes">Yes</option>
                    <option value="no">No</option>
                </select>
                <br />
                <button type="button" onClick={handleVoteSubmit}>
                    Submit Vote
                </button>
            </header>
        </div>
    );
}

export default Vote;
