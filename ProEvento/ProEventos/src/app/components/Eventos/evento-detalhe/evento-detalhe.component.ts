import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';

import { BsLocaleService } from 'ngx-bootstrap/datepicker';
import { ToastrService } from 'ngx-toastr';
import { NgxSpinnerService } from 'ngx-spinner';

import { Evento } from '@app/models/Evento';
import { EventoService } from '@app/services/evento.service';
import { idLocale } from 'ngx-bootstrap/chronos';


@Component({
  selector: 'app-evento-detalhe',
  templateUrl: './evento-detalhe.component.html',
  styleUrls: ['./evento-detalhe.component.scss']
})
export class EventoDetalheComponent implements OnInit {

  form!: FormGroup;
  evento!: Evento;
  estadoSalvar = 'post';

  get f(): any{
    return this.form.controls;
  }

  get bsConfig(): any{
    return { isAnimated: true,
             adaptivePosition: true,
             dateInputFormat: 'DD/MM/YYYY hh:mm a',
             containerClass: 'theme-default',
             showWeekNumbers: false
            }
  }

  constructor(private fb: FormBuilder,
              private localeService: BsLocaleService,
              private router: ActivatedRoute,
              private eventoService: EventoService,
              private spinner: NgxSpinnerService,
              private toast: ToastrService)
   {
    this.localeService.use('pt-br');
   }

   public carregarEvento(): void{
    const idEvento = this.router.snapshot.paramMap.get('id');

    if(idEvento != null){
      this.spinner.show();

      this.estadoSalvar = 'put'
      this.eventoService.getEventoById(+idEvento).subscribe( // o '+' na frente é como se fosse um cast em c#, usado pra converter algo para int
        (evento: Evento) =>{
          this.evento = {...evento} //gerando uma copia de evento - chama isso de spread
          this.form.patchValue(this.evento);
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
    this.carregarEvento();
    this.validation();
  }

  public validation(): void{
    this.form = this.fb.group(
      {
        tema: ['', [Validators.required, Validators.minLength(4), Validators.maxLength(50)]],
        local: ['', Validators.required],
        dataEvento: ['', Validators.required],
        quantidadePessoas: ['', [Validators.required, Validators.max(120000)]],
        imagemURL: ['', Validators.required],
        telefone: ['', Validators.required],
        email: ['', Validators.required],
      });
  }

  public resetForm():void{
    this.form.reset();
  }

  public CssVaalidator(campoform: FormControl): any{
      return {'is-invalid': campoform.errors && campoform.touched};
  }

  public SalvarAlteracoes(): void{
    this.spinner.show();

    if(this.form.valid){

      this.evento = (this.estadoSalvar === 'post')
          ? {...this.form.value}
          : {id: this.evento.id, ...this.form.value};

      this.eventoService[this.estadoSalvar](this.evento).subscribe(
        () => this.toast.success(`Evento salvo com sucesso`, `Sucesso`),
        (error: any) => {
          console.error(error);
          this.toast.error(`Erro ao salvar evento`, `Erro!`);
        }
      ).add(() => this.spinner.hide());
    }
  }
}
