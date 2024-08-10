# Auth

```
14:44:24 [vite] page reload src/routes/(app)/settings/+page.server.ts
OPEN_SESSION HEADERS: HeadersList {
  cookies: [
    'id=e9704b0f-f332-453f-af45-4b8199917d36; HttpOnly; SameSite=Strict; Path=/; Max-Age=86399'
  ],
  [Symbol(headers map)]: Map(6) {
    'content-type' => { name: 'content-type', value: 'application/json' },
    'content-length' => { name: 'content-length', value: '237' },
    'access-control-allow-origin' => { name: 'access-control-allow-origin', value: '*' },
    'vary' => {
      name: 'vary',
      value: 'origin, access-control-request-method, access-control-request-headers'
    },
    'set-cookie' => {
      name: 'set-cookie',
      value: 'id=e9704b0f-f332-453f-af45-4b8199917d36; HttpOnly; SameSite=Strict; Path=/; Max-Age=86399'
    },
    'date' => { name: 'date', value: 'Wed, 17 Jan 2024 13:44:34 GMT' }
  },
  [Symbol(headers map sorted)]: null
}

Server Cookie: id=e9704b0f-f332-453f-af45-4b8199917d36; HttpOnly; SameSite=Strict; Path=/; Max-Age=86399

ME HEADERS: HeadersList {
  cookies: null,
  [Symbol(headers map)]: Map(5) {
    'content-type' => { name: 'content-type', value: 'application/json' },
    'content-length' => { name: 'content-length', value: '237' },
    'access-control-allow-origin' => { name: 'access-control-allow-origin', value: '*' },
    'vary' => {
      name: 'vary',
      value: 'origin, access-control-request-method, access-control-request-headers'
    },
    'date' => { name: 'date', value: 'Wed, 17 Jan 2024 13:44:34 GMT' }
  },
  [Symbol(headers map sorted)]: null
}
USER: {
  id: '8bb5644a-1803-41da-9379-11b717ed7083',
  email: 'leon.delahamette@hotmail.de',
  first_name: 'Hax0r',
  last_name: 'Hax0r',
  avatar_url: null,
  role: 'student',
  created_at: '2023-11-21T00:07:08.656',
  updated_at: '2024-01-17T13:44:34.927'
}

# NOTE: Server tells you to fuck off basically and that your cookie is worthless
USERS HEADERS: HeadersList {
  cookies: [ 'id=; Max-Age=0; Expires=Tue, 17 Jan 2023 13:44:42 GMT' ],
  [Symbol(headers map)]: Map(5) {
    'access-control-allow-origin' => { name: 'access-control-allow-origin', value: '*' },
    'vary' => {
      name: 'vary',
      value: 'origin, access-control-request-method, access-control-request-headers'
    },
    'set-cookie' => {
      name: 'set-cookie',
      value: 'id=; Max-Age=0; Expires=Tue, 17 Jan 2023 13:44:42 GMT'
    },
    'content-length' => { name: 'content-length', value: '0' },
    'date' => { name: 'date', value: 'Wed, 17 Jan 2024 13:44:42 GMT' }
  },
  [Symbol(headers map sorted)]: null
}

```
