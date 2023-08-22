import { Injectable } from '@angular/core';
import { CanActivate, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {

  constructor(
    private router: Router,
    private toaster: ToastrService
    ){}

    canActivate(): boolean {
      console.log(localStorage.getItem['user'])
      if(localStorage.getItem['user'] !== null
        && localStorage.getItem['user'] !== undefined){
        return true;
      }


      this.toaster.info('Usuário não autenticado')
      this.router.navigate(['/user/login'])
      return false;
    }

  }
