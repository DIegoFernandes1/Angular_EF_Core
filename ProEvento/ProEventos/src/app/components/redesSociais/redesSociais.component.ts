import { Component, Input, OnInit, TemplateRef } from '@angular/core';
import { AbstractControl, FormArray, FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { RedeSocial } from '@app/models/RedeSocial';
import { RedeSocialService } from '@app/services/RedeSocial.service';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-redesSociais',
  templateUrl: './redesSociais.component.html',
  styleUrls: ['./redesSociais.component.scss']
})
export class RedesSociaisComponent implements OnInit {

  modalRef!: BsModalRef;
  @Input() eventoId = 0;
  public formRS: FormGroup;
  public redeSocialAtual = {id: 0, nome: '', indice: 0};

  public get redesSociais(): FormArray{
    return this.formRS.get("redesSociais") as FormArray;
  }

  constructor(
    private fb: FormBuilder,
    private modalService: BsModalService,
    private spinner: NgxSpinnerService,
    private redeSocialService: RedeSocialService,
    private toast: ToastrService,
    ) { }

    ngOnInit() {
      this.carregarRedesSociais(this.eventoId);
      this.validation();
    }

    public validation(): void{
      this.formRS = this.fb.group({
        redesSociais: this.fb.array([]),
      })
    }

    private carregarRedesSociais(id: number = 0): void {
      this.spinner.show();
      let origem = 'palestrante';

        if(this.eventoId !== 0){
          origem = 'evento'
        }

      this.redeSocialService.getRedesSociais(origem, id).subscribe(
        (redeSocialRetorno: RedeSocial[]) => {
          redeSocialRetorno.forEach((redeSocial) => {
            this.redesSociais.push(this.criarRedeSocial(redeSocial))
          })
        },
        (error) => {
          this.toast.error('Erro ao tentar carregar lista de rede social');
          console.error(error)
        }
        ).add(() => this.spinner.hide())
      }


      adicionarRedeSocial():void{
        this.redesSociais.push(this.criarRedeSocial({id: 0} as RedeSocial));
      }

      criarRedeSocial(redeSocial: RedeSocial): FormGroup{
        return this.fb.group({
          id: [redeSocial.id],
          nome: [redeSocial.nome, [Validators.required, Validators.minLength(3)]],
          url: [redeSocial.url, Validators.required]
        });
      }

      public retornarTitulo(redeSocialNome: string): string{
        return redeSocialNome == null || redeSocialNome == ''
        ? 'Nome do rede social'
        : redeSocialNome
      }

      public CssVaalidator(campoform: FormControl | AbstractControl | null): any{
        return {'is-invalid': campoform?.errors && campoform?.touched};
      }

      public SalvarRedesSociais(): void{
        this.spinner.show();
        let origem = 'palestrante';

        if(this.eventoId !== 0){
          origem = 'evento'
        }

        if(this.formRS.controls.redesSociais.valid){
          this.redeSocialService.saveRedesSociais(origem, this.eventoId, this.formRS.value.redesSociais).subscribe(
            () => {
              this.toast.success(`Redes sociais foram salvos com sucesso`, `Sucesso`)
              this.redesSociais.reset();
            },
            (error: any) => {
              console.error(error)
              this.toast.error(`Erro ao salvar redes sociais`, `Erro!`)
            }
            ).add(() => this.spinner.hide())
          }
        }

        public DeletarRedeSocial(template: TemplateRef<any>, index: number){
          this.redeSocialAtual.id = this.redesSociais.get(index + '.id')?.value;
          this.redeSocialAtual.nome = this.redesSociais.get(index + '.nome')?.value;
          this.redeSocialAtual.indice = index;

          this.modalRef =  this.modalService.show(template, {class: 'modal-sm'});
        }

        public confirmeDeleteRedeSocial(): void{
          this.modalRef.hide();
          this.spinner.show();
          let origem = 'palestrante';

          if(this.eventoId !== 0){
            origem = 'evento'
          }

          this.redeSocialService.deleteRedeSocial(origem, this.eventoId, this.redeSocialAtual.id).subscribe(
            () => {
              this.toast.success(`Rede social deletado com sucesso`, `Sucesso`);
              this.redesSociais.removeAt(this.redeSocialAtual.indice);
            },
            (error: any) => {
              console.error(error);
              this.toast.error(`Erro ao tentar excluir rede social de ID: ${this.redeSocialAtual.id}`, `Erro!`);
            }
            ).add(() => this.spinner.hide())
          }

          public declineDeleteRedeSocial(): void{
            this.modalRef.hide();
          }

        }
