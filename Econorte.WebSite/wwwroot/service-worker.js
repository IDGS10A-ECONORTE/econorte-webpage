const CACHE_NAME = 'econorte-appshell-v1';
const APP_SHELL = [
  '/',
  '/Home/Login',
  '/Home/Index',
  '/css/site.css',
  '/js/site.js',
  '/favicon.ico',
];

self.addEventListener('install', (event) => {
  event.waitUntil(caches.open(CACHE_NAME).then((cache) => cache.addAll(APP_SHELL)));
  self.skipWaiting();
});

self.addEventListener('activate', (event) => {
  event.waitUntil(
    caches.keys().then((keys) => Promise.all(keys.map((k) => (k === CACHE_NAME ? null : caches.delete(k)))))
  );
  self.clients.claim();
});

self.addEventListener('fetch', (event) => {
  const req = event.request;
  if (req.mode === 'navigate') {
    event.respondWith(fetch(req).catch(() => caches.match('/')));
  } else {
    event.respondWith(caches.match(req).then((cached) => cached || fetch(req)));
  }
});
