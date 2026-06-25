import { Injectable } from '@angular/core';
 
@Injectable()
export class ConfigService {    

    constructor() {}

    get authApiURI() {
        return 'https://localhost:7271';
    }    
     
    get clientApiURI() {
        return 'https://localhost:44362/api';
    }  
}
