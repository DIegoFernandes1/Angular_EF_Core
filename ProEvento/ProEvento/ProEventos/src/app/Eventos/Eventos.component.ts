import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-Eventos',
  templateUrl: './Eventos.component.html',
  styleUrls: ['./Eventos.component.scss']
})
export class EventosComponent implements OnInit {

  public eventos: any = [];
  public eventosFiltrados: any = [];
  public MostrarImagem = false;
  private _filtroLista: string = '';

  constructor(private httpClient: HttpClient) { }

  ngOnInit() {
    this.getEventos();
  }

  public getEventos(): void{
    this.httpClient.get("https://localhost:5001/api/evento").subscribe(
      response => {
        this.eventos = response,
        this.eventosFiltrados = this.eventos
        },
      error => console.log(error)
    );
  }

  public get filtroLista(){
    return this._filtroLista;
  }

  public set filtroLista(value: string){
    this._filtroLista = value;
    this.eventosFiltrados = this.filtroLista ? this.filtrarEventos(this.filtroLista): this.eventos;
  }

  public filtrarEventos(filtrarPor: string): any{
    filtrarPor = filtrarPor.toLocaleLowerCase();
    return this.eventos.filter(
      (evento: any) => evento.tema.toLocaleLowerCase().indexOf(filtrarPor) !== -1
        || evento.local.toLocaleLowerCase().indexOf(filtrarPor) !== -1
    )
  }

}
