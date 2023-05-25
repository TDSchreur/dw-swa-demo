import React from 'react';
import { Outlet } from 'react-router-dom';
import Navbar from './navbar';
import Me from './me';

const Layout = () => {
  return (
    <>
      <Navbar />
      <Outlet />
      <Me />
    </>
  );
};

export default Layout;
