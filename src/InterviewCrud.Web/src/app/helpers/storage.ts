import { User } from "../models/token";

export const saveUser = (user: User) => {
  localStorage.setItem("app.token", JSON.stringify(user))
}

export const getUser = () : User | undefined => {
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
