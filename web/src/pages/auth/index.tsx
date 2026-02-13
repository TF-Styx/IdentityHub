// src/pages/auth/index.tsx
import { LoginForm } from '@/features/auth/ui/LoginForm';
import styles from './ui/AuthPage.module.css';

export const AuthPage = () => {
  return (
    <div className={styles.container}>
      <div className={styles.card}>
        <h1 className={styles.title}>Вход</h1>
        <LoginForm />
      </div>
    </div>
  );
};