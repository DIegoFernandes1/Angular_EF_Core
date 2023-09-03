import { Component, OnInit, TemplateRef } from '@angular/core';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { Evento } from '@app/models/Evento';
import { EventoService } from '@app/services/evento.service';
import { ToastrService } from 'ngx-toastr';
import { NgxSpinnerService } from 'ngx-spinner';
import { Router } from '@angular/router';
import { environment } from '@environments/environment';
import { PaginatedResult, Pagination } from '@app/models/Pagination';
import { Subject } from 'rxjs';
import { debounceTime } from 'rxjs/operators';

@Component({
  selector: 'app-evento-lista',
  templateUrl: './evento-lista.component.html',
  styleUrls: ['./evento-lista.component.scss']
})
export class EventoListaComponent implements OnInit {

  public eventos: Evento[] = [];
  public MostrarImagem = false;
  public idEvento = 0;
  public pagination = {} as Pagination;
  modalRef?: BsModalRef;

  termoBuscaChanged: Subject<string> = new Subject<string>();

  constructor(
    private eventoService: EventoService,
    private modalService: BsModalService,
    private ToastrService: ToastrService,
    private spinner: NgxSpinnerService,
    private router: Router
    ) { }

    public ngOnInit() {
      this.pagination = {currentPage: 1, itemsPerPage: 3, totalItems: 1} as Pagination;

      this.getAllEventos();
    }

    public ListarImagem(imagemURL: string): string{
      return (imagemURL !== '' && imagemURL !== null)
      ? `${environment.ApiURLResources}${environment.resourcesAPI}${imagemURL}`
      : 'assets/imagens/sem-foto.jpg';

    }

    public getAllEventos(): void{
      this.spinner.show()

      this.eventoService.getAllEventos(this.pagination.currentPage, this.pagination.itemsPerPage).subscribe(

        (paginatedResult: PaginatedResult<Evento[]>) =>{
          this.eventos = paginatedResult.result,
          this.pagination = paginatedResult.pagination
        },
        (error: any) =>
        {
          console.error(error)
          this.spinner.hide();
          this.ToastrService.error('Erro ao carregar os eventos', 'Erro');
        }
        ).add(() => this.spinner.hide());
      }

      public filtrarEventos(event: any): void{

        if(this.termoBuscaChanged.observers.length === 0){

          this.termoBuscaChanged.pipe(debounceTime(1000)).subscribe(
            filtarPor => {
              this.spinner.show()
              this.eventoService.getAllEventos(this.pagination.currentPage, this.pagination.itemsPerPage, filtarPor).subscribe(
                (paginatedResult: PaginatedResult<Evento[]>) =>{
                  this.eventos = paginatedResult.result,
                  this.pagination = paginatedResult.pagination
                },
                (error: any) =>
                {
                  console.error(error)
                  this.spinner.hide();
                  this.ToastrService.error('Erro ao carregar os eventos', 'Erro');
                }
                ).add(() => this.spinner.hide());
              });
            }

            this.termoBuscaChanged.next(event.value)
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

            public pageChanged(event): void{
              this.pagination.currentPage = event.page;
              this.getAllEventos();
            }
          }
