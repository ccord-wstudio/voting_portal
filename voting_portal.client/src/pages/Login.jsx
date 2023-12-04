import React from 'react'
import "../App.css";
import { useState } from "react";
import Cookies from 'js-cookie';

function Login() {
    const [email, setEmail] = useState('');
    const [password, setPassword] = useState('');

    const handleLogin = async () => {

        try {
            // Send a POST request to the backend API for login
            console.log("Start");
            const response = await fetch(
                'http://localhost:5213/api/auth/login',
                {

                    method: "POST",
                    headers: {
                        "Content-Type": "application/json",
                    },
                    body: JSON.stringify({
                        email: email,
                        password: password,
                    }),
                });
            console.log('Response:', response);
            if (response.ok) {
                const result = await response.json();
                alert(result.message); // Display success message
                Cookies.set('votingportal1234', result.userId);
                window.location.href = '/';
            } else {
                const error = await response.json();
                alert(error.message); // Display error message
            }
        } catch (error) {
            console.error("Error during login:", error);
            if (error.response) {
                console.log('Full Response:', error.response);
            }
        }
        console.log("End");
    };
    return (
        <div className="Login">
            <header className="App-header">
                <h1>Login</h1>
                <form>
                    <label>Email:</label>
                    <input
                        type="email"
                        value={email}
                        onChange={(e) => setEmail(e.target.value)}
                    />
                    <label>Password:</label>
                    <input
                        type="password"
                        value={password}
                        onChange={(e) => setPassword(e.target.value)}
                    />
                    <br />
                    <button type="button" onClick={handleLogin}>Login</button>
                </form>
            </header>
        </div>
    );
}

export default Login