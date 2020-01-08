import { SHA512 } from 'crypto-js';

function setCookie(cname: string, cvalue: string) {
  const d = new Date()
  const exdays = 30

  d.setTime(d.getTime() + (exdays * 24 * 60 * 60 * 1000));

  const expires = 'expires=' + d.toUTCString();
  document.cookie = cname + '=' + cvalue + ';' + expires + ';path=/'
}

function getCookie(cname: string): string {
  const name = cname + '='
  const decodedCookie = decodeURIComponent(document.cookie)
  const ca = decodedCookie.split(';')

  for (let c of ca) {
    while (c.charAt(0) === ' ') {
      c = c.substring(1)
    }
    if (c.indexOf(name) === 0) {
      return c.substring(name.length, c.length)
    }
  }
  return ''
}

function hashPassword(password: string) {
  return SHA512(password + 'consilium').toString()
}
export {
  getCookie,
  setCookie,
  hashPassword,
}
