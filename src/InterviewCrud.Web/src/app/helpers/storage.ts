import { Token } from "@angular/compiler";

export const saveToken = (token: Token) => {
  localStorage.setItem("app.token", JSON.stringify(token))
}

export const getToken = () : Token | undefined => {
  const token = localStorage.getItem("app.token")

  return token ? JSON.parse(token) : undefined;
}

export const signOut = () => {
  localStorage.clear()
}

export const isLogged = () : boolean => {
  const token = localStorage.getItem("app.token")

  return token ? true : false
}
