import { Component } from '@angular/core';
import { User } from './models/Identity/User';
import { AccountService } from './services/account.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {
  title = 'ProEventos';

  constructor(public accountService: AccountService){
  }

  ngOnInit(): void{
    this.setCurrentUser();
  }

  private setCurrentUser(): void{

    let user: User;

    if(localStorage.getItem('user')){
      user = JSON.parse(localStorage.getItem('user') ?? '{}');
    }else{
      user = null as any;
    }

    if(user)
      this.accountService.setCurrentUser(user);
  }
}
