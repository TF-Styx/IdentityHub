// src/app/router.tsx
import { createBrowserRouter, RouterProvider } from 'react-router-dom';
import { AuthPage } from '@/pages/auth';
import { RegisterPage } from '@/pages/register';

const router = createBrowserRouter([
  {
    path: '/auth',
    element: <AuthPage />,
  },
  {
    path: '/',
    element: <div>Главная</div>,
  },
  {
    path: '/register',
    element: <RegisterPage />,
  }
]);

export const AppRouter = () => <RouterProvider router={router} />;