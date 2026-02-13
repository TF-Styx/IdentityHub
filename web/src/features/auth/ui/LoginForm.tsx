// src/features/auth/ui/LoginForm.tsx
import { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { useAuthStore, useAuthActions } from '../model/authStore';
import styles from './LoginForm.module.css';

export const LoginForm = () => {
  const { stage, error } = useAuthStore();
  const [login, setLogin] = useState('');
  const [password, setPassword] = useState('');
  const { login: performLogin } = useAuthActions();
  const navigate = useNavigate();

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    await performLogin(login, password);
  };

    if (stage === 'success') {
    navigate('/', { replace: true });
  }

  return (
    <form onSubmit={handleSubmit} className={styles.form}>
      <div className={styles.field}>
        <label className={styles.label}>Логин</label>
        <input
          type="text"
          value={login}
          onChange={(e) => setLogin(e.target.value)}
          placeholder="Введите логин"
          required
          className={styles.input}
        />
      </div>

      <div className={styles.field}>
        <label className={styles.label}>Пароль</label>
        <input
          type="password"
          value={password}
          onChange={(e) => setPassword(e.target.value)}
          placeholder="Введите пароль"
          required
          className={styles.input}
        />
      </div>

      {/* Ошибка из store */}
      {error && <div className={styles.error}>{error}</div>}

      <button type="submit" className={styles.button}>
        Войти
      </button>
    </form>
  );
};