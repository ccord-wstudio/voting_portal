import React, { useState, useEffect } from "react";
import { useNavigate } from "react-router-dom";
import Cookies from 'js-cookie';
import "../App.css";

function Admin() {
    const [users, setUsers] = useState([]);
    const [votes, setVotes] = useState([]);
    const navigate = useNavigate();

    useEffect(() => {
        const isAdmin = Cookies.get('votingportal1234') === '1';
        if (!isAdmin) {
            navigate.push('/');
        } else {
            fetchData();
        }
    }, [navigate]);

    const fetchData = async () => {
        try {
            const userResponse = await fetch('http://localhost:5213/api/useradmin');
            const userData = await userResponse.json();
            setUsers(userData);
        } catch (error) {
            console.error('Error fetching users:', error);
        }

        try {
            const voteResponse = await fetch('http://localhost:5213/api/voteadmin');
            const voteData = await voteResponse.json();
            setVotes(voteData);
        } catch (error) {
            console.error('Error fetching votes:', error);
        }
    };

    const handleUserChange = async (userId, field, newValue) => {
        try {
            // Find the user object being updated
            const updatedUser = users.find(user => user.userID === userId);

            // Ensure the user object is found before proceeding
            if (updatedUser) {
                // Update local state optimistically
                setUsers(prevUsers =>
                    prevUsers.map(user =>
                        user.userID === userId ? { ...user, [field]: convertValue(newValue, field) } : user
                    )
                );

                // Prepare the updated user object with all fields
                const updatedUserWithAllFields = {
                    ...updatedUser,
                    [field]: convertValue(newValue, field),
                };

                // Make API call to update user data on the server
                const response = await fetch('http://localhost:5213/api/useradmin/update', {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json',
                    },
                    body: JSON.stringify(updatedUserWithAllFields),
                });

                if (!response.ok) {
                    const errorMessage = await response.text();
                    throw new Error(`HTTP error! Status: ${response.status}. ${errorMessage}`);
                }

                // If the update is successful, you can optionally fetch the updated user list
                // to refresh the data in your component
                fetchUsers();
            } else {
                console.error('User not found for ID:', userId);
            }
        } catch (error) {
            console.error('Error updating user:', error);
            // Handle errors, e.g., show an error message to the user
        }
    };
    const handleVoteChange = async (voteID, field, newValue) => {
        try {
            // Find the user object being updated
            const updatedVote = votes.find(vote => vote.voteID === voteID);

            // Ensure the user object is found before proceeding
            if (updatedVote) {
                // Update local state optimistically
                setVotes(prevVotes =>
                    prevVotes.map(vote =>
                        vote.voteID === voteID ? { ...vote, [field]: convertValue(newValue, field) } : vote
                    )
                );

                // Prepare the updated user object with all fields
                const updatedVoteWithAllFields = {
                    ...updatedVote,
                    [field]: convertValue(newValue, field),
                };

                // Make API call to update user data on the server
                const response = await fetch('http://localhost:5213/api/voteadmin/update', {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json',
                    },
                    body: JSON.stringify(updatedVoteWithAllFields),
                });

                if (!response.ok) {
                    const errorMessage = await response.text();
                    throw new Error(`HTTP error! Status: ${response.status}. ${errorMessage}`);
                }

                // If the update is successful, you can optionally fetch the updated user list
                // to refresh the data in your component
                fetchData();
            } else {
                console.error('Vote not found for ID:', voteId);
            }
        } catch (error) {
            console.error('Error updating vote:', error);
            // Handle errors, e.g., show an error message to the user
        }
    };

    const convertValue = (value, field) => {
        switch (field) {
            case 'DOB':
                // Assuming 'DOB' is a Date field, format it as desired
                return new Date(value).toLocaleDateString();
            case 'AddressLongitude':
            case 'AddressLatitude':
                // Assuming these are numeric fields
                return parseFloat(value);
            default:
                // Default to string for other fields
                return value.toString();
        }
    };

    const handleUserDelete = async (userId) => {
        try {
            const response = await fetch(`http://localhost:5213/api/useradmin/udelete/${userId}`, {
                method: 'DELETE',
                headers: {
                    'Content-Type': 'application/json',
                },
            });

            if (!response.ok) {
                const errorMessage = await response.text();
                throw new Error(`HTTP error! Status: ${response.status}. ${errorMessage}`);
            }

            setUsers(prevUsers => prevUsers.filter(user => user.userID !== userId));
        } catch (error) {
            console.error('Error deleting user:', error);
        }
    };

    const handleVoteDelete = async (voteID) => {
        try {
            const response = await fetch(`http://localhost:5213/api/useradmin/vdelete/${voteID}`, {
                method: 'DELETE',
                headers: {
                    'Content-Type': 'application/json',
                },
            });

            if (!response.ok) {
                const errorMessage = await response.text();
                throw new Error(`HTTP error! Status: ${response.status}. ${errorMessage}`);
            }

            setVotes(prevVotes => prevVotes.filter(vote => vote.voteID !== voteID));
        } catch (error) {
            console.error('Error deleting vote:', error);
        }
    };

    return (
        <div className="App">
            <header className="App-header">
                <h2>Admin</h2>
                <p>Edit values and click elsewhere to submit</p>
                <h3>Users</h3>
                <div id="table" className="table-editable">
                    <table className="table">
                        <thead>
                            <tr>
                                <th>UserID</th>
                                <th>Email</th>
                                <th>FirstName</th>
                                <th>LastName</th>
                                <th>Password</th>
                                <th>DOB</th>
                                <th>LocationLong</th>
                                <th>LocationLat</th>
                                <th>Country</th>
                                <th>Action</th>
                            </tr>
                        </thead>
                        <tbody>
                            {users.map(user => (
                                <tr key={user.userID}>
                                    <td>{user.userID}</td>
                                    <td
                                        contentEditable
                                        onBlur={(e) => handleUserChange(user.userID, 'Email', e.target.innerText)}
                                    >
                                        {user.email}
                                    </td>
                                    <td
                                        contentEditable
                                        onBlur={(e) => handleUserChange(user.userID, 'FirstName', e.target.innerText)}
                                    >
                                        {user.firstName}
                                    </td>
                                    <td
                                        contentEditable
                                        onBlur={(e) => handleUserChange(user.userID, 'LastName', e.target.innerText)}
                                    >
                                        {user.lastName}
                                    </td>
                                    <td
                                        contentEditable
                                        onBlur={(e) => handleUserChange(user.userID, 'Password', e.target.innerText)}
                                    >
                                        {user.password}
                                    </td>
                                    <td
                                        contentEditable
                                        onBlur={(e) => handleUserChange(user.userID, 'DOB', e.target.innerText)}
                                    >
                                        {user.dob}
                                    </td>
                                    <td
                                        contentEditable
                                        onBlur={(e) => handleUserChange(user.userID, 'AddressLongitude', e.target.innerText)}
                                    >
                                        {user.addressLongitude}
                                    </td>
                                    <td
                                        contentEditable
                                        onBlur={(e) => handleUserChange(user.userID, 'AddressLatitude', e.target.innerText)}
                                    >
                                        {user.addressLatitude}
                                    </td>
                                    <td
                                        contentEditable
                                        onBlur={(e) => handleUserChange(user.userID, 'Country', e.target.innerText)}
                                    >
                                        {user.country}
                                    </td>
                                    <td>
                                        {user.userID !== 1 && (<button onClick={() => handleUserDelete(user.userID)}>Delete</button>)}
                                    </td>
                                </tr>
                            ))}
                        </tbody>
                    </table>
                </div>
                <h3>Votes</h3>
                <div id="table" className="table-editable">
                    <table className="table">
                        <thead>
                            <tr>
                                <th>VoteID</th>
                                <th>UserID</th>
                                <th>QuestionID</th>
                                <th>Response</th>
                                <th>Action</th>
                            </tr>
                        </thead>
                        <tbody>
                            {votes.map(vote => (
                                <tr key={vote.voteID}>
                                    <td>{vote.voteID}</td>
                                    <td>{vote.userID}</td>
                                    <td>{vote.questionID}</td>
                                    <td
                                        contentEditable
                                        onBlur={(e) => handleVoteChange(vote.voteID, 'Response', e.target.innerText)}
                                    >
                                        {vote.response}
                                    </td>
                                    <td>
                                        <button onClick={() => handleVoteDelete(vote.voteID)}>Delete</button>
                                    </td>
                                </tr>
                            ))}
                        </tbody>
                    </table>
                </div>
            </header>
        </div>
    );
}

export default Admin;