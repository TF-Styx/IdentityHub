// src/features/auth/model/authStore.ts
import { create } from 'zustand';
import { SecureDataService } from '@/entities/user/lib/secureDataService';
import { getSRPChallenge, verifySRPProof } from '@/features/auth/api/authApi';

interface AuthState {
  stage: 'idle' | 'init' | 'verifying' | 'success' | 'error';
  error: string | null;
  session?: { user: { login: string } };
}

export const useAuthStore = create<AuthState>(() => ({
  stage: 'idle',
  error: null,
}));

export const useAuthActions = () => {
  const setState = useAuthStore.setState;

  return {
    async login(login: string, password: string) {
      setState({ stage: 'verifying', error: null });

      try {
        // 1. Получаем challenge от сервера
        const { salt, b } = await getSRPChallenge({ Login: login });

        // 2. Генерируем клиентское доказательство
        const crypto = new SecureDataService();
        const srpProof = await crypto.generateSrpProof(password, salt, b);

        // 3. Отправляем A и M1 на сервер
        const { m2 } = await verifySRPProof({
          Login: login,
          A: srpProof.A,
          M1: srpProof.M1,
        });

        // 4. Проверяем ответ сервера (M2)
        const isValid = await crypto.verifyServerM2(
          srpProof.A,
          srpProof.M1,
          srpProof.S,
          m2
        );

        if (!isValid) {
          throw new Error('Подлинность сервера не подтверждена');
        }

        // ✅ Успешная аутентификация
        setState({
          stage: 'success',
          session: { user: { login } },
        });
      } catch (err) {
        const message = err instanceof Error ? err.message : 'Неизвестная ошибка при входе';
        console.error('[Auth] Login failed:', err);
        setState({ stage: 'error', error: message });
      }
    },

    logout() {
      // TODO: вызов /auth/logout и очистка состояния
      setState({ stage: 'idle', error: null, session: undefined });
    },
  };
};


// // src/features/auth/model/authStore.ts
// import { create } from 'zustand';
// import { SecureDataService } from '@/entities/user/lib/srpClient';
// import { BFF_URL } from '@/app/config';

// interface AuthState {
//   stage: 'idle' | 'init' | 'verifying' | 'success' | 'error';
//   error: string | null;
//   session?: { user: { login: string } }; // или из entities/user/model/types
// }

// export const useAuthStore = create<AuthState>(() => ({
//   stage: 'idle',
//   error: null,
// }));

// export const useAuthActions = () => {
//   const setState = useAuthStore.setState;
//   const getState = useAuthStore.getState;

//   return {
//     async login(login: string, password: string) {
//       setState({ stage: 'verifying', error: null });

//       try {
//         const { getSRPChallenge, verifySRPProof } = await import('@/features/auth/api/authApi');

//         // 1. Получаем challenge
//         const { salt, b } = await getSRPChallenge({ Login: login });

//         // 2. Генерируем proof
//         const crypto = new SecureDataService();
//         const srpProof = await crypto.generateSrpProof(password, salt, b);

//         // 3. Отправляем proof
//         const { m2 } = await verifySRPProof({
//           Login: login,
//           A: srpProof.A,
//           M1: srpProof.M1,
//         });

//         // 4. Проверяем M2
//         const isValid = await crypto.verifyServerM2(
//           srpProof.A,
//           srpProof.M1,
//           srpProof.S,
//           m2
//         );

//         if (!isValid) {
//           throw new Error('Подлинность сервера не подтверждена');
//         }

//         setState({ stage: 'success', session: { user: { login } } });
//       } catch (err) {
//         const message = err instanceof Error ? err.message : 'Неизвестная ошибка';
//         setState({ stage: 'error', error: message });
//       }
//     },
//   };
// };