// src/features/register/ui/RegisterForm.tsx
import { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { useRegisterStore, useRegisterActions } from '../model/registerStore';
import styles from './RegisterForm.module.css';

export const RegisterForm = () => {
  // Состояние из store
  const { stage, error } = useRegisterStore();
  const { register } = useRegisterActions();
  const navigate = useNavigate();

  // Локальные состояния полей
  const [login, setLogin] = useState('');
  const [username, setUsername] = useState('');
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const [confirmPassword, setConfirmPassword] = useState('');
  const [phone, setPhone] = useState(''); // ← новое поле

  const validate = (): boolean => {
    if (password !== confirmPassword) return false;
    if (password.length < 8) return false;
    return true;
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    if (!validate()) return;

    // Передаём phone как string | undefined
    await register(login, username, email, password, phone || undefined);
  };

  if (stage === 'success') {
    navigate('/auth', { replace: true });
  }

  const isSubmitting = stage === 'encrypting' || stage === 'sending';

  return (
    <form onSubmit={handleSubmit} className={styles.form}>
      {/* Логин */}
      <div className={styles.field}>
        <label className={styles.label}>Логин</label>
        <input
          type="text"
          value={login}
          onChange={(e) => setLogin(e.target.value)}
          placeholder="Введите логин"
          required
          disabled={isSubmitting}
          className={styles.input}
        />
      </div>

      {/* Имя */}
      <div className={styles.field}>
        <label className={styles.label}>Имя</label>
        <input
          type="text"
          value={username}
          onChange={(e) => setUsername(e.target.value)}
          placeholder="Ваше имя"
          required
          disabled={isSubmitting}
          className={styles.input}
        />
      </div>

      {/* Email */}
      <div className={styles.field}>
        <label className={styles.label}>Email</label>
        <input
          type="email"
          value={email}
          onChange={(e) => setEmail(e.target.value)}
          placeholder="example@mail.com"
          required
          disabled={isSubmitting}
          className={styles.input}
        />
      </div>

      {/* Телефон (необязательный) */}
      <div className={styles.field}>
        <label className={styles.label}>Телефон (необязательно)</label>
        <input
          type="tel"
          value={phone}
          onChange={(e) => setPhone(e.target.value)}
          placeholder="+7 (999) 123-45-67"
          disabled={isSubmitting}
          className={styles.input}
        />
      </div>

      {/* Пароль */}
      <div className={styles.field}>
        <label className={styles.label}>Пароль</label>
        <input
          type="password"
          value={password}
          onChange={(e) => setPassword(e.target.value)}
          placeholder="Не менее 8 символов"
          required
          disabled={isSubmitting}
          className={styles.input}
        />
        {password && password.length < 8 && (
          <div className={styles.error}>Пароль должен быть не менее 8 символов</div>
        )}
      </div>

      {/* Подтверждение пароля */}
      <div className={styles.field}>
        <label className={styles.label}>Подтверждение пароля</label>
        <input
          type="password"
          value={confirmPassword}
          onChange={(e) => setConfirmPassword(e.target.value)}
          placeholder="Повторите пароль"
          required
          disabled={isSubmitting}
          className={styles.input}
        />
        {confirmPassword && password !== confirmPassword && (
          <div className={styles.error}>Пароли не совпадают</div>
        )}
      </div>

      {/* Ошибка из store */}
      {error && <div className={styles.error}>{error}</div>}

      {/* Кнопка */}
      <button
        type="submit"
        disabled={
          isSubmitting ||
          !login ||
          !username ||
          !email ||
          !password ||
          !confirmPassword ||
          password !== confirmPassword ||
          password.length < 8
        }
        className={styles.button}
      >
        {stage === 'encrypting'
          ? 'Шифрование...'
          : stage === 'sending'
          ? 'Отправка...'
          : 'Зарегистрироваться'}
      </button>

      {/* Ссылка на вход */}
      <p style={{ textAlign: 'center', marginTop: '24px', color: '#a0a0a0' }}>
        Уже есть аккаунт?{' '}
        <a
          href="/auth"
          style={{
            color: '#4a90e2',
            textDecoration: 'none',
            fontWeight: 500,
          }}
        >
          Войти
        </a>
      </p>
    </form>
  );
};


// src/features/register/ui/RegisterForm.tsx
// import { useState } from 'react';
// import { useRegisterStore, useRegisterActions } from '../model/registerStore';
// import { useNavigate } from 'react-router-dom';
// import styles from './RegisterForm.module.css';

// export const RegisterForm = () => {
//   // Состояние из store
//   const { stage, error } = useRegisterStore();
//   const { register } = useRegisterActions();
//   const navigate = useNavigate();

//   // Локальные состояния полей (они не глобальные — это нормально!)
//   const [login, setLogin] = useState('');
//   const [username, setUsername] = useState('');
//   const [email, setEmail] = useState('');
//   const [password, setPassword] = useState('');
//   const [confirmPassword, setConfirmPassword] = useState('');

//   // Валидация перед отправкой
//   const validate = (): boolean => {
//     if (password !== confirmPassword) {
//       // Для мгновенной ошибки без запроса
//       return false;
//     }
//     if (password.length < 8) {
//       return false;
//     }
//     return true;
//   };

//   const handleSubmit = async (e: React.FormEvent) => {
//     e.preventDefault();

//     // Быстрая клиентская валидация
//     if (!validate()) {
//       // Ошибки покажем ниже через условия
//       return;
//     }

//     // Вызываем регистрацию из store
//     await register(login, username, email, password);
//   };

//   // После успешной регистрации — переходим на вход
//   if (stage === 'success') {
//     navigate('/auth', { replace: true });
//   }

//   // Состояния загрузки
//   const isEncrypting = stage === 'encrypting';
//   const isSending = stage === 'sending';
//   const isSubmitting = isEncrypting || isSending;

//   return (
//     <form onSubmit={handleSubmit} className={styles.form}>
//       {/* === Логин === */}
//       <div className={styles.field}>
//         <label className={styles.label}>Логин</label>
//         <input
//           type="text"
//           value={login}
//           onChange={(e) => setLogin(e.target.value)}
//           placeholder="Введите логин"
//           required
//           disabled={isSubmitting}
//           className={styles.input}
//         />
//       </div>

//       {/* === Имя === */}
//       <div className={styles.field}>
//         <label className={styles.label}>Имя</label>
//         <input
//           type="text"
//           value={username}
//           onChange={(e) => setUsername(e.target.value)}
//           placeholder="Ваше имя"
//           required
//           disabled={isSubmitting}
//           className={styles.input}
//         />
//       </div>

//       {/* === Email === */}
//       <div className={styles.field}>
//         <label className={styles.label}>Email</label>
//         <input
//           type="email"
//           value={email}
//           onChange={(e) => setEmail(e.target.value)}
//           placeholder="example@mail.com"
//           required
//           disabled={isSubmitting}
//           className={styles.input}
//         />
//       </div>

//       {/* === Пароль === */}
//       <div className={styles.field}>
//         <label className={styles.label}>Пароль</label>
//         <input
//           type="password"
//           value={password}
//           onChange={(e) => setPassword(e.target.value)}
//           placeholder="Не менее 8 символов"
//           required
//           disabled={isSubmitting}
//           className={styles.input}
//         />
//         {password && password.length < 8 && (
//           <div className={styles.error}>Пароль должен быть не менее 8 символов</div>
//         )}
//       </div>

//       {/* === Подтверждение пароля === */}
//       <div className={styles.field}>
//         <label className={styles.label}>Подтверждение пароля</label>
//         <input
//           type="password"
//           value={confirmPassword}
//           onChange={(e) => setConfirmPassword(e.target.value)}
//           placeholder="Повторите пароль"
//           required
//           disabled={isSubmitting}
//           className={styles.input}
//         />
//         {confirmPassword && password !== confirmPassword && (
//           <div className={styles.error}>Пароли не совпадают</div>
//         )}
//       </div>

//       {/* === Глобальная ошибка (из store) === */}
//       {error && <div className={styles.error}>{error}</div>}

//       {/* === Кнопка === */}
//       <button
//         type="submit"
//         disabled={
//           isSubmitting ||
//           !login ||
//           !username ||
//           !email ||
//           !password ||
//           !confirmPassword ||
//           password !== confirmPassword ||
//           password.length < 8
//         }
//         className={styles.button}
//       >
//         {isEncrypting
//           ? 'Шифрование...'
//           : isSending
//           ? 'Отправка...'
//           : 'Зарегистрироваться'}
//       </button>

//       {/* === Ссылка на вход === */}
//       <p style={{ textAlign: 'center', marginTop: '24px', color: '#a0a0a0' }}>
//         Уже есть аккаунт?{' '}
//         <a
//           href="/auth"
//           style={{
//             color: '#4a90e2',
//             textDecoration: 'none',
//             fontWeight: 500,
//           }}
//         >
//           Войти
//         </a>
//       </p>
//     </form>
//   );
// };


// // // src/features/auth/ui/RegisterForm.tsx
// // import { useState } from 'react';
// // import styles from './RegisterForm.module.css';

// // export const RegisterForm = () => {
// //   const [login, setLogin] = useState('');
// //   const [username, setUsername] = useState('');
// //   const [email, setEmail] = useState('');
// //   const [password, setPassword] = useState('');
// //   const [confirmPassword, setConfirmPassword] = useState('');
// //   const [error, setError] = useState('');

// //   const handleSubmit = (e: React.FormEvent) => {
// //     e.preventDefault();
// //     setError('');

// //     if (password !== confirmPassword) {
// //       setError('Пароли не совпадают');
// //       return;
// //     }

// //     if (password.length < 8) {
// //       setError('Пароль должен быть не менее 8 символов');
// //       return;
// //     }

// //     // Здесь будет вызов API регистрации
// //     console.log({ login, username, email, password });
// //   };

// //   return (
// //     <form onSubmit={handleSubmit} className={styles.form}>
// //       <div className={styles.field}>
// //         <label className={styles.label}>Логин</label>
// //         <input
// //           type="text"
// //           value={login}
// //           onChange={(e) => setLogin(e.target.value)}
// //           placeholder="Введите логин"
// //           required
// //           className={styles.input}
// //         />
// //       </div>

// //       <div className={styles.field}>
// //         <label className={styles.label}>Имя</label>
// //         <input
// //           type="text"
// //           value={username}
// //           onChange={(e) => setUsername(e.target.value)}
// //           placeholder="Ваше имя"
// //           required
// //           className={styles.input}
// //         />
// //       </div>

// //       <div className={styles.field}>
// //         <label className={styles.label}>Email</label>
// //         <input
// //           type="email"
// //           value={email}
// //           onChange={(e) => setEmail(e.target.value)}
// //           placeholder="example@mail.com"
// //           required
// //           className={styles.input}
// //         />
// //       </div>

// //       <div className={styles.field}>
// //         <label className={styles.label}>Пароль</label>
// //         <input
// //           type="password"
// //           value={password}
// //           onChange={(e) => setPassword(e.target.value)}
// //           placeholder="Не менее 8 символов"
// //           required
// //           className={styles.input}
// //         />
// //       </div>

// //       <div className={styles.field}>
// //         <label className={styles.label}>Подтверждение пароля</label>
// //         <input
// //           type="password"
// //           value={confirmPassword}
// //           onChange={(e) => setConfirmPassword(e.target.value)}
// //           placeholder="Повторите пароль"
// //           required
// //           className={styles.input}
// //         />
// //       </div>

// //       {error && <div className={styles.error}>{error}</div>}

// //       <button
// //         type="submit"
// //         disabled={!login || !email || !password || !confirmPassword}
// //         className={styles.button}
// //       >
// //         Зарегистрироваться
// //       </button>
// //     </form>
// //   );
// // };