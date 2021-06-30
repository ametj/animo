interface ILogin {
  userNameOrEmail: string;
  password: string;
}

interface ILoginToken {
  token: string;
}

interface IRegister {
  userName: string;
  email: string;
  password: string;
}