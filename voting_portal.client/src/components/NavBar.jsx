import { NavLink } from 'react-router-dom';
import Cookies from 'js-cookie';
import "../App.css";

const NavBar = () => {
    const isLoggedIn = Cookies.get('votingportal1234') !== undefined;
    const isAdmin = Cookies.get('votingportal1234') === '1';

    const handleLogout = () => {
        // Clear the cookie and perform any other necessary logout actions
        Cookies.remove('votingportal1234');
        window.location.href = '/';
        // Redirect to the home page or any other desired page after logout
    };

    return (
        <nav>
            <ul>
                <li>
                    <NavLink to="/">Home</NavLink>
                </li>
                {isLoggedIn ? (
                    <>
                        <li>
                            <NavLink to="/vote">Vote</NavLink>
                        </li>
                        {isAdmin && (
                            <li>
                                <NavLink to="/admin">Admin</NavLink>
                            </li>
                        )}
                        <li>
                            <NavLink onClick={handleLogout}>Logout</NavLink>
                        </li>
                    </>
                ) : (
                    <>
                        <li>
                            <NavLink to="/login">Login</NavLink>
                        </li>
                        <li>
                            <NavLink to="/register">Register</NavLink>
                        </li>
                    </>
                )}
            </ul>
        </nav>
    );
};

export default NavBar;
