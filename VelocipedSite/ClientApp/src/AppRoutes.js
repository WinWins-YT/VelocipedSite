import { Counter } from "./components/Counter";
import { FetchData } from "./components/FetchData";
import { Home } from "./components/Home";
import Catalog from "./components/Catalog";

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
    path: '/catalog',
    element: <Catalog />
  }, 
  {
    path: '/fetch-data',
    element: <FetchData />
  }
];

export default AppRoutes;
