import React from 'react';
import { BrowserRouter, Route, Link } from 'react-router-dom';

function Navbar() {
  return (
    <nav>
      <ul>
        <li>
          <Link to="/">Home</Link>
        </li>
        <li>
          {/* <Link to="/admin/home">Admin</Link> */}
          <a href="/admin/home">Admin</a>
        </li>
      </ul>
    </nav>
  );
}

export default Navbar;
