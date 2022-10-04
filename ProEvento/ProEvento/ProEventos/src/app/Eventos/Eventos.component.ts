import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-Eventos',
  templateUrl: './Eventos.component.html',
  styleUrls: ['./Eventos.component.scss']
})
export class EventosComponent implements OnInit {

  public eventos: any;

  constructor(private httpClient: HttpClient) { }

  ngOnInit() {
    this.getEventos();
  }

  public getEventos(): void{
    this.httpClient.get("https://localhost:5001/api/evento").subscribe(
      response => this.eventos = response,
      error => console.log(error)
    );
  }

}
