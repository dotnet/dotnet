import React, { Component } from 'react';
import { Route, Routes } from 'react-router-dom';
import AppRoutes from './AppRoutes';
////#if (IndividualLocalAuth)
import AuthorizeRoute from './components/api-authorization/AuthorizeRoute';
////#endif
import { Layout } from './components/Layout';
import './custom.css';

export default class App extends Component {
  static displayName = App.name;

  render() {
    return (
      <Layout>
        <Routes>
          {AppRoutes.map((route, index) => {
////#if (!IndividualLocalAuth)
            const { element, ...rest } = route;
            return <Route key={index} {...rest} element={element} />;
////#else
            const { element, requireAuth, ...rest } = route;
            return <Route key={index} {...rest} element={requireAuth ? <AuthorizeRoute {...rest} element={element} /> : element} />;
////#endif
          })}
        </Routes>
      </Layout>
    );
  }
}
