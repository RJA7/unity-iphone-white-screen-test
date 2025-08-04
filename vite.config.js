import { defineConfig } from 'vite';

export default defineConfig({
    root: './Build',
    server: {
        host: true,
        port: 5171,
        allowedHosts: true,
    },
    preview: {
        host: true,
        port: 5171,
        allowedHosts: true,
    },
});
