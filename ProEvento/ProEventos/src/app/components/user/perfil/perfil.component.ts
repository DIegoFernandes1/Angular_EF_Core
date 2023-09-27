import { Component, OnInit } from '@angular/core';
import { UserUpdate } from '@app/models/Identity/UserUpdate';
import { AccountService } from '@app/services/account.service';
import { environment } from '@environments/environment';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-perfil',
  templateUrl: './perfil.component.html',
  styleUrls: ['./perfil.component.scss']
})
export class PerfilComponent implements OnInit {

  public usuario = {} as UserUpdate;
  public imagemURL= '';
  public file: File;

  public get ehPalestrante(): boolean {
    return this.usuario.funcao?.toLowerCase() == 'palestrante';
  }

  constructor(
    private spinner: NgxSpinnerService,
    private toaster: ToastrService,
    private accountService: AccountService
    ) { }

    ngOnInit() {
    }

    public setFormValue(usuario: UserUpdate ): void {
      this.usuario = usuario;

      if(this.usuario.imagemURL)
      this.imagemURL = `${environment.ApiURLResources}${environment.ImagemPerfil}${usuario.imagemURL}`
      else
      this.imagemURL = `${environment.imagemDefaultURL}`

    }

    public onFileChange(ev: any):void{
      const reader = new FileReader();

      reader.onload = (event: any) => this.imagemURL = event.target.result;
      this.file = ev.target.files;
      reader.readAsDataURL(this.file[0])
      this.UploadImage();
    }

    private UploadImage(): void {
      this.spinner.show();

      this.accountService.postUpload(this.file).subscribe(
        () => this.toaster.success('Imagem atualizada com sucesso', 'Sucesso'),
        (error: any) => {
          this.toaster.error('Erro ao tentar atualizar imagem.', 'Erro');
          console.error(error)
        }
        ).add(() => this.spinner.hide())
      }
    }
