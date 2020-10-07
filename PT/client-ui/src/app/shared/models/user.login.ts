export class UserLogin {

    constructor(
        public username: string,
        public password: string,
        public rememberLogin: boolean,
        public returnUrl: string          
      ) {  }
}