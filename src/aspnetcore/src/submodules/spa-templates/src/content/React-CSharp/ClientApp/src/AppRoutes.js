////#if (IndividualLocalAuth)
import ApiAuthorzationRoutes from './components/api-authorization/ApiAuthorizationRoutes';
////#endif
import { Counter } from "./components/Counter";
import { FetchData } from "./components/FetchData";
import { Home } from "./components/Home";

const AppRoutes = [
  {
    index: true,
    element: <Home />
  },
  {
    path: '/counter',
    element: <Counter />
  },
////#if (!IndividualLocalAuth)
  {
    path: '/fetch-data',
    element: <FetchData />
  }
////#else
  {
    path: '/fetch-data',
    requireAuth: true,
    element: <FetchData />
  },
  ...ApiAuthorzationRoutes
////#endif
];

export default AppRoutes;
