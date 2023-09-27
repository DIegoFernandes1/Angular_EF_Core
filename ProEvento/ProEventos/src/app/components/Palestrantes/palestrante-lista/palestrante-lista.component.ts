import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Palestrante } from '@app/models/Palestrante';
import { PaginatedResult, Pagination } from '@app/models/Pagination';
import { PalestranteService } from '@app/services/palestrante.service';
import { BsModalService } from 'ngx-bootstrap/modal';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';
import { Subject } from 'rxjs';
import { debounceTime } from 'rxjs/operators';
import { environment } from '@environments/environment';

@Component({
  selector: 'app-palestrante-lista',
  templateUrl: './palestrante-lista.component.html',
  styleUrls: ['./palestrante-lista.component.scss']
})
export class PalestranteListaComponent implements OnInit {

  termoBuscaChanged: Subject<string> = new Subject<string>();
  public palestrantes: Palestrante[] = [];
  public pagination = {} as Pagination;

  constructor(
    private palestranteService: PalestranteService,
    private modalService: BsModalService,
    private spinner: NgxSpinnerService,
    private toastrService: ToastrService,
    private router: Router
    ) { }

    public ngOnInit() {
      this.pagination = {currentPage: 1, itemsPerPage: 3, totalItems: 1} as Pagination;

      this.getAllPalestrantes();
    }

    public filtrarPalestrantes(event: any): void{

      if(this.termoBuscaChanged.observers.length === 0){

        this.termoBuscaChanged.pipe(debounceTime(1000)).subscribe(
          filtarPor => {
            this.spinner.show()
            this.palestranteService.getAllPalestrantes(this.pagination.currentPage, this.pagination.itemsPerPage, filtarPor).subscribe(
              (paginatedResult: PaginatedResult<Palestrante[]>) =>{
                this.palestrantes = paginatedResult.result,
                this.pagination = paginatedResult.pagination
              },
              (error: any) =>
              {
                console.error(error)
                this.spinner.hide();
                this.toastrService.error('Erro ao carregar os Palestrantes', 'Erro');
              }
              ).add(() => this.spinner.hide());
            });
          }

          this.termoBuscaChanged.next(event.value)
        }

        public getAllPalestrantes(): void{
          this.spinner.show()

          this.palestranteService.getAllPalestrantes(this.pagination.currentPage, this.pagination.itemsPerPage).subscribe(

            (paginatedResult: PaginatedResult<Palestrante[]>) =>{
              this.palestrantes = paginatedResult.result,
              this.pagination = paginatedResult.pagination
            },
            (error: any) =>
            {
              console.error(error)
              this.spinner.hide();
              this.toastrService.error('Erro ao carregar os Palestrantes', 'Erro');
            }
            ).add(() => this.spinner.hide());
          }

          public getImagemURL(imagemName: string): string {

            if(imagemName)
            return `${environment.ApiURLResources}${environment.ImagemPerfil}${imagemName}`
            else
            return `${environment.imagemDefaultURL}`
          }

        }
