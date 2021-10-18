import ky from 'ky';

const api = ky.create({
  prefixUrl: '/api',
  retry: 0,
});

export default api;