import React, { Component } from 'react';
import { Route, Routes } from 'react-router-dom';
import AppRoutes from './AppRoutes';
import { Layout } from './components/Layout';
import './custom.css';

export default class App extends Component {
  static displayName = App.name;

  render() {
    let cart = JSON.parse(localStorage.getItem("cart"));
    if (cart == null)
        localStorage.setItem("cart", "[]");
    
    return (
        <>
            <link rel="stylesheet" href="css/bootstrap.min.css" />
            <Layout>
                <Routes>
                    {AppRoutes.map((route, index) => {
                        const { element, ...rest } = route;
                        return <Route key={index} {...rest} element={element} />;
                    })}
                </Routes>
            </Layout>
            <script src="js/bootstrap.bundle.min.js"></script>
        </>
    );
  }
}
