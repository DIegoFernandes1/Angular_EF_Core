import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { AbstractControlOptions, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ValidatorField } from '@app/helpers/ValidatorField';
import { UserUpdate } from '@app/models/Identity/UserUpdate';
import { AccountService } from '@app/services/account.service';
import { PalestranteService } from '@app/services/palestrante.service';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-perfil-detalhe',
  templateUrl: './perfil-detalhe.component.html',
  styleUrls: ['./perfil-detalhe.component.scss']
})
export class PerfilDetalheComponent implements OnInit {

  @Output() changeFormValue = new EventEmitter();
  UserUpdate = {} as UserUpdate;
  form!: FormGroup;

  get f(): any{ return this.form.controls; }

  constructor(private fb: FormBuilder,
    public accountService: AccountService,
    public palestranteService: PalestranteService,
    private router: Router,
    private toaster: ToastrService,
    private spinner: NgxSpinnerService) { }

    ngOnInit() {
      this.validation();
      this.carregarUsuario();
      this.verificaForm();
    }

    private verificaForm(): void {
      this.form.valueChanges.subscribe(
        () => this.changeFormValue.emit({...this.form.value})
        )
      }

      private carregarUsuario(): void {
        this.spinner.show()
        this.accountService.getUser().subscribe(
          (userReturn: UserUpdate) => {
            this.UserUpdate = userReturn;
            this.form.patchValue(this.UserUpdate);
            this.toaster.success('Usuário carregado', 'Sucesso');
          },
          (error) => {
            console.error(error)
            this.toaster.error('Usuário não carregado', 'Erro');
            this.router.navigate(['/dashboard'])
          }
          )
          .add(() => this.spinner.hide());
        }

        public validation(): void{
          const formOptions: AbstractControlOptions = {
            validators: ValidatorField.Mustmatch('password', 'confirmePassword')
          };

          this.form = this.fb.group({
            titulo: ['NaoInformado', Validators.required],
            nome: ['', Validators.required],
            imagemURL: [''],
            sobrenome: ['', Validators.required],
            email: ['', [Validators.required, Validators.email]],
            phoneNumber: ['', Validators.required],
            descricao: ['', Validators.required],
            funcao: ['NaoInformado', Validators.required],
            password: ['', [Validators.required, Validators.minLength(4)]],
            confirmePassword: ['', Validators.required],
            username: ['']
          },formOptions
          );
        }


        onSubmit() : void{

          this.atualizarUsuario();
        }

        public atualizarUsuario() {
          this.UserUpdate =  {... this.form.value}
          this.spinner.show();

          if(this.f.funcao.value == 'Palestrante'){
            this.palestranteService.post().subscribe(
              () => this.toaster.success('Função palestrante atividade!', 'Sucesso'),
              (error) => {
                this.toaster.error('A função não pode ser ativada', 'Error');
                console.error(error);
              }
            )
          }

          this.accountService.updateUser(this.UserUpdate).subscribe(
            () => this.toaster.success('Usuário atualizado com sucesso!', 'Sucesso'),
            (error) => {
              this.toaster.error(error.error);
              console.error(error);
            }
            )
            .add(() => this.spinner.hide())
          }

          public resetForm(event: any): void {
            event.preventDefault();
            this.form.reset();
          }

        }
