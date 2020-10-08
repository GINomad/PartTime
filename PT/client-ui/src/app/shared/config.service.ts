import { Injectable } from '@angular/core';
 
@Injectable()
export class ConfigService {    

    constructor() {}

    get authApiURI() {
        return 'https://localhost:44331';
    }    
     
    get clientApiURI() {
        return 'https://localhost:44362/api';
    }  
}