import { Counter } from "./components/Counter";
import { FetchData } from "./components/FetchData";
import { Home } from "./components/Home";
import CatalogCategories from "./components/CatalogCategories";
import Category from "./components/Category";
import Cart from "./components/Cart";
import Profile from "./components/Profile";
import Login from "./components/Login";
import Register from "./components/Register";
import Product from "./components/Product";

const AppRoutes = [
  {
    index: true,
    element: <Home />
  },
  {
    path: '/counter',
    element: <Counter />
  },
  {
    path: '/catalog/categories',
    element: <CatalogCategories />
  },
  {
    path: '/category',
    element: <Category />
  },
  {
    path: '/cart',
    element: <Cart />
  },
  {
    path: '/profile',
    element: <Profile />
  },
  {
    path: '/login',
    element: <Login />
  },
  {
    path: '/register',
    element: <Register />
  },
  {
    path: '/product',
    element: <Product />
  },
  {
    path: '/fetch-data',
    element: <FetchData />
  }
];

export default AppRoutes;
