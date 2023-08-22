import { Component, OnInit, TemplateRef } from '@angular/core';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { Evento } from '@app/models/Evento';
import { EventoService } from '@app/services/evento.service';
import { ToastrService } from 'ngx-toastr';
import { NgxSpinnerService } from 'ngx-spinner';
import { Router } from '@angular/router';
import { environment } from '@environments/environment';

@Component({
  selector: 'app-evento-lista',
  templateUrl: './evento-lista.component.html',
  styleUrls: ['./evento-lista.component.scss']
})
export class EventoListaComponent implements OnInit {

  public eventos: Evento[] = [];
  public eventosFiltrados: Evento[] = [];
  public MostrarImagem = false;
  private filtroListado = '';
  public idEvento = 0;
  modalRef?: BsModalRef;

  constructor(
    private eventoService: EventoService,
    private modalService: BsModalService,
    private ToastrService: ToastrService,
    private spinner: NgxSpinnerService,
    private router: Router
    ) { }

    public ngOnInit() {
      this.spinner.show()
      this.getAllEventos();
    }

    public ListarImagem(imagemURL: string): string{
      console.log(imagemURL);
      console.log(`${environment.apiURL+environment.resourcesAPI}/${imagemURL}}`);

      return (imagemURL !== '' && imagemURL !== null)
      ? `${environment.ApiURLResources}${environment.resourcesAPI}${imagemURL}`
      : 'assets/imagens/sem-foto.jpg';

    }

    public getAllEventos(): void{
      this.eventoService.getAllEventos().subscribe(
        {
          next: (_eventos: Evento[]) =>{
            this.eventos = _eventos,
            this.eventosFiltrados = this.eventos
          },
          error: (error: any) =>
          {
            this.spinner.hide();
            this.ToastrService.error('Erro ao carregar os eventos', 'Erro');
          },
          complete: () => {
            this.spinner.hide();
            this.ToastrService.success('Eventos carregados com sucesso', 'Carregado');
          }
        }
        );
      }

      public get filtroLista(){
        return this.filtroListado;
      }

      public set filtroLista(value: string){
        this.filtroListado = value;
        this.eventosFiltrados = this.filtroLista ? this.filtrarEventos(this.filtroLista): this.eventos;
      }

      public filtrarEventos(filtrarPor: string): Evento[]{
        filtrarPor = filtrarPor.toLocaleLowerCase();
        return this.eventos.filter(
          (evento: Evento) => evento.tema.toLocaleLowerCase().indexOf(filtrarPor) !== -1
          || evento.local.toLocaleLowerCase().indexOf(filtrarPor) !== -1
          );
        }


        public openModal(event: any, template: TemplateRef<any>, idEvento: number) {
          event.stopPropagation();
          this.idEvento = idEvento;
          this.modalRef = this.modalService.show(template, {class: 'modal-sm'});
        }

        public confirm(): void {
          this.modalRef?.hide();
          this.spinner.show();

          this.eventoService.deleteEvento(this.idEvento).subscribe(
            (result: string) => {
              this.ToastrService.success('O evento foi excluido com sucesso!', 'Deletado!');
              this.getAllEventos();
            },
            (error: any) => {
              this.ToastrService.error(`Erro ao tentar deletar o evento ${this.idEvento}`, 'Erro!')
              console.error(error);
            }
          ).add(() => this.spinner.hide());
        }

        public decline(): void {
          this.modalRef?.hide();
        }

        public detalheEvento(id: number): void{
          this.router.navigate([`eventos/detalhe/${id}`]);
        }

      }
