import logo from "./logo.svg";
import "./App.css";
import { useState } from "react";
import { Routes, Route } from 'react-router-dom';
import NavBar from './Components/NavBar';
import Home from './Pages/Home';
import Login from './Pages/Login';
import Register from './Pages/Register';
import Vote from './Pages/Vote';
import Admin from './Pages/Admin';


const App = () => {
    return (
        <>
            <NavBar />
            <Routes>
                <Route path="/" element={<Home />} />
                <Route path="/login" element={<Login />} />
                <Route path="/register" element={<Register />} />
                <Route path="/vote" element={<Vote />} />
                <Route path="/admin" element={<Admin />} />
            </Routes>
        </>
    );
};

export default App;