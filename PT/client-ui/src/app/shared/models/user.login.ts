export class UserLogin {

    constructor(
        public email: string,
        public password: string,
        public rememberLogin: boolean,
        public returnUrl: string          
      ) {  }
}
