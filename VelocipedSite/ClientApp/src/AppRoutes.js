import { Counter } from "./components/Counter";
import { FetchData } from "./components/FetchData";
import { Home } from "./components/Home";
import CatalogCategories from "./components/CatalogCategories";
import Category from "./components/Category";

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
    path: '/catalog',
    element: <Category />
  },
  {
    path: '/fetch-data',
    element: <FetchData />
  }
];

export default AppRoutes;
