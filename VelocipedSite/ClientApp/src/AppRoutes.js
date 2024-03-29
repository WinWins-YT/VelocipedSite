import { Home } from "./components/Home";
import CatalogCategories from "./components/CatalogCategories";
import Category from "./components/Category";
import Cart from "./components/Cart";
import Profile from "./components/Profile";
import Login from "./components/Login";
import Register from "./components/Register";
import Product from "./components/Product";
import Checkout from "./components/Checkout";
import OrderHistory from "./components/OrderHistory";
import Help from "./components/Help";

const AppRoutes = [
  {
    index: true,
    element: <Home />
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
    path: '/checkout',
    element: <Checkout />
  },
  {
    path: '/order-history',
    element: <OrderHistory />
  },
  {
    path: '/help',
    element: <Help />
  }
];

export default AppRoutes;
