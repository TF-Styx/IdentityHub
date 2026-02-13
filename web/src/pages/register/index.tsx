// src/pages/register/index.tsx
import { RegisterForm } from '@/features/register/ui/RegisterForm';
import styles from './ui/RegisterPage.module.css';

export const RegisterPage = () => {
  return (
    <div className={styles.container}>
      <div className={styles.card}>
        <h1 className={styles.title}>Регистрация</h1>
        <RegisterForm />
      </div>
    </div>
  );
};