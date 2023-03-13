import { Component, OnInit, TemplateRef } from '@angular/core';
import { AbstractControl, FormArray, FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';

import { BsLocaleService } from 'ngx-bootstrap/datepicker';
import { ToastrService } from 'ngx-toastr';
import { NgxSpinnerService } from 'ngx-spinner';

import { Evento } from '@app/models/Evento';
import { Lote } from '@app/models/Lote';

import { EventoService } from '@app/services/evento.service';
import { LoteService } from '@app/services/Lote.service';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';



@Component({
  selector: 'app-evento-detalhe',
  templateUrl: './evento-detalhe.component.html',
  styleUrls: ['./evento-detalhe.component.scss']
})
export class EventoDetalheComponent implements OnInit {

  form!: FormGroup;
  evento!: Evento;
  estadoSalvar = 'post';
  idEvento: any;
  modalRef!: BsModalRef;
  loteAtual = {id: 0, nome: '', index: 0}

  get f(): any{
    return this.form.controls;
  }

  get modoEditarLote(): boolean{
    return this.estadoSalvar === 'put';
  }

  get bsConfig(): any{
    return { isAnimated: true,
             adaptivePosition: true,
             dateInputFormat: 'DD/MM/YYYY hh:mm a',
             containerClass: 'theme-default',
             showWeekNumbers: false
            }
  }

  get bsConfigLote(): any{
    return { isAnimated: true,
             adaptivePosition: true,
             dateInputFormat: 'DD/MM/YYYY',
             containerClass: 'theme-default',
             showWeekNumbers: false
            }
  }

  get lotes(): FormArray{
    return this.form.get("lotes") as FormArray;
  }

  constructor(private fb: FormBuilder,
              private localeService: BsLocaleService,
              private activatedRouter: ActivatedRoute,
              private eventoService: EventoService,
              private loteService: LoteService,
              private spinner: NgxSpinnerService,
              private toast: ToastrService,
              private Router: Router,
              private modalService: BsModalService)
   {
    this.localeService.use('pt-br');
   }

   public CarregarEvento(): void{
     this.idEvento = this.activatedRouter.snapshot.paramMap.get('id');

    if(this.idEvento != null || this.idEvento === 0){
      this.spinner.show();

      this.estadoSalvar = 'put'
      this.eventoService.getEventoById(this.idEvento).subscribe( // o '+' na frente é como se fosse um cast em c#, usado pra converter algo para int
        (evento: Evento) =>{
          this.evento = {...evento} //gerando uma copia de evento - chama isso de spread
          this.form.patchValue(this.evento);

          // duas formas de carregar os lotes
          //1° forma mais simplificada
          this.evento.lote.forEach(lote => {
            this.lotes.push(this.CriarLote(lote));
          });

          //2° forma
          //this.CarregarLotes(); //forma mais "dificil" de carregar os lotes
        },
        (error: any) =>{
          this.spinner.hide()
          this.toast.error('Erro ao tentar carregar evento.','Erro!');
          console.error(error);
        },
        () => this.spinner.hide(),
      );
    }
   }

  ngOnInit() {
    this.CarregarEvento();
    this.Validation();
  }

  public Validation(): void{
    this.form = this.fb.group(
      {
        tema: ['', [Validators.required, Validators.minLength(4), Validators.maxLength(50)]],
        local: ['', Validators.required],
        dataEvento: ['', Validators.required],
        quantidadePessoas: ['', [Validators.required, Validators.max(120000)]],
        imagemURL: ['', Validators.required],
        telefone: ['', Validators.required],
        email: ['', Validators.required],
        lotes: this.fb.array([])
      });
  }

  AdicionarLote():void{
    this.lotes.push(this.CriarLote({id: 0} as Lote));
  }

  CriarLote(lote: Lote): FormGroup{
    return this.fb.group({
      id: [lote.id],
      nome: [lote.nome, [Validators.required, Validators.minLength(3)]],
      preco: [lote.preco, Validators.required],
      dataInicio: [lote.dataInicio],
      dataFim: [lote.dataFim],
      quantidade: [lote.quantidade, Validators.required],
    });
  }

  public ResetForm():void{
    this.form.reset();
  }

  public CssVaalidator(campoform: FormControl | AbstractControl | null): any{
      return {'is-invalid': campoform?.errors && campoform?.touched};
  }

  public SalvarEvento(): void{
    this.spinner.show();

    if(this.form.valid){

      this.evento = (this.estadoSalvar === 'post')
          ? {...this.form.value}
          : {id: this.evento.id, ...this.form.value};

      this.eventoService[this.estadoSalvar](this.evento).subscribe(
        (evento: Evento) => {
          this.toast.success(`Evento salvo com sucesso`, `Sucesso`)
          this.Router.navigate([`eventos/detalhe/${evento.id}`]);
        },
        (error: any) => {
          console.error(error);
          this.toast.error(`Erro ao salvar evento`, `Erro!`);
        }
      ).add(() => this.spinner.hide());
    }
  }

  public SalvarLotes(): void{
    this.spinner.show();

    if(this.form.controls.lotes.valid){
      this.loteService.saveLotes(this.idEvento, this.form.value.lotes).subscribe(
        () => {
          this.toast.success(`Lotes salvos com sucesso`, `Sucesso`)
          this.lotes.reset();
        },
        (error: any) => {
          console.error(error)
          this.toast.error(`Erro ao salvar lotes`, `Erro!`)
        }
      ).add(() => this.spinner.hide())
    }
  }

  public CarregarLotes(): void{
    this.loteService.getLotesByIdEventoAsync(this.idEvento).subscribe(
      (RetornoLotes: Lote[]) => {
        RetornoLotes.forEach(lote => {
          this.lotes.push(this.CriarLote(lote));
        });
      },
      (error: any) => {
        this.toast.error(`Erro ao tentar carregar lotes`, `Error!`);
        console.error(error);
      }
    ).add(() => this.spinner.hide())
  }

  public DeletarLote(template: TemplateRef<any>, index: number){
    this.loteAtual.id = this.lotes.get(index + '.id')?.value;
    this.loteAtual.nome = this.lotes.get(index + '.nome')?.value;
    this.loteAtual.index = index;

    this.modalRef =  this.modalService.show(template, {class: 'modal-sm'});
  }

  public ConfirmeDeleteLote(): void{
    this.modalRef.hide();
    this.spinner.show();

    this.loteService.deleteLote(this.idEvento, this.loteAtual.id).subscribe(
      () => {
        this.toast.success(`Lote excluido com sucesso`, `Sucesso`);
        this.lotes.removeAt(this.loteAtual.index);
      },
      (error: any) => {
        console.error(error);
        this.toast.error(`Erro ao tentar excluir lote de ID: ${this.loteAtual.id}`, `Erro!`);
      }
    ).add(() => this.spinner.hide())
  }

  public DeclineDeleteLote(): void{
    this.modalRef.hide();
  }

  public RetornarTituloLote(loteNome: string): string{
    return loteNome == null || loteNome == ''
    ? 'Nome do Lote'
    : loteNome
  }
}
