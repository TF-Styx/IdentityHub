// vite.config.ts
import { defineConfig } from 'vite';
import react from '@vitejs/plugin-react';
import path from 'node:path';

export default defineConfig({
  plugins: [react()],
  resolve: {
    alias: {
      '@': path.resolve(__dirname, './src'),
    },
  },
  server: {
    port: 5173,
    host: true, // ← потому что ты запускаешь с --host
    proxy: {
      '/registration': 'http://localhost:5077',
      '/get-public-key': 'http://localhost:5077',
      // Можно добавить другие эндпоинты BFF
    },
  },
});

// // vite.config.ts
// import { defineConfig } from 'vite';
// import react from '@vitejs/plugin-react';
// import path from 'node:path';

// export default defineConfig({
//   plugins: [react()],
//   resolve: {
//     alias: {
//       '@': path.resolve(__dirname, './src'),
//     },
//   },
//   server: {
//     port: 5173,
//     proxy: {
//       '/registration': {
//         target: 'http://localhost:5077',
//         changeOrigin: true,
//         secure: false,
//         ws: false,
//       },
//       '/auth/public-key': {
//         target: 'http://localhost:5077',
//         changeOrigin: true,
//         secure: false,
//       },
//     },
//   },
// });

// import { defineConfig } from 'vite';
// import react from '@vitejs/plugin-react';
// import path from 'node:path';

// export default defineConfig({
//   plugins: [
//     react({
//       // Можно оставить babel-плагин, если используешь
//       // babel: {
//       //   plugins: [['babel-plugin-react-compiler']],
//       // },
//     }),
//   ],
//   resolve: {
//     alias: {
//       '@': path.resolve(__dirname, './src'), // ← КРИТИЧЕСКИ ВАЖНО!
//     },
//   },
//   server: {
//     port: 5173,
//     proxy: {
//       '/registration': {
//         target: 'http://localhost:5077',
//         changeOrigin: true,
//         secure: false,
//         ws: false,
//       },
//       '/auth/public-key': {
//         target: 'http://localhost:5077',
//         changeOrigin: true,
//         secure: false,
//       },
//     },
//   },
// });


// // import { defineConfig } from 'vite';
// // import react from '@vitejs/plugin-react';
// // import path from 'node:path';

// // // https://vite.dev/config/
// // export default defineConfig({
// //   plugins: [
// //     react({
// //       babel: {
// //         plugins: [['babel-plugin-react-compiler']],
// //       },
// //     }),
// //   ],
// //   resolve: {
// //     alias: {
// //       '@': path.resolve(__dirname, './src'),
// //     },
// //   },
// // });