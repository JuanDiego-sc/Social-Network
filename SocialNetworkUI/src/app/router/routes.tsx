import { createBrowserRouter, Navigate } from "react-router";
import App from "../layout/App";
import HomePage from "../../features/home/HomePage";
import ActivityDashboard from "../../features/activities/dashboard/ActivityDashboard";
import ActivityForm from "../../features/activities/form/ActivityForm";
import ActivityDetailsPage from "../../features/activities/details/ActivityDetailsPage";
import Counter from "../../features/counter/Counter";
import TestErrors from "../../features/errors/TestErrors";
import NotFound from "../../features/errors/NotFound";
import ServerError from "../../features/errors/ServerError";
import LoginForm from "../../features/account/LoginForm";
import RequiredAuth from "./RequiredAuth";
import RegisterForm from "../../features/account/RegisterForm";

export const router = createBrowserRouter([
    {
        path: '/',
        element: <App></App>,
        children: [
            //The children from RequiredAuth needs to authenticate to access
            {element: <RequiredAuth/>, children:[
                {path: 'activities', element: <ActivityDashboard></ActivityDashboard>},
                {path: 'activities/:id', element: <ActivityDetailsPage></ActivityDetailsPage>},
                {path: 'createActivity', element: <ActivityForm key={'create'}></ActivityForm>},
                {path: 'manage/:id', element: <ActivityForm></ActivityForm>},
            ]},
            {path: '', element: <HomePage></HomePage>},
            {path: 'counter', element: <Counter></Counter>},
            {path: 'error', element: <TestErrors></TestErrors>},
            {path: 'not-found', element: <NotFound></NotFound>},
            {path: 'server-error', element: <ServerError></ServerError>},
            {path: 'login', element: <LoginForm></LoginForm>},
            {path: 'register', element: <RegisterForm></RegisterForm>},
            {path: '*', element: <Navigate replace to='/not-found'></Navigate>}
        ]
    }
]);