import { Component, OnInit } from '@angular/core';
import { AbstractControlOptions, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ValidatorField } from '@app/helpers/ValidatorField';
import { User } from '@app/models/Identity/User';
import { AccountService } from '@app/services/account.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-registration',
  templateUrl: './registration.component.html',
  styleUrls: ['./registration.component.scss']
})
export class RegistrationComponent implements OnInit {

  user = {} as User;
  form!: FormGroup;

  get f(): any{
    return this.form.controls;
  }
  constructor(private fb: FormBuilder,
              private accountService: AccountService,
              private router: Router,
              private toaster: ToastrService) { }

  ngOnInit() {
    this.validation();
  }

  public validation(): void{
    const formOptions: AbstractControlOptions = {
      validators: ValidatorField.Mustmatch('password', 'confirmePassword')
    };

    this.form = this.fb.group({
        nome: ['', Validators.required],
        sobrenome: ['', Validators.required],
        email: ['', Validators.required],
        username: ['', Validators.required],
        password: ['', [Validators.required, Validators.minLength(4)]],
        confirmePassword: ['', Validators.required]
    },formOptions
    );
  }

 public register(): void{
  this.user = {...this.form.value};
  this.accountService.Register(this.user).subscribe(
    () => this.router.navigateByUrl('/dashboard'),
    (error: any) => {
      console.error(error);
      this.toaster.error('Erro ao cadastrar usuário', 'Erro!');
    }
  )
 }
}
