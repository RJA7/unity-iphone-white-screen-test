import { defineConfig } from 'vite';
import fs from 'fs';

export default defineConfig({
    root: './Build',
    server: {
        host: true,
        port: 5171,
        allowedHosts: true,
        https: {
            key: fs.readFileSync('./192.168.31.102+3-key.pem'),
            cert: fs.readFileSync('./192.168.31.102+3.pem'),
        },
    },
});
