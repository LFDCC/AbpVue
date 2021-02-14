import Cookies from 'js-cookie'

const TokenKey = 'av_token'
const RefreshTokenKey='av_refresh_token'


export function getToken() {
  return Cookies.get(TokenKey)
}

export function setToken(token) {
  const result = Cookies.set(TokenKey, token)
  return result
}

export function removeToken() {
  const result = Cookies.remove(TokenKey)
  return result;
}
export function getRefreshToken() {
  return Cookies.get(RefreshTokenKey)
}
export function setRefreshToken(token) {
  const result = Cookies.set(RefreshTokenKey, token)
  return result
}

export function removeRefreshToken() {
  const result = Cookies.remove(RefreshTokenKey)
  return result;
}