import React from 'react'
import "../App.css";
import { useState } from "react";

function Register() {
    const [email, setEmail] = useState('');
    const [password, setPassword] = useState('');
    const [firstname, setFirstname] = useState('');
    const [lastname, setLastname] = useState('');
    const [dob, setDob] = useState('');
    const [postcode, setPostcode] = useState('');

    const handleRegister = async () => {
        try {
            const response = await fetch('http://localhost:5213/api/auth/register', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify({
                    email: email,
                    password: password,
                    firstname: firstname,
                    lastname: lastname,
                    dob: new Date(dob).toISOString(),
                    postcode: postcode,
                }),
            });

            if (response.ok) {
                const result = await response.json();
                alert(result.message); // Display success message
                window.location.href = '/';
            } else {
                const error = await response.json();
                if (error.message === "Incorrect Postcode") {
                    alert("Incorrect Postcode");
                } else {
                    alert(error.message); // Display other error messages
                }
            }
        } catch (error) {
            console.error('Error during registration:', error);
            if (error.response) {
                console.log('Full Response:', error.response);
            }
        }
    };

    return (
        <div className="Register">
            <header className="App-header">
                <h1>Register</h1>
                <form>
                    <label>Email:</label>
                    <br />
                    <input
                        type="email"
                        value={email}
                        onChange={(e) => setEmail(e.target.value)}
                    />
                    <br />
                    <label>Password:</label>
                    <br />
                    <input
                        type="password"
                        value={password}
                        onChange={(e) => setPassword(e.target.value)}
                    />
                    <br />
                    <label>First Name:</label>
                    <br />
                    <input
                        type="text"
                        value={firstname}
                        onChange={(e) => setFirstname(e.target.value)}
                    />
                    <br />
                    <label>Last Name:</label>
                    <br />
                    <input
                        type="text"
                        value={lastname}
                        onChange={(e) => setLastname(e.target.value)}
                    />
                    <br />
                    <label>Date of Birth:</label>
                    <br/>
                    <input
                        type="date"
                        value={dob}
                        onChange={(e) => setDob(e.target.value)}
                    />
                    <br />
                    <label>Post Code:</label>
                    <br />
                    <input
                        type="text"
                        value={postcode}
                        onChange={(e) => setPostcode(e.target.value)}
                    />
                    <br />
                    <button type="button" onClick={handleRegister}>Register</button>
                </form>
            </header>
        </div>
    );
}

export default Register