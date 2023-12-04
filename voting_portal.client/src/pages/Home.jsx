import React from 'react';
import logo from "../logo.svg";
import "../App.css";
import Cookies from 'js-cookie';

function Home() {
    const isLoggedIn = Cookies.get('votingportal1234') !== undefined;

    return (
        <div className="App">
            <header className="App-header">
                <img src={logo} className="App-logo" alt="logo" />
                <h2>Should Chris get the job?</h2>
                {isLoggedIn ? (
                    <p>Click the link above to vote.</p>
                ) : (
                    <p>Please login or register to vote.</p>
                )}
            </header>
        </div>
    );
}

export default Home;